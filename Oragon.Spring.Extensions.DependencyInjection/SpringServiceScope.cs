using Microsoft.Extensions.DependencyInjection;
using System;

namespace Oragon.Spring.Extensions.DependencyInjection
{
    /// <summary>
    /// SpringServiceScope implement a fake IServiceScope returning always the same service provider SpringServiceProvider
    /// </summary>
    public class SpringServiceScope : IServiceScope
    {
        private SpringServiceProvider springServiceProvider;

        /// <summary>
        /// Create a new instance of SpringServiceScope
        /// </summary>
        /// <param name="springServiceProvider"></param>
        public SpringServiceScope(SpringServiceProvider springServiceProvider)
        {
            this.springServiceProvider = springServiceProvider;
        }

        /// <summary>
        /// Return a SpringServiceProvider as scoped IServiceProvider 
        /// </summary>
        public IServiceProvider ServiceProvider => this.springServiceProvider;

        /// <summary>
        /// Dispose scope
        /// </summary>
        public void Dispose()
        {
            this.springServiceProvider = null;
        }
    }
}
