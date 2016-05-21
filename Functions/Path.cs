/*
 *
 * User: github.com/marc365
 * Updated: 2016
 */

using System;

namespace HiSystems.Interpreter
{
    public class Path : Function
    {
        public override string Name
        {
            get
            {
                return "Path";
            }
        }

        public override string Description
        {
            get { return "Returns the file system path."; }
        }

        public override string Usage
        {
            get { return "Path()"; }
        }

        public override Literal Execute(IConstruct[] arguments)
        {
            return new Text(Execute("~/"));
        }

        public static string Execute(string path)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            return System.IO.Path.Combine(baseDirectory, path);

        }
    }
}