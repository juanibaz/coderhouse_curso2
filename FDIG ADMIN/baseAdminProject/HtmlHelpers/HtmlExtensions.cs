using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Linq.Expressions;

namespace Nimok.Mvc.Extensions
{
    public static class HtmlExtensions
    {
        /// <summary>
        /// Retorna DropDownList. Establece si DropDownList es editable o no.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <param name="selectList"></param>
        /// <param name="optionText"></param>
        /// <param name="canEdit"></param>
        /// <returns></returns>
        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionText, bool canEdit)
        {
            if (canEdit) return html.DropDownListFor(expression, selectList, optionText);
            return html.DropDownListFor(expression, selectList, optionText, new { @disabled = true });
        }

        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionText, object htmlAttributes, bool canEdit)
        {
            if (canEdit) return html.DropDownListFor(expression, selectList, optionText, htmlAttributes);
            return html.DropDownListFor(expression, selectList, optionText, new { @disabled = true });
        }

        /// <summary>
        /// Retorna TextBox. Establece si TextBox es editable o no.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="canEdit"></param>
        /// <returns></returns>
        public static MvcHtmlString TextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes, bool canEdit)
        {
            if (canEdit) return htmlHelper.TextBoxFor(expression, htmlAttributes);
            return htmlHelper.TextBoxFor(expression, new { @disabled = true });
        }
    }
}