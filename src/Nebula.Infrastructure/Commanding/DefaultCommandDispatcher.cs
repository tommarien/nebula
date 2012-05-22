namespace Nebula.Infrastructure.Commanding
{
    public class DefaultCommandDispatcher : ICommandDispatcher
    {
        private readonly ICommandHandlerFactory commandHandlerFactory;

        public DefaultCommandDispatcher(ICommandHandlerFactory commandHandlerFactory)
        {
            this.commandHandlerFactory = commandHandlerFactory;
        }

        public virtual ICommandResult Dispatch<T>(T command) where T : ICommand
        {
            var handler = commandHandlerFactory.CreateHandler<T>();

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