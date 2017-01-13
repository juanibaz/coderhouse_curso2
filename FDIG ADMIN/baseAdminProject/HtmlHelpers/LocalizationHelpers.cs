using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nimok.Mvc.Extensions
{
	public static class LocalizationHelpers
	{
		public static IHtmlString MetaAcceptLanguage<t>(this HtmlHelper<t> html)
		{
			var acceptLanguage = HttpUtility.HtmlAttributeEncode(System.Threading.Thread.CurrentThread.CurrentUICulture.ToString());
			return new HtmlString(String.Format("<meta name=\"accept-language\" content=\"{0}\" >", acceptLanguage));
		}

		public static IHtmlString MetaBaseUrl<t>(this HtmlHelper<t> html)
		{
			var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
			var request = HttpContext.Current.Request;
			var baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, urlHelper.Content("~"));
			return new HtmlString(String.Format("<meta name=\"base-url\" content=\"{0}\" >", baseUrl));
		}
	}
}