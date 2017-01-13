using Nimok.Mvc.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nimok.Mvc.Bootstrap.Extensions
{

    public class Button<TModel> : BootstrapControl<TModel, Button<TModel>>
    {
        private readonly HtmlHelper htmlHelper;
        private string caption;
        private string href;
        private string icon;
        private string buttonType;
        private string buttonSize;
        private string hint;

        private bool popupable;
        private string popupTitle;
        private string popupCallbackOnClose;

        public Button(HtmlHelper htmlHelper, string caption, string href, string buttonType, string buttonSize, string icon)
        {
            this.htmlHelper = htmlHelper;
            this.caption = caption;
            this.href = href;
            this.icon = icon;
            this.buttonType = buttonType;
            this.buttonSize = buttonSize;
        }

        public override MvcHtmlString Render()
        {
            Dictionary<string, object> htmlAttributes = GetHtmlAttributes();
            TagBuilder tagBuilder = new TagBuilder("a");

            if (enabled)
                tagBuilder.Attributes.Add("href", href);

            tagBuilder.AddCssClass(buttonSize);
            tagBuilder.AddCssClass(buttonType);
            tagBuilder.AddCssClass("btn");

            if (hint != null)
                tagBuilder.Attributes.Add("title", hint);

            string text = caption;
            if (icon != null) {
                TagBuilder iconTag = new TagBuilder("i");
                iconTag.AddCssClass(icon);
                text = iconTag.ToString() + "&nbsp;" + caption;
            }
            tagBuilder.InnerHtml = text;

            if (enabled && popupable)
            {
                //new { @class = "btn btn-small popup-dialog", data_popup_dialog_title = "Crear nota", data_popup_dialog_onclose = "noteIndex_reload" }) *@
                tagBuilder.AddCssClass("popup-dialog");
                htmlAttributes.Add("data-popup-dialog-title", popupTitle);
                htmlAttributes.Add("data-popup-dialog-onclose", popupCallbackOnClose);
            }

            tagBuilder.MergeAttributes<string, object>(htmlAttributes, false);
            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }

        public Button<TModel> WithIcon(string icon)
        {
            this.icon = icon;
            return this;
        }

        public Button<TModel> Popup(string title)
        {
            this.popupable = true;
            this.popupTitle = title;
            return this;
        }

        public Button<TModel> Popup(string title, string callbackOnClose)
        {
            this.Popup(title);
            this.popupCallbackOnClose = callbackOnClose;
            return this;
        }

        public Button<TModel> Hint(string hint)
        {
            this.hint = hint;
            return this;
        }

        public Button<TModel> Action(string actionName, string controllerName, string queryParams = "")
        {
            UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            string url = urlHelper.Action(actionName, controllerName);
            this.href = url + queryParams;

            bool actionAuthorized = SecurityManager.Current.IsAuthorized(actionName, controllerName);
            this.Enabled(actionAuthorized);
            return this;
        }

    }

}