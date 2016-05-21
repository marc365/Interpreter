/*
 *
 * User: github.com/marc365
 * Created: 2016
 */

using System.Linq;

namespace HiSystems.Interpreter
{
    public class Inc : Function
    {
        public override string Name
        {
            get
            {
                return "Inc";
            }
        }

        public override string Description
        {
            get { return "Increment the variable."; }
        }

        public override string Usage
        {
            get { return "Inc({variable})"; }
        }

        public override Literal Execute(IConstruct[] arguments)
        {
            if (base.EnsureArgumentCountIs(arguments, 1))
            {
                var value = base.GetArgument(arguments, argumentIndex: 0);

                var newNumber = new Number(Parse.Int(base.GetTransformedArgument<Number>(arguments, argumentIndex: 0).ToString()) + 1);

                if (newNumber != null)
                {
                    var existingVariable = Program.Variables.SingleOrDefault(x => x.Name == value.ToString().Split(':')[0]);

                    if (existingVariable != null)
                    {
                        existingVariable.Construct = new Number(newNumber);
                    }
                }
                return base.Nothing();
            }
            else
            {
                return base.Error(1); //Wrong number of arguments
            }
        }
    }
}