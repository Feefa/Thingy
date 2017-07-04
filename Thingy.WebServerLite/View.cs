using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Thingy.WebServerLite.Api;

namespace Thingy.WebServerLite
{
    /// <summary>
    /// This class represents a set of view templates selected from various files to render a
    /// page.
    /// 
    /// The view can produce any kind of text file. The ContentType will be dictated by
    /// the extension of the controllerMethodPage
    /// 
    /// Parse-time Directives
    /// 
    /// {@TemplateName ...} - Declare a named template
    /// {!FileName} - Include a template file
    /// {* ... } - A comment
    /// 
    /// Render-time Directives
    /// 
    /// {PropertyName[.PropertyName]} - Render this property here
    /// {#TemplateName} - Render this named template here
    /// {=FunctionName(Params)} - Render the output of this function here
    ///
    /// </summary>
    public class View : IView
    {
        private readonly char[] period = new char[] { '.' };
        private readonly char[] comma = new char[] { ',' };
        private readonly IDictionary<string, string> templates = new Dictionary<string, string>();
        private readonly IViewLibrary[] commandLibraries;
        private readonly string contentType;

        public View(IViewLibrary[] commandLibraries, IMimeTypeProvider mimeTypeProvider, string masterPage, string controllerMasterPage, string controllerMethodPage)
        {
            this.commandLibraries = commandLibraries;
            templates["."] = string.Empty;

            if (!string.IsNullOrEmpty(masterPage))
            {
                AddPage(masterPage);
            }

            if (!string.IsNullOrEmpty(controllerMasterPage))
            {
                AddPage(controllerMasterPage);
            }

            if (!string.IsNullOrEmpty(controllerMethodPage))
            {
                AddPage(controllerMethodPage);
            }

            this.contentType = mimeTypeProvider.GetMimeType(controllerMethodPage);
        }

        private void AddPage(string path)
        {
            using (ViewReader reader = new ViewReader(path))
            {
                do
                {
                    string content = reader.ReadContent();

                    AddContent(content);

                    if (!reader.EndOfView)
                    {
                        string template = reader.ReadTemplate();

                        if (template[0] == '@')
                        {
                            AddTemplate(template);
                        }
                        else
                        {
                            if (template[0] == '!')
                            {
                                IncludeTemplate(path, template);
                            }
                            else
                            {
                                if (template[0] != '*')
                                {
                                    AddContent(string.Format("{0}{1}{2}", "{", template, "}"));
                                }
                            }
                        }
                    }
                }
                while (!reader.EndOfView);
            }
        }

        private void IncludeTemplate(string path, string template)
        {
            AddPage(Path.Combine(Path.GetDirectoryName(path), GetFirstTemplateElement(template)));
        }

        private void AddTemplate(string template)
        {
            templates[GetFirstTemplateElement(template)] = GetTemplateContent(template);
        }

        private void AddContent(string content)
        {
            templates["."] = string.Format("{0}{1}", templates["."], content);
        }

        private string GetFirstTemplateElement(string template)
        {
            int spacePos = template.IndexOf(' ');
            int newlinePos = template.IndexOf(Environment.NewLine);
            int delimiterPos = spacePos < newlinePos || newlinePos == -1 ? spacePos : newlinePos;

            if (delimiterPos == -1)
            {
                return template.Substring(1);
            }
            else
            {
                return template.Substring(1, delimiterPos - 1);
            }
        }

        private string GetTemplateContent(string template)
        {
            int spacePos = template.IndexOf(' ');
            int newlinePos = template.IndexOf(Environment.NewLine);

            if (spacePos < newlinePos || newlinePos == -1)
            {
                return template.Substring(spacePos + 1, template.Length - spacePos - 1);
            }
            else
            {
                return template.Substring(newlinePos + 2, template.Length - newlinePos - 2);
            }

        }

        public IViewResult Render(object model)
        {
            return new ViewResult()
            {
                Content = InternalRender(templates["."], model),
                ContentType = contentType
            };
        }

