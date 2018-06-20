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

using System.Collections;
using NUnit.Framework;

using Rhino.Mocks;

using Oragon.Spring.Core.IO;
using Oragon.Spring.Util;

namespace Oragon.Spring.Objects.Factory.Config
{
    
    /// <summary>
    /// Unit tests for the TypeAliasConfigurer class
    /// </summary>
    /// <author>Mark Pollack</author>
    [TestFixture]
    public class ResourceHandlerConfigurerTests
    {
        private MockRepository mocks;

        [SetUp]
        public void SetUp()
        {
            mocks = new MockRepository();
        }

        [Test]
        public void Serialization()
        {
            IDictionary resourceHandlers = new Hashtable();
            resourceHandlers.Add("httpsss", typeof(UrlResource));

            ResourceHandlerConfigurer resourceHandlerConfiguer = new ResourceHandlerConfigurer();
            resourceHandlerConfiguer.ResourceHandlers = resourceHandlers;


            resourceHandlerConfiguer.Order = 1;

            SerializationTestUtils.SerializeAndDeserialize(resourceHandlerConfiguer);
        }

        [Test]
        public void UseInvalidTypeForDictionaryValue()
        {
            IDictionary resourceHandlers = new Hashtable();
            resourceHandlers.Add("httpsss", new Hashtable());

            ResourceHandlerConfigurer resourceHandlerConfiguer = new ResourceHandlerConfigurer();
            resourceHandlerConfiguer.ResourceHandlers = resourceHandlers;

            Assert.Throws<ObjectInitializationException>(() => resourceHandlerConfiguer.PostProcessObjectFactory((IConfigurableListableObjectFactory) mocks.DynamicMock(typeof(IConfigurableListableObjectFactory))));
        }

        [Test]
        public void UseNonResolvableTypeForDictionaryValue()
        {
            IDictionary resourceHandlers = new Hashtable();
            resourceHandlers.Add("httpsss", "Oragon.Spring.Core.IO.UrrrrlResource, Oragon.Spring.Core");

            ResourceHandlerConfigurer resourceHandlerConfiguer = new ResourceHandlerConfigurer();
            resourceHandlerConfiguer.ResourceHandlers = resourceHandlers;


            Assert.Throws<ObjectInitializationException>(() => resourceHandlerConfiguer.PostProcessObjectFactory((IConfigurableListableObjectFactory) mocks.DynamicMock(typeof(IConfigurableListableObjectFactory))));
        }

        [Test]
        public void SunnyDayScenarioUsingType()
        {


            IDictionary resourceHandlers = new Hashtable();
            resourceHandlers.Add("httpsss", typeof(UrlResource));

            CreateConfigurerAndTestNewProtcol(resourceHandlers);
        }

        [Test]
        public void SunnyDayScenarioUsingTypeString()
        {
            IDictionary typeAliases = new Hashtable();
            typeAliases.Add("httpsss", "Oragon.Spring.Core.IO.UrlResource, Oragon.Spring.Core");
            CreateConfigurerAndTestNewProtcol(typeAliases);

        }

        private void CreateConfigurerAndTestNewProtcol(IDictionary resourceHandlers)
        {
            ResourceHandlerConfigurer resourceHandlerConfiguer = new ResourceHandlerConfigurer();
            resourceHandlerConfiguer.ResourceHandlers = resourceHandlers;

            resourceHandlerConfiguer.Order = 1;


            resourceHandlerConfiguer.PostProcessObjectFactory((IConfigurableListableObjectFactory) mocks.DynamicMock(typeof(IConfigurableListableObjectFactory)));

            //todo investigate mocking the typeregistry, for now ask the actual one for information.
            Assert.IsTrue(ResourceHandlerRegistry.IsHandlerRegistered("httpsss"),
                          "ResourceHandlerConfigurer did not register a protocol handler with the ResourceHandlerRegistry");

            Assert.IsTrue(ResourceHandlerRegistry.IsHandlerRegistered("httpsss"), "Custom IResource not registered.");
            Assert.AreEqual(1, resourceHandlerConfiguer.Order);
        }
    }
}
