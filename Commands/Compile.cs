/*
 *
 * User: github.com/marc365
 * Created: 2016
 */

using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Text;

namespace HiSystems.Interpreter
{
    public class Compile : Command
    {

        private static readonly System.Reflection.Assembly ThisAssembly = typeof(Compile).Assembly;

        public override string Name
        {
            get
            {
                return "Compile";
            }
        }

        public override string Description
        {
            get { return "Compile a c# plugin source file and inject into the system."; }
        }

        public override string Usage
        {
            get { return "{filename}"; }
        }

        private string result = string.Empty;

        public override Literal Execute(string[] args)
        {
           if (base.EnsureArgumentCountIs(args, 1))
           {
                if (System.IO.File.Exists(Path.Execute(args[0])))
                {
                    return Execute(Encoding.UTF8.GetString((ByteArray)Read.Execute(Path.Execute(args[0]).ToString())));
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

        public static Literal Execute(string csharp)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters();

            // References
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add(Path.Execute("~/HiSystems.Interpreter.exe"));
            parameters.GenerateInMemory = false;
            parameters.GenerateExecutable = false;

            CompilerResults results = provider.CompileAssemblyFromSource(parameters, csharp);

            if (results.Errors.HasErrors)
            {
                StringBuilder sb = new StringBuilder();

                foreach (CompilerError error in results.Errors)
                {
                    sb.AppendLine(String.Format("Error ({0}): Line ({1}) {2}", error.ErrorNumber, error.Line, error.ErrorText));
                }

                return new Error(sb.ToString());
            }
            
            //load the temporary file
            Assembly _assembly = Assembly.LoadFile(Path.Execute(results.PathToAssembly));

            //plugin the plugin
            Program.RegisterTypes(_assembly.GetTypes());

            return new Text("compiled to " + results.PathToAssembly);
        }

    }
}