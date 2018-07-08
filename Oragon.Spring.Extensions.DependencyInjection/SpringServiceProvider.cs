using Microsoft.Extensions.DependencyInjection;
using Oragon.Spring.Collections;
using Oragon.Spring.Context;
using Oragon.Spring.Context.Support;
using System;
using System.Linq;

namespace Oragon.Spring.Extensions.DependencyInjection
{
    public class SpringServiceProvider : IServiceProvider, ISupportRequiredService
    {
        private readonly IApplicationContext container;
        private readonly ServiceProvider original;

        internal SpringServiceProvider(IServiceCollection services, string[] configurationLocations = null)
        {
            if (configurationLocations == null || configurationLocations.Length == 0)
            {
                configurationLocations = new[] { @".\AppContext.xml" };
            }

            this.container = new XmlApplicationContext(new XmlApplicationContextArgs()
            {
                CaseSensitive = true,
                Name = "root",
                ConfigurationLocations = configurationLocations,
                //ParentContext = services.BuildContainerFromServiceCollection(this)
            });

            services.AddScoped<IServiceScopeFactory>(it => new SpringServiceScopeFactory((SpringServiceProvider)it));
            this.original = services.BuildServiceProvider();
        }

        public object GetRequiredService(Type serviceType)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"SpringServiceProvider.GetRequiredService({serviceType.ToString()})");

            var returnValue = this.GetService(serviceType);

            return returnValue ?? throw new NoElementsException();
        }

        public object GetService(Type serviceType)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"SpringServiceProvider.GetService({serviceType.ToString()})");
            var returnValue = this.GetServiceInternal(serviceType);

            if (returnValue == null)
            {
                var itemFound = container.GetObjectsOfType(serviceType);
                if (itemFound.Any() == false)
                {
                    itemFound = container.ParentContext.GetObjectsOfType(serviceType);
                }
                returnValue = itemFound.Any() ? itemFound.Values.First() : returnValue;
            }

            return returnValue;
        }

        private Type serviceScopeFactoryInterfaceType = typeof(IServiceScopeFactory);
        private Object GetServiceInternal(Type serviceType)
        {
            if (serviceType.IsAssignableFrom(serviceScopeFactoryInterfaceType))
            {
                return new SpringServiceScopeFactory(this);
            }
            else
            {
                return this.original.GetService(serviceType);
            }
        }

    }
}
