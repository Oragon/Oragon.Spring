#region License

/*
 * Copyright � 2002-2011 the original author or authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#endregion

#region Imports

using System;
using AopAlliance.Aop;
using Oragon.Spring.Core;

#endregion

namespace Oragon.Spring.Aop.Support
{
	/// <summary>
	/// Convenient superclass for <see cref="Oragon.Spring.Aop.IAdvisor"/>s
	/// that are also dynamic pointcuts.
	/// </summary>
	/// <author>Rod Johnson</author>
	/// <author>Aleksandar Seovic (.NET)</author>
	[Serializable]
    public abstract class DynamicMethodMatcherPointcutAdvisor
		: DynamicMethodMatcher, IPointcutAdvisor, IPointcut, IOrdered
	{
		private int _order = Int32.MaxValue;
		private IAdvice _advice;

		/// <summary>
		/// Creates a new instance of the
		/// <see cref="Oragon.Spring.Aop.Support.DynamicMethodMatcherPointcutAdvisor"/>
		/// class.
		/// </summary>
		/// <remarks>
		/// <p>
		/// This is an abstract class, and as such has no publicly
		/// visible constructors.
		/// </p>
		/// </remarks>
		protected DynamicMethodMatcherPointcutAdvisor()
		{
		}

		/// <summary>
		/// Creates a new instance of the
		/// <see cref="Oragon.Spring.Aop.Support.DynamicMethodMatcherPointcutAdvisor"/>
		/// class for the supplied <paramref name="advice"/>.
		/// </summary>
		/// <remarks>
		/// <p>
		/// This is an abstract class, and as such has no publicly
		/// visible constructors.
		/// </p>
		/// </remarks>
		/// <param name="advice">
		/// The advice portion of this advisor.
		/// </param>
		protected DynamicMethodMatcherPointcutAdvisor(IAdvice advice)
		{
			this._advice = advice;
		}

		/// <summary>
		/// Is this advice associated with a particular instance?
		/// </summary>
		/// <remarks>
		/// <p>
		/// Not supported for dynamic advisors.
		/// </p>
		/// </remarks>
		/// <value>
		/// <see langword="true"/> if this advice is associated with a
		/// particular instance.
		/// </value>
		/// <exception cref="System.NotSupportedException">Always.</exception>
		/// <see cref="Oragon.Spring.Aop.IAdvisor.IsPerInstance"/>
		public virtual bool IsPerInstance
		{
			get
			{
				throw new NotSupportedException(
					"The 'IsPerInstance' property of the IAdvisor interface " +
					"is not yet supported in Oragon.Spring.NET.");
			}
		}

		/// <summary>
		/// The <see cref="Oragon.Spring.Aop.ITypeFilter"/> for this pointcut.
		/// </summary>
		/// <remarks>
		/// <p>
		/// This implementation always returns a filter that evaluates to <see langword="true"/>
		/// for any <see cref="System.Type"/>.
		/// </p>
		/// </remarks>
		/// <value>
		/// The current <see cref="Oragon.Spring.Aop.ITypeFilter"/>.
		/// </value>
		public virtual ITypeFilter TypeFilter
		{
			get { return TrueTypeFilter.True; }
		}

		/// <summary>
		/// The <see cref="Oragon.Spring.Aop.IMethodMatcher"/> for this pointcut.
		/// </summary>
		/// <remarks>
		/// <p>
		/// This implementation always returns itself (this object).
		/// </p>
		/// </remarks>
		/// <value>
		/// The current <see cref="Oragon.Spring.Aop.IMethodMatcher"/>.
		/// </value>
		public virtual IMethodMatcher MethodMatcher
		{
			get { return this; }
		}

		/// <summary>
		/// Returns this <see cref="Oragon.Spring.Aop.IAdvisor"/>s order in the
		/// interception chain.
		/// </summary>
		/// <returns>
		/// This <see cref="Oragon.Spring.Aop.IAdvisor"/>s order in the
		/// interception chain.
		/// </returns>
		public virtual int Order
		{
			get { return this._order; }
			set { this._order = value; }
		}

		/// <summary>
		/// Return the advice part of this aspect.
		/// </summary>
		/// <returns>
		/// The advice that should apply if the pointcut matches.
		/// </returns>
		/// <see cref="Oragon.Spring.Aop.IAdvisor.Advice"/>
		public virtual IAdvice Advice
		{
			get { return this._advice; }
			set { this._advice = value; }
		}

		/// <summary>
		/// The <see cref="Oragon.Spring.Aop.IPointcut"/> that drives this advisor.
		/// </summary>
		/// <remarks>
		/// <p>
		/// This implementation always returns itself (this object).
		/// </p>
		/// </remarks>
		public IPointcut Pointcut
		{
			get { return this; }
		}
	}
}