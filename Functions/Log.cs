/*
 *
 * User: github.com/marc365
 * Updated: 2016
 */

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace HiSystems.Interpreter
{
    public class Log : Function
    {

        public static FileStream data;
        public static byte[] Bytes;
        public static int count;
        public static string[] lines;

        public override string Name
        {
            get
            {
                return "Log";
            }
        }

        public override string Description
        {
            get { return "Save a message to the system log."; }
        }

        public override string Usage
        {
            get { return "Log(text)"; }
        }

        public override Literal Execute(IConstruct[] arguments)
        {
            if (base.EnsureArgumentCountIs(arguments, 1))
            {
                var value = base.GetTransformedArgument<Literal>(arguments, argumentIndex: 0);

                Execute(value.ToString());

                return value;
            }
            else
            {
                return base.Error(1); //Wrong number of agruments
            }
        }

        public static void Execute(string text)
        {
            try
            {
                data = new FileStream(Path.Execute(Repo.log), FileMode.Append, System.IO.FileAccess.Write, FileShare.None);
                Bytes = Encoding.UTF8.GetBytes(string.Format("{0}{1}", text, Environment.NewLine));
                data.Write(Bytes, 0, Bytes.Length);
                data.Close();
                data.Dispose();
            }
            catch
            {
                Output.Text("Error saving log");
            }
        }

        public static string Tail(int LIFO)
        {
            //count = 0;
            //data = new FileStream(Path.Execute(Repo.log), FileMode.Open, System.IO.FileAccess.Read, FileShare.Read);
            //Bytes = new byte[data.Length];
            //data.Read(Bytes, 0, (int)data.Length);
            //lines = Parse.GetString(Bytes).Split(new string[] { Repo.nl }, StringSplitOptions.RemoveEmptyEntries);
            //data.Close();
            //data.Dispose();
            lines = File.ReadAllLines(Path.Execute(Repo.log));
            for(int i = lines.Count() - 1; i > 0; i--)
            {
                if (count == LIFO)
                    return(lines[i]);

                count--;              
            }

            return lines.Last();
        }
    }
}