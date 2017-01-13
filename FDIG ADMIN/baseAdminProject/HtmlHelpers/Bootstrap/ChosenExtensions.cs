using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Nimok.Mvc.Bootstrap.Extensions
{
    
    public enum CascadeDropdownType
    {
        DependOnParent, FilteredByParent
    }

    public class CascadeParentType
    {
        public string FieldName { get; set; }
        public CascadeDropdownType Type { get; set; }
        public string DataProviderParamName { get; set; }
    }

    public class ChosenDropDownList<TModel> : BootstrapControl<TModel, ChosenDropDownList<TModel>>
    {
        private readonly HtmlHelper htmlHelper;
        private readonly string expression;
        private readonly ModelMetadata metadata; 
        private readonly IEnumerable<SelectListItem> selectList;
        private readonly IDictionary<string, object> htmlAttributes;
        private readonly string optionLabel;
        private Label<TModel> label;

        private List<CascadeParentType> CascadeParentTypes = new List<CascadeParentType>();

        private string providerUrl;
        private string providerKeyProperty;
        private string providerTextProperty;

        public ChosenDropDownList(HtmlHelper htmlHelper, ModelMetadata metadata, string expression, IEnumerable<SelectListItem> selectList, string optionLabel,
            IDictionary<string, object> htmlAttributes)
        {
            if (metadata == null)
                throw new ArgumentNullException("metadata");

            this.htmlHelper = htmlHelper;
            this.expression = expression;
            this.metadata = metadata;
            this.selectList = selectList;
            this.optionLabel = optionLabel;
            this.htmlAttributes = htmlAttributes;
        }

        public override MvcHtmlString Render()        
        {
            Dictionary<CascadeDropdownType, string> cascadeDropdownTypes = new Dictionary<CascadeDropdownType, string>() { 
                { CascadeDropdownType.DependOnParent, "dependOnParent" }, 
                { CascadeDropdownType.FilteredByParent, "filteredByParent"} };

            IDictionary<string, object> htmlAttrs = GetHtmlAttributes();
            htmlAttrs["class"] = htmlAttrs["class"] + " chzn-select";

            if (!String.IsNullOrEmpty(providerUrl)) 
            {
                htmlAttrs.Add("data-provider-url", providerUrl);
                htmlAttrs.Add("data-provider-key", providerKeyProperty);
                htmlAttrs.Add("data-provider-text", providerTextProperty);
            }
            
            for (int i = 0; i < CascadeParentTypes.Count; i++)
            {
                htmlAttrs.Add(String.Format("data-cascade-type_{0}", i), cascadeDropdownTypes[CascadeParentTypes[i].Type]);
                htmlAttrs.Add(String.Format("data-parent-select_{0}", i), CascadeParentTypes[i].FieldName);
                if (!String.IsNullOrEmpty(CascadeParentTypes[i].DataProviderParamName))
                    htmlAttrs.Add(String.Format("data-provider-param-name_{0}", i), CascadeParentTypes[i].DataProviderParamName);
            }

            if (optionLabel != null)
                htmlAttrs.Add("placeholder", optionLabel);
            htmlAttributes.ToList().ForEach(p =>
                htmlAttrs.Add(p)
            );
            

            StringBuilder sb = new StringBuilder();
            if (label != null)
                sb.Append(label.Render().ToHtmlString());

            sb.Append(htmlHelper.DropDownList(expression, selectList, optionLabel, htmlAttrs));
            return MvcHtmlString.Create(sb.ToString());
        }

        public Label<TModel> Label()
        {
            if (label == null) 
            {
                label = new Label<TModel>(htmlHelper, metadata, expression) { ParentControl = this };
            }
            return label;
        }
        public ChosenDropDownList<TModel> DependOnParent<TProperty>(Expression<Func<TModel, TProperty>> parentField, CascadeDropdownType cascadeDropdownType, string dataProviderParamName = null)
        {
            string fieldName = ExpressionHelper.GetExpressionText(parentField);
            string parentFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(fieldName);
            if (String.IsNullOrEmpty(parentFieldName))
                throw new Exception("invalid parent field expression");

            // new implementation
            this.CascadeParentTypes.Add(new CascadeParentType() { FieldName = parentFieldName, Type = cascadeDropdownType, DataProviderParamName = dataProviderParamName });
            return this;
        }

        public ChosenDropDownList<TModel> DataProvidedBy(string providerUrl, string providerKeyProperty, string providerTextProperty)
        {
            this.providerUrl = providerUrl;
            this.providerKeyProperty = providerKeyProperty;
            this.providerTextProperty = providerTextProperty;
            return this;
        }
    }


    // todo: migrar a Bootstrap().ChosenDropDown
    //public static class ChosenExtensions
    //{

    //    public static MvcHtmlString ChosenDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression,
    //        IEnumerable<SelectListItem> selectList, string optionLabel,
    //        CascadeDropdownType cascadeDropdownType, Expression<Func<TModel, TProperty>> parentField, string providerUrl, string providerKeyProperty, string providerTextProperty)
    //    {
    //        return htmlHelper.ChosenDropDownListFor<TModel, TProperty>(expression, selectList, optionLabel, new Dictionary<string, object>(),
    //            cascadeDropdownType, parentField, providerUrl, providerKeyProperty, providerTextProperty);
    //    }

    //    public static MvcHtmlString ChosenDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression,
    //        IEnumerable<SelectListItem> selectList, string optionLabel, IDictionary<string, object> htmlAttributes,
    //        CascadeDropdownType cascadeDropdownType, Expression<Func<TModel, TProperty>> parentField, string providerUrl, string providerKeyProperty, string providerTextProperty)
    //    {

    //        string parentFieldName = ExpressionHelper.GetExpressionText(parentField);
    //        parentFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(parentFieldName);
    //        if (String.IsNullOrEmpty(parentFieldName))
    //        {
    //            throw new Exception("invalid parent field expression");
    //        }

    //        Dictionary<CascadeDropdownType, string> cascadeDropdownTypes = new Dictionary<CascadeDropdownType, string>() { 
    //            { CascadeDropdownType.DependOnParent, "dependOnParent" }, 
    //            { CascadeDropdownType.FilteredByParent, "filteredByParent"} };

    //        //IDictionary<string, object> htmlAttributes = new Dictionary<string, object>();
    //        htmlAttributes.Add("data-cascade-type", cascadeDropdownTypes[cascadeDropdownType]);
    //        htmlAttributes.Add("data-parent-select", parentFieldName);
    //        htmlAttributes.Add("data-placeholder", optionLabel); //???
    //        htmlAttributes.Add("data-provider-url", providerUrl);
    //        htmlAttributes.Add("data-provider-key", providerKeyProperty);
    //        htmlAttributes.Add("data-provider-text", providerTextProperty);

    //        return htmlHelper.ChosenDropDownListFor<TModel, TProperty>(expression, selectList, optionLabel, htmlAttributes);
    //    }

    //    public static MvcHtmlString ChosenDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression,
    //        IEnumerable<SelectListItem> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
    //    {
    //        htmlAttributes.Add("class", "chzn-select");

    //        MvcHtmlString html = htmlHelper.DropDownListFor<TModel, TProperty>(expression, selectList, optionLabel, htmlAttributes);
    //        return html;
    //    }
    //    public static MvcHtmlString ChosenDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression,
    //        IEnumerable<SelectListItem> selectList, string optionLabel)
    //    {
    //        return htmlHelper.ChosenDropDownListFor<TModel, TProperty>(expression, selectList, optionLabel, new Dictionary<string, object>());
    //    }

    //}
}