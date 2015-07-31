using System;
using System.Linq;
using System.Web.Http;

namespace SMEasy
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            if (config == null) return;
            //var json = config.Formatters.JsonFormatter;
            //json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            //config.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            //config.Formatters.Remove(config.Formatters.XmlFormatter);

            config.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional }
        );

            config.Routes.MapHttpRoute(
                    name: "DefaultApi1",
                    routeTemplate: "api/{controller}/{action}/{defaultValue}",
                    defaults: new { defaultValue = RouteParameter.Optional }
                );

            config.Routes.MapHttpRoute(
               name: "DefaultApiWithActionSearch",
               routeTemplate: "api/{controller}/{action}/{searchString}",
               defaults: new { searchString = RouteParameter.Optional }
           );

        }
    }
}
