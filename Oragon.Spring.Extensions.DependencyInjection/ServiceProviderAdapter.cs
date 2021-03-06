﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Oragon.Spring.Context.Support;
using Oragon.Spring.Objects.Factory.Support;

namespace Oragon.Spring.Extensions.DependencyInjection
{
    public static class ServiceProviderAdapter
    {
        static string[] nameBlackList = new[] { "ServiceProvider", "messageSource" };

        static Type[] typeBlackList = new[] { typeof(IServiceProvider), typeof(ISupportRequiredService), typeof(IServiceScopeFactory) };

        internal static void Adapt(IServiceCollection services, AbstractApplicationContext applicationContext, OptionsBuilder config)
        {
            RegisterServices(services, applicationContext);

            RegisterDefaults(services, applicationContext);

            //RegisterBridges(applicationContext, config);
        }

        public static void RegisterBridges(AbstractApplicationContext applicationContext, OptionsBuilder config, IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();

            foreach (var replica in config.Replications)
            {
                if (replica.Singleton)
                {
                    var singletonInstance = serviceProvider.GetService(replica.Type);

                    applicationContext.ObjectFactory.RegisterSingleton(replica.Name, singletonInstance);
                }
                else
                {
                    throw new NotImplementedException("Non singleton replica is not working yet");
                }
            }
        }

        private static void RegisterServices(IServiceCollection services, AbstractApplicationContext applicationContext)
        {
            foreach (var definition in applicationContext.ObjectFactory.GetObjectDefinitionNames().Select(it => new { Name = it, Definition = applicationContext.ObjectFactory.GetObjectDefinition(it) }))
            {
                List<Type> types = new List<Type>();
                var interfaces = definition.Definition.ObjectType.GetInterfaces();
                if (interfaces.Any())
                {
                    types.AddRange(interfaces);
                }

                Type currentType = definition.Definition.ObjectType;
                do
                {
                    types.Add(currentType);
                    currentType = currentType.BaseType;

                } while (currentType != typeof(object));


                foreach (var type in types)
                {
                    services.AddTransient(type, (sp) =>
                    {
                        return applicationContext.GetObject(definition.Name);
                    });
                }
            }

        }

        private static void RegisterDefaults(IServiceCollection services, AbstractApplicationContext applicationContext)
        {
            applicationContext.ObjectFactory.RegisterSingleton("ServiceProvider", new SpringServiceProvider(applicationContext, services));
            applicationContext.ObjectFactory.RegisterSingleton("SpringServiceScopeFactory", new SpringServiceScopeFactory(applicationContext));
        }

        private static string BuildName(System.Type objectType)
        {
            string returnValue = $"{objectType.Namespace}.{objectType.Name}";

            if (objectType.IsGenericTypeDefinition)
            {
                returnValue += $"<{ string.Join(", ", objectType.GetGenericArguments().Select(BuildName).ToArray())}>";
                ///Console.WriteLine("");
            }
            if (objectType.IsGenericParameter)
            {
                returnValue += $"<{ string.Join(", ", objectType.DeclaringType.GetGenericArguments().Select(objectType => $"{objectType.Namespace}.{objectType.Name}").ToArray())}>";
                //Console.WriteLine("");
            }
            if (objectType.IsGenericType && objectType.GenericTypeArguments.Any())
            {
                returnValue += $"<{ string.Join(", ", objectType.GenericTypeArguments.Select(objectType => $"{objectType.Namespace}.{objectType.Name}").ToArray())}>";
                //Console.WriteLine("");
            }
            return returnValue;
        }

    }
}
