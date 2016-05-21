/*
 *
 * User: github.com/marc365
 * Created: 2016
 */

using System;
using System.Security.Cryptography;

namespace HiSystems.Interpreter
{
    public class Random : Function
    {
        static byte[] v = new byte[4];

        public static RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider();

        public override string Name
        {
            get
            {
                return "Random";
            }
        }

        public override string Description
        {
            get { return "Returns a random number below the specified maximum."; }
        }

        public override string Usage
        {
            get { return "{maxvalue}"; }
        }

        public override Literal Execute(IConstruct[] arguments)
        {
            base.EnsureArgumentCountIs(arguments, 1);

            var value = base.GetTransformedArgument<Number>(arguments, argumentIndex: 0).ToString();

            try
            {
                return new Number(Next(Parse.Int(value)));
            }
            catch (Exception exc)
            {
                return new Error(exc.Message);
            }
        }

        public static byte[] GetBytes(byte[] bytes)
        {
            rand.GetBytes(bytes);

            return bytes;
        }

        public static int Next(int maxValue)
        {
            rand.GetBytes(v);
            int t = BitConverter.ToInt32(v, 0);
            if (t < 0)
            {
                t = t * -1;
            }
            return t % maxValue;
        }
    }
}