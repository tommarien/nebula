using System.Web.Mvc;
using Nebula.Infrastructure.Commanding;
using Nebula.Infrastructure.Querying;

namespace Nebula.MvcApplication.Controllers
{
    public abstract class CQSController : Controller
    {
        private readonly ICommandBus commandBus;
        private readonly IQueryHandlerFactory queryHandlerFactory;

        protected CQSController(ICommandBus commandBus, IQueryHandlerFactory queryHandlerFactory)
        {
            this.commandBus = commandBus;
            this.queryHandlerFactory = queryHandlerFactory;
        }

        public IQueryHandlerFactory QueryHandlerFactory
        {
            get { return queryHandlerFactory; }
        }

        protected void Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            commandBus.Send(command);
        }

        protected TResult Query<TQuery, TResult>(TQuery query)
        {
            IQueryHandler<TQuery, TResult> handler = queryHandlerFactory.CreateHandler<TQuery, TResult>();
            TResult result = handler.Execute(query);
            queryHandlerFactory.Release(handler);
            return result;
        }
    }
}