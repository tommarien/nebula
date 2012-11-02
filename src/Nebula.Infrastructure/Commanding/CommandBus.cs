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

        public virtual TResult SendAndReply<TCommand, TResult>(TCommand command)
            where TCommand : ICommand
            where TResult : ICommandResult
        {
            ICommandHandler<TCommand, TResult> handler = commandHandlerFactory.CreateHandler<TCommand, TResult>();

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