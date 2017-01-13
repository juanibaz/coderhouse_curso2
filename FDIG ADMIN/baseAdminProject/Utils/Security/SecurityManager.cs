using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nimok.Mvc.Security
{
    public class SecurityManager
    {
        private static SecurityManager securityManager = null;
        public static SecurityManager Current { 
            get {
                return securityManager ?? (securityManager = new SecurityManager());
            } 
        }
        
        public bool IsAuthorized(string actionName, string controllerName)
        {
            string username = HttpContext.Current.User.Identity.Name;

            // todo:
            //using (var man = new CRIA.Business.BAL.Manager())
            //{
            //    return man.DynamicAuthorizationManager.Validate(username, controllerName, actionName);
            //}

            return true;
        }



    }
}