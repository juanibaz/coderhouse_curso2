using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace Nimok.Mvc.Bootstrap.Extensions
{
    public class ButtonSize
    {
        public const string Default = "";
        public const string Small = "btn-small";
        public const string Mini = "btn-mini";
        public const string Large = "btn-large";
    }

    public class ButtonType
    {
        public const string Default = "";
        public const string Primary = "btn-primary";
        public const string Danger = "btn-danger";
        public const string Warning = "btn-warning";
        public const string Success = "btn-success";
        public const string Info = "btn-info";
        public const string Inverse = "btn-inverse";
    }

    public enum DropPosition
    {
        Dropdown,
        DropdownRight,
        DropupLeft,
        DropupRight
    }

    public class DropdownActions : IDisposable
    {
        private TextWriter writer;
        private bool disposed = false;
        protected TagBuilder ulTag;
        public string LabelText { get; set; }
        public string ButtonSize { get; set; }
        public string ButtonType { get; set; }
        public DropPosition DropPosition { get; set; }

        public DropdownActions(TextWriter writer)
        {
            this.writer = writer;
            this.DropPosition = DropPosition.Dropdown;

            // create ul container for action links (see Add method)
            ulTag = new TagBuilder("ul");
        }

        private string GetHtml()
        {
            //<div class="btn-group">
            //    <a class="btn btn-small btn-primary dropdown-toggle" data-toggle="dropdown" href="#">
            //    Acción
            //    <span class="caret"></span>
            //    </a>
            //    <ul class="dropdown-menu">
            //      <li>{action}</li>
            //    </ul>
            //</div>

            // set ul class
            ulTag.AddCssClass("dropdown-menu");
            if ((DropPosition == DropPosition.DropdownRight) || (DropPosition == DropPosition.DropupRight))
                ulTag.AddCssClass("pull-right");

            // create dropdown container
            var container = new TagBuilder("div");
            container.AddCssClass("btn-group");
            if ((DropPosition == DropPosition.DropupLeft) || (DropPosition == DropPosition.DropupRight))
                container.AddCssClass("dropup");

            var aRef = new TagBuilder("a");
            aRef.AddCssClass("btn dropdown-toggle");
            aRef.AddCssClass(ButtonSize);
            aRef.AddCssClass(ButtonType);
            aRef.MergeAttribute("data-toggle", "dropdown");
            aRef.MergeAttribute("href", "#");
            aRef.InnerHtml = LabelText + " <span class=\"caret\"></span>";

            container.InnerHtml = aRef.ToString(TagRenderMode.Normal) + ulTag.ToString(TagRenderMode.Normal);
            return container.ToString();
        }

        public void Add(MvcHtmlString htmlAction)
        {
            var uiTag = new TagBuilder("li");
            uiTag.InnerHtml = htmlAction.ToString();

            // append li element to ul container
            ulTag.InnerHtml += uiTag.ToString(TagRenderMode.Normal);
        }

        public void Dispose()
        {
            // if not disposed, and we have an assigned writer, write closing divs
            if (!disposed && writer != null)
            {
                disposed = true;
                writer.Write(GetHtml());
            }
        }
    }

    /// <summary>
    /// Extension for Boostrap Dropdown actions commonly used in actions rows on grids.
    /// </summary>
    public static class BootstrapDropdownExtensions
    {
        public static DropdownActions BeginDropdownActions(this HtmlHelper html, string label, string buttonType, string buttonSize,
            DropPosition dropPosition = DropPosition.Dropdown)
        {
            var dropdownActions = new DropdownActions(html.ViewContext.Writer);
            dropdownActions.LabelText = label;
            dropdownActions.ButtonType = buttonType;
            dropdownActions.ButtonSize = buttonSize;
            dropdownActions.DropPosition = dropPosition;
            return dropdownActions;
        }

        public static DropdownActions BeginDropdownActions(this HtmlHelper html, string label)
        {
            return html.BeginDropdownActions(label, ButtonType.Default, ButtonSize.Default);
        }
    }
}
