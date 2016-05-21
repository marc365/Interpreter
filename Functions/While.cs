/*
 *
 * User: github.com/marc365
 * Created: 2016
 */

using System;
using System.Threading;
using System.Linq;

namespace HiSystems.Interpreter
{
    public class While : Function
    {
        public IConstruct[] backgroundArguments;

        public override string Name
        {
            get
            {
                return "While";
            }
        }

        public override string Description
        {
            get { return "Run a background thread to repeat an expression while the condition is true."; }
        }

        public override string Usage
        {
            get { return "While(condition, expression)"; }

        }

        public string ThreadName { get; set;}

        public void BackgroundWork()
        {
            Literal value;
            Literal expression;
            bool condition = true;

            while (!_shouldStop && condition)
            {
                expression = base.GetTransformedArgument<Literal>(backgroundArguments, argumentIndex: 1);

                Output.Text(expression.ToString());

                value = base.GetTransformedArgument<Literal>(backgroundArguments, argumentIndex: 0);

                if (!bool.TryParse(value.ToString(), out condition))
                {
                    condition = false;
                }

                Thread.Sleep(0);
            }

            var thisThread = Program.Threads.FirstOrDefault(x => x.Item2.Name == ThreadName);

            if (thisThread != null)
            {
                Program.Threads.Remove(thisThread);
            }
        }

        public void RequestStop()
        {
            _shouldStop = true;
        }

        private volatile bool _shouldStop;

        public override Literal Execute(IConstruct[] arguments)
        {
            Literal value = base.GetTransformedArgument<Literal>(arguments, argumentIndex: 0);
            bool condition = true;

            if (value != null)
            {
                if (!bool.TryParse(value.ToString(), out condition))
                {
                    condition = false;
                }

                if (condition)
                {
                    While worker = new While();
                    worker.backgroundArguments = arguments;
                    Thread thread = thread = new Thread(worker.BackgroundWork);
                    worker.ThreadName = string.Format("Thread: {0} {1}", (Program.Threads.Count + 1).ToString(), (IConstruct)base.GetArgument(arguments, argumentIndex: 1)); ;
                    thread.Name = worker.ThreadName;
                    if (Program.Threads.Count == 0)
                    {
                        //system fails if the first task IsBackground
                    }
                    else
                    {
                        thread.IsBackground = true;
                    }

                    Program.Threads.Add(new Tuple<While, Thread>(worker, thread));
                    thread.Start();
                }

                return base.Nothing();
            }
            else
            {
                return base.Error(2); //Incorrect arguments
            }

        }
    }
}