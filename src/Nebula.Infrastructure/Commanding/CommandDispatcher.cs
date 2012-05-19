namespace Nebula.Infrastructure.Commanding
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly ICommandHandlerFactory commandHandlerFactory;

        public CommandDispatcher(ICommandHandlerFactory commandHandlerFactory)
        {
            this.commandHandlerFactory = commandHandlerFactory;
        }

        public virtual void Dispatch<T>(T command) where T : ICommand
        {
            var handler = commandHandlerFactory.CreateHandler<T>();

            try
            {
                handler.Handle(command);
            }
            finally
            {
                commandHandlerFactory.ReleaseHandler(handler);
            }
        }
    }
}