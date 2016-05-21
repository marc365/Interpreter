/*
 *
 * User: github.com/marc365
 * Updated: 2016
 */

namespace HiSystems.Interpreter
{
    public class Beep : Command
    {
        public override string Name
        {
            get
            {
                return "Beep";
            }
        }

        public override string Description
        {
            get { return "Beep."; }
        }

        public override string Usage
        {
            get { return ""; }
        }

        public override Literal Execute(string[] arguments)
        {
            Execute();

            return base.Nothing();
        }

        public static void Execute()
        {
            System.Console.Beep();
        }
    }
}