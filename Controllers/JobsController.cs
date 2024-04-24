using Microsoft.AspNetCore.Mvc;
using WebAdminScheduler.Models;
using WebAdminScheduler.Models.ViewModels;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;

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
        public JsonResult ListarProcess1() {

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
                textSearch +=" AND (IDCONEX like '%"+searchValue.ToLower()+"%')";  
                textSearch +=" OR (NOMBRE like '%"+searchValue.ToLower()+"%')";  
                textSearch +=" OR (DESCRIPCION like '%"+searchValue.ToLower()+"%')";  
                textSearch +=" OR (PATH like '%"+searchValue.ToLower()+"%')";  
                textSearch +=" OR (PARAMETRO1 like '%"+searchValue.ToLower()+"%')";
                textSearch +=" OR (PARAMETRO2 like '%"+searchValue.ToLower()+"%')";  
                textSearch +=" OR (PARAMETRO3 like '%"+searchValue.ToLower()+"%')";  
                textSearch +=" OR (PARAMETRO4 like '%"+searchValue.ToLower()+"%'))"; 
                textSearch +=" OR (DEPENDENCIA like '%"+searchValue.ToLower()+"%'))"; 
                textSearch +=" OR (INTENTOS like '%"+searchValue.ToLower()+"%'))";   
                textSearch +=" OR (ESPERA_INTENTO like '%"+searchValue.ToLower()+"%'))"; 
                textSearch +=" OR (ESTADO like '%"+searchValue.ToLower()+"%'))"; 
                textSearch +=" OR (FTP like '%"+searchValue.ToLower()+"%'))";  
                textSearch +=" OR (IDHOST like '%"+searchValue.ToLower()+"%'))"; 
                textSearch +=" OR (COMPRENSION like '%"+searchValue.ToLower()+"%'))"; 
                textSearch +=" OR (IDCRONTAB like '%"+searchValue.ToLower()+"%'))"; 
                textSearch +=" OR (NODE like '%"+searchValue.ToLower()+"%'))"; 
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
					cp_procesos.PATH= oraReader.GetString(4);
					cp_procesos.PARAMETRO1= oraReader.GetString(5);
					cp_procesos.PARAMETRO2= oraReader.GetString(6);
					cp_procesos.PARAMETRO3= oraReader.GetString(7);
					cp_procesos.PARAMETRO4= oraReader.GetString(8);
					cp_procesos.DEPENDENCIA= oraReader.GetInt32(9);
					cp_procesos.INTENTOS= oraReader.GetInt32(10);
					cp_procesos.ESPERA_INTENTO= oraReader.GetInt32(11);
					cp_procesos.ESTADO= oraReader.GetString(12);
					cp_procesos.FTP= oraReader.GetInt32(13);
					cp_procesos.IDHOST= oraReader.GetInt32(14);
					cp_procesos.COMPRESION= oraReader.GetInt32(15);
					cp_procesos.IDCRONTAB= oraReader.GetInt32(16);
					cp_procesos.NODE= oraReader.GetString(17);
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
   }
}