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
		public JsonResult Save([FromBody] CrontabVM dtocrontab)
		{
           CP_CRONTAB crontabt = new CP_CRONTAB();
           DateTime fechaActual = DateTime.Today;
        

            crontabt.IDCRONTAB=WACustomHelper.GetLasIdCRON(_DBContext);
            crontabt.FECHA=dtocrontab.oCrontab.FECHA; //fechaActual.ToString("ddMMyyyy");
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
        public JsonResult Update([FromBody] CrontabVM dtocrontab)
        {
          
            CP_CRONTAB crontabt = (from s in _DBContext.CP_CRONTABS.Where(x => x.IDCRONTAB == dtocrontab.oCrontab.IDCRONTAB)
                                select s).ToList().AsQueryable().FirstOrDefault();
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
		   //List<CP_CRONTAB> cp_contrabList = new List<CP_CRONTAB>();
		   List<object> cp_contrabList = new List<object>();
           var idcrontab=0;
           var fecha="";
           var horaInicio="";
            var horaFin="";
            var recurrencia="";
            var wday_m2s_ex="";
            var dayex="";
            var monthex="";
            var repeatevery_mins=0;
            var repeatafter_finish="";
			if (oraReader.HasRows)
			{
				while (oraReader.Read())
				{
					CP_CRONTAB cp_crontab = new CP_CRONTAB();
					/*cp_crontab.IDCRONTAB*/ idcrontab= oraReader.GetInt32(0);
					/*cp_crontab.FECHA */fecha= oraReader.GetString(1);
					/*cp_crontab.HORA_INICIO*/   string horain= oraReader.GetString(2);
                    horaInicio = horain[0] + "" + "" + horain[1] + ":" + horain[2] + "" + horain[3];

					/*cp_crontab.RECURRENCIA */recurrencia= oraReader.GetString(3);
					/*cp_crontab.HORA_FIN */string horafin= oraReader.GetString(4);
                    horaFin=horafin[0]+""+""+horafin[1]+":"+horafin[2]+""+horafin[3];
                    string dias = oraReader.GetString(5);
                    string tdias = "";
                    string textdia = "";
                    if(dias.Substring(0,1) == "1") 
                    { 
                        tdias = "Lun, ";
                        textdia += tdias;
                    }

                    if(dias.Substring(1,1) == "1") 
                    { 
                        tdias = "Mar, "; 
                        textdia += tdias;
                    }

                    if(dias.Substring(2,1) == "1") 
                    {
                        tdias = "Mie, ";
                        textdia += tdias;
                    }

                    if(dias.Substring(3,1) == "1") 
                    {
                        tdias = "Jue, ";
                        textdia += tdias;
                    }

                    if(dias.Substring(4,1) == "1") {
                        tdias = "Vie, ";
                        textdia += tdias;
                    }

                    if(dias.Substring(5,1) == "1") { 
                        tdias = "Sab, ";
                        textdia += tdias;
                    }

                    if(dias.Substring(6,1) == "1") { 
                        tdias = "Dom.";
                        textdia += tdias;
                    }
                        
					/*cp_crontab.WDAY_M2S_EX */wday_m2s_ex = textdia;
					/*cp_crontab.DAY_EX*/ dayex= oraReader.GetString(6);
					string monts_exe = oraReader.GetString(7);
                    string tmonts_exe = "";
                    string textmonts_exe = "";
                    string[] month_tmp = monts_exe.Split(',');
                   foreach (var month in month_tmp)
                    {
                        
                   if(month == "1") 
                    {
                        tmonts_exe = "Ene, ";
                        textmonts_exe += tmonts_exe;
                    }

                    if(month == "2") 
                    { 
                        tmonts_exe = "Feb, "; 
                        textmonts_exe += tmonts_exe;
                    }

                    if(month == "3") 
                    { 
                        tmonts_exe = "Marz, "; 
                        textmonts_exe += tmonts_exe;
                    }

                    if(month == "4") 
                    { 
                        tmonts_exe = "Abr, "; 
                        textmonts_exe += tmonts_exe;
                    }

                    if(month == "5") 
                    { 
                        tmonts_exe = "May, "; 
                        textmonts_exe += tmonts_exe;
                    }

                    if(month == "6") 
                    { 
                        tmonts_exe = "Jun, "; 
                        textmonts_exe += tmonts_exe;
                    }

                    if(month == "7") 
                    { 
                        tmonts_exe = "Jul, "; 
                        textmonts_exe += tmonts_exe;
                    }

                    if(month == "8") 
                    { 
                        tmonts_exe = "Agos, "; 
                        textmonts_exe += tmonts_exe;
                    }

                    if(month == "9") 
                    { 
                        tmonts_exe = "Sept, "; 
                        textmonts_exe += tmonts_exe;
                    }

                    if(month == "10") 
                    { 
                        tmonts_exe = "Oct, "; 
                        textmonts_exe += tmonts_exe;
                    }

                    if(month== "11") 
                    { 
                        tmonts_exe = "Nov, "; 
                        textmonts_exe += tmonts_exe;
                    }

                    if(month == "12") 
                    { 
                        tmonts_exe = "Dic, "; 
                        textmonts_exe += tmonts_exe;
                    }



                    }
 
                 

                    /*cp_crontab.MONTH_EX */monthex= textmonts_exe.TrimEnd(' ').TrimEnd(',');
                   
                    if (!oraReader.IsDBNull(8))
                    {
                       /* cp_crontab.REPEAT_EVERY_MINS */repeatevery_mins= oraReader.GetInt32(8);   
                    }

                    if (!oraReader.IsDBNull(9))
                    {
                        var repeat_aft = oraReader.GetInt32(9);
                        string frepeat_aft = "";
                        if(repeat_aft == 1) 
                        { 
                            frepeat_aft = "Y";
                        } else {
                            frepeat_aft = "N";
                        } 

                        repeatafter_finish=frepeat_aft;
                    }
                    var c = new { IDCRONTAB = idcrontab,FECHA=fecha,HORA_INICIO=horaInicio,RECURRENCIA=recurrencia,
                        HORA_FIN=horaFin,WDAY_M2S_EX=wday_m2s_ex,DAY_EX=dayex,MONTH_EX=monthex,REPEAT_EVERY_MINS=repeatevery_mins,
                        REPEAT_AFTER_FINISH=repeatafter_finish
                    };
                    cp_contrabList.Add(c);
				}
				filterRecord = cp_contrabList.Count();
			}
			else
			{
				return Json(new {
            draw = draw, 
                iTotalRecords = 0,
                iDisplayLength = 0,
                iTotalDisplayRecords = 0,
            aaData=new {}});
			}

			oraReader.Close();
			_DBContext.Database.CloseConnection();
		    return Json(new {
                draw = draw, 
                iTotalRecords = totalRecord,
                iDisplayLength = 10,
                iTotalDisplayRecords = totalRecord,
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