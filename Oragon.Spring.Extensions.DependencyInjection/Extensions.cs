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
        /// Integrate with Spring.NET creating SpringServiceProvider as a .NET Core Service Provider
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurationLocations"></param>
        /// <returns>Returns a new SpringServiceProvider</returns>
        public static IHostBuilder ConfigureOragonSpring(this IHostBuilder hostBuilder, params string[] configurationLocations)
            =>
            hostBuilder.UseServiceProviderFactory(new SpringServiceProviderFactory(configurationLocations));

        


    }
}
