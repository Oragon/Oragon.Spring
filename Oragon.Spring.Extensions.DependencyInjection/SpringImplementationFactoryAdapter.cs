using Microsoft.Extensions.DependencyInjection;
using Oragon.Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oragon.Spring.Extensions.DependencyInjection
{
    public class SpringImplementationFactoryAdapter
    {
        private readonly XmlApplicationContext container;
        private readonly ServiceDescriptor serviceDescriptor;

        public SpringImplementationFactoryAdapter(XmlApplicationContext container, ServiceDescriptor serviceDescriptor)
        {
            this.container = container;
            this.serviceDescriptor = serviceDescriptor;
        }

        public object GetObject()
        {
            object returnValue = this.serviceDescriptor.ImplementationFactory(container.GetObject<IServiceProvider>());
            return returnValue;
        }
    }
}
