/*
 *
 * User: github.com/marc365
 * Created: 2016
 */

namespace HiSystems.Interpreter
{
    public class Threads : Command
    {
        public override string Name
        {
            get
            {
                return "Threads";
            }
        }

        public override string Description
        {
            get { return "Returns information about the current threads."; }
        }

        public override string Usage
        {
            get { return ""; }
        }

        public override Literal Execute(string[] args)
        {
            if (base.EnsureArgumentCountIs(args, 0))
            {
                string output = string.Empty;
                foreach (var a in Program.Threads)
                {
                    output += a.Item2.Name + Repo.nl;
                }
                return new Text(output);
            }
            else
            {
                return base.Error(1); //Wrong number of arguments
            }
        }
    }
}