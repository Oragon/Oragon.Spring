using System;
using System.Collections.Generic;
using System.Text;

namespace Oragon.Spring.Core.ConsoleTest
{
    public class Person : IPerson
    {
        public string Name { get; set; }

        public string Country { get; set; }

        public DateTime Birthday { get; set; }

        public IList<Contact> Contacts { get; set; }

        public void Talk()
        {
            Console.WriteLine("Hello");
        }
    }

    public interface IPerson
    {
        void Talk();
    }

    public class Contact
    {
        public string Adddress { get; set; }

        public AddressType Type { get; set; }
    }

    public enum AddressType
    {
        Site,
        Email,
        Twitter,
        GitHub
    }
}
