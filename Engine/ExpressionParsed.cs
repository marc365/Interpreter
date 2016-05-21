/*
 *
 * User: github.com/marc365
 * Updated: 2016
 */

/* _________________________________________________

  (c) Hi-Integrity Systems 2012. All rights reserved.
  www.hisystems.com.au - Toby Wicks
  github.com/hisystems/Interpreter
 
  Licensed under the Apache License, Version 2.0 (the "License");
  you may not use this file except in compliance with the License.
  You may obtain a copy of the License at

      http://www.apache.org/licenses/LICENSE-2.0

  Unless required by applicable law or agreed to in writing, software
  distributed under the License is distributed on an "AS IS" BASIS,
  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
  See the License for the specific language governing permissions and
  limitations under the License.
 ___________________________________________________ */

namespace HiSystems.Interpreter
{
    /// <summary>
    /// Represents an expression that has been parsed and can be executed immediately without compilation/parsing.
    /// </summary>
    internal class ExpressionParsed : Expression
    {
        /// <summary>
        /// The root construct of the expression tree.
        /// </summary>
        private IConstruct construct;
        
        internal ExpressionParsed(string expression, IConstruct value)
            : base(expression)
        {
            this.construct = value;
        }

        /// <summary>
        /// Returns the calculated value for the expression.
        /// Any variables should be set before calling this function.
        /// Will typically return a Number or Boolean literal value (depending on the type of expression).
        /// </summary>
        public override Literal Execute()
        {
            if (construct != null)
                return construct.Transform();

            return null;
        }
    }
}