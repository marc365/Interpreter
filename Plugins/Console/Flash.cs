/*
 *
 * User: github.com/marc365
 * Updated: 2016
 */

using System;

namespace HiSystems.Interpreter
{
    public class Flash : Command
    {
        public override string Name
        {
            get
            {
                return "Flash";
            }
        }

        public override string Description
        {
            get { return "Flash the console background."; }
        }

        public override string Usage
        {
            get { return ""; }
        }

        public override Literal Execute(string[] arguments)
        {
            Execute();
            return base.Nothing();
        }

        public static void Execute()
        {
            System.Console.BackgroundColor = ConsoleColor.White;
            Cls.Execute();
            Sleep.Milliseconds(50);
            System.Console.BackgroundColor = ConsoleColor.Black;
            Cls.Execute();
        }
    }
}
