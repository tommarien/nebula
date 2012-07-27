﻿using System.Web.Mvc;
using Nebula.Contracts.System.Commands;
using Nebula.Contracts.System.Queries;
using Nebula.Infrastructure.Commanding;
using Nebula.Infrastructure.Querying;
using Nebula.MvcApplication.Models;

namespace Nebula.MvcApplication.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class LogController : Controller
    {
        private readonly ICommandDispatcher commandDispatcher;
        private readonly IQueryHandlerFactory queryHandlerFactory;

        public LogController(IQueryHandlerFactory queryHandlerFactory, ICommandDispatcher commandDispatcher)
        {
            this.queryHandlerFactory = queryHandlerFactory;
            this.commandDispatcher = commandDispatcher;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult Data(DatatableModel model)
        {
            var queryHandler = queryHandlerFactory.CreateQuery<IPagedLogSummaryQueryHandler>();

            var result = queryHandler.Execute(new LogSummarySearch {Skip = model.iDisplayStart, Take = model.iDisplayLength});

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
        public PartialViewResult Details(int id)
        {
            var queryHandler = queryHandlerFactory.CreateQuery<ILogEntryDetailsQueryHandler>();

            var result = queryHandler.Execute(id);

            return PartialView(result);
        }

        [HttpGet]
        public ActionResult Purge()
        {
            commandDispatcher.Dispatch(new PurgeEventLogOlderThan1WeekCommand());

            return RedirectToAction("Index");
        }
    }
}