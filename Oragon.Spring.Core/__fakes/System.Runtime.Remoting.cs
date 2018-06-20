using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace System.Runtime.Remoting
{


    [Serializable]
    public class RemotingTimeoutException : Exception
    {
        public RemotingTimeoutException() { }
        public RemotingTimeoutException(string message) : base(message) { }
        public RemotingTimeoutException(string message, Exception inner) : base(message, inner) { }
        protected RemotingTimeoutException(
          Serialization.SerializationInfo info,
          Serialization.StreamingContext context) : base(info, context) { }
    }


    [Serializable]
    public class RemotingException : Exception
    {
        public RemotingException() { }
        public RemotingException(string message) : base(message) { }
        public RemotingException(string message, Exception inner) : base(message, inner) { }
        protected RemotingException(
          Serialization.SerializationInfo info,
          Serialization.StreamingContext context) : base(info, context) { }
    }

    public class RemotingServices
    {
        public static bool IsTransparentProxy(object newValue) => false;

        public static RealProxy GetRealProxy(object target) => new RealProxy(target);
    }

    public class RealProxy
    {
        private object target;

        public RealProxy(object target)
        {
            this.target = target;
        }

        public Type GetProxiedType()
        {
            return target.GetType();
        }

        public virtual IMessage Invoke(IMessage msg)
        {
            return new MethodCallMessage();
        }

        public object GetTransparentProxy()
        {
            return this.target;
        }
    }

    public interface IRemotingTypeInfo
    {
        bool CanCastTo(Type requiredType, object target);
    }

    public interface IMessage
    {

    }

    public interface IMethodCallMessage
    {
        string MethodName { get; set; }
        Type[] MethodSignature { get; set; }
        object[] Args { get; set; }
    }

    public class MethodCallMessage : IMethodCallMessage, IMessage
    {
        public string MethodName { get; set; }
        public Type[] MethodSignature { get; set ; }
        public object[] Args { get; set; }
    }

    public class ReturnMessage : IMessage
    {
        private object result;
        private object p1;
        private int v;
        private object p2;
        private IMethodCallMessage callMsg;

        public ReturnMessage(object result, object p1, int v, object p2, IMethodCallMessage callMsg)
        {
            this.result = result;
            this.p1 = p1;
            this.v = v;
            this.p2 = p2;
            this.callMsg = callMsg;
        }
    }
}

namespace System.Runtime.Remoting.Messaging
{
    public class CallContext
    {
        static ConcurrentDictionary<string, AsyncLocal<object>> state = new ConcurrentDictionary<string, AsyncLocal<object>>();


        public static object GetData(string name) =>
             state.TryGetValue(name, out AsyncLocal<object> data) ? data.Value : null;

        public static void SetData(string name, object data) =>
           state.GetOrAdd(name, _ => new AsyncLocal<object>()).Value = data;

        public static void FreeNamedDataSlot(string name)
        {
            
        }
    }


}

namespace System.Runtime.Remoting.Proxies
{
    class b
    {
    }
}