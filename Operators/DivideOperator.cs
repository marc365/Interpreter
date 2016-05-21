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
    /// Divides numeric values.
    /// Usage: numericValue / numericValue
    /// Example: 1 / 2
    /// </summary>
    public class DivideOperator : Operator
    {
        public DivideOperator()
        {
        }

        internal override Literal Execute(IConstruct argument1, IConstruct argument2)
        {
            var argument2Value = base.GetTransformedConstruct<Number>(argument2);

            if (argument2Value == 0)
                return new Error("Divide by zero");

            return base.GetTransformedConstruct<Number>(argument1) / argument2Value;
        }

        public override string Token
        {
            get
            {
                return "/";
            }
        }
    }
}