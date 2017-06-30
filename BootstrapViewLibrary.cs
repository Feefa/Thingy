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

        public string Container()
        {
            return "<div class=\"container\">[[Content]]</div>";
        }
        public string ContainerFluid()
        {
            return "<div class=\"container-fluid\">[[Content]]</div>";
        }
        public string GridRow()
        {
            return "<div class=\"row\">[[Content]]</div>";
        }
        public string GridCell(int xs = -1, int sm = -1, int md = -1, int lg = -1)
        {
            return new MarkUpBuilder()
                .Append("<div class=\"")
                .AppendFormatConditional(xs != -1, "col-xs-{0}", xs)
                .AppendFormatConditional(sm != -1, "col-sm-{0}", sm)
                .AppendFormatConditional(md != -1, "col-md-{0}", md)
                .AppendFormatConditional(lg != -1, "col-lg-{0}", lg)
                .Append("\">[[Content]]</div>")
                .ToString();                
        }
        public string Form(string method, string action, string className = "", string name = "", string attributes = "")
        {
            return new MarkUpBuilder()
                .AppendFormat("<form method=\"{0}\" action=\"{1}\"", method, action)
                .AppendAttributeIfPopulated("class", className)
                .AppendAttributeIfPopulated("name", name)
                .AppendConditional(!string.IsNullOrEmpty(attributes), " ")
                .Append(attributes)
                .Append(">[[Content]]</form>")
                .ToString();
        }
        public string FormInput(string id, string label, string name = "", string value = "", string type = "text", string placeholder = "", bool visibleLabel = true, bool required = false, bool autofocus = false)
        {
            return new MarkUpBuilder()
                .AppendConditional(visibleLabel, "<div class=\"form-group\">")
                .Append("<label")
                .AppendConditional(!visibleLabel, " class=\"sr-only\"")
                .AppendAttribute("for", id)
                .Append(">")
                .Append(label)
                .Append("</label><input")
                .AppendAttributeIfPopulated("name", name)
                .AppendAttributeIfPopulated("value", value)
                .AppendAttribute("type", type)
                .AppendAttribute("id", id)
                .AppendAttributeIfPopulated("placeholder", placeholder)
                .AppendConditional(required, " required")
                .AppendConditional(autofocus, " autofocus")
                .Append(" />")
                .AppendConditional(visibleLabel, "</div>")
                .ToString();
        }
        public string FormCheckbox(string id, string label, string name = "", string value = "", bool inline = false, bool disabled = false)
        {
            return new MarkUpBuilder()
                .Append("<div class=\"checkbox\"><label")
                .AppendConditional(inline, " class=\"checkbox-inline\"")
                .Append("><input")
                .AppendAttribute("id", id)
                .AppendAttributeIfPopulated("name", name)
                .AppendAttributeIfPopulated("value", value)
                .AppendConditional(disabled, " disabled")
                .Append(" />")
                .Append(label)
                .Append("</label></div>")
                .ToString();
        }
        public string Button(string caption, string type = "button", string name = "", string style = "default", string size = "", bool block = false, bool disabled = false)
        {
            return new MarkUpBuilder()
                .Append("<button class=\"btn btn-", style)
                .AppendConditional(!string.IsNullOrEmpty(size), " btn-", size)
                .AppendConditional(block, " btn-block")
                .Append("\"")
                .AppendAttribute("type", type)
                .AppendAttributeIfPopulated("name", name)
                .AppendConditional(disabled, " disabled=\"disabled\"")
                .Append(">", caption, "</button>")
                .ToString();
        }

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

        public string ButtonGroup(string label, string type = "btn-group")
        {
            return string.Format("<div class=\"{1}\" role=\"group\" aria-label=\"{0}\">[[Content]]</div>", label, type);
        }

        public string ButtonToolbar(string label)
        {
            return string.Format("<div class=\"btn-toolbar\" role=\"toolbar\" aria-label=\"{0}\">[[Content]]</div>", label);
        }

        public string UnorderedList(string caption)
        {
            return string.Format("<ul>{0}[[Content]]</ul>", caption);
        }

        public string UnorderedListItem(string caption)
        {
            return string.Format("<li>{0}</li>", caption);
        }

    }
}
