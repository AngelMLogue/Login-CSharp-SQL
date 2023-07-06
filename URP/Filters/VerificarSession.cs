using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using URP.Controllers;
using URP.Models;

namespace URP.Filters
{
    public class VerificarSession : ActionFilterAttribute
    {
        private Users oUsuario;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                base.OnActionExecuting(filterContext);

                oUsuario = (Users)HttpContext.Current.Session["User"];
                if (oUsuario == null && filterContext.ActionDescriptor.ActionName.Equals("LogOn", StringComparison.OrdinalIgnoreCase) == false)
                {
                    if (filterContext.ActionDescriptor.ActionName.Equals("LogIn", StringComparison.OrdinalIgnoreCase) == false)
                    {
                        filterContext.HttpContext.Response.Redirect("~/Home/LogIn");
                    }
                }
                else if(oUsuario == null)
                {
                         if (filterContext.ActionDescriptor.ActionName.Equals("LogOn", StringComparison.OrdinalIgnoreCase) == false)
                        {
                            filterContext.HttpContext.Response.Redirect("~/Home/LogOn");
                        }
                }

            }
            catch (Exception)
            {
                filterContext.Result = new RedirectResult("~/Home/LogIn");
                throw;
            }
        }
    }
}