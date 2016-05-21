/*
 *
 * User: github.com/marc365
 * Updated: 2016
 */

using System;

namespace HiSystems.Interpreter
{
    public class Cursor : Function
    {
        public override string Name
        {
            get
            {
                return "Cursor";
            }
        }

        public override string Description
        {
            get { return "Report cursor locations or place the cursor at the position and set its visibility."; }
        }

        public override string Usage
        {
            get { return "Cursor(vertical) Cursor(vertical,horizontal) Cursor(true/false) Cursor(vertical,true/false) Cursor(vertical,horizontal,true/false)"; }
        }

        public override Literal Execute(IConstruct[] arguments)
        {
            var value = base.GetTransformedArgument<Number>(arguments, argumentIndex: 0);

            if (value == null) value = base.GetTransformedArgument<Boolean>(arguments, argumentIndex: 0);

            int number = 0;
            bool boolean = false;

            if (int.TryParse(value.ToString(), out number))
            {
                
                Top(number); //vertical
                if (arguments.Length == 2)
                {
                    value = base.GetTransformedArgument<Number>(arguments, argumentIndex: 1);

                    if (value == null) value = base.GetTransformedArgument<Boolean>(arguments, argumentIndex: 1);

                    if (int.TryParse(value.ToString(), out number))
                    {
                        System.Console.CursorLeft = number;
                    }
                    else if (bool.TryParse(value.ToString(), out boolean))
                    {
                        System.Console.CursorVisible = boolean;
                    }
                    else
                    {
                        return new Error("Wrong arguments");
                    }
                }
                if (arguments.Length == 3)
                {
                    value = base.GetTransformedArgument<Number>(arguments, argumentIndex: 1);
                    if (value != null && int.TryParse(value.ToString(), out number))
                    {
                        System.Console.CursorLeft = number;
                    }
                    else
                    {
                        return new Error("Wrong arguments");
                    }

                    value = base.GetTransformedArgument<Boolean>(arguments, argumentIndex: 2);
                        
                    if (value != null && bool.TryParse(value.ToString(), out boolean))
                    {
                        System.Console.CursorVisible = boolean;
                    }
                    else
                    {
                        return new Error("Wrong arguments");
                    }
                }
            }
            else if (bool.TryParse(value.ToString(), out boolean))
            {
                System.Console.CursorVisible = boolean;
            }
            else
            {
                return new Text(string.Format("Left: {0} Top: {0}", System.Console.CursorLeft, System.Console.CursorTop));
            }

            return base.Nothing();
        }

        public static void Visible(bool visible)
        {
            System.Console.CursorVisible = visible;
        }

        public static void Top(int vertical)
        {
            if (vertical < 0) vertical = 0;
            if (vertical > 24) vertical = 24;
            System.Console.CursorTop = vertical;
        }

        public static void Up()
        {
            if (System.Console.CursorTop > 0)
            System.Console.CursorTop--;
        }

        public static void Down()
        {
            System.Console.CursorTop++;
        }

        public static void Prev()
        {
            //todo threadsag#fe error
            try
            {
                if (System.Console.CursorLeft > 0)
                    System.Console.CursorLeft--;
            }
            catch
            {
            }
        }

        public static void Next()
        {
            System.Console.CursorLeft++;
        }

        public static void Colour(ConsoleColor colour)
        {
            System.Console.ForegroundColor = colour;
        }

        public static void Left(int horitontal)
        {
            if (horitontal < 0) horitontal = 0;
            if (horitontal > 79) horitontal = 79;
            System.Console.CursorLeft = horitontal;
        }

        public static void Position(int vertical, int horitontal)
        {
            Top(vertical);
            Left(horitontal);
        }


    }
}