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
using Oragon.Spring.Caching;
using Oragon.Spring.Context;

#endregion

namespace Oragon.Spring.Aspects.Cache
{
    /// <summary>
    /// Unit tests for the CacheParameterAdvice class.
    /// </summary>
    /// <author>Aleksandar Seovic</author>
    [TestFixture]
    public sealed class CacheParameterAdviceTests
    {
        private MockRepository mocks;
        private IApplicationContext mockContext;
        private CacheParameterAdvice advice;
        private ICache cache;

        [SetUp]
        public void SetUp()
        {
            mocks = new MockRepository();
            mockContext = (IApplicationContext) mocks.CreateMock(typeof (IApplicationContext));

            advice = new CacheParameterAdvice();
            advice.ApplicationContext = mockContext;

            cache = new NonExpiringCache();
        }

        [Test]
        public void TestSimpleParameterCaching()
        {
            MethodInfo method = typeof(SimpleCacheParameterTarget).GetMethod("Save");
            object[] args = new object[] {new Inventor("Nikola Tesla", new DateTime(1856, 7, 9), "Serbian")};

            ExpectCacheInstanceRetrieval("cache", cache);
            mocks.ReplayAll();

            // parameter value should be added to cache
            advice.AfterReturning(null, method, args, null);
            Assert.AreEqual(1, cache.Count);
            Assert.AreEqual(args[0], cache.Get("Nikola Tesla"));

            mocks.VerifyAll();
        }

        [Test]
        public void TestSimpleWithMethodInfoParameterCaching()
        {
            MethodInfo method = typeof(SimpleWithMethodInfoCacheParameterTarget).GetMethod("Save");
            object[] args = new object[] { new Inventor("Nikola Tesla", new DateTime(1856, 7, 9), "Serbian") };

            ExpectCacheInstanceRetrieval("cache", cache);
            mocks.ReplayAll();

            // parameter value should be added to cache
            advice.AfterReturning(null, method, args, null);
            Assert.AreEqual(1, cache.Count);
            Assert.AreEqual(args[0], cache.Get("Save-Nikola Tesla"));

            mocks.VerifyAll();
        }

        [Test]
        public void TestMultipleParameterCaching()
        {
            MethodInfo method = typeof(MultipleCacheParameterTarget).GetMethod("Save");
            object[] args = new object[] { new Inventor("Nikola Tesla", new DateTime(1856, 7, 9), "Serbian") };

            ExpectCacheInstanceRetrieval("cache", cache);
            ExpectCacheInstanceRetrieval("cache", cache);  
            mocks.ReplayAll();

            // parameter value should be added to both cache
            advice.AfterReturning(null, method, args, null);
            Assert.AreEqual(2, cache.Count);
            Assert.AreEqual(args[0], cache.Get("Nikola Tesla"));
            Assert.AreEqual(args[0], cache.Get("Serbian"));

            mocks.VerifyAll();
        }

        [Test]
        public void TestConditionParameterCaching()
        {
            MethodInfo method = typeof(ConditionCacheParameterTarget).GetMethod("Save");
            object[] args = new object[] { new Inventor("Nikola Tesla", new DateTime(1856, 7, 9), "Serbian") };

            // parameter value should not be added to cache
            advice.AfterReturning(null, method, args, null);
            Assert.AreEqual(0, cache.Count);
        }

        #region Helper methods

        private void ExpectCacheInstanceRetrieval(string cacheName, ICache cacheToReturn)
        {
            Expect.Call(mockContext.GetObject(cacheName)).Return(cacheToReturn);
        }

        #endregion
    }

    #region Inner Class : CacheParameterTarget

    public interface ICacheParameterTarget
    {
        void Save(Inventor inventor);
    }

    public sealed class SimpleCacheParameterTarget : ICacheParameterTarget
    {
        public void Save([CacheParameter("cache", "Name")] Inventor inventor)
        {
        }
    }

    public sealed class SimpleWithMethodInfoCacheParameterTarget : ICacheParameterTarget
    {
        public void Save([CacheParameter("cache", "#Save.Name + '-' + Name")] Inventor inventor)
        {
        }
    }

    public sealed class MultipleCacheParameterTarget : ICacheParameterTarget
    {
        public void Save([CacheParameter("cache", "Name")][CacheParameter("cache", "Nationality")] Inventor inventor)
        {
        }
    }

    public sealed class ConditionCacheParameterTarget : ICacheParameterTarget
    {
        public void Save([CacheParameter("cache", "Name", Condition = "Nationality == 'French'")] Inventor inventor)
        {
        }
    }

    #endregion
}