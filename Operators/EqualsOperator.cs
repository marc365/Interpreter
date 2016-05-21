/*
 *
 * User: github.com/marc365
 * Created: 2016
 */

using System.Linq;

namespace HiSystems.Interpreter
{
    /// <summary>
    /// Sets a variable
    /// Usage: 
    ///   variableName = numericValue
    ///   variableName = booleanValue
    ///   variableName = text
    ///   variableName = dateTime
    /// Examples:
    ///   count = 2
    ///   finished = false
    ///   letter = 'b'
    ///   when = #2000-1-2#
    /// </summary>
    public class EqualsOperator : Operator
    {
        public EqualsOperator()
        {
        }

        internal override Literal Execute(IConstruct argument1, IConstruct argument2)
        {
            var argument2Transformed = base.GetTransformedConstruct<Literal>(argument2);

            var existingVariable = Program.Variables.SingleOrDefault(x => x.Name == argument1.ToString().Split(':')[0]);

            if (existingVariable == null)
            {
                Program.Variables.Add(new Variable(argument1.ToString().Split(':')[0], argument2Transformed));
            }
            else
            {
                existingVariable.Construct = argument2Transformed;
            }

            return base.Empty();
            
        }

        public override string Token
        {
            get 
            {
                return "=";
            }
        }
    }
}
