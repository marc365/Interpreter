/*
 *
 * User: github.com/marc365
 * Created: 2016
 */

namespace HiSystems.Interpreter
{
    public class Exit : Command
    {
        public override string Name
        {
            get
            {
                return "Exit";
            }
        }

        public override string Description
        {
            get { return "Stop all threads and quit."; }
        }

        public override string Usage
        {
            get { return ""; }
        }

        public override Literal Execute(string[] args)
        {
            System.Environment.Exit(0);

            return null;
        }
    }
}