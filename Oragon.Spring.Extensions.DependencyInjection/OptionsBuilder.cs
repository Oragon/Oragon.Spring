using System;
using System.Collections.Generic;
using System.Text;

namespace Oragon.Spring.Extensions.DependencyInjection
{
    public class OptionsBuilder
    {
        internal List<string> ConfigLocations = new List<string>();
        internal List<Replication> Replications = new List<Replication>();

        internal OptionsBuilder() { }


        /// <summary>
        /// Add Oragon Spring Configuration File
        /// </summary>
        /// <param name="configLocation"></param>
        /// <returns></returns>
        public OptionsBuilder AddConfigLocation(string configLocation)
        {
            this.ConfigLocations.Add(configLocation);
            return this;
        }


        /// <summary>
        /// Create a reference for T from default ASP.NET Dependency Injection Container registring on Oragon Spring.
        /// </summary>
        /// <typeparam name="T">Type to evaluate ASP.NET Dependency Injection Container </typeparam>
        /// <param name="name">Name to register on Oragon Spring</param>
        /// <returns></returns>
        public OptionsBuilder MakeBridgeFor<T>(string name)
        {
            this.Replications.Add(new Replication { Type = typeof(T), Name = name, Singleton = true });
            return this;
        }

        internal class Replication
        {
            public Type Type { get; set; }

            public string Name { get; set; }

            public bool Singleton { get; set; }
        }

    }


}
