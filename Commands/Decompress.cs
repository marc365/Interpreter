/*
 *
 * User: github.com/marc365
 * Created: 2016
 */

namespace HiSystems.Interpreter
{
    public class Decompress : Command
    {
        public override string Name
        {
            get
            {
                return "Decompress";
            }
        }

        public override string Description
        {
            get { return "Decompress a file using the chosen algorithm (Gzip by default)."; }
        }

        public override string Usage
        {
            get { return "{infilename} {outfilename} ({gzip|deflate|...})"; }

        }
        public override Literal Execute(string[] args)
        {
            if (args.Length == 2)
            {
                return Parse.Execute(string.Format("write(\"{1}\", gzip(read(\"{0}\"), false))", args[0], args[1]));
            }
            if (args.Length == 3)
            {
                return Parse.Execute(string.Format("write(\"{1}\", {2}(read(\"{0}\"), false))", args[0], args[1], args[2]));
            }
            else
            {
                return base.Error(1); //Wrong number of arguments
            }
        }
    }
}