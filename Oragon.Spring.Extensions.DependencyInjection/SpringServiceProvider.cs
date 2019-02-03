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
        private readonly IApplicationContext spring;
        private readonly IServiceProvider original;
        private readonly IServiceScopeFactory originalEngine;
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

            this.spring = new XmlApplicationContext(new XmlApplicationContextArgs()
            {
                CaseSensitive = true,
                Name = "root",
                ConfigurationLocations = configurationLocations,
                //ParentContext = services.BuildContainerFromServiceCollection(this)
            });

            services.AddScoped<IServiceScopeFactory>(myself =>
            {
                SpringServiceProvider springServiceProvider = (SpringServiceProvider)myself;
                SpringServiceScopeFactory springServiceScopeFactory = new SpringServiceScopeFactory(springServiceProvider);
                return springServiceScopeFactory;
            });
            this.original = services.BuildServiceProvider();

            this.originalEngine = this.GetPrivateField<IServiceScopeFactory>(this.original, "_engine");
        }


        internal SpringServiceProvider(SpringServiceProvider parent, IServiceProvider scopedServiceProvider)
        {
            this.spring = parent.spring;
            this.original = scopedServiceProvider;
            this.originalEngine = parent.originalEngine;
        }



        private TResult GetPrivateField<TResult>(object target, string fieldName)
        {
            if (target == null)
                return default(TResult);

            var field = target.GetType().GetField(fieldName, System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            TResult result = (TResult)Spring.Reflection.Dynamic.DynamicReflectionManager.CreateFieldGetter(field).Invoke(target);
            return result;
        }


        internal IServiceScopeFactory OriginalScopeFactory => this.originalEngine;


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
                IApplicationContext currentContext = spring;
                IDictionary<string, object> itemFound = null;
                while ((itemFound == null || !itemFound.Any()) && currentContext != null)
                {
                    itemFound = spring.GetObjectsOfType(serviceType);
                    currentContext = spring.ParentContext;
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