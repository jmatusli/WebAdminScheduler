using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using WebAdminScheduler.Models;
using WebAdminScheduler.Models.ViewModels;
using System.Linq.Expressions;
using WebAdminScheduler.helpers;
using Microsoft.AspNetCore.Razor.Language;
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
            return View();
        }
        public IActionResult Create()
        {
            Int32 result = WACustomHelper.GetLasIdCRON(_DBContext);
            
            ViewBag.LasIdcrontab=result;
             
            return View();
        }
		
		[HttpPost]
		public JsonResult Save([FromBody] CrontabVM  dtocrontab)
		{
           CP_CRONTAB crontabt = new CP_CRONTAB();
           
            crontabt.IDCRONTAB=WACustomHelper.GetLasIdCRON(_DBContext);
            crontabt.FECHA=dtocrontab.oCrontab.FECHA;
            crontabt.HORA_INICIO=dtocrontab.oCrontab.HORA_INICIO;
            crontabt.HORA_FIN=dtocrontab.oCrontab.HORA_FIN;
            crontabt.RECURRENCIA=dtocrontab.oCrontab.RECURRENCIA;
            crontabt.WDAY_M2S_EX=dtocrontab.oCrontab.WDAY_M2S_EX;
            crontabt.DAY_EX=dtocrontab.oCrontab.DAY_EX;
            crontabt.MONTH_EX=dtocrontab.oCrontab.MONTH_EX;
            crontabt.REPEAT_EVERY_MINS=dtocrontab.oCrontab.REPEAT_EVERY_MINS;
            crontabt.REPEAT_AFTER_FINISH=dtocrontab.oCrontab.REPEAT_AFTER_FINISH;
           _DBContext.CP_CRONTABS.Add(crontabt);
           _DBContext.SaveChanges();
            Console.WriteLine("ejemplo "+crontabt.IDCRONTAB);
            return Json(crontabt);
		}
        public IActionResult Detalle(int id)
        {
            CP_CRONTAB data = (from s in _DBContext.CP_CRONTABS.Where(x => x.IDCRONTAB == id)
            select s).ToList().AsQueryable().FirstOrDefault();
			ViewBag.IDCRONTAB = id;  
            return View(data);
        }
        public IActionResult Edit(int id)
        {
            CP_CRONTAB data = (from s in _DBContext.CP_CRONTABS.Where(x => x.IDCRONTAB == id)
            select s).ToList().AsQueryable().FirstOrDefault();  
            ViewBag.IDCRONTAB = id;  
            //ViewBag.data=data;
            return View(data);
        }
       
        [HttpPost]
         
        public JsonResult Update([FromBody] CrontabVM  dtocrontab)
        {
            
            CP_CRONTAB crontabt = new CP_CRONTAB();
    
            /* CP_CRONTAB data = (from s in _DBContext.CP_CRONTABS.Where(x => x.IDCRONTAB == dtocrontab.oCrontab.IDCRONTAB)
            select s).ToList().AsQueryable().FirstOrDefault();*/
            crontabt.IDCRONTAB=dtocrontab.oCrontab.IDCRONTAB;
            crontabt.FECHA=dtocrontab.oCrontab.FECHA;
            crontabt.HORA_INICIO=dtocrontab.oCrontab.HORA_INICIO;
            crontabt.HORA_FIN=dtocrontab.oCrontab.HORA_FIN;
            crontabt.RECURRENCIA=dtocrontab.oCrontab.RECURRENCIA;
            crontabt.WDAY_M2S_EX=dtocrontab.oCrontab.WDAY_M2S_EX;
            crontabt.DAY_EX=dtocrontab.oCrontab.DAY_EX;
            crontabt.MONTH_EX=dtocrontab.oCrontab.MONTH_EX;
            crontabt.REPEAT_EVERY_MINS=dtocrontab.oCrontab.REPEAT_EVERY_MINS;
            crontabt.REPEAT_AFTER_FINISH=dtocrontab.oCrontab.REPEAT_AFTER_FINISH;
            _DBContext.CP_CRONTABS.Update(crontabt);
            _DBContext.SaveChanges();
        
            return Json(crontabt);
          
        }
        private bool CrontabsExists(int id)
        {
          return (_DBContext.CP_CRONTABS?.Any(e => e.IDCRONTAB == id)).GetValueOrDefault();
        }

	   [HttpPost]
        public JsonResult ListarCrontabs()
        {
			int totalRecord = 0;
			int filterRecord = 0;
			string textOrder="";
			string textSearch="";
			var draw = Request.Form["draw"].FirstOrDefault();
			var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
			var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
			var searchValue = Request.Form["search[value]"].FirstOrDefault();
			int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
			int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
			IQueryable<CP_CRONTAB>  data = _DBContext.Set < CP_CRONTAB > ().AsQueryable();
			//Obtener el total de los datos de la tabla
			totalRecord = data.Count();
			// Buscar datos cuando se encuentre el valor de búsqueda
            if (!string.IsNullOrEmpty(searchValue)) {  
				textSearch +=" AND ((FECHA like '%' || :psearch || '%')";
				textSearch +=" OR (HORA_INICIO like '%' || :psearch || '%')";
				textSearch +=" OR (RECURRENCIA like '%' || :psearch || '%')";
				textSearch +=" OR (HORA_FIN like '%' || :psearch || '%')";
				textSearch +=" OR (WDAY_M2S_EX like '%' || :psearch || '%')";
				textSearch +=" OR (DAY_EX like '%' || :psearch || '%')";
				textSearch +=" OR (MONTH_EX like '%' || :psearch || '%')";
				textSearch +=" OR (REPEAT_EVERY_MINS like '%' || :psearch || '%')";
				textSearch +=" OR (REPEAT_AFTER_FINISH like '%' || :psearch || '%'))";
			}
			// get total count of records after search
			  
			if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection)) 
			textOrder=" ORDER BY "+sortColumn+" "+sortColumnDirection;
			 
			_DBContext.Database.OpenConnection();
            String _query="SELECT * FROM (SELECT cc.*,row_number() over "
            +"(ORDER BY cc.idcrontab ASC) line_number FROM APP_SCL_ALTAMIRA.CP_CRONTAB cc ) "
            +" WHERE line_number BETWEEN  "+(skip+1)+" AND "+(skip+pageSize)+" "+textSearch+" "+textOrder;
	
			OracleCommand oraCommand = new OracleCommand(_query, 
			(OracleConnection)_DBContext.Database.GetDbConnection());
            oraCommand.Parameters.Add(new OracleParameter("psearch", searchValue));
		   OracleDataReader oraReader = oraCommand.ExecuteReader();
		   List<CP_CRONTAB> cp_contrabList = new List<CP_CRONTAB>();
		   
			if (oraReader.HasRows)
			{
				while (oraReader.Read())
				{
					CP_CRONTAB cp_crontab = new CP_CRONTAB();
					cp_crontab.IDCRONTAB = oraReader.GetInt32(0);
					cp_crontab.FECHA = oraReader.GetString(1);
					cp_crontab.HORA_INICIO = oraReader.GetString(2);
					cp_crontab.RECURRENCIA= oraReader.GetString(3);
					cp_crontab.HORA_FIN= oraReader.GetString(4);
                    string dias= oraReader.GetString(5);
                    string tdias="";
                    string textdia="";
                       if(dias.Substring(0,1)=="1") 
                       { tdias="l";
                       textdia += tdias;
                       }
                       if(dias.Substring(1,1)=="1") 
                       { tdias="m"; 
                       textdia += tdias;} 
                       if(dias.Substring(2,1)=="1") 
                        {tdias="m";
                        textdia += tdias;
                        }
                        if(dias.Substring(3,1)=="1") 
                        {
                        tdias="j";
                        textdia += tdias;
                        }
                        if(dias.Substring(4,1)=="1") {
                        tdias="v";
                        textdia += tdias;
                        }
                        if(dias.Substring(5,1)=="1") { 
                        tdias="s";
                        textdia += tdias;
                        }
                        if(dias.Substring(6,1)=="1") { 
                        tdias="d";
                        textdia += tdias;
                        }
                        

					cp_crontab.WDAY_M2S_EX= textdia;
					cp_crontab.DAY_EX= oraReader.GetString(6);
					cp_crontab.MONTH_EX= oraReader.GetString(7);
                     if (!oraReader.IsDBNull(8))
                    {
                    cp_crontab.REPEAT_EVERY_MINS= oraReader.GetInt32(8);
                        
                    }

                    if (!oraReader.IsDBNull(9))
                    {
                    cp_crontab.REPEAT_AFTER_FINISH= oraReader.GetInt32(9);
                        
                    }
					 
					 
					cp_contrabList.Add(cp_crontab);
					
				}
				filterRecord = cp_contrabList.Count();
		   
			}
			else
			{
				return Json(null);
			}

			oraReader.Close();
			_DBContext.Database.CloseConnection();
		    return Json(new {
                draw = draw, 
                iTotalRecords = totalRecord,
                iDisplayLength=10,
                iTotalDisplayRecords=totalRecord,
                aaData = cp_contrabList,
            });

		}
        public JsonResult ListarJobsAsoc(int idcrontab) {
            var data = (from s in _DBContext.CP_PROCESOS.Where(x => x.IDCRONTAB == idcrontab)
            select s).ToList();  
               // ViewBag.CP_PROCESOS = data;

               return Json(new {
                draw = 1, 
                iTotalRecords = 1,
                iDisplayLength=10,
                iTotalDisplayRecords=data.Count(),
                aaData = data,
            });   
              
        }
    }

}