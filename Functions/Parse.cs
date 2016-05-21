/*
 *
 * User: github.com/marc365
 * Created: 2016
 */

using System;
using System.Text;

namespace HiSystems.Interpreter
{
    public class Parse : Function
    {
        public override string Name
        {
            get
            {
                return "Parse";
            }
        }

        public override string Description
        {
            get { return "Parse the argument into the interpreter or reads the keyboard entry into the interpreter."; }
        }

        public override string Usage
        {
            get { return "Parse({text})"; }
        }

        public override Literal Execute(IConstruct[] arguments)
        {
            if (base.EnsureArgumentCountIs(arguments, 1))
            {
                var input = base.GetArgument(arguments, argumentIndex: 0);

                var output = Execute(input.ToString());

                //check if string was evaluated
                if (input.ToString().Replace(" ", "") == output.ToString())
                {
                    return new Text(input.ToString());
                }
                else
                {
                    return output;
                }
            }
            else
            {
                var input = (Literal)Execute("_Input");

                var output = Execute(input.ToString());

                //check if string was evaluated
                if (input.ToString().Replace(" ", "") == output.ToString())
                {
                    return base.Nothing(); // new Text(input.ToString());
                }
                else
                {
                    return output;
                }
            }
        }

        public static Literal Execute(string script)
        {
            return Program.engine.Parse(script).Execute<Literal>();
        }

        //an optimized int parse method.
        public static int Int(string value)
        {
            
            int result = 0;
            for (int i = 0; i < value.Length; i++)
            {
                result = 10 * result + (value[i] - 48);
            }
            return result;
        }

        public static byte[] GetBytes(string text)
        {
            if (text == null) return new byte[0];
            return Encoding.ASCII.GetBytes(text);
        }

        public static string GetString(byte[] bytes)
        {
            if (bytes == null) return string.Empty;
            return Encoding.ASCII.GetString(bytes);
        }

        public static string GetUTF8String(byte[] bytes)
        {
            if (bytes == null) return string.Empty;
            return Encoding.UTF8.GetString(bytes);
        }

        public static byte[] GetUTF8Bytes(string text)
        {
            if (text == null) return new byte[0];
            return Encoding.UTF8.GetBytes(text);
        }
    }
}