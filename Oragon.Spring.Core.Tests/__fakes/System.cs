using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;

namespace System
{
    public static class AppDomainExtensions
    {
        public static AssemblyBuilder DefineDynamicAssembly(this AppDomain domain, AssemblyName name, AssemblyBuilderAccess access)
        {
            return AssemblyBuilder.DefineDynamicAssembly(name, access);
        }

        public static object CreateInstanceAndUnwrap(this AppDomain domain, string assembly, string type)
        {
            return Activator.CreateInstance(Type.GetType($"{type}, {assembly}"));
        }

        public static object Evidence(this AppDomain domain)
        {
            return "";
        }


    }

    public class AppDomainSetup
    {
        public string ApplicationBase;
    }

    public class Evidence
    {
        private object v;

        public Evidence(object v)
        {
            this.v = v;
        }
    }

    public class _AppDomain
    {
        public static AppDomain CreapteDomain(params object[] data)
        {
            return null;
        }
    }


    public class SecurityTemplate
    {
        public static void MediumTrustInvoke(ThreadStart action)
        {
            action();
        }
    }


    [Serializable]
    public class HttpException : Exception
    {
        public HttpException() { }
        public HttpException(string message) : base(message) { }
        public HttpException(string message, Exception inner) : base(message, inner) { }
        protected HttpException(
          Runtime.Serialization.SerializationInfo info,
          Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
