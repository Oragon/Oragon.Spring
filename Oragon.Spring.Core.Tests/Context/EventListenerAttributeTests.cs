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

using System;
using NUnit.Framework;

namespace Oragon.Spring.Context
{
	[TestFixture]
	public sealed class EventListenerAttributeTests
	{
		[Test]
		public void ReadEventListenerAttribute()
		{
			Attribute[] attributes = Attribute.GetCustomAttributes(typeof (IApplicationEventListener));
			bool found = false;
			foreach (Attribute attribute in attributes)
			{
				if (attribute is EventListenerAttribute)
				{
					found = true;
				}
			}
			Assert.IsTrue(found, "EventListener Attribute not found");
		}
	}
}