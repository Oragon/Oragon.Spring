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
        private OptionsBuilder config;

        public SpringServiceProviderFactory(Action<OptionsBuilder> configFunc)
        {
            var memConfig = new OptionsBuilder();
            configFunc?.Invoke(memConfig);

            if (memConfig.ConfigLocations.Any() == false)
            {
                memConfig.AddConfigLocation(@".\Oragon.Spring.xml");
            }

            this.config = memConfig;
        }

        public XmlApplicationContext CreateBuilder(IServiceCollection services)
        {
            var rootApplicationContext = new CodeConfigApplicationContext(caseSensitive: true);

            ServiceProviderAdapter.RegisterBridges(rootApplicationContext, this.config, services);

            var applicationContext = new XmlApplicationContext(new XmlApplicationContextArgs()
            {
                CaseSensitive = true,
                Name = "root",
                ConfigurationLocations = this.config.ConfigLocations.AsEnumerable(),
                ParentContext = rootApplicationContext
            });

            ServiceProviderAdapter.Adapt(services, applicationContext, this.config);

            return applicationContext;
        }

        public IServiceProvider CreateServiceProvider(XmlApplicationContext applicationContext) =>
            applicationContext.GetObject<IServiceProvider>("ServiceProvider");





    }
}
