using Microsoft.Extensions.DependencyInjection;
using System;
using Oragon.Spring.Context.Support;
using System.Collections.Generic;
using System.Text;

namespace Oragon.Spring.Extensions.DependencyInjection
{
    public static class Extensions
    {
        public static IServiceProvider WithSpring(this IServiceCollection services, params string[] configurationLocations)
        {
            return new SpringServiceProvider(services, configurationLocations);
        }

        //public static StaticApplicationContext BuildContainerFromServiceCollection(this IServiceCollection services, SpringServiceProvider serviceProvider)
        //{
        //    StaticApplicationContext configContext = new StaticApplicationContext();
        //    configContext.DefaultListableObjectFactory.RegisterSingleton("SpringServiceProvider", serviceProvider);

        //    int seq = 1;
        //    foreach (var descriptor in services)
        //    {
        //        Type type = descriptor.ImplementationType ?? descriptor.ServiceType;
        //        string registrationName = $"DEFAULT:{type.ToString()}:{seq++}";
        //        string factoryName = registrationName + ":Factory";

        //        if (descriptor.ImplementationInstance != null)
        //        {
        //            configContext.DefaultListableObjectFactory.RegisterSingleton(registrationName, descriptor.ImplementationInstance);

        //            Console.ForegroundColor = ConsoleColor.Magenta;
        //        }
        //        else if (descriptor.ImplementationFactory != null)
        //        {

        //            //Configure Factory
        //            configContext.DefaultListableObjectFactory.RegisterSingleton(factoryName, descriptor.ImplementationFactory);

        //            Spring.Objects.Factory.Support.RootObjectDefinition objectDefinition = new Spring.Objects.Factory.Support.RootObjectDefinition
        //            {
        //                ObjectType = type,
        //                IsSingleton = descriptor.Lifetime == ServiceLifetime.Singleton,
        //                FactoryMethodName = "Invoke",
        //                FactoryObjectName = factoryName,
        //            };
        //            objectDefinition.ConstructorArgumentValues.AddNamedArgumentValue("arg", serviceProvider);

        //            //Add Instance dependent of Factory
        //            configContext.RegisterObjectDefinition(registrationName, objectDefinition);

        //            Console.ForegroundColor = ConsoleColor.Yellow;
        //        }
        //        else
        //        {                    

        //            Spring.Objects.Factory.Support.RootObjectDefinition objectDefinition = new Spring.Objects.Factory.Support.RootObjectDefinition
        //            {
        //                ObjectType = type,
        //                IsSingleton = descriptor.Lifetime == ServiceLifetime.Singleton,
        //                AutowireMode = Objects.Factory.Config.AutoWiringMode.AutoDetect,
        //            };
        //            configContext.RegisterObjectDefinition(registrationName, objectDefinition);

        //            Console.ForegroundColor = ConsoleColor.Blue;
        //        }

        //        Console.WriteLine($"" + Environment.NewLine +
        //            $"ServiceType: {descriptor.ServiceType}" + Environment.NewLine +
        //            $"ImplementationFactory: {descriptor.ImplementationFactory}" + Environment.NewLine +
        //            $"ImplementationInstance: {descriptor.ImplementationInstance}" + Environment.NewLine +
        //            $"ImplementationType: {descriptor.ImplementationType}" + Environment.NewLine +
        //            $"Lifetime: {descriptor.Lifetime}" + Environment.NewLine +
        //            $"");
        //    }
        //    return configContext;
        //}


    }
}
