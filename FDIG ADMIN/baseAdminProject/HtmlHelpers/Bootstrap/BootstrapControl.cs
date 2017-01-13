using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Nimok.Mvc.Bootstrap.Extensions
{


    /// <summary>
    /// Interface that is used to build fluent interfaces and hides methods declared by <see cref="object"/> from IntelliSense.
    /// </summary>
    /// <remarks>
    /// Code that consumes implementations of this interface should expect one of two things:
    /// <list type = "number">
    ///   <item>When referencing the interface from within the same solution (project reference), you will still see the methods this interface is meant to hide.</item>
    ///   <item>When referencing the interface through the compiled output assembly (external reference), the standard Object methods will be hidden as intended.</item>
    /// </list>
    /// See http://bit.ly/ifluentinterface for more information.
    /// </remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IFluentInterface
    {
        /// <summary>
        /// Redeclaration that hides the <see cref="object.GetType()"/> method from IntelliSense.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        Type GetType();

        /// <summary>
        /// Redeclaration that hides the <see cref="object.GetHashCode()"/> method from IntelliSense.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        int GetHashCode();

        /// <summary>
        /// Redeclaration that hides the <see cref="object.ToString()"/> method from IntelliSense.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        string ToString();

        /// <summary>
        /// Redeclaration that hides the <see cref="object.Equals(object)"/> method from IntelliSense.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        bool Equals(object obj);
    }

    public interface IBootstrapControl
    {
        MvcHtmlString Render();
    }

    public class BootstrapControl<TModel, TControl> : IBootstrapControl, IHtmlString, IFluentInterface where TControl : class
    {
        protected string classNames { get; set; }
        protected bool enabled;
        protected bool visible;
        protected string disabledIfOtherFieldIsNull;
        public IBootstrapControl ParentControl { get; set; }

        public BootstrapControl()
        {
            enabled = true;
            visible = true;
            classNames = "";
            disabledIfOtherFieldIsNull = null;
        }
        public string ToHtmlString()
        {
            return ToString();
        }

        public override string ToString()
        {
            if (ParentControl != null)
                return ParentControl.ToString();
            else
            {
                if (visible)
                    return Render().ToString();
                else
                    return String.Empty;
            }
                
        }

        public virtual MvcHtmlString Render()
        {
            return MvcHtmlString.Empty;
        }

        protected Dictionary<string, object> GetHtmlAttributes()
        {
            Dictionary<string, object> htmlAttributes = new Dictionary<string, object>();
            htmlAttributes.Add("class", classNames);
            if (!enabled)
                htmlAttributes.Add("disabled", "disabled");

            if (disabledIfOtherFieldIsNull != null)
                htmlAttributes.Add("data-nkdisabled-ifOtherFieldIsNull", disabledIfOtherFieldIsNull);
            return htmlAttributes;
        }
            
        // aplicar mismo patrón para id, class, data, etc.
        public TControl Class(string classNames)
        {
            this.classNames = classNames;
            return this as TControl;
        }

        public TControl Enabled(bool enabled)
        {
            this.enabled = enabled;
            return this as TControl;
        }
        public TControl Visible(bool visible)
        {
            this.visible = visible;
            return this as TControl;
        }

        public TControl DisabledIfOtherFieldIsNull<TProperty>(Expression<Func<TModel, TProperty>> otherField)
        {
            string fieldName = ExpressionHelper.GetExpressionText(otherField);
            // parentFieldName = HtmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            if (String.IsNullOrEmpty(fieldName))
                throw new Exception("Invalid other field expression");
            this.disabledIfOtherFieldIsNull = fieldName;
            return this as TControl;
        }

    }
}