/*
 *
 * User: github.com/marc365
 * Created: 2016
 */

namespace HiSystems.Interpreter
{
    public class CommandOperation : IConstruct
    {
        private Command command;
        private string[] arguments;

        public CommandOperation(Command command, string[] arguments)
        {
            this.command = command;
            this.arguments = arguments;
        }
        
        Literal IConstruct.Transform()
        {
            return command.Execute(this.arguments);
        }

        public Command Command
        {
            get
            {
                return this.command;
            }
        }

        public string[] Arguments
        {
            get
            {
                return this.arguments;
            }
        }
    }
}