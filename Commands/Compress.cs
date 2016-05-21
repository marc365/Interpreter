/*
 *
 * User: github.com/marc365
 * Created: 2016
 */

namespace HiSystems.Interpreter
{
    public class Compress : Command
    {
        public override string Name
        {
            get
            {
                return "Compress";
            }
        }

        public override string Description
        {
            get { return "Compress a file using the chosen algorithm (Gzip by default)."; }
        }

        public override string Usage
        {
            get { return "{filename} ({gzip|deflate|...})"; }

        }
        public override Literal Execute(string[] args)
        {
            if (args.Length == 1)
            {
                return Parse.Execute(string.Format("write(\"{0}.gz\", gzip(read(\"{0}\"), true))", args[0]));
            }
            else if (args.Length == 2)
            {
                return Parse.Execute(string.Format("write(\"{0}.{1}\", {2}(read(\"{0}\"), true))", args[0], args[1].Substring(0, 3), args[1]));
            }
            else
            {
                return new Error("Wrong numer of arguments");
            }


        }
    }
}