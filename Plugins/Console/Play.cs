/*
 *
 * User: github.com/marc365
 * Created: 2016
 */

namespace HiSystems.Interpreter
{
    public class Play : Command
    {
        public static bool playing = true;

        public override string Name
        {
            get
            {
                return "Play";
            }
        }

        public override string Description
        {
            get { return "Play a text video."; }
        }

        public override string Usage
        {
            get { return "{filename}"; }
        }

        private int BoundsLeft(int i)
        {
            if (i > 79)
                return 79;
            if (i < 0)
                return 0;

            return i;
        }

        private int BoundsTop(int i)
        {
            if (i > 24)
                return 24;
            if (i < 0)
                return 0;

            return i;
        }

        public override Literal Execute(string[] args)
        {
           if (base.EnsureArgumentCountIs(args, 1))
            {
                System.Console.CancelKeyPress += delegate
                {
                    playing = false;
                };

                string prevwrite = string.Empty;

                if (System.IO.File.Exists(Path.Execute(args[0])))
                {

                    System.Console.CursorVisible = false;
                    string Text = System.IO.File.ReadAllText(Path.Execute(args[0])); //.Replace("\r\n", "!").Replace("\b", "").Replace("\n", "").Replace("\n", "");

                    int cut = 0;
                    string splice = string.Empty;
                    //bool topleft = false;
                    bool done = false;
                    int parse = 0;
                    string splicer = string.Empty; //debug
                    int cursor = 0;
                    Cls.Execute();
                    string output = string.Empty;
                    for (int i = 0; playing && i < Text.Length; i++)
                    {
                        if (Key.Execute())
                            return new Text("Playing interupted");

                        switch ((int)Text[i])
                        {
                            case 27:
                                if (cut > 0)
                                {
                                    cut = cut + 2;
                                    done = false;
                                    output = string.Empty;

                                    for (int e = cut; !done && e < i; e++)
                                    {
                                        splicer = Text.Substring(e, i - (e));
                                        switch (Text[e])
                                        {
                                            case 'h': // todo is this a default settings?
                                                //cursorDefault = 7;
                                                done = true;
                                                break;
                                            case 'H':

                                                if (e - cut > 0)
                                                {
                                                    string[] nums = Text.Substring(cut, e - (cut)).Split(';');
                                                    if (nums.Length == 2)
                                                    {
                                                        System.Console.CursorLeft = Parse.Int(nums[1]) - 1;
                                                        System.Console.CursorTop = Parse.Int(nums[0]) - 1;
                                                        output = Text.Substring(e + 1, i - (e + 1));
                                                    }
                                                    else
                                                    {
                                                        return new Error("Video error");
                                                    }
                                                }
                                                else
                                                {
                                                    System.Console.CursorLeft = 0;
                                                    System.Console.CursorTop = 0;
                                                }


                                                done = true;
                                                break;
                                            case 'G':
                                                //single value G
                                                if (int.TryParse(Text.Substring(cut, e - cut), out parse))
                                                {
                                                    //System.Console.CursorTop++;
                                                    System.Console.CursorLeft = parse - 1;
                                                    output = Text.Substring(e + 1, i - (e + 1));
                                                }
                                                else
                                                {
                                                    // System.Console.CursorLeft = cursorDefault;
                                                }

                                                done = true;
                                                break;
                                            case 'K': //erase part of line K 1K 2K
                                                cursor = System.Console.CursorLeft;
                                                //erase from cursor to end of line
                                                if (int.TryParse(Text.Substring(cut, e - cut), out parse))
                                                {
                                                    //todo empty string of length
                                                    for (int c = 0; c <= 79 - System.Console.CursorLeft; c++)
                                                    {
                                                        output += " ";
                                                    }
                                                    System.Console.CursorLeft = System.Console.CursorLeft - 8;
                                                    System.Console.Write(output);
                                                    System.Console.CursorTop++;
                                                }
                                                //erase from start of line to cursor
                                                else
                                                {
                                                    //todo empty string of length
                                                    for (int c = 0; c <= System.Console.CursorLeft; c++)
                                                    {
                                                        output += " ";
                                                    }
                                                    System.Console.CursorLeft = 0;
                                                    System.Console.Write(output);
                                                    System.Console.CursorLeft = 0;
                                                    System.Console.CursorTop++;
                                                }
                                                output = string.Empty;
                                                done = true;
                                                break;
                                            case 'X': //todo unknown - not in all videos
                                                done = true;
                                                break;
                                            case 'J': // refresh
                                                Cls.Execute();
                                                System.Console.CursorLeft = 0;
                                                System.Console.CursorTop = 0;
                                                done = true;
                                                break;
                                            case 'm':
                                            case 'r':
                                            case 'B':
                                            case 'd':
                                            case 'M':
                                            case 'P':
                                            case 'l':
                                                done = true;
                                                break;
                                            default:
                                                break;
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(@output))
                                    {
                                        for (int c = 0; c < output.Length; c++)
                                        {
                                            switch (output[c])
                                            {
                                                case '\n': //linefeed
                                                    System.Console.CursorTop++;
                                                    break;
                                                case '\r': //carriage return
                                                    System.Console.CursorLeft = 0;
                                                    //System.Console.CursorTop++;
                                                    break;
                                                case (char)8: //backspace
                                                    System.Console.CursorLeft--;
                                                    System.Console.Write(' ');
                                                    System.Console.CursorLeft--;
                                                    break;
                                                default:
                                                    System.Console.Write(output[c]);
                                                    break;
                                            }
                                        }
                                    }
                                }
                                cut = i;
                                break;
                            default:
                                break;
                        }
                    }

                    System.Console.CursorVisible = true;
                    playing = true;
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