/*
 *
 * User: github.com/marc365
 * Updated: 2016
 */

using System.Security.Cryptography;
using System.Text;

namespace HiSystems.Interpreter
{
    public class Salt : Function
    {
        static RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider();
        static byte[] v = new byte[4];
        static byte[] r;
        static char[] letters = Shuffle.Execute<char>("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray());
        static char[] numbers;
        static StringBuilder result;

        public Salt()
        {
            rand.GetBytes(v);
            numbers = Shuffle.Execute<char>("0123456789".ToCharArray());
        }

        public override string Name
        {
            get
            {
                return "Salt";
            }
        }

        public override string Description
        {
            get { return "Returns the specified amount of random text."; }
        }

        public override string Usage
        {
            get { return "Salt({size})"; }
        }

        public override Literal Execute(IConstruct[] arguments)
        {
            if (base.EnsureArgumentCountIs(arguments, 1))
            {
                var data = (Literal)base.GetArgument(arguments, argumentIndex: 0).Transform();

                if (data is Number)
                {
                    return (Text)Execute(Parse.Int((Text)data.ToString()));
                }
                else
                {
                    return base.Error(2); //Incorrect arguments
                }
            }
            else
            {
                return base.Error(1); //Wrong number of arguments
            }
        }

        public static Literal Execute(int charaters)
        {
            return new Text(Word(charaters));
        }

        public static string Word(int characters)
        {
            r = Data(characters);
            result = new StringBuilder(characters);
            foreach (byte b in r)
            {
                result.Append(letters[b % letters.Length]);
            }
            return result.ToString();
        }

        public static int Number(int characters)
        {
            r = Data(characters);
            result = new StringBuilder(characters);
            foreach (byte b in r)
            {
                result.Append(numbers[b % numbers.Length]);
            }
            return Parse.Int(result.ToString());
        }

        public static byte[] Data(int characters)
        {
            r = new byte[characters];
            rand.GetBytes(r);
            return r;
        }

    }


}