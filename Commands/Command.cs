/*
 *
 * User: github.com/marc365
 * Created: 2016
 */

using System;

namespace HiSystems.Interpreter
{
    public abstract class Command
    {
        public abstract string Name { get; }

        public abstract string Description { get; }

        public abstract string Usage { get; }

        public abstract Literal Execute(string[] arguments);

        public Command()
        {
        }

        protected bool EnsureArgumentCountIs(string[] arguments, int expectedArgumentCount)
        {
            return expectedArgumentCount == arguments.Length;
        }
        
        protected bool EnsureArgumentCountIsAtLeast(string[] arguments, int minimumArgumentCount)
        {
            if (minimumArgumentCount > arguments.Length)
            {
                Output.Text(String.Format("{0} has been supplied {1} arguments, but expects at least {2} arguments", this.Name, arguments.Length, minimumArgumentCount));
                return false;
            }
            else
            {
                return true;
            }
        }
        
        protected Text Nothing()
        {
            return new Text(string.Empty);
        }

        //todo centralize error
        protected Error Error(int errNumber)
        {
            switch (errNumber)
            {
                case 1:
                    Output.Text(string.Format("{0} {1}", Repo.s, this.Name));
                    Output.Text(string.Format("{0} {1}", Repo.s, this.Description));
                    Output.Text(string.Format("{0} {1}", Repo.s, this.Usage));
                    return new Error("Wrong number of arguments");
                case 2:
                    Output.Text(string.Format("{0} {1}", Repo.s, this.Name));
                    Output.Text(string.Format("{0} {1}", Repo.s, this.Usage));
                    return new Error("Incorrect arguments");
                default:
                    return new Error("Error without a message");
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}