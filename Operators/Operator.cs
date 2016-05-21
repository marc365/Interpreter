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

namespace HiSystems.Interpreter
{
    /// <summary>
    /// Base class for all arithmetic / logical / equality operators.
    /// </summary>
    public abstract class Operator 
    {
        /// <summary>
        /// The unique token that indicates this operation.
        /// For special character tokens (non alpha-numeric characters) this can be at most 2 characters.
        /// For example '*' for multiply, or '/' for divide.
        /// </summary>
        public abstract string Token { get; }

        /// <summary>
        /// Should execute the operation and return the appropriate construct.
        /// </summary>
        internal abstract Literal Execute(IConstruct argument1, IConstruct argument2);
        
        /// <summary>
        /// Gets the construct and transforms/executes it and returns it as of type T.
        /// If the transformed result is not of type T then an exception is thrown.
        /// Minimise the use of this function because it will traverse and execute the entire expression tree if the construct represents an operation or function.
        /// </summary>
        protected T GetTransformedConstruct<T>(IConstruct construct) where T : Literal
        {
            var transformedConstruct = construct.Transform();

            return CastConstructToType<T>(transformedConstruct);
        }

        private T CastConstructToType<T>(IConstruct construct)
        {
            if (!(construct is T))
                return default(T);

            return (T)construct;
        }

        protected Text Empty()
        {
            return new Text(string.Empty);
        }
    }
}