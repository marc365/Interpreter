/*
 *
 * User: github.com/marc365
 * Created: 2016
 */

using System;
using System.Linq;
using System.Collections.Generic;

namespace HiSystems.Interpreter
{
    public class Help : Command
    {
        private static int width = 54;
        private static string output;
        private static int page;
        private static int count;

        public override string Name
        {
            get
            {
                return "Help";
            }
        }

        public override string Description
        {
            get { return "Functions, commands, their descriptions and usage."; }
        }

        public override string Usage
        {
            get { return "{command}/{function}/list"; }
        }

        //todo optimize
        public override Literal Execute(string[] arguments)
        {
            List<Function> list = new List<Function>();

            if (arguments.Length == 1 && (arguments[0] == "list" || arguments[0] == "help" || arguments[0] == "all"))
            {
                string output = string.Format("{0}>> Functions{0}", Repo.nl);
                Output.Text(output);
                foreach (var function in Program.Functions.OrderBy(x => x.Name))
                {
                    output = string.Empty;
                    if (function.Name.Length < 8)
                    {
                        output = string.Format("{0}{3}{1}{3}{3}{2}{4}", output, function.Name, Paragraph(function.Description), '\t', Environment.NewLine);
                        output = string.Format("{0}{2}{2}{2}{1}{3}", output, Paragraph(string.Format("Usage: {0}", function.Usage)), '\t', Environment.NewLine);
                    }
                    else
                    {
                        output = string.Format("{0}{3}{1}{3}{2}{4}", output, function.Name, Paragraph(function.Description), '\t', Environment.NewLine);
                        output = string.Format("{0}{2}{2}{2}{1}{3}", output, Paragraph(string.Format("Usage: {0}", function.Usage)), '\t', Environment.NewLine);
                    }
                    Output.Text(output);
                }

                output = string.Format("{0}{1}>> Commands{1}", output, Environment.NewLine);
                Output.Text(output);
                foreach (var command in Program.Commands.OrderBy(x => x.Name))
                {
                    output = string.Empty;
                    if (command.Name.Length < 8)
                    {
                        output = string.Format("{0}{3}{1}{3}{3}{2}{4}", output, command.Name, Paragraph(command.Description), '\t', Environment.NewLine);
                        output = string.Format("{0}{2}{2}{2}{1}{3}", output, Paragraph(string.Format("Usage: {0}", command.Usage)), '\t', Environment.NewLine);
                    }
                    else
                    {
                        output = string.Format("{0}{3}{1}{3}{2}{4}", output, command.Name, Paragraph(command.Description), '\t', Environment.NewLine);
                        output = string.Format("{0}{2}{2}{2}{1}{3}", output, Paragraph(string.Format("Usage: {0}", command.Usage)), '\t', Environment.NewLine);
                    }
                    Output.Text(output);
                }
            }
            else if (arguments.Length == 1)
            {
                var function = Program.Functions.FirstOrDefault(x => x.Name.ToLower() == arguments[0]);
                var command = Program.Commands.FirstOrDefault(x => x.Name.ToLower() == arguments[0]);
                if (function != null)
                {
                    string output = string.Empty;
                    if (function.Name.Length < 8)
                    {
                        output = string.Format("{0}{1}{3}{3}{2}{4}", output, function.Name, Paragraph(function.Description, true), '\t', Environment.NewLine);
                        output = string.Format("{0}{2}{2}{1}{3}", output, Paragraph(string.Format("Usage: {0}", function.Usage), true), '\t', Environment.NewLine);
                    }
                    else
                    {
                        output = string.Format("{0}{1}{3}{2}{4}", output, function.Name, Paragraph(function.Description, true), '\t', Environment.NewLine);
                        output = string.Format("{0}{2}{2}{1}{3}", output, Paragraph(string.Format("Usage: {0}", function.Usage), true), '\t', Environment.NewLine);
                    }
                    Output.Text(output);
                }
                else if (command != null)
                {
                    string output = string.Empty;
                    if (command.Name.Length < 8)
                    {
                        output = string.Format("{0}{1}{3}{3}{2}{4}", output, command.Name, Paragraph(command.Description, true), '\t', Environment.NewLine);
                        output = string.Format("{0}{2}{2}{1}{3}", output, Paragraph(string.Format("Usage: {0}", command.Usage), true), '\t', Environment.NewLine);
                    }
                    else
                    {
                        output = string.Format("{0}{1}{3}{2}{4}", output, command.Name, Paragraph(command.Description, true), '\t', Environment.NewLine);
                        output = string.Format("{0}{2}{2}{1}{3}", output, Paragraph(string.Format("Usage: {0}", command.Usage), true), '\t', Environment.NewLine);
                    }
                    Output.Text(output);
                }
                else
                {
                    return new Error("No help available");
                }
            }
            else
            {
                string output = string.Empty;

                output = "use Help <option> or 'list' for more information.";
                Output.Text(output);

                output = string.Format("{0}>> Functions{0}", Repo.nl);
                Output.Text(output);
                output = string.Empty;
                foreach (var f in Program.Functions.OrderBy(x=> x.Name))
                {
                    output += f.Name + " ";
                }
                Output.Text(output + Repo.nl);
                
                output = string.Format("{0}>> Commands{0}", Environment.NewLine);
                Output.Text(output);
                output = string.Empty;
                foreach (var f in Program.Commands.OrderBy(x => x.Name))
                {
                    output += f.Name + " ";
                }
                Output.Text(output + Repo.nl);

                //Signal.Execute("help");
            }

            return base.Nothing();
        }

        private string Paragraph(string text, bool single = false)
        {
            if (text.Length > width)
            {
                output = string.Empty;
                page = 0;
                count = 0;

                while (count + (page * width) < text.Length)
                {
                    output += text[count + (page * width)];
                    if (count == width)
                    {
                        if (single)
                        {
                            output += "\r\n\t\t";
                        }
                        else
                        {
                            output += "\r\n\t\t\t";
                        }
                        count = 0;
                        page++;
                    }
                    count++;
                }

                return output;
            }

            return text;
        }

    }
}