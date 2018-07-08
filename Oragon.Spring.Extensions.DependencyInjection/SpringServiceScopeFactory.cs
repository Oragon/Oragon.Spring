using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oragon.Spring.Extensions.DependencyInjection
{
    public class SpringServiceScopeFactory : IServiceScopeFactory
    {
        private SpringServiceProvider springServiceProvider;

        internal SpringServiceScopeFactory(SpringServiceProvider springServiceProvider)
        {
            this.springServiceProvider = springServiceProvider;
        }


        public IServiceScope CreateScope()
        {
            return new SpringServiceScope(this.springServiceProvider);
        }
    }
}
