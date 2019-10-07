using Microsoft.Extensions.DependencyInjection;
using Oragon.Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oragon.Spring.Extensions.DependencyInjection
{
    public class SpringServiceScopeFactory : IServiceScopeFactory
    {
        private AbstractApplicationContext applicationContext;

        public SpringServiceScopeFactory(AbstractApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public IServiceScope CreateScope()
        {
            return new SpringServiceScope(this.applicationContext);
        }
    }
}
