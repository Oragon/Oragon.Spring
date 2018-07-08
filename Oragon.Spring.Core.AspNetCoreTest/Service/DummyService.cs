namespace Oragon.Spring.Core.AspNetCoreTest.Service
{
    public class DummyService : IDummyService
    {
        public string Return { get; set; }

        public string Ping()
        {
            return this.Return;
        }
    }
}
