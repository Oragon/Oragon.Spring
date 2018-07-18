using AopAlliance.Intercept;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oragon.Spring
{
	public static partial class OragonExtensions
	{
		#region Public Methods

		public static IEnumerable<T> GetAttibutes<T>(this IMethodInvocation invocation, Func<T, bool> predicate = null)
			where T : Attribute
		{
			if (predicate == null)
				predicate = (it => true);

			//Recupera os atributos do método
			IEnumerable<T> returnValue = invocation.Method.GetCustomAttributes(typeof(T), true)
			.Cast<T>()
			.Where(predicate);
			return returnValue;
		}

		#endregion Public Methods
	}
}