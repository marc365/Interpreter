/*
 *
 * User: github.com/marc365
 * Created: 2016
 */

namespace HiSystems.Interpreter
{
    public abstract class Action
    {
        public abstract string Name { get; }

        public abstract string Description { get; }

        public abstract Literal Execute(string action);

        public Action()
        {
        }

        protected Text Nothing()
        {
            return new Text(string.Empty);
        }

        public override string ToString()
        {
            return this.Description;
        }
    }
}