/*
 *
 * User: github.com/marc365
 * Created: 2016
 */

using System.Linq;

namespace HiSystems.Interpreter
{
    public class Kill : Command
    {
        public override string Name
        {
            get
            {
                return "Kill";
            }
        }

        public override string Description
        {
            get { return "Stop a thread by it's Id number."; }
        }

        public override string Usage
        {
            get { return "{number}"; }
        }

        public override Literal Execute(string[] args)
        {
            string name = string.Format("Thread: {0} ", args[0]);

            var thread = Program.Threads.FirstOrDefault(x => x.Item2.Name.StartsWith(name));
            if (thread != null)
            {
                thread.Item1.RequestStop();
                thread.Item2.Join();
                Program.Threads.Remove(thread);
            }
            else
            {
                return new Error("Thread not found");
            }

            return base.Nothing();
        }
    }
}