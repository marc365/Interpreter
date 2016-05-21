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
using System.Linq;

namespace HiSystems.Interpreter
{
    public abstract class Function
    {
        public abstract string Name { get; }

        public abstract string Description { get; }

        public abstract string Usage { get; }

        public abstract Literal Execute(IConstruct[] arguments);

        public Function()
        {
        }

        protected bool EnsureArgumentCountIs(IConstruct[] arguments, int expectedArgumentCount)
        {
            return expectedArgumentCount == arguments.Length;
        }

        protected Text Nothing()
        {
            return new Text(null);
        }

        //todo centralize error
        protected Error Error(int errNumber)
        {
            switch (errNumber)
            {
                case 1:              
                    Output.Text(string.Format("{0} {1}", Repo.s, this.Name));
                    Output.Text(string.Format("{0} {1}", Repo.s, this.Usage));
                    return new Error(string.Format("{0} Wrong number of arguments", Repo.s));
                case 2:
                    Output.Text(string.Format("{0} {1}", Repo.s, this.Name));
                    Output.Text(string.Format("{0} {1}", Repo.s, this.Usage));
                    return new Error(string.Format("{0} Incorrect arguments", Repo.s));
                default:
                    return new Error(string.Format("{0} Error without a message", Repo.s));
            }
        }

        protected bool EnsureArgumentCountIsAtLeast(IConstruct[] arguments, int minimumArgumentCount)
        {
            if (minimumArgumentCount > arguments.Length)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        
        protected bool EnsureArgumentCountIsBetween(IConstruct[] arguments, int minimumArgumentCount, int maximumArgumentCount)
        {
            EnsureArgumentCountIsAtLeast(arguments, minimumArgumentCount);

            if (maximumArgumentCount < arguments.Length)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        
        protected decimal[] GetTransformedArgumentDecimalArray(IConstruct[] arguments, int argumentIndex)  
        {
            return this.GetTransformedArgumentArray<Number>(arguments, argumentIndex).Select(number => (decimal)number).ToArray();
        }

		protected T[] GetTransformedArgumentArray<T>(IConstruct[] arguments, int argumentIndex) where T : Literal
		{
			var argument = GetArgument(arguments, argumentIndex);
			var transformedArgument = argument.Transform();

			var transformedArray = CastArgumentToType<Array>(transformedArgument, argumentIndex).Select(construct => construct.Transform());

			if (!transformedArray.All(item => item is T))
				throw new InvalidOperationException(String.Format("{0} argument {1} does not contain only {2} values and cannot be used with the {3} function", argument.ToString(), argumentIndex + 1, typeof(T).Name, this.Name));

			return transformedArray.Cast<T>().ToArray();
		}
  
        protected Literal GetTransformedArgument<T>(IConstruct[] arguments, int argumentIndex) where T : Literal
        {
            var argument = GetArgument(arguments, argumentIndex);
            var transformedArgument = argument.Transform();

            if (arguments.Length == 0 && transformedArgument.ToString().StartsWith("Syntax Error:"))
            {
                return CastArgumentToType<Error>(transformedArgument, argumentIndex);
            }

            return CastArgumentToType<T>(transformedArgument, argumentIndex);
        }

		public IConstruct GetArgument(IConstruct[] arguments, int argumentIndex) 
		{
			if (argumentIndex >= arguments.Length)
			{
				return new Error(String.Format("Syntax Error: Function {0} is missing argument {1}.", this.Name, argumentIndex + 1));
			}
			return arguments[argumentIndex];
		}

        private T CastArgumentToType<T>(IConstruct argument, int argumentIndex)
        {
            return !(argument is T) ? default(T) : (T)argument;
        }

        public override string ToString()
        {
            return this.Name + "()";
        }
    }
}