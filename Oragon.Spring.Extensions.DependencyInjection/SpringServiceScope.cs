using Microsoft.Extensions.DependencyInjection;
using System;

namespace Oragon.Spring.Extensions.DependencyInjection
{
    public class SpringServiceScope : IServiceScope
    {
        private SpringServiceProvider springServiceProvider;

        public SpringServiceScope(SpringServiceProvider springServiceProvider)
        {
            this.springServiceProvider = springServiceProvider;
        }

        public IServiceProvider ServiceProvider => this.springServiceProvider;

        public void Dispose()
        {
            this.springServiceProvider = null;
        }
    }
}
