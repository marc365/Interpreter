/*
 *
 * User: github.com/marc365
 * Created: 2016
 */

namespace HiSystems.Interpreter
{
    public class Edit : Command
    {
        public override string Name
        {
            get
            {
                return "Edit";
            }
        }

        public override string Description
        {
            get { return "Edit a text file. Currently on windows it launches notepad and on *NX it uses gedit."; }
        }

        public override string Usage
        {
            get { return "{filename}"; }
        }

        public override Literal Execute(string[] args)
        {
            if (base.EnsureArgumentCountIs(args, 1))
            {
                System.Diagnostics.Process ChannelProcess = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo ChannelInfo = new System.Diagnostics.ProcessStartInfo();

                if (Program._Mono)
                {
                    ChannelInfo.FileName = "gedit";
                }
                else
                {
                    ChannelInfo.FileName = "notepad.exe";
                }

                ChannelInfo.Arguments = Path.Execute(args[0]);

                ChannelInfo.UseShellExecute = false;
                ChannelInfo.RedirectStandardInput = true;
                ChannelInfo.RedirectStandardOutput = true;
                ChannelProcess.StartInfo = ChannelInfo;

                ChannelProcess.Start();

                return base.Nothing();
            }
            else
            {
                return base.Error(1); //Wrong number of arguments
            }
        }

    }
}