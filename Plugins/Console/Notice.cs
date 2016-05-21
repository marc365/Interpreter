/*
 *
 * User: github.com/marc365
 * Created: 2016
 */

using System;
using System.IO;

namespace HiSystems.Interpreter
{
    public class Notice : Function
    {
        public static string[,] _h1charset = new string[37, 7]; //large header
        public static string[,] _h2charset = new string[37, 5];
        public static string[,] _h3charset = new string[37, 5];
        public static string[,] _h4charset = new string[37, 3];
        public static string[,] _s1charset = new string[37, 6]; //cosmic
        public static string[,] _s2charset = new string[37, 15]; //franktur
        public static string[,] _s3charset = new string[27, 10]; //broadway

        public override string Name
        {
            get
            {
                return "Notice";
            }
        }

        public override string Description
        {
            get { return "Large ASCII font notices. (h1,h2,h3,h4,s1,s2,s3)"; }
        }

        public override string Usage
        {
            get { return "Notice(text) Notice(text,font) Notice(text,font,offset) Notice(text,font,offset, vertical)"; }
        }

        public override Literal Execute(IConstruct[] arguments)
        {
            string output = string.Empty;
            string newline = string.Empty;
            bool scrolling = false;
            bool position = false;
            int top = 0;
            int length = 78;
            int height = 0;
            int offset = 0;
            string _size = "s2";

            #region build fonts

            if (_h1charset[0, 0] == null)
            {
                System.Reflection.Assembly _assembly = System.Reflection.Assembly.GetExecutingAssembly();

                StreamReader _streamReader = new StreamReader(_assembly.GetManifestResourceStream("Console.Data.font.h1.txt"));

                for (int line = 0; line < 7; line++)
                {
                    string[] chunk = _streamReader.ReadLine().Split('|');

                    for (int x = 0; x < 37; x++)
                    {
                        _h1charset[x, line] = chunk[x];
                    }
                }

                _streamReader = new StreamReader(_assembly.GetManifestResourceStream("Console.Data.font.h2.txt"));

                for (int line = 0; line < 5; line++)
                {
                    string[] chunk = _streamReader.ReadLine().Split('|');

                    for (int x = 0; x < 37; x++)
                    {
                        _h2charset[x, line] = chunk[x];
                    }
                }

                _streamReader = new StreamReader(_assembly.GetManifestResourceStream("Console.Data.font.h3.txt"));

                for (int line = 0; line < 5; line++)
                {
                    string[] chunk = _streamReader.ReadLine().Split('|');

                    for (int x = 0; x < 37; x++)
                    {
                        _h3charset[x, line] = chunk[x];
                    }
                }

                _streamReader = new StreamReader(_assembly.GetManifestResourceStream("Console.Data.font.h4.txt"));

                for (int line = 0; line < 3; line++)
                {
                    string[] chunk = _streamReader.ReadLine().Split('.');

                    for (int x = 0; x < 37; x++)
                    {
                        _h4charset[x, line] = chunk[x];
                    }
                }

                _streamReader = new StreamReader(_assembly.GetManifestResourceStream("Console.Data.font.cosmic.txt"));

                for (int line = 0; line < 6; line++)
                {
                    string[] chunk = _streamReader.ReadLine().Split('|');

                    for (int x = 0; x < 37; x++)
                    {
                        _s1charset[x, line] = chunk[x];
                    }
                }

                _streamReader = new StreamReader(_assembly.GetManifestResourceStream("Console.Data.font.franktur.txt"));

                for (int line = 0; line < 15; line++)
                {
                    string[] chunk = _streamReader.ReadLine().Split('|');

                    for (int x = 0; x < 37; x++)
                    {
                        _s2charset[x, line] = chunk[x];
                    }
                }

                _streamReader = new StreamReader(_assembly.GetManifestResourceStream("Console.Data.font.broadway.txt"));

                for (int line = 0; line < 10; line++)
                {
                    string[] chunk = _streamReader.ReadLine().Split('|');

                    for (int x = 0; x < 27; x++)
                    {
                        _s3charset[x, line] = chunk[x];
                    }
                }
            }

            #endregion

            #region arguments

            var value = base.GetTransformedArgument<Literal>(arguments, argumentIndex: 0);
            var args = base.GetTransformedArgument<Text>(arguments, argumentIndex: 0); //dummy :-(

            if (arguments.Length == 2 )
            {
                args = base.GetTransformedArgument<Text>(arguments, argumentIndex: 1);
                if (args != null)
                {
                    _size = args.ToString();
                }
                else
                {
                    return base.Error(1); //Wrong number of arguments
                }
            }

            if (arguments.Length == 3)
            {
                return base.Error(1); //Wrong number of arguments
            }

            ;
            if (arguments.Length == 4)
            {
                args = base.GetTransformedArgument<Text>(arguments, argumentIndex: 1);
                if (args != null)
                {
                    _size = args.ToString();
                }
                else
                {
                    return base.Error(1); //Wrong number of arguments
                }

                args = base.GetTransformedArgument<Number>(arguments, argumentIndex: 2);
                if (args != null)
                {
                    offset = Parse.Int(args.ToString());
                }
                else
                {
                    return base.Error(1); //Wrong number of arguments
                }

                args = base.GetTransformedArgument<Number>(arguments, argumentIndex: 3);
                if (args != null)
                {
                    position = true;
                    top = Parse.Int(args.ToString());
                }
                else
                {
                    return base.Error(1); //Wrong number of arguments
                }
            }

            #endregion

            #region Build Text

            output = string.Empty;

            if (_size == "h1")//large header
            {
                height = 8;
                for (int line = 0; line < 7; line++)
                {
                    newline = string.Empty;

                    foreach (char c in value.ToString().ToUpperInvariant())
                    {
                        if ((int)c > 64 && (int)c < 91)
                        {
                            newline = string.Format("{0}{1}", newline, _h1charset[(int)c - 65, line]);
                        }
                        else //" "
                        {
                            newline = string.Format("{0}{1}", newline, _h1charset[36, line]);
                        }
                    }

                    if (newline.Length < length)
                    {
                        output = string.Format("{0}{1}{2}", output, newline, Environment.NewLine);
                    }
                    else
                    {
                        scrolling = true;
                        output = string.Format("{0}{1}{2}", output, newline.Substring(offset, length), Environment.NewLine);
                    }
                }
            }
            else if (_size == "h2")//medium header
            {
                height = 6;
                for (int line = 0; line < 5; line++)
                {
                    newline = string.Empty;

                    foreach (char c in value.ToString().ToUpperInvariant())
                    {
                        if ((int)c > 64 && (int)c < 91)
                        {
                            newline = string.Format("{0}{1}", newline, _h2charset[(int)c - 65, line]);
                        }
                        else //" "
                        {
                            newline = string.Format("{0}{1}", newline, _h2charset[36, line]);
                        }
                    }

                    if (newline.Length < length)
                    {
                        output = string.Format("{0}{1}{2}", output, newline, Environment.NewLine);
                    }
                    else
                    {
                        scrolling = true;
                        output = string.Format("{0}{1}{2}", output, newline.Substring(offset, length), Environment.NewLine);
                    }
                }
            }
            else if (_size == "h3")//tall small header
            {
                height = 6;
                for (int line = 0; line < 5; line++)
                {
                    newline = string.Empty;

                    foreach (char c in value.ToString().ToUpperInvariant())
                    {
                        if ((int)c > 64 && (int)c < 91)
                        {
                            newline = string.Format("{0}{1}", newline, _h3charset[(int)c - 65, line]);
                        }
                        else //" "
                        {
                            newline = string.Format("{0}{1}", newline, _h3charset[36, line]);
                        }
                    }

                    if (newline.Length < length)
                    {
                        output = string.Format("{0}{1}{2}", output, newline, Environment.NewLine);
                    }
                    else
                    {
                        scrolling = true;
                        output = string.Format("{0}{1}{2}", output, newline.Substring(offset, length), Environment.NewLine);
                    }
                }
            }
            else if (_size == "h4")//small header
            {
                height = 4;
                for (int line = 0; line < 3; line++)
                {
                    newline = string.Empty;

                    foreach (char c in value.ToString().ToUpperInvariant())
                    {
                        if ((int)c > 64 && (int)c < 91)
                        {
                            newline = string.Format("{0}{1}", newline, _h4charset[(int)c - 65, line]);
                        }
                        else //" "
                        {
                            newline = string.Format("{0}{1}", newline, _h4charset[36, line]);
                        }
                    }

                    if (newline.Length < length)
                    {
                        output = string.Format("{0}{1}{2}", output, newline, Environment.NewLine);
                    }
                    else
                    {
                        scrolling = true;
                        output = string.Format("{0}{1}{2}", output, newline.Substring(offset, length), Environment.NewLine);
                    }
                }
            }
            else if (_size == "cosmic" || _size == "s1")
            {
                height = 7;
                for (int line = 0; line < 6; line++)
                {
                    newline = string.Empty;

                    foreach (char c in value.ToString().ToUpperInvariant())
                    {

                        if ((int)c > 64 && (int)c < 91)
                        {
                            newline = string.Format("{0}{1}", newline, _s1charset[(int)c - 65, line]);
                        }
                        else if ((int)c > 47 && (int)c < 58)
                        {
                            newline = string.Format("{0}{1}", newline, _s1charset[(int)c - 22, line]);
                        }
                        else //" "
                        {
                            newline = string.Format("{0}{1}", newline, _s2charset[36, line]);
                        }
                    }

                    if (newline.Length < length)
                    {
                        output = string.Format("{0}{1}{2}", output, newline, Environment.NewLine);
                    }
                    else
                    {
                        scrolling = true;
                        output = string.Format("{0}{1}{2}", output, newline.Substring(offset, length), Environment.NewLine);
                    }
                }
            }
            else if (_size == "franktur" || _size == "s2")
            {
                height = 16;
                for (int line = 0; line < 15; line++)
                {
                    newline = string.Empty;

                    foreach (char c in value.ToString().ToUpperInvariant())
                    {
                        if ((int)c > 64 && (int)c < 91)
                        {
                            newline = string.Format("{0}{1}", newline, _s2charset[(int)c - 65, line]);
                        }
                        else if ((int)c > 47 && (int)c < 58)
                        {
                            newline = string.Format("{0}{1}", newline, _s2charset[(int)c - 22, line]);
                        }
                        else //" "
                        {
                            newline = string.Format("{0}{1}", newline, _s2charset[36, line]);
                        }
                    }

                    if (newline.Length < length)
                    {
                        output = string.Format("{0}{1}{2}", output, newline, Environment.NewLine);
                    }
                    else
                    {
                        scrolling = true;
                        output = string.Format("{0}{1}{2}", output, newline.Substring(offset, length), Environment.NewLine);
                    }
                }
            }
            else if (_size == "broadway" || _size == "s3")
            {
                height = 11;
                for (int line = 0; line < 10; line++)
                {
                    newline = string.Empty;

                    foreach (char c in value.ToString().ToUpperInvariant())
                    {
                        if ((int)c > 64 && (int)c < 91)
                        {
                            newline = string.Format("{0}{1}", newline, _s3charset[(int)c - 65, line]);
                        }
                        else //" "
                        {
                            newline = string.Format("{0}{1}", newline, _s3charset[26, line]);
                        }
                    }

                    if (newline.Length < length)
                    {
                        
                        output = string.Format("{0}{1}{2}", output, newline, Environment.NewLine);
                    }
                    else
                    {
                        scrolling = true;
                        output = string.Format("{0}{1}{2}", output, newline.Substring(offset, length), Environment.NewLine);
                    }
                }
            }

            #endregion

            if (scrolling)
            {
                System.Console.CursorLeft = 0;

                if (position)
                {
                    System.Console.CursorTop = top;
                }
            }
            else
            {
                int left = System.Console.CursorLeft;

                if (position == true)
                    System.Console.CursorTop = top;
            }

            return new Text(output);
        }
    }
}
