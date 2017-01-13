using Microsoft.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Text;

namespace NavigationRoutes
{
    public class Defaults
    {
        public const string FILTER_TOKEN_KEY = "FilterToken";
    }

    public class CompositeMvcHtmlString : IHtmlString
    {
        readonly IEnumerable<IHtmlString> _strings;

        public CompositeMvcHtmlString(IEnumerable<IHtmlString> strings)
        {
            _strings = strings;
        }

        public string ToHtmlString()
        {
            return string.Join(string.Empty, _strings.Select(x => x.ToHtmlString()));
        }
    }

    public static class NavigationRouteFilters
    {
        public static List<INavigationRouteFilter> Filters = new List<INavigationRouteFilter>();
    }
    public static class NavigationViewExtensions
    {

        public static IHtmlString Navigation(this HtmlHelper helper)
        {
            return new CompositeMvcHtmlString(
                GetRoutesForCurrentRequest(RouteTable.Routes, NavigationRouteFilters.Filters)
                .GroupBy(route => route.NavigationGroup)
                .Select(routeGroup => helper.NavigationListItemRouteLink(routeGroup.Select(g => g))));
        }

        public static IEnumerable<NamedRoute> GetRoutesForCurrentRequest(RouteCollection routes, IEnumerable<INavigationRouteFilter> routeFilters)
        {
            var navigationRoutes = routes.OfType<NamedRoute>().Where(r => r.IsChild == false).ToList();
            if (routeFilters.Count() > 0)
            {
                foreach (var route in navigationRoutes.ToArray())
                {
                    foreach (var filter in routeFilters)
                    {
                        if (filter.ShouldRemove(route))
                        {
                            navigationRoutes.Remove(route);
                            break;
                        }
                    }
                }
            }
            return navigationRoutes;
        }

        public static MvcHtmlString NavigationListItemRouteLink(this HtmlHelper html, IEnumerable<NamedRoute> routes)
        {
            var ul = new TagBuilder("ul");
            ul.AddCssClass("nav nav-list");

            var namedRoutes = routes as IList<NamedRoute> ?? routes.ToList();
            if (namedRoutes.Any(r => r.Options.IsRightAligned))
            {
                ul.AddCssClass("pull-right");
            }

            var tagBuilders = new List<TagBuilder>();

            foreach (var route in namedRoutes)
            {
                var li = new TagBuilder("li");
                if (route.Options.IsRightAligned)
                {
                    li.AddCssClass("pull-right");
                }

                if (CurrentRouteMatchesName(html, route.Name))
                {
                    li.AddCssClass("active");
                }

                if (route.Children.Any())
                {
                    BuildChildMenu(html, route, li);
                }
                else
                {
                    string caption = "";
                    if (route.Options.Icon != null)
                        caption = "<i class=\"" + route.Options.Icon + "\"></i>";
                    caption = html.RouteLink("#", route.Name).ToString().Replace("#", caption + route.DisplayName);
                    li.InnerHtml = caption;                
                }

                tagBuilders.Add(li);
                if (route.Options.HasBreakAfter)
                {
                    var breakLi = new TagBuilder("li");
                    breakLi.AddCssClass("divider-vertical");
                    tagBuilders.Add(breakLi);
                }

            }
            var tags = new StringBuilder();
            tagBuilders.ForEach(b => tags.Append(b.ToString(TagRenderMode.Normal)));
            ul.InnerHtml = tags.ToString();

            return MvcHtmlString.Create(ul.ToString(TagRenderMode.Normal));
        }

        private static void BuildChildMenu(HtmlHelper html, NamedRoute route, TagBuilder li)
        {
            // build LIs for the children
            var ul = new TagBuilder("ul");
            ul.AddCssClass("submenu");

            bool aChildIsActive = false;

            foreach (var child in route.Children)
            {
                var childLi = new TagBuilder("li");
                string childAHRef = html.RouteLink("#", child.Name).ToString();
                childAHRef = childAHRef.Replace("#", "<i class=\"icon-double-angle-right\"></i> " + child.DisplayName);
                childLi.InnerHtml = childAHRef;
                
                if (CurrentRouteMatchesName(html, child.Name))
                {
                    aChildIsActive = true;
                    childLi.AddCssClass("active");
                }
                
                // add child item to submenu UL
                ul.InnerHtml += childLi.ToString();

                // support for drop down list breaks
                if (child.Options.HasBreakAfter)
                {
                    var breakLi = new TagBuilder("li");
                    breakLi.AddCssClass("divider");
                    ul.InnerHtml += breakLi.ToString();
                }
            }

            var ahref = new TagBuilder("a");
            ahref.AddCssClass("dropdown-toggle");
            if (route.Options.Icon != null) 
                ahref.InnerHtml += "<i class=\"" + route.Options.Icon + "\"></i>";
            ahref.InnerHtml += route.DisplayName + "<b class='arrow icon-angle-down'></b>";

            if (aChildIsActive)
            {
                li.AddCssClass("open");
                li.AddCssClass("active"); //?
            }
            li.InnerHtml = ahref + ul.ToString();
        }


        static bool CurrentRouteMatchesName(HtmlHelper html, string routeName)
        {
            //var namedRoute = html.ViewContext.RouteData.Route as NamedRoute;
            var namedRoute = html.ViewContext.RequestContext.RouteData.Route as NamedRoute;
            if (namedRoute != null)
            {
                if (string.Equals(routeName, namedRoute.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }

    public static class RouteValueDictionaryExtensions
    {
        public static string FilterToken(this RouteValueDictionary routeValues)
        {
            return (string)routeValues[Defaults.FILTER_TOKEN_KEY];
        }

        public static bool HasFilterToken(this RouteValueDictionary routeValues)
        {
            return routeValues.ContainsKey(Defaults.FILTER_TOKEN_KEY);
        }
    }
}