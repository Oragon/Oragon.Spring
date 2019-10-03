using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Oragon.Spring.Context.Support;

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
            foreach (var descriptor in services)
            {
                if (descriptor.ImplementationType != null)
                {
                    // Test if the an open generic type is being registered
                    var serviceTypeInfo = descriptor.ServiceType.GetTypeInfo();
                    if (serviceTypeInfo.IsGenericTypeDefinition)
                    {
                        builder
                            .RegisterGeneric(descriptor.ImplementationType)
                            .As(descriptor.ServiceType)
                            .ConfigureLifecycle(descriptor.Lifetime, lifetimeScopeTagForSingletons);
                    }
                    else
                    {
                        builder
                            .RegisterType(descriptor.ImplementationType)
                            .As(descriptor.ServiceType)
                            .ConfigureLifecycle(descriptor.Lifetime, lifetimeScopeTagForSingletons);
                    }
                }
                else if (descriptor.ImplementationFactory != null)
                {
                    var registration = RegistrationBuilder.ForDelegate(descriptor.ServiceType, (context, parameters) =>
                    {
                        var serviceProvider = context.Resolve<IServiceProvider>();
                        return descriptor.ImplementationFactory(serviceProvider);
                    })
                        .ConfigureLifecycle(descriptor.Lifetime, lifetimeScopeTagForSingletons)
                        .CreateRegistration();

                    builder.RegisterComponent(registration);
                }
                else
                {
                    builder
                        .RegisterInstance(descriptor.ImplementationInstance)
                        .As(descriptor.ServiceType)
                        .ConfigureLifecycle(descriptor.Lifetime, null);
                }
            }
        }

        private static void RegisterDefaults(IServiceCollection services, XmlApplicationContext applicationContext)
        {
            applicationContext.ObjectFactory.RegisterSingleton("ServiceProvider", new SpringServiceProvider(applicationContext));
            applicationContext.ObjectFactory.RegisterSingleton("SpringServiceScopeFactory", new SpringServiceScopeFactory(applicationContext));
        }

    }
}
