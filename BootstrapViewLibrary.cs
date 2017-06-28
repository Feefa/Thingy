using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thingy.WebServerLite.Api;

namespace Thingy.WebServerLite
{
    public class BootstrapViewLibrary : IViewLibrary
    {
        public readonly char[] pipe = { '|', '¦' };

        public string DropDown(string id, string caption, string[] items, string type = "dropdown", string alignment = "")
        {
            MarkUpBuilder builder = new MarkUpBuilder()
                .AppendFormat("<div class=\"{0}\"", type)
                .AppendFormat("<button class=\"btn btn-default dropdown-toggle\" type=\"button\" id=\"{0}\" data-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\">", id)
                .Append(caption)
                .Append("<span class=\"caret\"></span>")
                .Append("</button>")
                .AppendFormat("<ul class=\"dropdown-menu{1}{2}\" aria-labelledby=\"{0}\">", id, string.IsNullOrEmpty(alignment) ? string.Empty : " ", string.IsNullOrEmpty(alignment) ? string.Empty : alignment);

            DropDownAppendItems(items, builder);

            return builder
                .Append("</ul>")
                .Append("</div>")
                .ToString();
        }

        private void DropDownAppendItems(string[] items, MarkUpBuilder builder)
        {
            foreach (string[] itemElements in items.Select(i => i.Split(pipe)))
            {
                if (string.IsNullOrEmpty(itemElements[0]) || itemElements[0][1] == '-')
                {
                    builder.Append("<li role=\"separator\" class=\"divider\"></li>");
                }
                else
                {
                    if (itemElements[0][1] == '#')
                    {
                        builder.AppendFormat("<li class=\"dropdown-header\">{0}</li>", itemElements[0].Substring(1));
                    }
                    else
                    {
                        string link = itemElements.Length > 1 ? itemElements[1] : "#";

                        if (itemElements[0][1] == '~')
                        {
                            builder.AppendFormat("<li class=\"disabled\"><a href=\"{1}\">{0}</a></li>", itemElements[0].Substring(1), link);
                        }
                        else
                        {
                            builder.AppendFormat("<li><a href=\"{1}\">{0}</a></li>", itemElements[0], link);
                        }
                    }
                }
            }
        }

        public string ButtonGroupBegin(string label, string type = "btn-group")
        {
            return string.Format("<div class=\"{1}\" role=\"group\" aria-label=\"{0}\">", label, type);
        }

        public string ButtonGroupEnd()
        {
            return ("</div");
        }

        public string ButtonToolbarBegin(string label)
        {
            return string.Format("<div class=\"btn-toolbar\" role=\"toolbar\" aria-label=\"{0}\">", label);
        }

        public string ButtonToolbarEnd()
        {
            return "</div>";
        }

        public string DropDownButton()
        {

        }
    }
}
