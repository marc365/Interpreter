/*
 *
 * User: github.com/marc365
 * Created: 2016
 */

using HiSystems.Interpreter.Converters;
using System;
using System.ComponentModel;
using System.Text;

namespace HiSystems.Interpreter
{
    [TypeConverter(typeof(ByteArrayTypeConverter))]
    public class ByteArray : Literal
    {
        private byte[] value;

        public ByteArray(byte[] value)
        {
            this.value = value;
        }

        public static implicit operator byte[](ByteArray @byte)
        {
            return @byte.value;
        }

        public static implicit operator ByteArray(byte[] @byte)
        {
            return new ByteArray(@byte);
        }

        public static Boolean operator ==(ByteArray value1, ByteArray value2)
        {
            return AreEqual(value1, value2);
        }

        public static Boolean operator !=(ByteArray value1, ByteArray value2)
        {
            return !AreEqual(value1, value2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is ByteArray))
                return false;
            else
                return AreEqual(this, (ByteArray)obj);
        }

        private static Boolean AreEqual(ByteArray value1, ByteArray value2)
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
        
        public override string ToString()
        {
            return Encoding.UTF8.GetString(value);
        }
    }
}