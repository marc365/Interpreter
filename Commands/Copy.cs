/*
 *
 * User: github.com/marc365
 * Created: 2016
 */

namespace HiSystems.Interpreter
{
    public class Copy : Command
    {
        public override string Name
        {
            get
            {
                return "Copy";
            }
        }

        public override string Description
        {
            get { return "Copy a file."; }
        }

        public override string Usage
        {
            get { return "{filename}"; }
        }

        public override Literal Execute(string[] args)
        {
            if (base.EnsureArgumentCountIs(args, 2))
            {
                if (System.IO.File.Exists(Path.Execute(args[1])))
                {
                    return new Error("Destination file already exists");
                }

                if (System.IO.File.Exists(Path.Execute(args[0])))
                {
                    System.IO.File.Copy(Path.Execute(args[0]), Path.Execute(args[1]));

                    return new Text(string.Format("{0} copied to {1} successfully", args[0], args[1]));
                }
                else
                {
                    return new Error("File not found");
                }
            }
            else
            {
                return base.Error(1); //Wrong number of arguments
            }
        }
    }
}