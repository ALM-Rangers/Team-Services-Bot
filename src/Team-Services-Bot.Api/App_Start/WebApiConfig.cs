﻿// ———————————————————————————————
// <copyright file="WebApiConfig.cs">
// Licensed under the MIT License. See License.txt in the project root for license information.
// </copyright>
// <summary>
// Contains the configuration for Web Api.
// </summary>
// ———————————————————————————————

namespace Vsar.TSBot
{
    using System;
    using System.Web.Http;
    using Autofac;
    using Autofac.Integration.WebApi;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// Class that bootstraps anything regarding Web Api.
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Bootstraps Web Api.
        /// </summary>
        /// <param name="config">A <see cref="HttpConfiguration"/>.</param>
        /// <param name="lifetimeScope"><see cref="ILifetimeScope"/> for autofac.</param>
        public static void Register(HttpConfiguration config, ILifetimeScope lifetimeScope)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (lifetimeScope == null)
            {
                throw new ArgumentNullException(nameof(lifetimeScope));
            }

            // Json settings
            config.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
            };

            // Web API configuration and services
            config.DependencyResolver = new AutofacWebApiDependencyResolver(lifetimeScope);

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });
        }
    }
}
