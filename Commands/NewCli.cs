/*
 *
 * User: github.com/marc365
 * Created: 2016
 */

namespace HiSystems.Interpreter
{
    public class NewCli : Command
    {
        public override string Name
        {
            get
            {
                return "NewCli";
            }
        }

        public override string Description
        {
            get { return "Start an interpreter in a new window."; }
        }

        public override string Usage
        {
            get { return ""; }
        }

        public override Literal Execute(string[] args)
        {
            System.Diagnostics.Process ChannelProcess = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo ChannelInfo = new System.Diagnostics.ProcessStartInfo();

            if (Program._Mono)
            {
                ChannelInfo.FileName = "mono";
                ChannelInfo.Arguments = "HiSystems.Interpreter.exe " + Salt.Word(6);
            }
            else
            {
                ChannelInfo.FileName = "program";
                ChannelInfo.Arguments = Salt.Word(6);
            }

            ChannelInfo.UseShellExecute = true;
            ChannelInfo.CreateNoWindow = false;
            ChannelInfo.RedirectStandardInput = false;
            ChannelInfo.RedirectStandardOutput = false;
            ChannelProcess.StartInfo = ChannelInfo;

            ChannelProcess.Start();

            return base.Nothing();
        }
    }
}