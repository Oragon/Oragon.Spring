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
using System.Runtime.Serialization;
using System.Security.Permissions;

#endregion

namespace Oragon.Spring.Aop
{
	/// <summary>
	/// Canonical <see cref="Oragon.Spring.Aop.ITypeFilter"/> instances.
	/// </summary>
	/// <remarks>
	/// <p>
	/// Only one canonical <see cref="Oragon.Spring.Aop.ITypeFilter"/> instance is
	/// provided out of the box. The <see cref="TrueTypeFilter.True"/>
	/// <see cref="Oragon.Spring.Aop.ITypeFilter"/> matches all classes.
	/// </p>
	/// </remarks>
	/// <author>Rod Johnson</author>
	/// <author>Aleksandar Seovic (.NET)</author>
	[Serializable]
	public sealed class TrueTypeFilter : ITypeFilter, ISerializable
	{
		/// <summary>
		/// Canonical <see cref="Oragon.Spring.Aop.ITypeFilter"/> instance that
		/// matches all classes.
		/// </summary>
		public static readonly ITypeFilter True = new TrueTypeFilter();

		/// <summary>
		/// Creates a new instance of the
		/// <see cref="TrueTypeFilter"/> class.
		/// </summary>
		/// <remarks>
		/// <p>
		/// This is a utility class, and as such has no publicly visible
		/// constructors.
		/// </p>
		/// </remarks>
		private TrueTypeFilter()
		{
		}

		/// <summary>
		/// Should the pointcut apply to the supplied
		/// <see cref="System.Type"/>?
		/// </summary>
		/// <param name="type">
		/// The candidate <see cref="System.Type"/>.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the advice should apply to the supplied
		/// <paramref name="type"/>
		/// </returns>
		/// <seealso cref="Oragon.Spring.Aop.ITypeFilter.Matches(Type)"/>
		public bool Matches(Type type)
		{
			return true;
		}

		/// <summary>
		/// A <see cref="System.String"/> that represents the current
		/// <see cref="Oragon.Spring.Aop.ITypeFilter"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current
		/// <see cref="Oragon.Spring.Aop.ITypeFilter"/>.
		/// </returns>
		public override string ToString()
		{
			return "TrueTypeFilter.True";
		}

		/// <summary>
		/// Populates a <see cref="System.Runtime.Serialization.SerializationInfo"/> with
		/// the data needed to serialize the target object.
		/// </summary>
		/// <param name="info">
		/// The <see cref="System.Runtime.Serialization.SerializationInfo"/> to populate
		/// with data.
		/// </param>
		/// <param name="context">
		/// The destination (see <see cref="System.Runtime.Serialization.StreamingContext"/>)
		/// for this serialization.
		/// </param>
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter=true)]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.SetType(typeof (TrueTypeFilterObjectReference));
		}

		[Serializable]
		private sealed class TrueTypeFilterObjectReference : IObjectReference
		{
			public object GetRealObject(StreamingContext context)
			{
				return TrueTypeFilter.True;
			}
		}
	}
}