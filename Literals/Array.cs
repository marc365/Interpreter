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

using System.Collections.Generic;
using System.Linq;

namespace HiSystems.Interpreter
{
    public class Array : Literal, IEnumerable<IConstruct>
    {
        private List<IConstruct> items = new List<IConstruct>();
        
        public Array(decimal[] values)
        {
            items.AddRange(values.Select(item => (Number)item).ToArray());
        }

        public Array(IConstruct[] values)
        {
            items.AddRange(values);
        }
        
        public static implicit operator List<IConstruct>(Array array)
        {
            return array.items;
        }
        
        public static implicit operator Array(IConstruct[] constructs)
        {
            return new Array(constructs);
        }

        public override string ToString()
        {
            return "Array";
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
        {
            return items.GetEnumerator();
        }

        IEnumerator<IConstruct> IEnumerable<IConstruct>.GetEnumerator ()
        {
            return items.GetEnumerator();
        }
    }
}
