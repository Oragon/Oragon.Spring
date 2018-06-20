#region License

/*
 * Copyright 2002-2010 the original author or authors.
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
using System.Collections.Generic;
using Oragon.Spring.Objects.Factory.Config;
using Oragon.Spring.Objects.Factory.Support;
using Oragon.Spring.Validation.Actions;

namespace Oragon.Spring.Validation
{
    public abstract class BaseTestValidator : BaseSimpleValidator
    {
        private bool _wasCalled;

        public bool WasCalled
        {
            get { return _wasCalled; }
        }

        public override bool Validate(object validationContext, IDictionary<string, object> contextParams, IValidationErrors errors)
        {
            _wasCalled = true;
            return base.Validate (validationContext, contextParams, errors);
        }
    
    }

    /// <summary>
    /// Helper classes for validation tests.
    /// </summary>
    /// <author>Aleksandar Seovic</author>
    public class TrueValidator : BaseTestValidator
    {

        public TrueValidator()
        {}

        /// <summary>
        /// Validates test object.
        /// </summary>
        /// <param name="objectToValidate">Object to validate.</param>
        /// <returns><c>True</c> if specified object is valid, <c>False</c> otherwise.</returns>
        protected override bool Validate(object objectToValidate)
        {
            return true;
        }
    }

    public class FalseValidator : BaseTestValidator
    {
        public FalseValidator()
        {
            this.Actions.Add(new ErrorMessageAction("error", "errors"));
        }

        /// <summary>
        /// Validates test object.
        /// </summary>
        /// <param name="objectToValidate">Object to validate.</param>
        /// <returns><c>True</c> if specified object is valid, <c>False</c> otherwise.</returns>
        protected override bool Validate(object objectToValidate)
        {
            return false;
        }
    }

    public sealed class MockObjectDefinitionRegistry : IObjectDefinitionRegistry
    {
        private IDictionary<string, IObjectDefinition> objects = new Dictionary<string, IObjectDefinition>();

        public int ObjectDefinitionCount
        {
            get { return this.objects.Count; }
        }

        public IList<string> GetObjectDefinitionNames()
        {
            return new List<string>(this.objects.Keys);
        }

        public IList<string> GetObjectDefinitionNames(bool includeAncestor)
        {
            return new List<string>(this.objects.Keys);
        }

        public IList<IObjectDefinition> GetObjectDefinitions()
        {
            return new List<IObjectDefinition>(this.objects.Values);
        }

        public bool ContainsObjectDefinition(string name)
        {
            return objects.ContainsKey(name);
        }

        public IObjectDefinition GetObjectDefinition(string name)
        {
            IObjectDefinition definition;
            objects.TryGetValue(name, out definition);
            return definition;
        }

        public void RegisterObjectDefinition(string name, IObjectDefinition definition)
        {
            this.objects[name] = definition;
        }

        public IList<string> GetAliases(string name)
        {
            throw new NotImplementedException();
        }

        public void RegisterAlias(string name, string theAlias)
        {
            throw new NotImplementedException();
        }

        public bool IsObjectNameInUse(string objectName)
        {
            return this.objects[objectName] != null;
        }
    }

}