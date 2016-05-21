/*
 *
 * User: github.com/marc365
 * Updated: 2016
 */

namespace HiSystems.Interpreter
{
    public class Len : Function
    {
        public override string Name
        {
            get
            {
                return "Len";
            }
        }

        public override string Description
        {
            get { return "Returns the length of the text."; }
        }

        public override string Usage
        {
            get { return "Len({text})"; }
        }

        public override Literal Execute(IConstruct[] arguments)
        {
            if (base.EnsureArgumentCountIs(arguments, 1))
            {
                var text = (Literal)base.GetArgument(arguments, argumentIndex: 0).Transform();

                return (Number)Execute(text.ToString());
            }
            else
            {
                return base.Error(1); //Wrong number of arguments
            }
        }

        public static int Execute(string text)
        {
            return text.Length;
        }
    }
}