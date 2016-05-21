/*
 *
 * User: github.com/marc365
 * Updated: 2016
 */

using System;
using System.IO;
using System.Text;
using System.Linq;

namespace HiSystems.Interpreter
{
    public class Output : Function
    {
        public override string Name
        {
            get
            {
                return "Output";
            }
        }

        public override string Description
        {
            get { return "Send the output to the available actions."; }
        }

        public override string Usage
        {
            get { return "Output(message)"; }
        }

        public static void OutputAction(string message)
        {
            foreach (var action in Program.Actions)
            {
                action.Execute(message);
            }
        }

        public override Literal Execute(IConstruct[] arguments)
        {
            base.EnsureArgumentCountIs(arguments, 1);

            var value = base.GetTransformedArgument<Literal>(arguments, argumentIndex: 0);

            return new Text(value.ToString());
        }

        public static void Text(string text, bool chat = false)
        {
            if (!string.IsNullOrEmpty(text))
			{
                OutputAction(string.Format("\r{0}{1}", text, Repo.nl));
			}
        }

        public static void Line(string text)
        {
            OutputAction(string.Format("\r{0}", text));
        }

        public static void Append(string text)
        {
            OutputAction(text);
        }

        public static void Append(char text)
        {
            OutputAction(text.ToString());
        }

        public static void NewLine()
        {
            OutputAction(Repo.nl);
        }

        //public static void Notice(string message, string system = null)
        //{
        //    Cursor.Colour(ConsoleColor.White);
        //    Output.Line(message);
        //    System.Console.ForegroundColor = ConsoleColor.Gray;
        //}

        //public static void Success(string message, string system = null)
        //{
        //    if (system != null)
        //    {
        //        System.Console.ForegroundColor = ConsoleColor.DarkGreen;
        //        System.Console.Write(system + " : ");
        //    }

        //    System.Console.ForegroundColor = ConsoleColor.Green;

        //    string[] animate = new string[4] { "\\", "|", "/", "-" };
        //    int delay = 1;
        //    int consoleSize = 79;
        //    int lines = 0;
        //    for (int i = 0; i < message.Length; i++)
        //    {
        //        for (int a = 0; a < animate.Length; a++)
        //        {

        //            System.Console.Write(animate[a]);
        //            Sleep.Milliseconds(delay);
        //            if (System.Console.CursorLeft == consoleSize)
        //            {
        //                System.Console.WriteLine("");
        //                lines++;
        //            }

        //            System.Console.CursorLeft = i - (lines * (consoleSize - 2));

        //        }
        //        System.Console.Write(message[i]);
        //    }
        //    System.Console.WriteLine("");
        //    System.Console.ForegroundColor = ConsoleColor.White;
        //}

        //public static void Debug(string message, string system = null)
        //{
        //    if (system != null)
        //    {
        //        System.Console.ForegroundColor = ConsoleColor.DarkYellow;
        //        System.Console.Write(system + " : ");
        //    }
        //    System.Console.ForegroundColor = ConsoleColor.Yellow;
        //    System.Console.WriteLine(message);
        //    System.Console.ForegroundColor = ConsoleColor.White;

        //    // Create CSV
        //    string errorFile = "error.csv";

        //    string file = Now.Execute().ToString("dd-MM-yyyy hh:mm:ss") + "," + system + "," + message + Environment.NewLine;
        //    try
        //    {
        //        FileStream data = new FileStream(@errorFile, FileMode.Append, FileAccess.Write, FileShare.None);
        //        byte[] Bytes = Encoding.UTF8.GetBytes(file.ToCharArray());
        //        data.Write(Bytes, 0, (int)Bytes.Length);

        //        data.Close();
        //        data.Dispose();
        //    }
        //    catch
        //    {
        //        System.Environment.Exit(-1);
        //    }

        //}

        ////for serious errors only, no jokes :-)
        //public static void Error(string message, string system = null)
        //{
        //    if (system != null)
        //    {
        //        System.Console.ForegroundColor = ConsoleColor.DarkRed;
        //        System.Console.Write(system + " : ");
        //    }

        //    System.Console.ForegroundColor = ConsoleColor.Red;

        //    string[] animate = new string[4] { "\\", "|", "/", "-" };
        //    int delay = 1;
        //    int consoleSize = 79;
        //    int lines = 0;
        //    for (int i = 0; i < message.Length; i++)
        //    {
        //        for (int a = 0; a < animate.Length; a++)
        //        {

        //            System.Console.Write(animate[a]);
        //            Sleep.Milliseconds(delay);
        //            if (System.Console.CursorLeft == consoleSize)
        //            {
        //                System.Console.WriteLine("");
        //                lines++;
        //            }

        //            System.Console.CursorLeft = i - (lines * (consoleSize - 2));

        //        }
        //        System.Console.Write(message[i]);
        //    }
        //    System.Console.WriteLine("");
        //    System.Console.ForegroundColor = ConsoleColor.White;
        //}
    }
}