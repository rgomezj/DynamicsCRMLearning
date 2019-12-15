using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Formatting;

namespace Pavliks.WAM.ManagementConsole.ManagementAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jsonFormatter.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

            config.Formatters.Clear();
            config.Formatters.Add(jsonFormatter);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "EventChildAction",
                routeTemplate: "api/event/{eventId}/{controller}/{action}");


            config.Routes.MapHttpRoute(
                name: "EventChilds",
                routeTemplate: "api/event/{eventId}/{controller}");

            config.Routes.MapHttpRoute(
                name: "SalesChildAction",
                routeTemplate: "api/salesorder/{salesOrderId}/{controller}/{action}");

            config.Routes.MapHttpRoute(
                name: "SalesChildId",
                routeTemplate: "api/salesorder/{salesOrderId}/{controller}");

            config.Routes.MapHttpRoute(
                name: "APIIdActionRegistration",
                routeTemplate: "api/registration/{registrationId}/{action}", defaults: new { controller = "Registration" } );

            config.Routes.MapHttpRoute(
               name: "APIIdActionOrderManagementItem",
               routeTemplate: "api/orderManagementItem/{action}", defaults: new { controller = "OrderManagementItem" });

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "APIIdAction",
                routeTemplate: "api/{controller}/{id}/{action}",
                defaults: new { id = RouteParameter.Optional }
            );


            config.Routes.MapHttpRoute(
                name: "NameSearch",
                routeTemplate: "api/{controller}/{name}"
            );
        }
    }
}
