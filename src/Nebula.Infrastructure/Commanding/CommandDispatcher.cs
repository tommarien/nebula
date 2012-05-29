namespace Nebula.Infrastructure.Commanding
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly ICommandHandlerFactory commandHandlerFactory;

        public CommandDispatcher(ICommandHandlerFactory commandHandlerFactory)
        {
            this.commandHandlerFactory = commandHandlerFactory;
        }

        public virtual void Dispatch<TCommand>(TCommand command)
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

        public virtual TResult Dispatch<TCommand, TResult>(TCommand command)
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