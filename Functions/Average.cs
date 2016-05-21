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

using System.Linq;

namespace HiSystems.Interpreter
{
    public class Average : Function
    {
        public override string Name
        {
            get
            {
                return "Avg";
            }
        }

        public override string Description
        {
            get { return "Accepts one argument of type Array containing objects of type Number."; }
        }

        public override string Usage
        {
            get { return "Avg(array) Example: Avg(Array(1, 2, 3))"; }
        }

        public override Literal Execute(IConstruct[] arguments)
        {
            base.EnsureArgumentCountIs(arguments, 1);

            return new Number(base.GetTransformedArgumentDecimalArray(arguments, argumentIndex:0).Average());
        }
    }
}