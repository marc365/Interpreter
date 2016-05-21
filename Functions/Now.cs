/*
 *
 * User: github.com/marc365
 * Created: 2016
 */

namespace HiSystems.Interpreter
{
    public class Now : Function
    {
        public override string Name
        {
            get
            {
                return "Now";
            }
        }

        public override string Description
        {
            get { return "Returns the current date and time."; }
        }

        public override string Usage
        {
            get { return "Now()"; }
        }

        public override Literal Execute(IConstruct[] arguments)
        {
            return new DateTime(Execute());
        }

        public static System.DateTime Execute()
        {
            return System.DateTime.UtcNow;
        }
    }
}