using Microsoft.Extensions.Configuration;

namespace Oragon.Spring.Core.AspNetCoreTest.Service
{
    public class DummyService : IDummyService
    {
        public string Return { get; set; }

        public IConfiguration Configuration { get; set; }

        public string Ping()
        {
            return this.Return;
        }

        public string GetConfiguration(string key)
        {
            return this.Configuration.GetValue<string>(key);
        }
    }
}
