/*
 *
 * User: github.com/marc365
 * Created: 2016
 */

using System.ComponentModel;
using System.Globalization;

namespace HiSystems.Interpreter.Converters
{
    public class ErrorTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type sourceType)
        {
            return false;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, System.Type destinationType)
        {
            return destinationType == typeof(string); 
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return false;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, System.Type destinationType)
        {
            var text = value as Text;
            return text == null ? string.Empty : (string)text;
        }
    }
}