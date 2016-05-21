/*
 *
 * User: github.com/marc365
 * Created: 2016
 */

using System;

namespace HiSystems.Interpreter
{
    public class Del : Command
    {
        public override string Name
        {
            get
            {
                return "Del";
            }
        }

        public override string Description
        {
            get { return "Delete a file"; }
        }

        public override string Usage
        {
            get { return "{filename}"; }
        }

        public override Literal Execute(string[] args)
        {
            if (base.EnsureArgumentCountIs(args, 1))
            {
                if (System.IO.File.Exists(Path.Execute(args[0])))
                {
                    Execute(Path.Execute(args[0]));

                    return new Text(string.Format("{0} deleted successfully", args[0]));
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

        public static void Execute(string arg)
        {
            try
            {
                System.IO.File.Delete(arg);
            }
            catch (Exception exc)
            {
                Output.Text(exc.Message);
            }
        }
    }
}