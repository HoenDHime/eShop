using Castle.Windsor;
using Castle.Windsor.Installer;
using E_shop.Installer;
using E_shop.Utility;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace E_shop
{
    // Примечание: Инструкции по включению классического режима IIS6 или IIS7 
    // см. по ссылке http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "PaymentSystems",
                "PaymentSystems/{action}",
                new { controller = "PaymentSystem", action = "Index" }
                );

            routes.MapRoute(
                "Default", // Имя маршрута
                "{controller}/{action}/{id}", // URL-адрес с параметрами
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Параметры по умолчанию
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // Использовать LocalDB для Entity Framework по умолчанию
            Database.DefaultConnectionFactory = new SqlConnectionFactory(@"Data Source=(localdb)\v11.0; Integrated Security=True; MultipleActiveResultSets=True");

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            BootstrapContainer();
        }

        private static IWindsorContainer container;

        private static void BootstrapContainer()
        {
            container = new WindsorContainer()
                .Install(FromAssembly.This());
            var controllerFactory = new WindsorControllerFactory(container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }

        //public override string GetVaryByCustomString(HttpContext context, string arg)
        //{
        //    // It seems this executes multiple times and early, so we need to extract language again from cookie.
        //    if (arg == "culture") // culture name (e.g. "en-US") is what should vary caching
        //    {
        //        string cultureName = null;
        //        // Attempt to read the culture cookie from Request
        //        HttpCookie cultureCookie = Request.Cookies["_culture"];
        //        if (cultureCookie != null)
        //            cultureName = cultureCookie.Value;
        //        else
        //            cultureName = Request.UserLanguages[0]; // obtain it from HTTP header AcceptLanguages

        //        // Validate culture name
        //        cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe

        //        return cultureName.ToLower();// use culture name as cache key, "es", "en-us", "es-cl", etc.
        //    }

        //    return base.GetVaryByCustomString(context, arg);
        //}

        public void X(string culture) 
        {
            if (culture == "culture")
            {
                string cultureName = null;

                HttpCookie cultureCookie = Request.Cookies["_culture"];
                if (cultureCookie != null)
                    cultureName = cultureCookie.Value;
                else
                    cultureName = Request.UserLanguages[0]; 

                // Validate culture name
                cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe

                //return cultureName.ToLower();// use culture name as cache key, "es", "en-us", "es-cl", etc.
            }

        }
    }
}