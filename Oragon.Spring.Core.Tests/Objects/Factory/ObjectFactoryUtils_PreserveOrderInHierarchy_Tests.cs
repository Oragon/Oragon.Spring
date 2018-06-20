using System;
using System.Collections.Generic;
using NUnit.Framework;
using Oragon.Spring.Objects.Factory.Support;

namespace Oragon.Spring.Objects.Factory
{
    [TestFixture]
    public class ObjectFactoryUtils_PreserveOrderInHierarchy_Tests
    {
        private readonly Type _expectedtype = typeof(ITestObject);
        private DefaultListableObjectFactory _factory;

        [SetUp]
        public void _TestSetUp()
        {
            DefaultListableObjectFactory parentFactory = new DefaultListableObjectFactory();
            _factory = new DefaultListableObjectFactory(parentFactory);

            RootObjectDefinition rodA = new RootObjectDefinition(_expectedtype);
            RootObjectDefinition rodB = new RootObjectDefinition(_expectedtype);
            RootObjectDefinition rodC = new RootObjectDefinition(_expectedtype);
            RootObjectDefinition rod2A = new RootObjectDefinition(_expectedtype);
            RootObjectDefinition rod2C = new RootObjectDefinition(_expectedtype);

            _factory.RegisterObjectDefinition("objA", rodA);
            _factory.RegisterObjectDefinition("objB", rodB);
            _factory.RegisterObjectDefinition("objC", rodC);

            parentFactory.RegisterObjectDefinition("obj2A", rod2A);
            parentFactory.RegisterObjectDefinition("objB", rodB);
            parentFactory.RegisterObjectDefinition("obj2C", rod2C);
        }

        [Test]
        public void ObjectNamesIncludingAncestorsPreserveOrderOfRegistration()
        {
            IList<string> names = ObjectFactoryUtils.ObjectNamesIncludingAncestors(_factory);
            Assert.AreEqual(5, names.Count);
            Assert.AreEqual(new string[] { "objA", "objB", "objC", "obj2A", "obj2C" }, names);
        }

        [Test]
        public void ObjectNamesForTypeIncludingAncestorsPreserveOrderOfRegistration()
        {
            IList<string> names = ObjectFactoryUtils.ObjectNamesForTypeIncludingAncestors(_factory, _expectedtype);
            Assert.AreEqual(5, names.Count);
            Assert.AreEqual(new string[] { "objA", "objB", "objC", "obj2A", "obj2C" }, names);
        }

        [Test]
        public void ObjectNamesForTypeIncludingAncestorsPrototypesAndFactoryObjectsPreserveOrderOfRegistration()
        {
            IList<string> names = ObjectFactoryUtils.ObjectNamesForTypeIncludingAncestors(_factory, _expectedtype, false, false);
            Assert.AreEqual(5, names.Count);
            Assert.AreEqual(new string[] { "objA", "objB", "objC", "obj2A", "obj2C" }, names);
        }
    }
}