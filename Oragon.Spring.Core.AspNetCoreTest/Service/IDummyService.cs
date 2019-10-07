namespace Oragon.Spring.Core.AspNetCoreTest.Service
{
    public interface IDummyService
    {
        string Ping();

        string GetConfiguration(string key);
    }
}