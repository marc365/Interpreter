/*
 *
 * User: github.com/marc365
 * Updated: 2016
 */

namespace HiSystems.Interpreter
{
    public class Guid : Function
    {
        public override string Name
        {
            get
            {
                return "Guid";
            }
        }

        public override string Description
        {
            get { return "Return a machine generated guid."; }
        }

        public override string Usage
        {
            get { return "Guid()"; }
        }

        public override Literal Execute(IConstruct[] arguments)
        {
            return new Text(Execute());
        }

        public static string Execute()
        {
            return System.Guid.NewGuid().ToString();
        }
    }
}