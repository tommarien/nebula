namespace Nebula.Infrastructure.Commanding
{
    public class SyncCommandDispatcher : ICommandDispatcher
    {
        private readonly ICommandHandlerFactory commandHandlerFactory;

        public SyncCommandDispatcher(ICommandHandlerFactory commandHandlerFactory)
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