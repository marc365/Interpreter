/*
 *
 * User: github.com/marc365
 * Updated: 2016
 */

using System;

namespace HiSystems.Interpreter
{
    public class Fasthash : Function
    {
        public override string Name
        {
            get
            {
                return "Fasthash";
            }
        }

        public override string Description
        {
            get { return "Returns the hash of the data quickly."; }
        }

        public override string Usage
        {
            get { return "Fasthash({data})"; }
        }

        public override Literal Execute(IConstruct[] arguments)
        {
            if (base.EnsureArgumentCountIs(arguments, 1))
            {
                var data = (Literal)base.GetArgument(arguments, argumentIndex: 0).Transform();

                if (data is ByteArray)
                {
                    return (Number)(int)Execute(Parse.GetString((ByteArray)data));
                }
                if (data is Text)
                {
                    return (Number)(int)Execute((Text)data);
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

        //public static Literal Execute(string data)
        //{
        //    return new Number((int)Execute(data));
        //}

        //CalculateHash //http://stackoverflow.com/questions/9545619/a-fast-hash-function-for-string-in-c-sharp
        public static UInt32 Execute(string read)
        {
            UInt32 hashedValue = 0;
            int i = 0;
            UInt32 multiplier = 1;
            while (i < read.Length)
            {
                hashedValue += read[i] * multiplier;
                multiplier *= 37;
                i += 2;
            }
            return hashedValue;
        }

    }


}
