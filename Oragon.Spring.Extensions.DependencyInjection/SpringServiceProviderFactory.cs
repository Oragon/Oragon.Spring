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
            if (configurationLocations == null || configurationLocations.Length == 0)
            {
                this.configurationLocations = new[] { @".\AppContext.xml" };
            }
            else
            {
                this.configurationLocations = configurationLocations;
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

            ServiceProviderAdapter.Adapt(services, applicationContext);

           
            //applicationContext.ObjectFactory.RegisterSingleton("ServiceProvider", services.BuildServiceProvider());

            return applicationContext;
        }

        public IServiceProvider CreateServiceProvider(XmlApplicationContext applicationContext) =>
            applicationContext.GetObject<ServiceProvider>("ServiceProvider");





    }
}
