using Microsoft.Extensions.DependencyInjection;
using Oragon.Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Spring.Extensions.DependencyInjection
{
    public class SpringServiceProvider : IServiceProvider, ISupportRequiredService, IDisposable
    {
        private readonly XmlApplicationContext applicationContext;

        public SpringServiceProvider(XmlApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }



        public object GetRequiredService(Type serviceType)
        {
            object returnValue = null;
            IList<string> keys = this.applicationContext.GetObjectNamesForType(serviceType);
            if (keys.Any())
            {
                string name = keys.Single();
                returnValue = this.applicationContext.GetObject(name);
            }
            return returnValue;
        }

        public object GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }


        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }


        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    //this._lifetimeScope.Dispose();
                }

                this._disposed = true;
            }
        }
    }
}
