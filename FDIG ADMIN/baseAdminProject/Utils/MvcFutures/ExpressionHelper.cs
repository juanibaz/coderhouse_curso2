﻿namespace Microsoft.Web.Mvc.Internal
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Microsoft.Web.Resources;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class ExpressionHelper
    {
        public static RouteValueDictionary GetRouteValuesFromExpression<TController>(Expression<Action<TController>> action) where TController : Controller
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            MethodCallExpression call = action.Body as MethodCallExpression;
            if (call == null)
            {
                throw new ArgumentException(MvcResources.ExpressionHelper_MustBeMethodCall, "action");
            }

            string controllerName = typeof(TController).Name;
            if (!controllerName.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException(MvcResources.ExpressionHelper_TargetMustEndInController, "action");
            }
            controllerName = controllerName.Substring(0, controllerName.Length - "Controller".Length);
            if (controllerName.Length == 0)
            {
                throw new ArgumentException(MvcResources.ExpressionHelper_CannotRouteToController, "action");
            }

            // TODO: How do we know that this method is even web callable?
            //      For now, we just let the call itself throw an exception.

            var rvd = new RouteValueDictionary();
            rvd.Add("Controller", controllerName);
            rvd.Add("Action", call.Method.Name);
            AddParameterValuesFromExpressionToDictionary(rvd, call);
            return rvd;
        }

        static void AddParameterValuesFromExpressionToDictionary(RouteValueDictionary rvd, MethodCallExpression call)
        {
            ParameterInfo[] parameters = call.Method.GetParameters();

            if (parameters.Length > 0)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    Expression arg = call.Arguments[i];
                    object value = null;
                    ConstantExpression ce = arg as ConstantExpression;
                    if (ce != null)
                    {
                        // If argument is a constant expression, just get the value
                        value = ce.Value;
                    }
                    else
                    {
                        // Otherwise, convert the argument subexpression to type object,
                        // make a lambda out of it, compile it, and invoke it to get the value
                        Expression<Func<object>> lambdaExpression = Expression.Lambda<Func<object>>(Expression.Convert(arg, typeof(object)));
                        Func<object> func = lambdaExpression.Compile();
                        value = func();
                    }
                    rvd.Add(parameters[i].Name, value);
                }
            }
        }

    }
}