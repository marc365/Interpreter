/*
 *
 * User: github.com/marc365
 * Created: 2016
 */

namespace HiSystems.Interpreter
{
    public class If : Function
    {
        public override string Name
        {
            get
            {
                return "If";
            }
        }

        public override string Description
        {
            get { return "Conditional evaluation of the expression based on the condition."; }
        }

        public override string Usage
        {
            get { return "If(condition, expression, expression)"; }
        }

        public override Literal Execute(IConstruct[] arguments)
        {
            if (base.EnsureArgumentCountIs(arguments, 3))
            {
                var condition = base.GetTransformedArgument<Boolean>(arguments, argumentIndex: 0);

                bool yesno;

                if (condition != null && bool.TryParse(condition.ToString(), out yesno))
                {
                    if (yesno)
                        return base.GetTransformedArgument<Literal>(arguments, argumentIndex: 1);
                    else
                        return base.GetTransformedArgument<Literal>(arguments, argumentIndex: 2);
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
    }
}