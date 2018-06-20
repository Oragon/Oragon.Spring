#region License

/*
 * Copyright 2004 the original author or authors.
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

using System.Collections;

#endregion

namespace Oragon.Spring.Objects.Factory.Xml {

	/// <summary>
	/// Summary description for MixedCollectionObject.
    /// </summary>
    /// <author>Rod Johnson</author>
    /// <author>Rick Evans (.NET)</author>
	public class MixedCollectionObject 
    {

        public MixedCollectionObject ()
        {
            ++nrOfInstances;
        }

        public ICollection Jumble
        {
            get
            {
                return jumble;
            }
            set
            {
                this.jumble = value;
            }
        }
		
        protected internal static int nrOfInstances = 0;
		
        public static void ResetStaticState ()
        {
            nrOfInstances = 0;
        }
		
        private ICollection jumble;
	}
}
