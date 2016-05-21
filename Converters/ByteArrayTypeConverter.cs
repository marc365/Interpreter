/*
 *
 * User: github.com/marc365
 * Created: 2016
 */

using System.ComponentModel;
using System.Globalization;

namespace HiSystems.Interpreter.Converters
{
    public class ByteArrayTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type sourceType)
        {
            return sourceType == typeof(byte[]);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, System.Type destinationType)
        {
            return destinationType == typeof(byte[]) || destinationType == typeof(Literal); 
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return new ByteArray((value as byte[]) ?? default(byte[]));
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, System.Type destinationType)
        {
            var @byte = value as ByteArray;
            return @byte == null ? default(byte[]) : (byte[])@byte;
        }
    }
}