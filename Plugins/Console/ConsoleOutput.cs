using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace HiSystems.Interpreter
{
    public class ConsoleOutput : Action
    {
        public override string Name
        {
            get
            {
                return "ConsoleOutput";
            }
        }

        public override string Description
        {
            get
            {
                return "Displays the output in the console.";
            }
        }

        public override Literal Execute(string output)
        {
            System.Console.Write(output);
            return base.Nothing();
        }
    }
}