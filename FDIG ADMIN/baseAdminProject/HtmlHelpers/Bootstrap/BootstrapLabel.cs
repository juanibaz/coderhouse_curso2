using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nimok.Mvc.Bootstrap.Extensions
{
    public class Label<TModel> : BootstrapControl<TModel, Label<TModel>>
    {
        private readonly HtmlHelper htmlHelper;
        private ModelMetadata metadata;
        private string htmlFieldName;

        private string labelText;
        private bool showRequiredStar;
        public Label(HtmlHelper htmlHelper, ModelMetadata metadata, string htmlFieldName)
        {
            this.htmlHelper = htmlHelper;
            this.htmlFieldName = htmlFieldName;
            this.metadata = metadata;

            if (htmlFieldName == null)
                throw new ArgumentNullException("metadata");

            LabelText(metadata.DisplayName);
            ShowRequiredStar(true);
            Class("control-label");

            /*
                <div class="form-group">
                    <label class="control-label col-md-2" for="RegionId">City</label>
                    <div class="col-md-4">
                        <label for="RegionId">Ciudade</label><select class="chzn-select" data-cascade-type="dependOnParent" data-parent-select="CountryId" data-provider-key="RegionId" data-provider-text="Name" data-provider-url="/UITest/GetCountryRegions" id="RegionId" name="RegionId"></select>
                        <span class="field-validation-valid" data-valmsg-for="RegionId" data-valmsg-replace="true"></span>
                    </div>
                </div>
            */
        }

        //public IBootstrapControl ParentControl { get; set; }

        public override MvcHtmlString Render()
        {
            Dictionary<string, object> htmlAttributes = GetHtmlAttributes();

            string tempLabelText = labelText;
            if (labelText == null && (tempLabelText = metadata.DisplayName) == null && (tempLabelText = metadata.PropertyName) == null)
            {
                tempLabelText = htmlFieldName.Split(new char[] { '.' }).Last<string>();
            }
            string text = tempLabelText;
            if (string.IsNullOrEmpty(text))
            {
                return MvcHtmlString.Empty;
            }
            TagBuilder tagBuilder = new TagBuilder("label");
            tagBuilder.Attributes.Add("for", TagBuilder.CreateSanitizedId(htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName)));
            tagBuilder.SetInnerText(text);

            if (showRequiredStar && metadata.IsRequired)
            {
                TagBuilder span = new TagBuilder("span");
                span.AddCssClass("required");
                span.SetInnerText("*");
                tagBuilder.InnerHtml += span.ToString();
            }

            tagBuilder.MergeAttributes<string, object>(htmlAttributes, true);
            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }

        public Label<TModel> LabelText(string labelText)
        {
            this.labelText = labelText;
            return this;
        }

        public Label<TModel> ShowRequiredStar(bool required)
        {
            showRequiredStar = required;
            return this;
        }
    }

}