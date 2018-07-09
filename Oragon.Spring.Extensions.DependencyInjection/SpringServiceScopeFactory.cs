using Microsoft.Extensions.DependencyInjection;

namespace Oragon.Spring.Extensions.DependencyInjection
{
    /// <summary>
    /// Implements a IServiceScopeFactory to create a scoped Spring.NET Container
    /// </summary>
    public class SpringServiceScopeFactory : IServiceScopeFactory
    {
        private SpringServiceProvider springServiceProvider;

        /// <summary>
        /// Create new instance of SpringServiceScopeFactory using a static SpringServiceProvider
        /// </summary>
        /// <param name="springServiceProvider"></param>
        internal SpringServiceScopeFactory(SpringServiceProvider springServiceProvider)
        {
            this.springServiceProvider = springServiceProvider;
        }

        // <summary>
        /// Create an <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceScope" /> which
        /// contains an <see cref="T:System.IServiceProvider" /> used to resolve dependencies from a
        /// newly created scope.
        /// </summary>
        /// <returns>
        /// An <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceScope" /> controlling the
        /// lifetime of the scope. Once this is disposed, any scoped services that have been resolved
        /// from the <see cref="P:Microsoft.Extensions.DependencyInjection.IServiceScope.ServiceProvider" />
        /// will also be disposed.
        /// </returns>
        public IServiceScope CreateScope()
        {
            return new SpringServiceScope(this.springServiceProvider);
        }
    }
}
