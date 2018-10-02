using Microsoft.Extensions.DependencyInjection;
using Oragon.Spring.Collections;
using Oragon.Spring.Context;
using Oragon.Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oragon.Spring.Extensions.DependencyInjection
{
    /// <summary>
    /// Implements IServiceProvider and ISupportRequiredService for .NET Core Dependency Injection infrastructure.
    ///
    /// </summary>
    public class SpringServiceProvider : IServiceProvider, ISupportRequiredService
    {
        private readonly IApplicationContext container;
        private readonly ServiceProvider original;
        private readonly Type serviceScopeFactoryInterfaceType = typeof(IServiceScopeFactory);

        /// <summary>
        /// Create a new SpringServiceProvider
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurationLocations"></param>
        internal SpringServiceProvider(IServiceCollection services, string[] configurationLocations = null)
        {
            if (configurationLocations == null || configurationLocations.Length == 0)
            {
                configurationLocations = new[] { @".\AppContext.xml" };
            }

            container = new XmlApplicationContext(new XmlApplicationContextArgs()
            {
                CaseSensitive = true,
                Name = "root",
                ConfigurationLocations = configurationLocations,
                //ParentContext = services.BuildContainerFromServiceCollection(this)
            });

            services.AddScoped<IServiceScopeFactory>(it => new SpringServiceScopeFactory((SpringServiceProvider)it));
            original = services.BuildServiceProvider();
        }

        /// <summary>
		/// Gets service of type <paramref name="serviceType" /> from the <see cref="T:System.IServiceProvider" /> implementing
		/// this interface.
		/// </summary>
		/// <param name="serviceType">An object that specifies the type of service object to get.</param>
		/// <returns>A service object of type <paramref name="serviceType" />.
		/// Throws an exception if the <see cref="T:System.IServiceProvider" /> cannot create the object.</returns>
        public object GetRequiredService(Type serviceType)
        {
#if DEBUG

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"SpringServiceProvider.GetRequiredService({serviceType.ToString()})");
#endif
            object returnValue = GetService(serviceType);

            return returnValue ?? throw new NoElementsException();
        }


        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>A service object of type serviceType. -or- null if there is no service object</returns>
        public object GetService(Type serviceType)
        {
#if DEBUG
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"SpringServiceProvider.GetService({serviceType.ToString()})");
#endif
            object returnValue = GetServiceInternal(serviceType);

            if (returnValue == null)
            {
                IApplicationContext currentContext = container;
                IDictionary<string, object> itemFound = null;
                while ((itemFound == null || !itemFound.Any()) && currentContext != null)
                {
                    itemFound = container.GetObjectsOfType(serviceType);
                    currentContext = container.ParentContext;
                }
                returnValue = itemFound.Any() ? itemFound.Values.First() : returnValue;
            }

            return returnValue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        private Object GetServiceInternal(Type serviceType)
        {
            if (serviceType.IsAssignableFrom(serviceScopeFactoryInterfaceType))
            {
                return new SpringServiceScopeFactory(this);
            }
            else
            {
                return original.GetService(serviceType);
            }
        }
    }
}