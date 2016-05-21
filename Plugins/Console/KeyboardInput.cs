/*
 *
 * User: github.com/marc365
 * Created: 2016
 */

using System;

namespace HiSystems.Interpreter
{
    public class KeyboardInput : Function
    {
        private static ConsoleKeyInfo readkey;

        private static System.DateTime StartTime = System.DateTime.MinValue;
        private static System.TimeSpan WritingTime = new System.TimeSpan();
        private static string readline = string.Empty;
        private static int cursor = 0;
        private static System.DateTime sleeping;
        private static bool typing = true;
        private static bool firstkey = true;

        public override string Name
        {
            get
            {
                return "KeyboardInput";
            }
        }

        public override string Description
        {
            get { return "Receive input from the keyboard."; }
        }

        public override string Usage
        {
            get { return "KeyboardInput()"; }
        }

        public override Literal Execute(IConstruct[] arguments)
        {
            return new Text(Execute());
        }

        public static string Execute()
        {
            readline = string.Empty;

            //todo prompt
            System.Console.Write("1> ");
            #region read from stdin

            int WordCount = 0;
            int previous = 0;

            cursor = 3;
            while (typing)
            {
                sleeping = Now.Execute().AddSeconds(3);

                while (!Key.Execute())
                {
                    if (firstkey)
                    {
                        StartTime = Now.Execute();
                        firstkey = false;
                    }

                    if (!Key.Execute())
                    {
                        Sleep.Milliseconds(20);
                    }

                    if (Now.Execute() > sleeping)
                    {
                        while (!Key.Execute())
                        {
                            Sleep.Milliseconds(160);
                        }
                    }
                }

                readkey = System.Console.ReadKey();

                switch (readkey.Key)
                {
                    case ConsoleKey.Escape:
                    case ConsoleKey.Tab:
                        Output.Line(string.Format("{0} ", readline));
                        Cursor.Left(cursor);
                        break;

                    #region Left Arrow

                    case ConsoleKey.LeftArrow:
						if(Program._Mono && System.Console.CursorLeft == cursor)
						{
							Cursor.Up();
						}
                        if (readline.Length / System.Console.BufferWidth >= 1)
                        {
                            if (cursor - 3 == readline.Length)
                            {
                                for (int i = 0; i < readline.Length / System.Console.BufferWidth; i++)
                                {
                                    Cursor.Up();
                                }
                            }
                            else
                            {
                                for (int i = -1; i < readline.Length / System.Console.BufferWidth; i++)
                                {
                                    Cursor.Up();
                                }
                            }
                        }
                        if (cursor - 3 > 0)
                        {
                            //todo prompt
                            Output.Line("1> " + readline.Substring(0, (cursor - 3) - 1));
                            //highlight the letter the cursor is on
                            System.Console.ForegroundColor = ConsoleColor.Magenta;
                            Output.Append(readline[(cursor - 3) - 1]);
                            System.Console.ForegroundColor = ConsoleColor.Gray;
                            Output.Append(readline.Substring(cursor - 3, readline.Length - (cursor - 3)));
                            cursor--;
                            Cursor.Left(cursor);
                        }
                        else
                        {
                            if (readline.Length > 0)
                            {
                                System.Console.ForegroundColor = ConsoleColor.Magenta;
                                //todo prompt
                                Output.Line("1> " + readline[0].ToString());
                                System.Console.ForegroundColor = ConsoleColor.Gray;
                                Output.Append(readline.Substring(1, readline.Length - 1));
                            }
                        }

                        break;
                    #endregion

                    #region Right Arrow
                    case ConsoleKey.RightArrow:
                        if (readline.Length / System.Console.BufferWidth >= 1)
                        {
                            if (cursor - 3 == readline.Length)
                            {
                                for (int i = 0; i < readline.Length / System.Console.BufferWidth; i++)
                                {
                                    Cursor.Up();
                                }
                            }
                            else
                            {
                                for (int i = -1; i < readline.Length / System.Console.BufferWidth; i++)
                                {
                                    Cursor.Up();
                                }
                            }
                        }
                        if ((cursor - 3) + 1 < readline.Length)
                        {
                            Output.Line("1> " + readline.Substring(0, (cursor - 3) + 1));
                            #region highlight the letter the cursor is on
                            System.Console.ForegroundColor = ConsoleColor.Magenta;
                            Output.Append(readline[(cursor - 3) + 1]);
                            System.Console.ForegroundColor = ConsoleColor.Gray;
                            Output.Append(readline.Substring((cursor - 3) + 2, readline.Length - (cursor - 3) - 2));
                            #endregion
                            cursor++;
                            Cursor.Left(cursor);
                        }
                        else if ((cursor - 3) + 1 == readline.Length)
                        {
                            //todo prompt
                            Output.Line("1> " + readline.Substring(0, readline.Length));
                            cursor++;
                        }
                        else
                        {
                            //todo prompt
                            Output.Line("1> " + readline.Substring(0, readline.Length));
                        }
                        break;
                    #endregion

                    case ConsoleKey.Home:
                        cursor = 3;
                        Cursor.Left(cursor);
                        break;
                    case ConsoleKey.End:
                        cursor = readline.Length + 3;
                        Cursor.Left(cursor);
                        break;

					#region Up Arrow

                    case ConsoleKey.UpArrow:
                        Output.Line("\r");
                        for (int i = 0; i < readline.Length; i++)
                        {
                            Output.Append(" ");
                        }
                        readline = Log.Tail(previous);
                        previous--;               
                        cursor = readline.Length + 3;
                        Cursor.Left(cursor);
                        //todo prompt
                        Output.Line("1> " + readline);
                        break;

					#endregion

					#region Down Arrow

                    case ConsoleKey.DownArrow:
                        Output.Line("\r");
                        for (int i = 0; i < readline.Length; i++)
                        {
                            Output.Append(" ");
                        }
                        readline = Log.Tail(previous);
                        previous++;
                        cursor = readline.Length + 3;
                        Cursor.Left(cursor);
                        //todo prompt
                        Output.Line("1> " + readline);
                        break;

					#endregion

					#region Backspace

                    case ConsoleKey.Backspace:
                        if (readline.Length / System.Console.BufferWidth >= 1)
                        {
                            if (cursor - 3 == readline.Length)
                            {
                                for (int i = 0; i < readline.Length / System.Console.BufferWidth; i++)
                                {
                                    Cursor.Up();
                                }
                            }
                            else
                            {
                                for (int i = -1; i < readline.Length / System.Console.BufferWidth; i++)
                                {
                                    Cursor.Up();
                                }
                            }
                        }
                        if (cursor - 3 == 0)
                        {
                            //do nothing
                        }
                        else if (cursor - 3 == readline.Length)
                        {
                            readline = readline.Substring(0, readline.Length - 1);
                            //todo prompt
                            Output.Line(string.Format("1> {0} ", readline));
                            cursor--;
                            Cursor.Left(cursor);
                        }
                        else
                        {
                            readline = string.Format("{0}{1}", readline.Substring(0, (cursor - 3) - 1), readline.Substring(cursor - 3, readline.Length - (cursor - 3)));
                            //todo prompt
                            Output.Line(string.Format("1> {0} ", readline));
                            cursor--;
                            Cursor.Left(cursor);
                        }
                        if (cursor == 0)
                        {
                            firstkey = true;
                        }
                        break;

					#endregion

					#region Delete;

                    case ConsoleKey.Delete:
                        if (cursor - 3 == readline.Length)
                        {
                            readline = readline.Substring(0, readline.Length - 1);
                            Output.Line(readline);
                        }
                        else
                        {
                            readline = readline.Substring(0, cursor - 3) + readline.Substring((cursor - 3) + 1, readline.Length - (cursor - 3) + 1);
                            //todo prompt
                            Output.Line(string.Format("1> {0} ", readline));
                            Cursor.Left(cursor);
                        }
                        break;

					#endregion

					#region Enter

                    case ConsoleKey.Enter:
                        Cursor.Left(0);
                        cursor = 0;
                        WritingTime = Now.Execute() - StartTime;
                        WordCount = readline.Split(' ').Length;
                        typing = false;
                        break;

					#endregion 

					#region Typing

                    default:
                        if (cursor - 3 == readline.Length)
                        {
                            readline += readkey.KeyChar;
                            cursor++;

                            //flash as one types
                            Cursor.Prev();
                            System.Console.ForegroundColor = ConsoleColor.White;
                            Output.Append(readkey.KeyChar);
                            if (!Key.Execute())
                            {
                                Sleep.Milliseconds(40);
                            }
                            Cursor.Prev();
                            System.Console.ForegroundColor = ConsoleColor.Gray;
                            Output.Append(readkey.KeyChar);
                        }
                        else
                        {
                            if (readline.Length / System.Console.BufferWidth >= 1)
                            {
                                if (cursor - 3 == readline.Length)
                                {
                                    for (int i = 0; i < readline.Length / System.Console.BufferWidth; i++)
                                    {
                                        Cursor.Up();
                                    }
                                }
                                else
                                {
                                    for (int i = -1; i < readline.Length / System.Console.BufferWidth; i++)
                                    {
                                        Cursor.Up();
                                    }
                                }
                            }
                            if (readline.Length > cursor - 3) //todo random errors?
                            {
                                readline = readline.Substring(0, cursor - 3) + readkey.KeyChar + readline.Substring(cursor - 3, readline.Length - (cursor - 3));
                            }
                            else
                            {
                                //todo what happens next?
                                Output.Text("Cursor Problem: " + readline);
                            }
                            //flash as one types
                            Cursor.Colour(ConsoleColor.White);
                            Output.Append(readkey.KeyChar);
                            if (!Key.Execute())
                            {
                                Sleep.Milliseconds(40);
                            }
                            Cursor.Colour(ConsoleColor.Gray);
                            //todo prompt
                            Output.Line("1> " + readline);
                            cursor++;
                            Cursor.Left(cursor);
                        }
                        //if (readline.Length > System.Console.BufferWidth)
                        //{
                        //    cursor = cursor - System.Console.BufferWidth;
                        //    Cursor.Up();
                        //}
                        break;

					#endregion

                }

            }

            #endregion

			if(Program._Mono) //it already took the Enter key into the console
				Cursor.Up();

			#region highlight the input

            Cursor.Colour(ConsoleColor.White);
            Output.Line(readline);
            System.Console.ForegroundColor = ConsoleColor.Gray;

			#endregion

			#region Words/m
            if (readline.Length > 12 && readline.Length < 65)
            {
                Cursor.Colour(ConsoleColor.DarkGray);
                Output.Append(string.Format(" @ {0} words/m", Math.Round(WordCount * (60.0 / WritingTime.TotalSeconds), 2)));
                Cursor.Colour(ConsoleColor.Gray);
            }
            else if (readline.Length <= 80)
            {
                //todo prompt
                Output.Append("   ");
            }
			#endregion

			Output.NewLine();

            #region underline the input

            if (readline.Length <= 80 && readline.Length >= 77)
            {
                Cursor.Up();
            }

            if (readline.Length <= 80)
            {
                foreach (char x in readline)
				{
					Output.Append('\u00af'); //MACRON ¯
				}
                Output.NewLine();
            }

            #endregion

            //reset
            cursor = 0;
            typing = true;
			firstkey = true;

            return readline; // = string.Empty;

        }
    }
}
