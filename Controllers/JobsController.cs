using Microsoft.AspNetCore.Mvc;
using WebAdminScheduler.Models;
using WebAdminScheduler.Models.ViewModels;
using System.Diagnostics;

namespace WebAdminScheduler.Controllers
{
    public class JobsController : Controller
    {
        private readonly WebAdminSchedulerContext _DBContext;
        private readonly ILogger<JobsController> _logger;
        public JobsController(WebAdminSchedulerContext context)
        {
            _DBContext = context;
        }
        public IActionResult Index()
        {
            var data = (from s in _DBContext.CP_PROCESOS select s).ToList();  
            ViewBag.CP_PROCESOS = data;  
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        public JsonResult ListarProcesos() {

            int NroPeticion = Convert.ToInt32(Request.Form["draw"].FirstOrDefault() ?? "0");

            //cuantos registros va a devolver
            int CantidadRegistros = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");

            //cuantos registros va a omitir
            int OmitirRegistros = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");

            //el texto de busqueda
            string ValorBuscado = Request.Form["search[value]"].FirstOrDefault() ?? "";

            IQueryable<CP_PROCESOS> queryProcesos = _DBContext.CP_PROCESOS;
            // Total de registros antes de filtrar.
            int TotalRegistros = queryProcesos.Count();

            int TotalRegistrosFiltrados = queryProcesos.Count();

            var listaCrons = queryProcesos.ToList();

            return Json(new {
                draw = NroPeticion,
                recordsTotal = TotalRegistros,
                recordsFiltered = TotalRegistros,
                data=listaCrons
            });
        }
   }
}