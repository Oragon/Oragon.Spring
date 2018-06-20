using System;
using AopAlliance.Intercept;

namespace Oragon.Spring.Aop.Framework
{
	public class UnsupportedInterceptor : IMethodInterceptor
	{
	  public object Invoke(IMethodInvocation invocation)
	  {
	    throw new NotImplementedException(invocation.Method.Name);
	  }
	}
}
