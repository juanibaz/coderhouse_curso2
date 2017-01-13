using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nimok.Mvc.Extensions
{
    public static class FlashMessageExtensions
    {
        public const int defaultLife = 5000;

        public static ActionResult Error(this ActionResult result, string message, int life = defaultLife)
        {
            CreateCookieWithFlashMessage(Notification.Error, message, life);
            return result;
        }

        public static ActionResult Warning(this ActionResult result, string message, int life = defaultLife)
        {
            CreateCookieWithFlashMessage(Notification.Warning, message, life);
            return result;
        }

        public static ActionResult Success(this ActionResult result, string message, int life = defaultLife)
        {
            CreateCookieWithFlashMessage(Notification.Success, message, life);
            return result;
        }

        public static ActionResult Information(this ActionResult result, string message, int life = defaultLife)
        {
            CreateCookieWithFlashMessage(Notification.Info, message, life);
            return result;
        }

        private static void CreateCookieWithFlashMessage(Notification notification, string message, int life = defaultLife)
        {
            HttpContext.Current.Response.Cookies.Add(new HttpCookie(string.Format("Flash.{0}", notification), message) { Path = "/" });
            HttpContext.Current.Response.Cookies.Add(new HttpCookie(string.Format("Flash.{0}.life", notification), life.ToString()) { Path = "/" });
        }

        private enum Notification
        {
            Error,
            Warning,
            Success,
            Info
        }
    }
}