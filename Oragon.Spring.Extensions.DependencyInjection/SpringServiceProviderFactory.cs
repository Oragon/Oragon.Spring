using Microsoft.Extensions.DependencyInjection;
using Oragon.Spring.Context;
using Oragon.Spring.Context.Support;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Oragon.Spring.Objects.Factory.Support;

namespace Oragon.Spring.Extensions.DependencyInjection
{
    public class SpringServiceProviderFactory : IServiceProviderFactory<XmlApplicationContext>
    {
        private readonly string[] configurationLocations;

        public SpringServiceProviderFactory(params string[] configurationLocations)
        {
            this.configurationLocations = configurationLocations;

            if (configurationLocations.Length == 0 || configurationLocations == null)
            {
                this.configurationLocations = new[] { @".\AppContext.xml" };
            }

        }

        public XmlApplicationContext CreateBuilder(IServiceCollection services)
        {
            XmlApplicationContext applicationContext = new XmlApplicationContext(new XmlApplicationContextArgs()
            {
                CaseSensitive = true,
                Name = "root",
                ConfigurationLocations = this.configurationLocations,
            });

            this.Adapt(services, applicationContext);

            applicationContext.ObjectFactory.RegisterSingleton("ServiceProvider", services.BuildServiceProvider());

            return applicationContext;
        }

        public IServiceProvider CreateServiceProvider(XmlApplicationContext applicationContext) =>
            applicationContext.GetObject<ServiceProvider>("ServiceProvider");




        private void Adapt(IServiceCollection services, XmlApplicationContext applicationContext)
        {
            string[] blackList = new[] { "ServiceProvider", "messageSource" };

            IDictionary<string, object> rootObjects = applicationContext.GetObjectsOfType(typeof(object));

            foreach (var item in rootObjects.Where(it => !blackList.Contains(it.Key)))
            {
                Type objectType = item.Value.GetType();
                
                services.AddTransient(objectType, sp => item.Value);

                Type[] interfaces = objectType.GetInterfaces();

                if (interfaces != null && interfaces.Length > 0)
                {

                    foreach (var serviceType in interfaces)
                    {

                        services.AddTransient(serviceType, sp => item.Value);

                    }

                }
            }
        }

    }
}
