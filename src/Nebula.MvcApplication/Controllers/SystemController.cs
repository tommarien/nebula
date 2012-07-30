using System.Globalization;
using System.Web.Mvc;
using Nebula.Bootstrapper;
using Nebula.Contracts.System.Queries;
using Nebula.Infrastructure.Querying;
using Nebula.MvcApplication.Models;

namespace Nebula.MvcApplication.Controllers
{
    [Authorize]
    public class SystemController : Controller
    {
        private readonly IQueryHandlerFactory queryHandlerFactory;

        public SystemController(IQueryHandlerFactory queryHandlerFactory)
        {
            this.queryHandlerFactory = queryHandlerFactory;
        }

        [HttpGet]
        public ActionResult About()
        {
            return View();
        }

        [HttpGet]
        [OutputCache(Duration = 43200)]
        public PartialViewResult Version()
        {
            var model = new VersionModel();

            var getLastRunMigration = queryHandlerFactory.CreateQuery<IGetLastRunMigration>();

            model.Assembly = typeof (Boot).Assembly.GetName().Version.ToString();
            model.Schema = getLastRunMigration.Execute(QuerySearch.Empty).ToString(CultureInfo.InvariantCulture);

            return PartialView(model);
        }
    }
}