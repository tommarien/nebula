namespace Nebula.Infrastructure.Commanding
{
    public class CommandMediator : IMediator
    {
        private readonly ICommandHandlerFactory commandHandlerFactory;

        public CommandMediator(ICommandHandlerFactory commandHandlerFactory)
        {
            this.commandHandlerFactory = commandHandlerFactory;
        }

        public void Execute<TCommand>(TCommand command) where TCommand : ICommand
        {
            var handler = commandHandlerFactory.CreateHandler<TCommand>();

            try
            {
                handler.Handle(command);
            }
            finally
            {
                commandHandlerFactory.ReleaseHandler(handler);
            }
        }

        public TResult Execute<TCommand, TResult>(TCommand command) where TCommand : ICommand
        {
            var handler = commandHandlerFactory.CreateHandler<TCommand,TResult>();

            try
            {
                var result = handler.Handle(command);
                return result;
            }
            finally
            {
                commandHandlerFactory.ReleaseHandler(handler);
            }
        }
    }
}