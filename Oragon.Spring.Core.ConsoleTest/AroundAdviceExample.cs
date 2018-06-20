using System;
using System.Collections.Generic;
using System.Text;
using AopAlliance.Intercept;

namespace Oragon.Spring.Core.ConsoleTest
{
    public class AroundAdviceExample : AopAlliance.Intercept.IMethodInterceptor
    {
        public object Invoke(IMethodInvocation invocation)
        {
            Console.WriteLine("Before Call");
            object returnValue = invocation.Proceed(); ;
            Console.WriteLine("After Call");
            return returnValue;
        }
    }
}
