using Microsoft.Extensions.DependencyInjection;
using Oragon.Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oragon.Spring.Extensions.DependencyInjection
{
    public class SpringServiceScope : IServiceScope
    {
        private AbstractApplicationContext applicationContext;

        public SpringServiceScope(AbstractApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public IServiceProvider ServiceProvider => this.applicationContext.GetObject<IServiceProvider>();

        public void Dispose()
        {
            
        }
    }
}
