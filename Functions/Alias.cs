/*
 *
 * User: github.com/marc365
 * Updated: 2016
 */

using System;
using System.Linq;

namespace HiSystems.Interpreter
{
    public class Alias : Function
    {
        public override string Name
        {
            get
            {
                return "Alias";
            }
        }

        public override string Description
        {
            get { return "Set a global variable."; }
        }

        public override string Usage
        {
            get { return "Alias(name, construct)"; }
        }

        public override Literal Execute(IConstruct[] arguments)
        {
            if (base.EnsureArgumentCountIs(arguments, 2))
            {
                var value = base.GetArgument(arguments, argumentIndex: 0);
                var expression = base.GetArgument(arguments, argumentIndex: 1);

                var existingVariable = Program.Variables.SingleOrDefault(x => x.Name == value.ToString().Split(':')[0]);

                if (existingVariable == null)
                {
                    Program.Variables.Add(new Variable(value.ToString().Split(':')[0], expression));
                }
                else
                {
                    existingVariable.Construct = expression;
                }

                return base.Nothing();               
            }
            else
            {
                string output = string.Empty;
                foreach (var item in Program.Variables)
                {
                    output = string.Format("{0}{1} {2} {3}", output, item.Name, item.Construct != null ? item.Construct.GetType().Name : "<null>", Environment.NewLine);
                }

                return new Text(output);
            }
        }
    }
}