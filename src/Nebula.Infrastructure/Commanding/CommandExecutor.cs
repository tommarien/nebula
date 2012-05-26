namespace Nebula.Infrastructure.Commanding
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly ICommandHandlerFactory commandHandlerFactory;

        public CommandExecutor(ICommandHandlerFactory commandHandlerFactory)
        {
            this.commandHandlerFactory = commandHandlerFactory;
        }

        public virtual void Execute<TCommand>(TCommand command)
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

        public virtual TResult Execute<TCommand, TResult>(TCommand command)
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