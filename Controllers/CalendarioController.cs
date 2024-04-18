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
		[HttpGet]
        public JsonResult ListarCrontabs()
        {
		    var listaCrons = (from s in _DBContext.CP_CRONTABS select s).ToList();  
            return Json(new {data = listaCrons });
        }
        
		[HttpGet]
        public JsonResult ListarCrontabs0()
        {
            List<CP_CRONTAB> listaCrons = new List<CP_CRONTAB>();
            var sqlQuery = "";
			var oracleParameters = new List<OracleParameter>();
            
            //Llamado getCrontabs que devuelve un objeto de tipo cursor Y recibe como parametros un id
			sqlQuery = @"BEGIN APP_SCL_ALTAMIRA.getCrontabs(:id, :p_cursor1, :p_cursor2, :p_cursor3);END;";
			
			oracleParameters = new List<OracleParameter>
            {
                new OracleParameter("p_idcrontab", 10),
                new OracleParameter("p_recordset", OracleDbType.RefCursor, ParameterDirection.Output),        
            };
			
            var connection = _DBContext.Database.GetDbConnection();            
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;
            command.Parameters.AddRange(oracleParameters.ToArray());
            using (var reader = command.ExecuteReader())
            {
                /*listaCrons = ((IObjectContextAdapter)_DBContext).ObjectContext
                .Translate<CP_CRONTAB>(reader)
                .ToList();*/
            }
            connection.Close();
 
            return Json(new {data = listaCrons });
        }

    }
}