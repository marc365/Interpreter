using System;
using System.Diagnostics;
using System.Text;

namespace Console.Interpreter
{
    public class Bench : Function
    {
        public override string Name
        {
            get
            {
                return "Bench";
            }
        }

        public override string Description
        {
            get { return "Some bench marks."; }
        }

        public override string Usage
        {
            get { return "Bench()"; }
        }

        public override Literal Execute(IConstruct[] arguments)
        {
            return new Text("Success!");
        }
    }
}