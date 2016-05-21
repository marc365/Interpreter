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

using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace HiSystems.Interpreter.Converters
{
    public class NumberTypeConverter : TypeConverter
    {
        private readonly IEnumerable<System.Type> _supportedTypes = new[] { typeof(int), typeof(double), typeof(decimal), typeof(Literal) };

        public override bool CanConvertTo(ITypeDescriptorContext context, System.Type destinationType)
        {
            foreach (var type in _supportedTypes)
            {
                if (destinationType == type)
                    return true;
            }
            return false;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type sourceType)
        {
            return sourceType == typeof(Number);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return value == null ? new Number(0) : new Number((decimal)value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, System.Type destinationType)
        {
            var number = value as Number;

            if (destinationType == typeof(int))
            {
                return number == null ? default(int) : (int)(decimal)number;
            }

            if (destinationType == typeof(decimal))
            {
                return number == null ? default(decimal) : (decimal)number;
            }

            return number == null ? default(double) : (double)number;
        }
    }
}