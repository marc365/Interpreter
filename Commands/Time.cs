/*
 *
 * User: github.com/marc365
 * Created: 2016
 */

namespace HiSystems.Interpreter
{
    public class Time : Command
    {
        public override string Name
        {
            get
            {
                return "Time";
            }
        }

        public override string Description
        {
            get { return "Returns the local time and date."; }
        }

        public override string Usage
        {
            get { return ""; }
        }

        public override Literal Execute(string[] args)
        {
            if (base.EnsureArgumentCountIs(args, 0))
            {
                return Parse.Execute("output(format(now(), \"ddd d MMMM yyyy HH:mm:ss\"))");
            }
            else
            {
                return base.Error(1); //Wrong number of arguments
            }
        }
    }
}