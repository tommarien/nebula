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

            var getLastRunMigration = queryHandlerFactory.CreateQuery<IGetLastRunMigration>();

            model.Assembly = typeof(Boot).Assembly.GetName().Version.ToString();
            model.Schema = getLastRunMigration.Execute(QuerySearch.Empty).ToString();

            return PartialView(model);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public JsonResult PagedLogSummary(DatatableModel model)
        {
            var queryHandler = queryHandlerFactory.CreateQuery<IPagedLogSummaryQueryHandler>();

            var result = queryHandler.Execute(new LogSummarySearch { Skip = model.iDisplayStart, Take = model.iDisplayLength });

            return Json(new
                            {
                                model.sEcho,
                                iTotalRecords = result.TotalResults,
                                iTotalDisplayRecords = result.TotalResults,
                                aaData = result.Results
                            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [OutputCache(Duration = 3600, VaryByParam = "*")]
        [Authorize(Roles = "Administrator")]
        public PartialViewResult LogDetail(int id)
        {
            var queryHandler = queryHandlerFactory.CreateQuery<ILogEntryDetailsQueryHandler>();

            var result = queryHandler.Execute(id);

            return PartialView(result);
        }
    }
}