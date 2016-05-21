/*
 *
 * User: github.com/marc365
 * Updated: 2016
 */

using System.IO;

namespace HiSystems.Interpreter
{
    public class Read : Function
    {
        public override string Name
        {
            get
            {
                return "Read";
            }
        }

        public override string Description
        {
            get { return "Read data from a file or returns data from text."; }
        }

        public override string Usage
        {
            get { return "Read({filename}|{text})"; }
        }

        public override Literal Execute(IConstruct[] arguments)
        {
            if (base.EnsureArgumentCountIs(arguments, 1))
            {
                return Execute((Text)base.GetTransformedArgument<Literal>(arguments, argumentIndex: 0));
            }
            else
            {
                return base.Error(1); //Wrong number of arguments
            }
        }

        public static Literal Execute(string arg)
        {
            string path = Path.Execute(string.Format("~/{0}", arg));

            if (path != string.Empty && File.Exists(path))
            {
                return new ByteArray(File.ReadAllBytes(Path.Execute(arg)));
            }
            else
            {
                return new ByteArray(Parse.GetBytes(arg));
            }
        }

    }
}