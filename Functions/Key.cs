/*
 *
 * User: github.com/marc365
 * Created: 2016
 */

namespace HiSystems.Interpreter
{
    public class Key: Function
    {
        public override string Name
        {
            get
            {
                return "Key";
            }
        }

        public override string Description
        {
            get { return "Returns true if the keyboard input has an available key."; }
        }

        public override string Usage
        {
            get { return "Key()"; }
        }

        public override Literal Execute(IConstruct[] arguments)
        {
            return (Boolean)Execute();
        }

        public static bool Execute()
        {
            return System.Console.KeyAvailable;
        }
    }
}