        private string InternalRender(string workingText, object model)
        {
            StringBuilder contentBuilder = new StringBuilder();

            while (!string.IsNullOrEmpty(workingText))
            {
                int openPos = workingText.IndexOf('{');

                if (openPos == -1)
                {
                    contentBuilder.Append(workingText);
                    workingText = string.Empty;
                }
                else
                {
                    workingText = ResolveInsert(model, contentBuilder, workingText, openPos);
                }
            }

            contentBuilder.Append(workingText);

            return contentBuilder.ToString();
        }

        private string ResolveInsert(object model, StringBuilder contentBuilder, string workingText, int openPos)
        {
            contentBuilder.Append(workingText.Substring(0, openPos));
            int closePos = FindMatchingClose(workingText, openPos);
            string newText;

            if (workingText[openPos + 1] == '#')
            {
                int spacePos = workingText.IndexOf(' ', openPos);

                if (spacePos == -1 || spacePos > closePos)
                {
                    string templateName = workingText.Substring(openPos + 2, closePos - openPos - 2);
                    newText = templates[templateName];
                }
                else
                {
                    string templateName = workingText.Substring(openPos + 2, spacePos - openPos - 2);
                    string propertyName = workingText.Substring(spacePos + 1, closePos - spacePos - 1);
                    object newModel = GetPropertyValueFromModel(model, propertyName);

                    if (newModel is Array)
                    {
                        StringBuilder newTextBuilder = new StringBuilder();

                        foreach (object modelElement in (Array)newModel)
                        {
                            newTextBuilder.Append(InternalRender(templates[templateName], modelElement));
                        }

                        newText = newTextBuilder.ToString();
                    }
                    else
                    {
                        newText = InternalRender(templates[templateName], newModel);
                    }
                }

            }
            else
            {
                if (workingText[openPos + 1] == '=' || workingText[openPos + 1] == '?')
                {
                    char action = workingText[openPos + 1];
                    string commandText = workingText.Substring(openPos + 2, closePos - openPos - 2);
                    newText = RunCommands(model, action, commandText);
                }
                else
                {
                    string propertyName = workingText.Substring(openPos + 1, closePos - openPos - 1);
                    newText = GetPropertyValueFromModel(model, propertyName).ToString();
                }
            }

            return string.Format("{0}{1}", newText, workingText.Substring(closePos + 1));
        }

        private string RunCommands(object model, char action, string commandText)
        {
            // The command up to the ) needs to be run regardless of if we have a template or not and wether we're doing a conditional or an insert
            int closePos = commandText.IndexOf(")");
            string content;

            if (action == '?')
            {
                // This is a conditional template. We only include it if the command returns true
                content = (bool)RunCommand(model, commandText.Substring(0, closePos + 1)) ? commandText.Substring(closePos + 1) : string.Empty;
            }
            else
            {
                object commandResult = RunCommand(model, commandText.Substring(0, closePos + 1)).ToString();
                string template = commandText.Substring(closePos + 1);

                if (template.All(c => Char.IsWhiteSpace(c)))
                {
                    // This is a simple command result insert
                    content = commandResult.ToString(); 
                }
                else
                {
                    if (commandResult is string && ((string)commandResult).StartsWith("[[HasContent]]"))
                    {
                        // This is a 'wrapping' function that contains a placeholder for the template
                        content = ((string)commandResult).Substring(14).Replace("[[Content]]", InternalRender(commandText.Substring(closePos + 1), model));
                    }
                    else
                    {
                        // This is a render-with-model template where the command result is the new model
                        content = InternalRender(commandText.Substring(closePos + 1), commandResult);
                    }
                }
            }

            return content;
        }

        private object RunCommand(object model, string commandText)
        {
            int openPos = commandText.IndexOf('(');
            string[] commandNameParts = commandText.Substring(0, openPos).Split(period);
            object commandLibrary;

            if (commandNameParts[0] == "model")
            {
                commandLibrary = model;

                for (int i = 1; i < commandNameParts.Length - 1; i++)
                {
                    PropertyInfo p = commandLibrary.GetType().GetProperty(commandNameParts[i]);
                    commandLibrary = p.GetValue(commandLibrary);
                }
            }
            else
            {
                commandLibrary = commandLibraries.FirstOrDefault(l => l.GetType().Name.StartsWith(commandNameParts[0]));

                if (commandLibrary == null)
                {
                    throw new ViewException(string.Format("Error running command \"{0}\". Could not find a command library named \"{1}\"", commandText, commandNameParts[0]));
                }
            }

