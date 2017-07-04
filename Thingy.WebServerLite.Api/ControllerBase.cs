using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.WebServerLite.Api
{
    public abstract class ControllerBase : IController
    {
        protected IWebServerRequest Request { get; private set; }
        protected IWebServerResponse Response { get; private set; }

        public ControllerBase()
        {
            Priority = Priorities.Normal;
        }

        public Priorities Priority { get; set; }

        public bool CanHandle(IWebServerRequest request)
        {
            string controllerName = string.IsNullOrEmpty(request.ControllerName) ? "Default" : request.ControllerName;
            bool canHandle = (GetType().Name.StartsWith(request.ControllerName));

            if (canHandle)
            {
                if (string.IsNullOrEmpty(request.ControllerName))
                {
                    request.ControllerName = GetType().Name.Substring(0, GetType().Name.Length - 10);
                }

                if (string.IsNullOrEmpty(request.ControllerMethodName))
                {
                    request.ControllerMethodName = "Index";
                }

                if (request.IsFile)
                {
                    request.AdjustFilePathForController();
                }
            }

            return canHandle;
        }

        public bool Handle(IWebServerRequest request, IWebServerResponse response)
        {
            Request = request;
            Response = response;

            MethodInfo method = GetType().GetMethods().FirstOrDefault(m => m.Name == request.ControllerMethodName && SupportsHttpMethod(m, request.HttpMethod));

            if (method != null)
            {
                if (UserAuthorized(method, request))
                {
                    try
                    {
                        object[] parameters = GetParametersFromRequest(method, request);
                        object o = method.Invoke(this, parameters);
                        IViewResult result = request.WebSite.ViewProvider.GetViewForRequest(request).Render(o);
                        response.FromString(result.Content, result.ContentType);
                    }
                    catch(Exception exception)
                    {
                        response.InternalError(request, exception);
                    }
                }
                else
                {
                    response.NotAllowed(request);
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        private object[] GetParametersFromRequest(MethodInfo method, IWebServerRequest request)
        {
            if (request.UrlValues.Any())
            {
                return BindUrlValuesByPosition(method, request.UrlValues);
            }
            else
            {
                return BindFieldsByName(method, request);
            }
        }

        private object[] BindUrlValuesByPosition(MethodInfo method, string[] values)
        {
            object[] parameters = new object[values.Length];
            int index = 0;
            ParameterInfo[] parameterInfos = method.GetParameters();

            if (parameters.Length > parameterInfos.Length)
            {
                throw new ControllerException(string.Format("Too many parameters calling {0}. {1} passed. {2} required.", method.Name, parameters.Length, parameterInfos.Length));
            }

            foreach (ParameterInfo parameterInfo in parameterInfos)
            {
                parameters[index] = StringToObject(values[index], parameterInfo.ParameterType);
                index++;
            }

            return parameters;
        }

        private object[] BindFieldsByName(MethodInfo method, IWebServerRequest request)
        {
            ParameterInfo[] parameterInfos = method.GetParameters();
            object[] parameters = new object[parameterInfos.Length];
            int index = 0;

            foreach (ParameterInfo parameterInfo in method.GetParameters())
            {
                parameters[index] = BindParameterByName(parameterInfo.ParameterType, parameterInfo.Name, request, string.Empty);
                index++;
            }

            return parameters;
        }

        private object BindParameterByName(Type bindType, string bindName, IWebServerRequest request, string prefix)
        {
            if (bindType.IsPrimitive || bindType == typeof(string))
            {
                string name = string.Format("{0}{1}", prefix, bindName);

                if (request.Fields.ContainsKey(name))
                {
                    return StringToObject(request.Fields[name], bindType);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                object o = Activator.CreateInstance(bindType); // If this is too limiting then could we use the CastleContainer??

                foreach (PropertyInfo propertyInfo in bindType.GetProperties(BindingFlags.Public | BindingFlags.FlattenHierarchy))
                {
                    object value = BindParameterByName(propertyInfo.PropertyType, propertyInfo.Name, request, string.Format("{0}{1}.", prefix, bindName));

                    if (value != null)
                    {
                        propertyInfo.SetValue(o, value);
                    }
                }

                return o;
            }
        }

        private object StringToObject(string value, Type type)
        {
            if (type == typeof(string))
            {
                return value;
            }

            if (type == typeof(int))
            {
                return Convert.ToUInt32(value);
            }

            if (type == typeof(DateTime))
            {
                return Convert.ToDateTime(value);
            }

            if (type == typeof(bool))
            {
                return Convert.ToBoolean(value);
            }

            if (type == typeof(long))
            {
                return Convert.ToInt64(value);
            }

            if (type == typeof(double))
            {
                return Convert.ToDouble(value);
            }

            if (type == typeof(byte))
            {
                return Convert.ToByte(value);
            }

            if (type == typeof(sbyte))
            {
                return Convert.ToSByte(value);
            }

            if (type == typeof(short))
            {
                return Convert.ToInt16(value);
            }

            if (type == typeof(ushort))
            {
                return Convert.ToUInt16(value);
            }

            if (type == typeof(uint))
            {
                return Convert.ToUInt32(value);
            }

            if (type == typeof(ulong))
            {
                return Convert.ToUInt64(value);
            }

            if (type == typeof(float))
            {
                return Convert.ToSingle(value);
            }

            if (type == typeof(decimal))
            {
                return Convert.ToDecimal(value);
            }

            if (type == typeof(char))
            {
                return Convert.ToChar(value);
            }

            throw new ArgumentException(string.Format("Unknown Primitive {0}, value \"{1}\"", type, value));
        }

        private bool UserAuthorized(MethodInfo method, IWebServerRequest request)
        {
            IList<AuthorizedRoleAttribute> attributes = method.GetCustomAttributes<AuthorizedRoleAttribute>().ToList();

            return !attributes.Any() || attributes.Any(a => request.User.Roles.Contains(a.Role));
        }

        private bool SupportsHttpMethod(MethodInfo m, string httpMethod)
        {
            return m.GetCustomAttributes<HttpMethodAttribute>().Any(a => a.Supports == httpMethod);
        }
    }
}
