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
using System.Collections;
using System.Runtime.Serialization;

namespace Oragon.Spring.Expressions
{
    /// <summary>
    /// Represents parsed map entry node.
    /// </summary>
    /// <author>Aleksandar Seovic</author>
    [Serializable]
    public class MapEntryNode : BaseNode
    {
        /// <summary>
        /// Creates a new instance of <see cref="MapEntryNode"/>.
        /// </summary>
        public MapEntryNode()
        {}

        /// <summary>
        /// Create a new instance from SerializationInfo
        /// </summary>
        protected MapEntryNode(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        
        /// <summary>
        /// Creates new instance of the map entry defined by this node.
        /// </summary>
        /// <param name="context">Context to evaluate expressions against.</param>
        /// <param name="evalContext">Current expression evaluation context.</param>
        /// <returns>Node's value.</returns>
        protected override object Get(object context, EvaluationContext evalContext)
        {
            BaseNode firstChild = (BaseNode)this.getFirstChild();
            object key = GetValue(firstChild, context, evalContext);
            object value = GetValue((BaseNode) firstChild.getNextSibling(), context, evalContext);

            return new DictionaryEntry(key, value);
        }
    }
}
