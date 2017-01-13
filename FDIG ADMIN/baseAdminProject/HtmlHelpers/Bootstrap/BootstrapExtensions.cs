using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Nimok.Mvc.Bootstrap.Extensions
{
    public class Bootstrap<TModel> : IFluentInterface
    {
        private readonly HtmlHelper<TModel> htmlHelper;

        public HtmlHelper<TModel> HtmlHelper { get { return htmlHelper; } }

        public Bootstrap(HtmlHelper<TModel> htmlHelper)
        {
            this.htmlHelper = htmlHelper;
        }
    }

    public static class BootstrapExtensions
    {
        public static Bootstrap<TModel> Bootstrap<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            return new Bootstrap<TModel>(htmlHelper);
        }


        public static ChosenDropDownList<TModel> DropDownListFor<TModel, TProperty>(this Bootstrap<TModel> bootstrap, Expression<Func<TModel, TProperty>> expression)
        {
            return bootstrap.DropDownListFor(expression, Enumerable.Empty<SelectListItem>(), "");
        }

        public static ChosenDropDownList<TModel> DropDownListFor<TModel, TProperty>(this Bootstrap<TModel> bootstrap, Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> selectList)
        {
            return bootstrap.DropDownListFor(expression, selectList, "");
        }

        public static ChosenDropDownList<TModel> DropDownListFor<TModel, TProperty>(this Bootstrap<TModel> bootstrap, Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> selectList, string optionLabel)
        {
            return bootstrap.DropDownListFor(expression, selectList, optionLabel, new Dictionary<string, object>());
        }

        public static ChosenDropDownList<TModel> DropDownListFor<TModel, TProperty>(this Bootstrap<TModel> bootstrap, Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes)
        {
            return bootstrap.DropDownListFor(expression, selectList, optionLabel, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static ChosenDropDownList<TModel> DropDownListFor<TModel, TProperty>(this Bootstrap<TModel> bootstrap, Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, bootstrap.HtmlHelper.ViewData);
            return new ChosenDropDownList<TModel>(bootstrap.HtmlHelper, metadata, ExpressionHelper.GetExpressionText(expression), 
                selectList, optionLabel, htmlAttributes);
        }

        public static Label<TModel> LabelFor<TModel, TProperty>(this Bootstrap<TModel> bootstrap, Expression<Func<TModel, TProperty>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, bootstrap.HtmlHelper.ViewData);
            return new Label<TModel>(bootstrap.HtmlHelper, metadata, ExpressionHelper.GetExpressionText(expression));
        }

        public static MvcHtmlString HelloWorld<TModel>(this Bootstrap<TModel> bootstrap)
        {
            return new MvcHtmlString("Hello World!");
        }

        /// <summary>
        /// Bootstrap button
        /// </summary>
        public static Button<TModel> Button<TModel>(this Bootstrap<TModel> bootstrap, string caption, string href, string buttonType, string buttonSize, string icon)
        {
            return new Button<TModel>(bootstrap.HtmlHelper, caption, href, buttonType, buttonSize, icon);
        }

        public static Button<TModel> Button<TModel>(this Bootstrap<TModel> bootstrap, string caption, string href, string buttonType, string buttonSize)
        {
            return new Button<TModel>(bootstrap.HtmlHelper, caption, href, buttonType, buttonSize, null);
        }

        public static Button<TModel> Button<TModel>(this Bootstrap<TModel> bootstrap, string caption, string buttonType, string buttonSize)
        {
            return new Button<TModel>(bootstrap.HtmlHelper, caption, null, buttonType, buttonSize, null);
        }

        public static Button<TModel> Button<TModel>(this Bootstrap<TModel> bootstrap, string caption, string buttonType)
        {
            return bootstrap.Button(caption, buttonType, ButtonSize.Small);
        }

        public static Button<TModel> Button<TModel>(this Bootstrap<TModel> bootstrap, string caption)
        {
            return bootstrap.Button(caption, ButtonType.Default, ButtonSize.Small);
        }


        /// <summary>
        /// Bootstrap InfoBox
        /// </summary>
        public static InfoBox<TModel> InfoBox<TModel>(this Bootstrap<TModel> bootstrap)
        {
            return new InfoBox<TModel>(bootstrap.HtmlHelper);
        }

    }
}