namespace Nebula.Infrastructure.Commanding
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly ICommandHandlerFactory commandHandlerFactory;

        public CommandExecutor(ICommandHandlerFactory commandHandlerFactory)
        {
            this.commandHandlerFactory = commandHandlerFactory;
        }

        public virtual ICommandResult Execute<T>(T command) where T : ICommand
        {
            ICommandHandler<T> handler = commandHandlerFactory.CreateHandler<T>();

            try
            {
                return handler.Handle(command);
            }
            finally
            {
                commandHandlerFactory.ReleaseHandler(handler);
            }
        }
    }
}