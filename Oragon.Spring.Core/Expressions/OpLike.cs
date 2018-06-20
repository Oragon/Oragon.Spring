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
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
#if !MONO_2_0
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
#endif

namespace Oragon.Spring.Expressions
{
    /// <summary>
    /// Represents VB-style logical LIKE operator.
    /// </summary>
    /// <author>Aleksandar Seovic</author>
    [Serializable]
    public class OpLike : BinaryOperator
    {
        /// <summary>
        /// Create a new instance
        /// </summary>
        public OpLike():base()
        {
        }

        /// <summary>
        /// Create a new instance from SerializationInfo
        /// </summary>
        protected OpLike(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        
        /// <summary>
        /// Returns a value for the logical LIKE operator node.
        /// </summary>
        /// <param name="context">Context to evaluate expressions against.</param>
        /// <param name="evalContext">Current expression evaluation context.</param>
        /// <returns>
        /// true if the left operand matches the right operand, false otherwise.
        /// </returns>
        protected override object Get(object context, EvaluationContext evalContext)
        {
#if !MONO_2_0
            string text = GetLeftValue( context, evalContext ) as string;
            string pattern = GetRightValue( context, evalContext ) as string;

            pattern = Regex.Escape(pattern).Replace(@"\*", ".*").Replace(@"\?", ".");

            throw new NotImplementedException("LikeOperator.LikeString");

            return new Regex(
                "^" + Regex.Escape(pattern).Replace(@"\*", ".*").Replace(@"\?", ".") + "$",
                RegexOptions.IgnoreCase | RegexOptions.Singleline
            ).IsMatch(text);

            //return LikeOperator.LikeString(text, pattern, CompareMethod.Text);
#else
            throw new NotSupportedException("'like' operator is only supported in .NET 2.0 or higher.");
#endif
        }
    }
}
