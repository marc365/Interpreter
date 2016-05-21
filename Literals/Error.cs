/*
 *
 * User: github.com/marc365
 * Created: 2016
 */

using HiSystems.Interpreter.Converters;
using System;
using System.ComponentModel;

namespace HiSystems.Interpreter
{
    [TypeConverter(typeof(ErrorTypeConverter))]
    public class Error : Literal
    {
        private string value;

        public Error(string value)
        {
            this.value = value;
        }
        
        public static implicit operator string(Error text)
        {
            return text.value;
        }

        public static implicit operator Error(string text)
        {
            return new Error(text);
        }

        public static Boolean operator ==(Error value1, Error value2)
        {
            return AreEqual(value1, value2);
        }

        public static Boolean operator !=(Error value1, Error value2)
        {
            return !AreEqual(value1, value2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Error))
                return false;
            else
                return AreEqual(this, (Error)obj);
        }

        private static Boolean AreEqual(Error value1, Error value2)
        {
            if (ReferenceEquals(value1, null) || ReferenceEquals(value2, null))
                return new Boolean(false);
            else
                return new Boolean(value1.value.Equals(value2.value, StringComparison.InvariantCulture));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        } 
        
        public override string ToString()
        {
            return value.ToString();
        }
    }
}