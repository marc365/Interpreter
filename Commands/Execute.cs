/*
 *
 * User: github.com/marc365
 * Created: 2016
 */

namespace HiSystems.Interpreter
{
    public class ExecuteCommand : Command
    {
        public override string Name
        {
            get
            {
                return "Execute";
            }
        }

        public override string Description
        {
            get { return "Evaluate a script file."; }
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
                    foreach (var line in System.IO.File.ReadAllLines(Path.Execute(args[0])))
                    {
                        if (line.Trim().StartsWith("#") || line.Trim().StartsWith("//"))
                        {
                            //this would be a comment
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(line))
                            {
                                Output.Text(Parse.Execute(line).ToString());
                            }
                        }
                    }
                }
                else
                {
                    return new Error("File not found");
                }

                return base.Nothing();
            }
            else
            {
                return base.Error(1); //Wrong number of arguments
            }
        }
    }
}