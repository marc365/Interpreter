/*
 *
 * User: github.com/marc365
 * Updated: 2016
 */

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace HiSystems.Interpreter
{
    public class Shuffle : Function
    {
        static RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider();
        static byte[] vector = new byte[4];

        public override string Name
        {
            get
            {
                return "Shuffle";
            }
        }

        public override string Description
        {
            get { return "Returns the string with the words inner letters scambled but still legiable, can process multiple times."; }
        }

        public override string Usage
        {
            get { return "Shuffle({text}(true|false)({depth}))"; }
        }

        public override Literal Execute(IConstruct[] arguments)
        {
            if (base.EnsureArgumentCountIsAtLeast(arguments, 1))
            {
                bool legible = true;
                var text = (Literal)base.GetArgument(arguments, argumentIndex: 0).Transform();

                if (base.EnsureArgumentCountIs(arguments, 2))
                {
                    var option = (Literal)base.GetArgument(arguments, argumentIndex: 1).Transform();

                    if (bool.TryParse(option.ToString(), out legible))
                    {
                        return new Text(Execute(text.ToString(), legible));
                    }
                }
                else if (base.EnsureArgumentCountIs(arguments, 3))
                {
                    var option = (Literal)base.GetArgument(arguments, argumentIndex: 1).Transform();
                    var multiple_option = (Number)base.GetArgument(arguments, argumentIndex: 2).Transform();

                    int multiple = Parse.Int(multiple_option.ToString());

                    if (bool.TryParse(option.ToString(), out legible))
                    {
                        Text output = new Text(Execute(text.ToString(), legible));
                        while (multiple > 0)
                        {
                            output = new Text(Execute(output.ToString(), legible));
                            multiple--;
                        }
                        return output;
                    }
                }

                return new Text(Execute(text.ToString()));
            }
            else
            {
                return base.Error(1); //Wrong number of arguments
            }
        }

        public static string Execute(string text, bool legible = true)
        {
            List<string> output = new List<string>();

            string[] words = text.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < words.Length; i++)
            {
                StringBuilder newword = new StringBuilder();

                if (!legible)
                {
                    newword.Append(new string(Execute<char>(words[i].Substring(0, words[i].Length).ToCharArray())));
                }
                else
                {
                    newword.Append(words[i][0]);

                    if (words[i].Length > 4)
                    {
                        newword.Append(new string(Execute<char>(words[i].Substring(1, words[i].Length - 2).ToCharArray())));
                    }
                    else if (words[i].Length > 3)
                    {
                        newword.Append(words[i][2]);
                        newword.Append(words[i][1]);
                    }
                    else if (words[i].Length > 2)
                    {
                        newword.Append(words[i][1]);
                    }

                    if (words[i].Length > 1)
                    {
                        newword.Append(words[i][words[i].Length - 1]);
                    }
                }

                output.Add(newword.ToString());
            }

            return string.Join(" ", output);
        }

        //Fisher-Yates shuffle
        public static T[] Execute<T>(T[] array)
        {
            int n = array.Length;
            for (int i = 0; i < n; i++)
            {
                int r = i + Random.Next(n - i);
                T t = array[r];
                array[r] = array[i];
                array[i] = t;
            }

            return array;
        }

    }
}