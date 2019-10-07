using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Oragon.Spring.Extensions.DependencyInjection
{

    /// <summary>
    /// Extensions methods for adapt .NET Core Dependency Injection model to Spring.NET model
    /// </summary>
    public static class Extensions
    {

        /// <summary>
        /// Setup Oragon Spring as Fallback ServiceProvider as a ASP.NET Core Service Provider
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurationLocations"></param>
        /// <returns>Returns a new SpringServiceProvider</returns>
        public static IHostBuilder ConfigureOragonSpring(this IHostBuilder hostBuilder, Action<OptionsBuilder> configFunc = null)
            =>
            hostBuilder.UseServiceProviderFactory(new SpringServiceProviderFactory(configFunc));




    }
}
