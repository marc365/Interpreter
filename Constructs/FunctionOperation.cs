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
    public class FunctionOperation : IConstruct
    {
        private Function function;
        private IConstruct[] arguments = null;

        public FunctionOperation(Function function, IConstruct[] arguments)
        {
            this.function = function;
            this.arguments = arguments;
        }
        
        Literal IConstruct.Transform()
        {
            return function.Execute(this.arguments);
        }

        public Function Function
        {
            get
            {
                return this.function;
            }
        }

        public IConstruct[] Arguments
        {
            get
            {
                return this.arguments;
            }
        }

        public override string ToString()
        {
            return this.function.ToString();
        }
    }
}