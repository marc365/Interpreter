﻿/*
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

using System;

namespace HiSystems.Interpreter
{
    /// <summary>
    /// Adds two numeric values, text or a date and numeric value (adds days).
    /// Usage: 
    ///   numericValue + numericValue
    ///   dateTime + numericDays
    ///   text + textToConcatenate
    /// Examples:
    ///   1 + 2
    ///   #2000-01-01# + 1
    ///   'ab' + 'c'
    /// </summary>
    public class AddOperator : Operator
    {
        public AddOperator()
        {
        }

        internal override Literal Execute(IConstruct argument1, IConstruct argument2)
        {
            var argument1Transformed = base.GetTransformedConstruct<Literal>(argument1);
            var argument2Transformed = base.GetTransformedConstruct<Literal>(argument2);

            if (argument1Transformed is Number && argument2Transformed is Number)
                return ((Number)argument1Transformed) + ((Number)argument2Transformed);
            else if (argument1Transformed is DateTime && argument2Transformed is Number)
                return ((DateTime)argument1Transformed) + ((Number)argument2Transformed);
            else if (argument1Transformed is DateTime && argument2Transformed is Text)
                return ((DateTime)argument1Transformed).ToString() + ((Text)argument2Transformed);
            else if (argument1Transformed is Text && argument2Transformed is DateTime)
                return ((Text)argument1Transformed) + ((DateTime)argument2Transformed).ToString();
            else if (argument1Transformed is Text && argument2Transformed is Text)
                return ((Text)argument1Transformed) + ((Text)argument2Transformed);
            else if (argument1Transformed is Text && argument2Transformed is Number)
                return ((Text)argument1Transformed) + ((Number)argument2Transformed).ToString();
            else if (argument1Transformed is Number && argument2Transformed is Text)
                return ((Number)argument1Transformed).ToString() + ((Text)argument2Transformed);
            else
                return new Error(String.Format("Add operator requires arguments of type Number, DateTime or Text. Argument types are {0} {1}.", argument1Transformed.GetType().Name, argument2Transformed.GetType().Name));
        }

        public override string Token
        {
            get 
            {
                return "+";
            }
        }
    }
}