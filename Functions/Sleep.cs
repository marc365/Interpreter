/*
 *
 * User: github.com/marc365
 * Created: 2016
 */

using System.Threading;

namespace HiSystems.Interpreter
{
    public class Sleep : Function
    {
        public override string Name
        {
            get
            {
                return "Sleep";
            }
        }

        public override string Description
        {
            get { return "Wait for n milliseconds."; }
        }

        public override string Usage
        {
            get { return "Sleep(milliseconds)"; }
        }

        //todo merge Sleep ans Wait functions.
        public override Literal Execute(IConstruct[] arguments)
        {
            if (base.EnsureArgumentCountIs(arguments, 1))
            {
                var value = base.GetTransformedArgument<Number>(arguments, argumentIndex: 0);

                if (value != null)
                {
                    Milliseconds(Parse.Int(value.ToString()));

                    return base.Nothing();
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

        public static void Milliseconds(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }
    }
}