﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace GeoDataService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            //TODO: try this:https://stackoverflow.com/questions/3409598/need-net-code-to-execute-only-when-in-debug-configuration
            //var cors = new EnableCorsAttribute("http://localhost:59823/", "*", "*");
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);


            //see this for more info: https://lostechies.com/jimmybogard/2012/04/18/custom-errors-and-error-detail-policy-in-asp-net-web-api/
            //since changing anything in DIT is so difficult, we won't make this configurable
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
