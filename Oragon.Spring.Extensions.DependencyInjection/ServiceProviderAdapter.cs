using System;
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

        internal static void Adapt(IServiceCollection services, XmlApplicationContext applicationContext)
        {
            RegisterServices(services, applicationContext);

            RegisterDefaults(services, applicationContext);
        }

        private static void RegisterServices(IServiceCollection services, XmlApplicationContext applicationContext)
        {
            Dictionary<string, List<ServiceDescriptor>> itemsAdded = new Dictionary<string, List<ServiceDescriptor>>();

            foreach (var descriptor in services)
            {
                if (descriptor.ImplementationType != null)
                {
                    var name = BuildName(descriptor.ImplementationType);

                    if (itemsAdded.ContainsKey(name))
                        itemsAdded[name].Add(descriptor);
                    else
                        itemsAdded.Add(name, new List<ServiceDescriptor>() { descriptor });


                    GenericObjectDefinition def = new GenericObjectDefinition();
                    def.ObjectType = descriptor.ImplementationType;
                    def.IsSingleton = false;
                    def.AutowireMode = Objects.Factory.Config.AutoWiringMode.Constructor;
                    applicationContext.ObjectFactory.RegisterObjectDefinition($"{name}#{itemsAdded[name].Count}", def);

                    Console.WriteLine($"SPRING A | {name}");
                    //Console.WriteLine("");
                }
                else if (descriptor.ImplementationFactory != null)
                {
                    var name = BuildName(descriptor.ServiceType);
                    string FactoryObjectName = $"{name}#Factory";

                    if (itemsAdded.ContainsKey(name))
                        itemsAdded[name].Add(descriptor);
                    else
                        itemsAdded.Add(name, new List<ServiceDescriptor>() { descriptor });

                    applicationContext.ObjectFactory.RegisterSingleton(FactoryObjectName, new SpringImplementationFactoryAdapter(applicationContext, descriptor));


                    GenericObjectDefinition definition = new GenericObjectDefinition();
                    definition.ObjectType = descriptor.ServiceType;
                    definition.IsSingleton = descriptor.Lifetime == ServiceLifetime.Singleton;
                    definition.FactoryObjectName = FactoryObjectName;
                    definition.FactoryMethodName = "GetObject";
                    applicationContext.ObjectFactory.RegisterObjectDefinition($"{name}#{itemsAdded[name].Count}", definition);

                    Console.WriteLine($"SPRING B | {name}");
                    //Console.WriteLine("");
                }
                else
                {
                    var name = BuildName(descriptor.ImplementationInstance.GetType());

                    if (itemsAdded.ContainsKey(name))
                        itemsAdded[name].Add(descriptor);
                    else
                        itemsAdded.Add(name, new List<ServiceDescriptor>() { descriptor });

                    
                    applicationContext.ObjectFactory.RegisterSingleton($"{name}#{itemsAdded[name].Count}", descriptor.ImplementationInstance);

                    

                    Console.WriteLine($"SPRING C | {name}");
                    //Console.WriteLine("");
                }
            }
        }

        private static void RegisterDefaults(IServiceCollection services, XmlApplicationContext applicationContext)
        {
            applicationContext.ObjectFactory.RegisterSingleton("ServiceProvider", new SpringServiceProvider(applicationContext));
            applicationContext.ObjectFactory.RegisterSingleton("SpringServiceScopeFactory", new SpringServiceScopeFactory(applicationContext));
        }

        private static string BuildName(System.Type objectType)
        {
            string returnValue = $"{objectType.Namespace}.{objectType.Name}";

            if (objectType.IsGenericTypeDefinition)
            {
                returnValue += $"<{ string.Join(", ", objectType.GetGenericArguments().Select(BuildName).ToArray())}>";
                //var x3 = $"                                         <{ string.Join(", ", objectType.GetGenericTypeDefinition().GetGenericArguments().Select(BuildName).ToArray())}>";
                //var x4 = $"                                         <{ string.Join(", ", objectType.GetGenericTypeDefinition().GetGenericParameterConstraints().Select(BuildName).ToArray())}>";
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
