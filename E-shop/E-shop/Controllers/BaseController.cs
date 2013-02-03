using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using E_shop;
using E_shop.Utility;

namespace E_shop.Controllers
{   
    public class BaseController : Controller
    {
        //Определение текущей культуры
        //Determination of the current culture
        protected override void ExecuteCore()
        {
            try
            {
                string cultureName = null;
                // Attempt to read the culture cookie from Request
                HttpCookie cultureCookie = Request.Cookies["_culture"];
                if (cultureCookie != null)
                    cultureName = cultureCookie.Value;
                else
                    cultureName = Request.UserLanguages[0]; // obtain it from HTTP header AcceptLanguages

                // Validate culture name
                cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe

                // Modify current thread's cultures     
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

                base.ExecuteCore();
            }
            catch (Exception) {  }
        }
    }
}
