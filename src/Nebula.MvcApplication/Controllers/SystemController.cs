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
        private readonly IQueryFactory queryFactory;

        public SystemController(IQueryFactory queryFactory)
        {
            this.queryFactory = queryFactory;
        }

        [HttpGet]
        public ActionResult About()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Logging()
        {
            return View();
        }

        [HttpGet]
        [OutputCache(Duration = 43200)]
        public PartialViewResult Version()
        {
            var model = new VersionModel();

            var getLastRunMigration = queryFactory.CreateQuery<IGetLastRunMigration>();

            model.Assembly = typeof (Boot).Assembly.GetName().Version.ToString();
            model.Schema = getLastRunMigration.Execute(QuerySearch.Empty).ToString();

            return PartialView(model);
        }
    }
}