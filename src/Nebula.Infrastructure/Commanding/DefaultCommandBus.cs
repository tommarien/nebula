namespace Nebula.Infrastructure.Commanding
{
    public class DefaultCommandBus : ICommandBus
    {
        private readonly ICommandHandlerFactory commandHandlerFactory;

        public DefaultCommandBus(ICommandHandlerFactory commandHandlerFactory)
        {
            this.commandHandlerFactory = commandHandlerFactory;
        }

        public void Send<T>(T command) where T : ICommand
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