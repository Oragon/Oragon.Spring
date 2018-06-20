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
using System.Reflection;
using NUnit.Framework;
using Rhino.Mocks;

#endregion

namespace Oragon.Spring.Objects.Factory.Support
{
	/// <summary>
	/// Unit tests for the LookupMethodReplacer class.
	/// </summary>
	/// <author>Rick Evans</author>
	[TestFixture]
	public sealed class LookupMethodReplacerTests
	{
        private MockRepository mocks;

        [SetUp]
        public void Setup()
        {
            mocks = new MockRepository();
        }

		[Test]
		public void InstantiationWithNullDefinition()
		{
		    IObjectFactory objectFactory = mocks.StrictMock<IObjectFactory>();
            Assert.Throws<ArgumentNullException>(() => new LookupMethodReplacer(null, objectFactory));
		}

		[Test]
		public void InstantiationWithNullFactory()
		{
		    var configurableObjectDefinition = mocks.StrictMock<IConfigurableObjectDefinition>();
            Assert.Throws<ArgumentNullException>(() => new LookupMethodReplacer(configurableObjectDefinition, null));
		}

		[Test]
		public void SunnyDayPath()
		{
            var objectFactory = mocks.StrictMock<IObjectFactory>();
            var configurableObjectDefinition = mocks.StrictMock<IConfigurableObjectDefinition>();
			
            object expectedLookup = new object();
			const string LookupObjectName = "foo";
			
		    Expect.Call(objectFactory.GetObject(LookupObjectName)).Return(expectedLookup);

			LookupMethodOverride ovr = new LookupMethodOverride("SunnyDayPath", LookupObjectName);
			MethodOverrides overrides = new MethodOverrides();
			overrides.Add(ovr);
		    Expect.Call(configurableObjectDefinition.MethodOverrides).Return(overrides);

            LookupMethodReplacer replacer = new LookupMethodReplacer(configurableObjectDefinition, objectFactory);
            mocks.ReplayAll();		    
			MethodInfo method = (MethodInfo) MethodBase.GetCurrentMethod();

			object lookup = replacer.Implement(this, method, new object[] {});
			Assert.AreSame(expectedLookup, lookup);

			mocks.VerifyAll();
		}
	}
}