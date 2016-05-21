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

using HiSystems.Interpreter.Converters;
using System;
using System.ComponentModel;

namespace HiSystems.Interpreter
{
    [TypeConverter(typeof(NumberTypeConverter))]
    public class Number : Literal
    {
        private decimal value;

        public Number(decimal value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString();
        }
        
        public string ToString(string format)
        {
            return value.ToString(format);
        }
        
        public static implicit operator decimal(Number number)
        {
            return number.value;
        }
        
        public static implicit operator double(Number number)
        {
            return (double)number.value;
        }

        public static implicit operator Number(decimal value)
        {
            return new Number(value);
        }
        
        public static implicit operator Number(double value)
        {
            return new Number((decimal)value);
        }
        
        public static implicit operator Number(int value)
        {
            return new Number((decimal)value);
        }

        public static Literal Parse(string value)
        {
            try
            {
                //otod int.parse with speedup if a '.' to create 2 values then inject int decimal.
                return new Number(Decimal.Parse(value));
            }
            catch(Exception exc)
            {
                return new Error(exc.Message);
            }
        }

        public static Boolean operator==(Number value1, Number value2)
        {
            return AreEqual(value1, value2);
        }
        
        public static Boolean operator!=(Number value1, Number value2)
        {
            return !AreEqual(value1, value2);
        }

        public static Number operator+(Number value1, Number value2)
        {
            return new Number(value1.value + value2.value);
        }
        
        public static Number operator-(Number value1, Number value2)
        {
            return new Number(value1.value - value2.value);
        }

        public static Number operator/(Number value1, Number value2)
        {
            return new Number(value1.value / value2.value);
        }
        
        public static Boolean operator>(Number value1, Number value2)
        {
            return new Boolean(value1.value > value2.value);
        }
        
        public static Boolean operator>=(Number value1, Number value2)
        {
            return new Boolean(value1.value >= value2.value);
        }

        public static Boolean operator<(Number value1, Number value2)
        {
            return new Boolean(value1.value < value2.value);
        }
        
        public static Boolean operator<=(Number value1, Number value2)
        {
            return new Boolean(value1.value <= value2.value);
        }
        
        public static Number operator*(Number value1, Number value2)
        {
            if (value1 == null || value2 == null)
                return new Number(0);

            try
            {
                return new Number(value1.value * value2.value);
            }
            catch
            {
                return new Number(0);
            }
        }

        public static Number operator%(Number value1, Number value2)
        {
            return new Number(value1.value % value2.value);
        }

        public override bool Equals (object obj)
        {
            if (obj == null || !(obj is Number))
                return false;
            else 
                return AreEqual(this, (Number)obj);
        }
        
        private static Boolean AreEqual(Number value1, Number value2)
        {
            if (ReferenceEquals(value1, null) || ReferenceEquals(value2, null))
                return new Boolean(false);
            else
                return new Boolean(value1.value == value2.value);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}