            MethodInfo methodInfo = commandLibrary.GetType().GetMethods().FirstOrDefault(m => m.Name == commandNameParts[1]);

            if (methodInfo == null)
            {
                throw new ViewException(string.Format("Error running command \"{0}\". Method could not be found.", commandText));
            }

            string parameterString = commandText.Substring(openPos + 1, commandText.Length - openPos - 2).Trim();

            if (string.IsNullOrEmpty(parameterString))
            {
                if (methodInfo.GetParameters().Length > 0)
                {
                    throw new ViewException(string.Format("Error running command \"{0}\". Not enough parameters.", commandText));
                }

                return methodInfo.Invoke(commandLibrary, null);
            }
            else
            {
                string[] parameterNames = parameterString.Split(comma).Select(p => p.Trim()).ToArray();
                object[] parameters = new object[parameterNames.Length];

                for (int index = 0; index < parameters.Length; index++)
                {
                    if (parameterNames[index][0] == '"')
                    {
                        parameters[index] = parameterNames[index].Substring(1, parameterNames[index].Length - 2);

                    }
                    else
                    {
                        if ((parameterNames[index][0] >= '0' && parameterNames[index][0] <= '9') || parameterNames[index][0] == '-' || parameterNames[index][0] == '+')
                        {
                            if (!parameterNames[index].Contains("."))
                            {
                                parameters[index] = Convert.ToInt32(parameterNames[index]);
                            }
                            else
                            {
                                parameters[index] = Convert.ToDecimal(parameterNames[index]);
                            }
                        }
                        else
                        {
                            if (parameterNames[index].ToLower() == "true" || parameterNames[index].ToLower() == "false")
                            {
                                parameters[index] = Convert.ToBoolean(parameterNames[index]);
                            }
                            else
                            {
                                parameters[index] = GetPropertyValueFromModel(model, parameterNames[index]);
                            }
                        }
                    }
                }

                ParameterInfo[] parameterInfos = methodInfo.GetParameters();

                if (parameterInfos.Length > parameters.Length)
                {
                    int start = parameters.Length;
                    Array.Resize(ref parameters, parameterInfos.Length);

                    for (int index = start; index < parameters.Length; index++)
                    {
                        if (!parameterInfos[index].HasDefaultValue)
                        {
                            throw new ViewException(string.Format("Error running command \"{0}\". Not enough parameters.", commandText));
                        }

                        parameters[index] = parameterInfos[index].DefaultValue;
                    }
                }

                return methodInfo.Invoke(commandLibrary, parameters);
            }
        }

        private static int FindMatchingClose(string workingText, int openPos)
        {
            int closePos = workingText.IndexOf('}', openPos);
            int nestedOpenPos = workingText.IndexOf('{', openPos + 1);
            int nestingLevel = 0;

            while (nestingLevel > 0 || nestedOpenPos < closePos)
            {
                if (nestedOpenPos == -1)
                {
                    nestedOpenPos = int.MaxValue;
                }
                else
                {
                    if (nestingLevel == -1 || nestedOpenPos < closePos)
                    {
                        nestingLevel++;
                        nestedOpenPos = workingText.IndexOf('{', nestedOpenPos + 1);
                    }
                    else
                    {
                        nestingLevel--;
                        closePos = workingText.IndexOf('}', closePos + 1);
                    }
                }
            }

            return closePos;
        }

        private object GetPropertyValueFromModel(object model, string propertyName)
        {
            string[] propertyNames = propertyName.Split(period);
            Type type = model.GetType();
            object nestedModel = model;

            for (int index = 0; index < propertyNames.Length; index++)
            {
                if (propertyNames[index] != "model")
                {
                    PropertyInfo propertyInfo = type.GetProperties().FirstOrDefault(p => p.Name == propertyNames[index]);

                    if (propertyInfo == null)
                    {
                        throw new ViewException(string.Format("Error resolving property \"{0}\". {1} does not exist.", propertyName, propertyNames[index]));
                    }

                    type = propertyInfo.PropertyType;
                    nestedModel = propertyInfo.GetValue(nestedModel);
                }
            }

            return nestedModel;
        }
    }
}
