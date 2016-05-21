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

namespace HiSystems.Interpreter
{
    /// <summary>
    /// Represents a left hand side value an operation and a right hand side value.
    /// </summary>
    public class Operation : IConstruct
    {
        private const int PrecedenceUndefined = -1;

        private IConstruct leftValue;
        private Operator @operator;
        private IConstruct rightValue;
        private int precedence = PrecedenceUndefined;

        internal Operation()
        {
        }

        /// <summary>
        /// Indicates the left hand side value, and right hand side values
        /// and the operation to be applied to the left hand side and right 
        /// hand side values.
        /// </summary>
        public Operation(IConstruct leftValue, Operator operation, IConstruct rightValue)
        {
            if (leftValue == null || rightValue == null || operation == null)
                throw new ArgumentNullException();

            this.leftValue = leftValue;
            this.@operator = operation;
            this.rightValue = rightValue;
        }

        Literal IConstruct.Transform ()
        {
            if (this.@operator == null)
            {
                this.@operator = new AddOperator();
            }

            return this.@operator.Execute(this.leftValue, this.rightValue);
        }

        /// <summary>
        /// Represents the relatively ordering precedence for this expression.
        /// The precedence is not an absolute value but a relative value.
        /// If two expressions have the same precedence then they are evaluated from left to right.
        /// The highest precedence value indicates that the expression will be executed first.
        /// </summary>
        internal int Precedence
        {
            get
            {
                return this.precedence;
            }

            set
            {
                if (value <= PrecedenceUndefined)
                    throw new ArgumentException(value.ToString());

                this.precedence = value;
            }
        }

        internal bool PrecedenceIsSet
        {
            get
            {
                return this.precedence != PrecedenceUndefined;
            }
        }

        /// <summary>
        /// The left-hand-side value that the operation will use.
        /// </summary>
        public IConstruct LeftValue
        {
            get
            {
                return this.leftValue;
            }

            internal set
            {
                if (value == null)
                    throw new ArgumentNullException();

                this.leftValue = value;
            }
        }

        /// <summary>
        /// The operator that will be executed when this operation is executed.
        /// </summary>
        public Operator Operator
        {
            get
            {
                return this.@operator;
            }

            internal set
            {
                if (value == null)
                    throw new ArgumentNullException();

                this.@operator = value;
            }
        }

        /// <summary>
        /// The right-hand-side value that the operation will use.
        /// </summary>
        public IConstruct RightValue
        {
            get
            {
                return this.rightValue;
            }

            internal set
            {
                if (value == null)
                    throw new ArgumentNullException();

                this.rightValue = value;
            }
        }
        
        public override string ToString()
        {
            try
            {
                if (@operator == null)
                {
                    return this.leftValue.ToString() + " " + this.rightValue.ToString();
                }
            }
            catch(Exception exc)
            {
                return exc.Message;
            }

            try
            {
                return this.leftValue.ToString() + " " + this.@operator.Token + " " + this.rightValue.ToString();
            }
            catch (Exception exc)
            {
                return exc.Message;
            }

        }
    }
}