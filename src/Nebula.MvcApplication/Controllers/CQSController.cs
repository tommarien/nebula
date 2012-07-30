using System.Web.Mvc;
using Nebula.Infrastructure.Commanding;
using Nebula.Infrastructure.Querying;

namespace Nebula.MvcApplication.Controllers
{
    public abstract class CQSController : Controller
    {
        private readonly ICommandDispatcher commandDispatcher;
        private readonly IQueryHandlerFactory queryHandlerFactory;

        protected CQSController(ICommandDispatcher commandDispatcher, IQueryHandlerFactory queryHandlerFactory)
        {
            this.commandDispatcher = commandDispatcher;
            this.queryHandlerFactory = queryHandlerFactory;
        }

        public IQueryHandlerFactory QueryHandlerFactory
        {
            get { return queryHandlerFactory; }
        }

        protected void Dispatch<TCommand>(TCommand command) where TCommand : ICommand
        {
            commandDispatcher.Dispatch(command);
        }

        protected TResult DispatchAndReturn<TCommand, TResult>(TCommand command) where TCommand : ICommand where TResult : ICommandResult
        {
            return commandDispatcher.Dispatch<TCommand, TResult>(command);
        }
    }
}