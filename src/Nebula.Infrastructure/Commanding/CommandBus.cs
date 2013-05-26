namespace Nebula.Infrastructure.Commanding
{
    public class CommandBus : ICommandBus
    {
        private readonly ICommandHandlerFactory commandHandlerFactory;

        public CommandBus(ICommandHandlerFactory commandHandlerFactory)
        {
            this.commandHandlerFactory = commandHandlerFactory;
        }

        public virtual void Send<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            ICommandHandler<TCommand> handler = commandHandlerFactory.CreateHandler<TCommand>();

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