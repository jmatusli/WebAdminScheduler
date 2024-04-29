using Microsoft.AspNetCore.Mvc;
using WebAdminScheduler.Models;
using WebAdminScheduler.Models.ViewModels;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using WebAdminScheduler.helpers;
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
        public IActionResult DetalleCrontab(int id)
        {
            CP_CRONTAB data = (from s in _DBContext.CP_CRONTABS.Where(x => x.IDCRONTAB == id)
            select s).ToList().AsQueryable().FirstOrDefault();
			ViewBag.IDCRONTAB = id;  
            return View(data);
        }
        public IActionResult Detalle(int id)
        {
            CP_PROCESOS data = (from s in _DBContext.CP_PROCESOS.Where(x => x.IDPROC == id)
            select s).ToList().AsQueryable().FirstOrDefault();
			ViewBag.IDPROC = id;
            return View(data);
        }
        public IActionResult Edit(int id)
        {
            CP_PROCESOS data = (from s in _DBContext.CP_PROCESOS.Where(x => x.IDPROC == id)
            select s).ToList().AsQueryable().FirstOrDefault();  
            ViewBag.IDPROC = id;  
            //ViewBag.data=data;
            return View(data);
        }
        [HttpPost]
        public JsonResult ListarProcesos()
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
			IQueryable<CP_PROCESOS>  data = _DBContext.Set < CP_PROCESOS > ().AsQueryable();
			//Obtener el total de los datos de la tabla
			totalRecord = data.Count();
			// Buscar datos cuando se encuentre el valor de búsqueda
			if (!string.IsNullOrEmpty(searchValue)) {
                textSearch +=" AND ((IDCONEX like '%' || :psearch || '%')";
                textSearch +=" OR (NOMBRE like '%' || :psearch || '%')";
                textSearch +=" OR (DESCRIPCION like '%' || :psearch || '%')";
                textSearch +=" OR (PATH like '%' || :psearch || '%')";
                textSearch +=" OR (PARAMETRO1 like '%' || :psearch || '%')";
                textSearch +=" OR (PARAMETRO2 like '%' || :psearch || '%')";
                textSearch +=" OR (PARAMETRO3 like '%' || :psearch || '%')";
                textSearch +=" OR (PARAMETRO4 like '%' || :psearch || '%')";
                textSearch +=" OR (DEPENDENCIA like '%' || :psearch || '%')";
                textSearch +=" OR (INTENTOS like '%' || :psearch || '%')";
                textSearch +=" OR (ESPERA_INTENTO like '%' || :psearch || '%')";
                textSearch +=" OR (ESTADO like '%' || :psearch || '%')";
                textSearch +=" OR (FTP like '%' || :psearch || '%')";
                textSearch +=" OR (IDHOST like '%' || :psearch || '%')";
                textSearch +=" OR (COMPRESION like '%' || :psearch || '%')";
                textSearch +=" OR (IDCRONTAB like '%' || :psearch || '%')";
 
                textSearch +=" OR (NODE like '%' || :psearch || '%'))";
			}
			// get total count of records after search
			  
			if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection)) 
			textOrder=" ORDER BY "+sortColumn+" "+sortColumnDirection;
			 
			_DBContext.Database.OpenConnection();
            String _query="SELECT * FROM (SELECT cc.*,row_number() over "
            +"(ORDER BY cc.IDPROC ASC) line_number FROM APP_SCL_ALTAMIRA.CP_PROCESOS cc ) "
            +" WHERE line_number BETWEEN  "+(skip+1)+" AND "+(skip+pageSize)+" "+textSearch+" "+textOrder;
	
			OracleCommand oraCommand = new OracleCommand(_query, 
			(OracleConnection)_DBContext.Database.GetDbConnection());
            oraCommand.Parameters.Add(new OracleParameter("psearch", searchValue));
		   OracleDataReader oraReader = oraCommand.ExecuteReader();
		   List<CP_PROCESOS> cp_procesosList = new List<CP_PROCESOS>();
		   
			if (oraReader.HasRows)
			{ 
				while (oraReader.Read())
				{
					CP_PROCESOS cp_procesos = new CP_PROCESOS();
					cp_procesos.IDPROC = oraReader.GetInt32(0);
					cp_procesos.IDCONEX = oraReader.GetInt32(1);
					cp_procesos.NOMBRE = oraReader.GetString(2);
					cp_procesos.DESCRIPCION= oraReader.GetString(3);
                    
                    if (!oraReader.IsDBNull(4))
                    {
                        cp_procesos.PATH= oraReader.GetString(4);
                    }
					 if (!oraReader.IsDBNull(5))
                    {
                        cp_procesos.PARAMETRO1= oraReader.GetString(5);
                    }
                    if (!oraReader.IsDBNull(6))
                    {
                        cp_procesos.PARAMETRO2= oraReader.GetString(6);
                    }

                    if (!oraReader.IsDBNull(7))
                    {
                        cp_procesos.PARAMETRO3= oraReader.GetString(7);
                    }

					if (!oraReader.IsDBNull(8))
                    {
                        cp_procesos.PARAMETRO4= oraReader.GetString(8);
                    }
 
					cp_procesos.DEPENDENCIA= oraReader.GetInt32(9);
					cp_procesos.INTENTOS= oraReader.GetInt32(10);
					cp_procesos.ESPERA_INTENTO= oraReader.GetInt32(11);

                    if (!oraReader.IsDBNull(12))
                    {
                        cp_procesos.ESTADO= oraReader.GetString(12);
                    }
				    if (!oraReader.IsDBNull(13))
                    {
                        cp_procesos.FTP= oraReader.GetInt32(13);
                    }
					if (!oraReader.IsDBNull(14))
                    {
                        cp_procesos.IDHOST= oraReader.GetInt32(14);
                    }
				    if (!oraReader.IsDBNull(15))
                    {
                        cp_procesos.COMPRESION= oraReader.GetInt32(15);
                    }
				    if (!oraReader.IsDBNull(16))
                    {
                        cp_procesos.IDCRONTAB= oraReader.GetInt32(16);
                    }
				    if (!oraReader.IsDBNull(17))
                    {
                        cp_procesos.NODE= oraReader.GetString(17);
                    }
				  
					cp_procesosList.Add(cp_procesos);
				}
				filterRecord = cp_procesosList.Count();
		   
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
                aaData = cp_procesosList,
            });
		}
        public JsonResult ListarRegistro(int idproc) {
            var data = (from s in _DBContext.CP_REGISTRO.Where(x => x.IDPROC == idproc)
            select s).ToList();

            return Json(new {
                draw = 1,
                iTotalRecords = 1,
                iDisplayLength=10,
                iTotalDisplayRecords=data.Count(),
                aaData = data,
            });
        }
		
        [HttpPost]
	    public JsonResult Save([FromBody] ProcesosVM  dtoprocesos)
		{
           CP_PROCESOS procesost = new CP_PROCESOS();
           
            procesost.IDPROC=WACustomHelper.GetLasIdPROC(_DBContext);
            procesost.COMPRESION=dtoprocesos.oProcesos.COMPRESION;
            procesost.DEPENDENCIA=dtoprocesos.oProcesos.DEPENDENCIA;
            procesost.DESCRIPCION=dtoprocesos.oProcesos.DESCRIPCION;
            procesost.IDCONEX=dtoprocesos.oProcesos.IDCONEX;
            procesost.IDCRONTAB=dtoprocesos.oProcesos.IDCRONTAB;
            procesost.ESPERA_INTENTO=dtoprocesos.oProcesos.ESPERA_INTENTO;
            procesost.ESTADO=dtoprocesos.oProcesos.ESTADO;
            procesost.FTP=dtoprocesos.oProcesos.FTP;
            procesost.IDHOST=dtoprocesos.oProcesos.IDHOST;
            procesost.INTENTOS=dtoprocesos.oProcesos.INTENTOS;
            procesost.NODE=dtoprocesos.oProcesos.NODE;
            procesost.NOMBRE=dtoprocesos.oProcesos.NOMBRE;
            procesost.PARAMETRO1=dtoprocesos.oProcesos.PARAMETRO1;
            procesost.PARAMETRO2=dtoprocesos.oProcesos.PARAMETRO2;
            procesost.PARAMETRO3=dtoprocesos.oProcesos.PARAMETRO3;
            procesost.PARAMETRO4=dtoprocesos.oProcesos.PARAMETRO4;
            procesost.PATH=dtoprocesos.oProcesos.PATH;

           _DBContext.CP_PROCESOS.Add(procesost);
           _DBContext.SaveChanges();
            Console.WriteLine("ejemplo "+procesost.IDPROC);
            return Json(procesost);
		}
   }
}