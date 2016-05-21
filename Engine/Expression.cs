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

using System;
using System.Collections.Generic;
using System.Linq;

namespace HiSystems.Interpreter
{
    /// <summary>
    /// Represents an expression and the variables defined in the expression.
    /// This is the base class for an already compiled expression or a just-in-time compiled expression.
    /// </summary>
    public abstract class Expression : IConstruct
    {
        /// <summary>
        /// Only used for error reporting and debugging as the ToString return value.
        /// </summary>
        private string expression;

        /// <summary>
        /// Initializes a new instance of the <see cref="HiSystems.Interpreter.Expression"/> class.
        /// </summary>
        /// <param name='value'>
        /// Root construct in the expression tree. Calling IConstruct.Transform will return the actual value.
        /// </param>
        internal Expression(string expression)
        {
            this.expression = expression;
        }

        /// <summary>
        /// Returns the calculated value for the expression.
        /// Any variables should be set before calling this function.
        /// Will typically return a Number or Boolean literal value (depending on the type of expression).
        /// </summary>
        public abstract Literal Execute();

        /// <summary>
        /// Returns the calculated value for the expression.
        /// Any variables should be set before calling this function.
        /// Casts the return value to type T (of type Literal)
        /// </summary>
        public T Execute<T>() where T : Literal
        {
            Literal result = this.Execute();

            if (result == null)
                Output.Text(String.Format("Return value from '{0}' is of type 'null', not of type {1}", this.expression, typeof(T).Name));


            if (!(result is T))
                Output.Text(String.Format("Return value from '{0}' is of type {1}, not of type {2}", this.expression, result.GetType().Name, typeof(T).Name));

            return (T)result;
        }

        /// <summary>
        /// The original / source expression which this expression represents.
        /// </summary>
        public string Source
        {
            get
            {
                return this.expression;
            }
        }

        /// <summary>
        /// Converts a string value to an Expression that when Execute()'d will return the same Text literal value.
        /// </summary>
        public static implicit operator Expression(string stringLiteral)
        {
            return new ExpressionParsed("\"" + stringLiteral + "\"", new Text(stringLiteral));
        }
        
        /// <summary>
        /// Converts a bool value to an Expression that when Execute()'d will return the same bool literal value.
        /// </summary>
        public static implicit operator Expression(bool value)
        {
            return new ExpressionParsed(value.ToString(), new Boolean(value));
        }

        /// Returns a distinct list of variables from the expression.
        /// </summary>
        protected static IDictionary<string, Variable> TranslateVariablesToDictionary(List<Variable> variables)
        {
            return variables.ToDictionary(variable => variable.Name, variable => variable);
        }

        /// <summary>
        /// Allows for an expression to also be considered as a construct.
        /// Used when an expression references an identifier which is also a separate expression.
        /// Allowing an expression to reference a chain of expressions.
        /// </summary>
        Literal IConstruct.Transform()
        {
            return this.Execute();
        }

        public override string ToString()
        {
            return expression;
        }
    }
}