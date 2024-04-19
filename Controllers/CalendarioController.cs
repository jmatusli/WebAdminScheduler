using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using WebAdminScheduler.Models;
using WebAdminScheduler.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace WebAdminScheduler.Controllers
{
    public class CalendarioController : Controller
    {
        private readonly WebAdminSchedulerContext _DBContext;
        private readonly ILogger<CalendarioController> _logger;
        public CalendarioController(WebAdminSchedulerContext context)
        {
            _DBContext = context;
        }
        public IActionResult Index()
        {
            var data = (from s in _DBContext.CP_CRONTABS select s).ToList();  
            ViewBag.CP_CRONTAB = data;  
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
		
		[HttpPost]
		public JsonResult Save(Cp_CrontabVM user)
		{
           CP_CRONTAB crontabt = new CP_CRONTAB();
           
            crontabt.IDCRONTAB=1000;
            crontabt.FECHA="00000000";
            crontabt.HORA_INICIO="0000";
            crontabt.HORA_FIN="0000";
            crontabt.RECURRENCIA="0000";
            crontabt.WDAY_M2S_EX="1111111";
            crontabt.DAY_EX="0";
            crontabt.MONTH_EX="0";
            crontabt.REPEAT_EVERY_MINS=0;
            crontabt.REPEAT_AFTER_FINISH=0;
           _DBContext.CP_CRONTABS.Add(crontabt);
           _DBContext.SaveChanges();
            Console.WriteLine("ejemplo "+crontabt.IDCRONTAB);
            return Json(crontabt);
		}
		[HttpPost]
        public JsonResult ListarCrontabs()
        {
            int NroPeticion = Convert.ToInt32(Request.Form["draw"].FirstOrDefault() ?? "0");

            //cuantos registros va a devolver
            int CantidadRegistros = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");

            //cuantos registros va a omitir
            int OmitirRegistros = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");

            //el texto de busqueda
            string ValorBuscado = Request.Form["search[value]"].FirstOrDefault() ?? "";

            IQueryable<CP_CRONTAB> queryCrontab = _DBContext.CP_CRONTABS;
            // Total de registros antes de filtrar.
            int TotalRegistros = queryCrontab.Count();

            int TotalRegistrosFiltrados = queryCrontab.Count();

            var listaCrons = queryCrontab/*.Skip(OmitirRegistros).Take(CantidadRegistros).*/.ToList();

            return Json(new {
                draw = NroPeticion,
                recordsTotal = TotalRegistros,
                recordsFiltered = TotalRegistros,
                data=listaCrons
            });
        }
    }
}