using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thingy.WebServerLite.Api;

namespace Thingy.WebServerLite
{
    /// <summary>
    /// POC Library
    /// </summary>
    public class HtmlViewLibrary : IViewLibrary
    {
        /// <summary>
        /// POC Function
        /// </summary>
        /// <param name="name"></param>
        /// <param name="caption"></param>
        /// <param name="type"></param>
        /// <param name="attributes"></param>
        /// <returns>A button</returns>
        public string Button(string name, string caption, string type = "button", string attributes = "")
        {
            return string.Format("<button name=\"{0}\" type=\"{2}\"{3}{4}>{1}</button>", name, caption, type, string.IsNullOrEmpty(attributes) ? string.Empty : " ", attributes);
        }

        /// <summary>
        /// POC Function
        /// </summary>
        /// <returns>A bordered div with [[Content]] marker and placeholder</returns>
        public string TestDiv()
        {
            return "[[HasContent]]<div style=\"border : 1px solid black\">[[Content]]</div>";
        }

        /// <summary>
        /// POC Function 
        /// </summary>
        /// <param name="yesOrNo">A string containing Yes or No in English</param>
        /// <returns>true if the string looks like a yes, otherwise no</returns>
        public bool YesNoToBool(string yesOrNo)
        {
            return yesOrNo.Trim().ToLower()[0] == 'y';
        }

        /// <summary>
        /// POC Function
        /// </summary>
        /// <returns>A value to indicate the new model</returns>
        public string ShinyNewModel()
        {
            return "I'm a shiny new model!";
        }
    }
}
