using Oragon.Spring.Context.Support;
using System;
using System.Configuration;

namespace Oragon.Spring.Core.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new XmlApplicationContext(@".\AppContext.xml");
            var person = context.GetObject<IPerson>("blablabla");
            person.Talk();

            Console.WriteLine($"OSVersion: {System.Environment.OSVersion}");
            Console.WriteLine($"MachineName: {System.Environment.MachineName}");
            Console.WriteLine($"Path: {System.Environment.CurrentDirectory}");
        }
    }
}
