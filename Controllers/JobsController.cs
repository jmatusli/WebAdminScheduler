using Microsoft.AspNetCore.Mvc;
using WebAdminScheduler.Models;
using WebAdminScheduler.Models.ViewModels;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using WebAdminScheduler.helpers;
using System.Linq.Dynamic.Core;
using System.Data;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Collections;
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
            Int32 result = WACustomHelper.GetLastIdPROC(_DBContext);
            ViewBag.LastIdproc = result;
            return View();
        }

        [HttpPost]
        public JsonResult Save([FromBody] ProcesosVM dtoprocesos)
        {
            CP_PROCESOS procesost = new CP_PROCESOS();

            procesost.IDPROC = WACustomHelper.GetLastIdPROC(_DBContext);
            procesost.COMPRESION = dtoprocesos.oProcesos.COMPRESION;
            procesost.DEPENDENCIA = dtoprocesos.oProcesos.DEPENDENCIA;
            procesost.DESCRIPCION = dtoprocesos.oProcesos.DESCRIPCION;
            procesost.IDCONEX = dtoprocesos.oProcesos.IDCONEX;
            procesost.IDCRONTAB = dtoprocesos.oProcesos.IDCRONTAB;
            procesost.ESPERA_INTENTO = dtoprocesos.oProcesos.ESPERA_INTENTO;
            procesost.ESTADO = dtoprocesos.oProcesos.ESTADO;
            procesost.FTP = dtoprocesos.oProcesos.FTP;
            procesost.IDHOST = dtoprocesos.oProcesos.IDHOST;
            procesost.INTENTOS = dtoprocesos.oProcesos.INTENTOS;
            procesost.NODE = dtoprocesos.oProcesos.NODE;
            procesost.NOMBRE = dtoprocesos.oProcesos.NOMBRE;
            procesost.PARAMETRO1 = dtoprocesos.oProcesos.PARAMETRO1;
            procesost.PARAMETRO2 = dtoprocesos.oProcesos.PARAMETRO2;
            procesost.PARAMETRO3 = dtoprocesos.oProcesos.PARAMETRO3;
            procesost.PARAMETRO4 = dtoprocesos.oProcesos.PARAMETRO4;
            procesost.PATH = dtoprocesos.oProcesos.PATH;

            _DBContext.CP_PROCESOS.Add(procesost);
            _DBContext.SaveChanges();
           
            List<CP_DEPENDENCIAS> depProcs = dtoprocesos.oDependencias;
            foreach(var dtoProc in depProcs)
            {
             CP_DEPENDENCIAS dependencias=new CP_DEPENDENCIAS();
             dependencias.IDDEP=WACustomHelper.GetLastIDDEP(_DBContext);
             dependencias.IDPROC=procesost.IDPROC;
             dependencias.IDPROC_DEP=dtoProc.IDPROC_DEP;
             _DBContext.CP_DEPENDENCIAS.Add(dependencias);
             _DBContext.SaveChanges();
             
            }     
 
            Console.WriteLine("ejemplo " + procesost.IDPROC);
            return Json(procesost);
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
        public IActionResult Dependencia(int id)
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
        public JsonResult Update([FromBody] ProcesosVM dtoprocesos)
        { 
            CP_PROCESOS procesost = (from s in _DBContext.CP_PROCESOS.Where(x => x.IDPROC == dtoprocesos.oProcesos.IDPROC)
                                select s).ToList().AsQueryable().FirstOrDefault();

            CP_PROCESOS procesosval = (from s in _DBContext.CP_PROCESOS.Where(x => x.NOMBRE == dtoprocesos.oProcesos.NOMBRE && x.IDPROC != dtoprocesos.oProcesos.IDPROC)
                                select s).ToList().AsQueryable().FirstOrDefault();
        
            if(procesosval!=null && procesosval.IDPROC>0)
            {
                return Json(new { error=true, msg = "Ya existe un proceso con el nombre "+dtoprocesos.oProcesos.NOMBRE });
            }
            else 
            {
                procesost.COMPRESION = dtoprocesos.oProcesos.COMPRESION;
                procesost.DEPENDENCIA = dtoprocesos.oProcesos.DEPENDENCIA;
                procesost.DESCRIPCION = dtoprocesos.oProcesos.DESCRIPCION;
                procesost.IDCONEX = dtoprocesos.oProcesos.IDCONEX;
                procesost.ESPERA_INTENTO = dtoprocesos.oProcesos.ESPERA_INTENTO;
                procesost.ESTADO = dtoprocesos.oProcesos.ESTADO;
                procesost.FTP = dtoprocesos.oProcesos.FTP;
                procesost.IDHOST = dtoprocesos.oProcesos.IDHOST;
                procesost.INTENTOS = dtoprocesos.oProcesos.INTENTOS;
                procesost.NODE = dtoprocesos.oProcesos.NODE;
                procesost.NOMBRE = dtoprocesos.oProcesos.NOMBRE;
                procesost.IDCRONTAB = dtoprocesos.oProcesos.IDCRONTAB;
                procesost.PARAMETRO1 = dtoprocesos.oProcesos.PARAMETRO1;
                procesost.PARAMETRO2 = dtoprocesos.oProcesos.PARAMETRO2;
                procesost.PARAMETRO3 = dtoprocesos.oProcesos.PARAMETRO3;
                procesost.PARAMETRO4 = dtoprocesos.oProcesos.PARAMETRO4;
                procesost.PATH = dtoprocesos.oProcesos.PATH;
        
                _DBContext.CP_PROCESOS.Update(procesost);
                _DBContext.SaveChanges();
            }

            return Json(procesost);
        }

        [HttpPost]
        public JsonResult ListarProcesos()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            string textOrder = "";
            string textSearch = "";
            var draw = Request.Form["draw"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            string estado_param = Request.Form["estado"].FirstOrDefault() ?? "Activo";
            string estado_paramtmp =""; 

            List<string> estados = new List<string> {};
            if(estado_param=="Activo")
            {
                estados.Add("Activo");
                estados.Add("Internal");
                estado_paramtmp=" WHERE estado IN ('Activo','Internal')";
            }
            else if(estado_param == "Inactivo")
            {
                estados.Add("Inactivo");
                estado_paramtmp=" WHERE estado = 'Inactivo'";
            }

            var dataproct= _DBContext.CP_PROCESOS.Where(x=>estados.Contains(x.ESTADO));
             
            var dataproc = (from ep in dataproct
                join e in _DBContext.Set<CP_CONEXION>() on ep.IDCONEX equals e.IDCONEX
                 select new {
                    IDCONEX=e.IDCONEX,
                    usuario=e.USUARIO
                }).AsQueryable();
 
            //IQueryable<CP_PROCESOS> data = (IQueryable<CP_PROCESOS>)dataproc;
            //Obtener el total de los datos de la tabla
            totalRecord = dataproc.Count();
            // Buscar datos cuando se encuentre el valor de búsqueda
            if (!string.IsNullOrEmpty(searchValue))
            {
                textSearch += " AND ((IDCONEX like '%' || :psearch || '%')";
                textSearch += " OR (NOMBRE like '%' || :psearch || '%')";
                textSearch += " OR (DESCRIPCION like '%' || :psearch || '%')";
                textSearch += " OR (PATH like '%' || :psearch || '%')";
                textSearch += " OR (PARAMETRO1 like '%' || :psearch || '%')";
                textSearch += " OR (PARAMETRO2 like '%' || :psearch || '%')";
                textSearch += " OR (PARAMETRO3 like '%' || :psearch || '%')";
                textSearch += " OR (PARAMETRO4 like '%' || :psearch || '%')";
                textSearch += " OR (DEPENDENCIA like '%' || :psearch || '%')";
                textSearch += " OR (INTENTOS like '%' || :psearch || '%')";
                textSearch += " OR (ESPERA_INTENTO like '%' || :psearch || '%')";
                textSearch += " OR (ESTADO like '%' || :psearch || '%')";
                textSearch += " OR (FTP like '%' || :psearch || '%')";
                textSearch += " OR (IDHOST like '%' || :psearch || '%')";
                textSearch += " OR (COMPRESION like '%' || :psearch || '%')";
                textSearch += " OR (IDCRONTAB like '%' || :psearch || '%')";
                textSearch += " OR (usuario like '%' || :psearch || '%')";
                textSearch += " OR (NODE like '%' || :psearch || '%'))";
            }
            // get total count of records after search

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
                textOrder = " ORDER BY " + sortColumn + " " + sortColumnDirection;

            _DBContext.Database.OpenConnection();
            String _query = "SELECT * FROM (SELECT cc.*,con.usuario,row_number() over "
            + "(ORDER BY cc.IDPROC DESC) line_number FROM APP_SCL_ALTAMIRA.CP_PROCESOS cc "
            + " JOIN APP_SCL_ALTAMIRA.CP_CONEXION con on cc.idconex=con.idconex "
            + estado_paramtmp+")"
            + " WHERE line_number BETWEEN  " + (skip + 1) + " AND " + (skip + pageSize) + " " + textSearch + " " + textOrder;

            OracleCommand oraCommand = new OracleCommand(_query,
            (OracleConnection)_DBContext.Database.GetDbConnection());
            oraCommand.Parameters.Add(new OracleParameter("psearch", searchValue));
            OracleDataReader oraReader = oraCommand.ExecuteReader();
            
            List<object> procesosList = new List<object>();
            var path="";
            var parametro1="";
            var parametro2="";
            var parametro3="";
            var parametro4="";   
            var estado=""; 
            var ftp=0;
            var idhost=0;
            var compresion=0;
            var idcrontab=0;
            var node="";
        
            if (oraReader.HasRows)
            {
                while (oraReader.Read())
                {
                    var cpprocesos =new object();
                    // cpprocesos.nombre=oraReader.GetString(2);
                    CP_PROCESOS cp_procesos = new CP_PROCESOS();
                    /*  cp_procesos.IDPROC */ var idproc= oraReader.GetInt32(0);
                    /* cp_procesos.IDCONEX*/  var usuario= oraReader.GetString(18);
                    /*cp_procesos.NOMBRE*/ var nombre= oraReader.GetString(2);
                    /*cp_procesos.DESCRIPCION */var descripcion= oraReader.GetString(3);

                    if (!oraReader.IsDBNull(4))
                    {
                        /*cp_procesos.PATH*/  path= oraReader.GetString(4);
                    }
                    if (!oraReader.IsDBNull(5))
                    {
                       /* cp_procesos.PARAMETRO1*/ parametro1= oraReader.GetString(5);
                    }
                    if (!oraReader.IsDBNull(6))
                    {
                        /*cp_procesos.PARAMETRO2*/  parametro2=oraReader.GetString(6);
                    }
                    if (!oraReader.IsDBNull(7))
                    {
                        /*cp_procesos.PARAMETRO3 */ parametro3=oraReader.GetString(7);
                    }
                    if (!oraReader.IsDBNull(8))
                    {
                       /* cp_procesos.PARAMETRO4 */parametro4= oraReader.GetString(8);
                    }

                    /*cp_procesos.DEPENDENCIA */var dependencia=oraReader.GetInt32(9);
                    /*cp_procesos.INTENTOS*/ var intentos= oraReader.GetInt32(10);
                    /* cp_procesos.ESPERA_INTENTO*/ var espera_intento= oraReader.GetInt32(11);

                    if (!oraReader.IsDBNull(12))
                    {
                       /* cp_procesos.ESTADO*/ estado= oraReader.GetString(12);
                    }
                    if (!oraReader.IsDBNull(13))
                    {
                       /* cp_procesos.FTP*/ ftp=oraReader.GetInt32(13);
                    }
                    if (!oraReader.IsDBNull(14))
                    {
                        /*cp_procesos.IDHOST*/ idhost= oraReader.GetInt32(14);
                    }
                    if (!oraReader.IsDBNull(15))
                    {
                       /* cp_procesos.COMPRESION */compresion=oraReader.GetInt32(15);
                    }
                    if (!oraReader.IsDBNull(16))
                    {
                        /*cp_procesos.IDCRONTAB */idcrontab=oraReader.GetInt32(16);
                    }
                    if (!oraReader.IsDBNull(17))
                    {
                       /* cp_procesos.NODE */node=oraReader.GetString(17);
                    }
                    var c = new { IDPROC = idproc, USUARIO = usuario,NOMBRE=nombre,DESCRIPCION =descripcion,
                        PATH=path,PARAMETRO1=parametro1,PARAMETRO2=parametro2,PARAMETRO3=parametro3,PARAMETRO4=parametro4,
                        DEPENDENCIA=dependencia,INTENTOS=intentos,ESPERA_INTENTO=espera_intento,ESTADO=estado,FTP=ftp,
                        IDHOST=idhost,COMPRESION=compresion,IDCRONTAB=idcrontab,NODE=node
                    };
                    procesosList.Add(c);  
                }
                filterRecord = procesosList.Count();
            }
            else
            {
                return Json(new {
                    draw = draw, 
                    iTotalRecords = 0,
                    iDisplayLength = 0,
                    iTotalDisplayRecords = 0,
                    aaData=new {}
                });
            }

            oraReader.Close();
            _DBContext.Database.CloseConnection();
            return Json(new
            {
                draw = draw,
                iTotalRecords = totalRecord,
                iDisplayLength = 10,
                iTotalDisplayRecords = totalRecord,
                aaData = procesosList,
            });
        }
        [HttpPost]
        public JsonResult ListarDependencias([FromBody] DependenciasVM dtodependencias) 
        {
            var id_proc = dtodependencias.oDependencias.IDPROC;
            _DBContext.Database.OpenConnection();
            /*String _query = "SELECT cpp.IDPROC,cpp.NOMBRE"
            +" FROM APP_SCL_ALTAMIRA.CP_PROCESOS cp "
            +" JOIN APP_SCL_ALTAMIRA.CP_DEPENDENCIAS cd on cd.IDPROC = cp.IDPROC "
            +" JOIN APP_SCL_ALTAMIRA.CP_PROCESOS cpp on cpp.IDPROC = cd.IDPROC_DEP "
            + " WHERE cp.idproc = :id_proc";*/

              String _query = "SELECT cp.IDPROC,cp.NOMBRE"
            +" FROM APP_SCL_ALTAMIRA.CP_PROCESOS cp WHERE DEPENDENCIA=1 ";
 
            Console.WriteLine(" EN EJECUCION "+_query);
            OracleCommand oraCommand = new OracleCommand(_query,
            (OracleConnection)_DBContext.Database.GetDbConnection());
            //oraCommand.Parameters.Add(new OracleParameter("id_proc", id_proc));
            OracleDataReader oraReader = oraCommand.ExecuteReader();
            
            List<object> dependenciaList = new List<object>();
             if (oraReader.HasRows)
            {
                while (oraReader.Read())
                {
                   var c = new { IDPROC=oraReader.GetInt32(0),NOMBRE=oraReader.GetString(1)};
                dependenciaList.Add(c);  
                }
            }       
            return Json(dependenciaList);
        }
        public JsonResult ListarRegistro()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            string textOrder = "";
            string textSearch = "";
            var draw = Request.Form["draw"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pidproc = Convert.ToInt32(Request.Form["idproc"].FirstOrDefault() ?? "0");
            int filterby = Convert.ToInt32(Request.Form["filterby"].FirstOrDefault() ?? "0");
            string textfilterby="";
            int NumeroDias = -1;

            if(filterby == 0){
                textfilterby =" AND cr.FEC_EJECUCION BETWEEN TO_DATE(SYSDATE) AND TO_DATE(SYSDATE)+1";
            }
            else if(filterby==1)
            {
                NumeroDias=-7;  
                textfilterby =" AND cr.FEC_EJECUCION BETWEEN TO_DATE(SYSDATE)-7 AND TO_DATE(SYSDATE)+1";
            }
            else
            {
                NumeroDias=-15;
                textfilterby =" AND cr.FEC_EJECUCION BETWEEN TO_DATE(SYSDATE)-15 AND TO_DATE(SYSDATE)+1";
            }

            Console.WriteLine("esto se va a evaluar "+DateTime.Today.AddDays(NumeroDias));
            var datareg=(from ep in _DBContext.Set<CP_REGISTRO>()
                where ep.IDPROC == pidproc && ep.FEC_EJECUCION >= DateTime.Today.AddDays(NumeroDias) 
                && ep.FEC_EJECUCION <= DateTime.Today
                && ep.ESTADO.Contains(searchValue) 
                select new {
                    IDREG=ep.IDREG 
                }).AsQueryable();
            
            totalRecord = datareg.Count();
            // Buscar datos cuando se encuentre el valor de búsqueda
            
            if (!string.IsNullOrEmpty(searchValue))
            {
                textSearch += " AND ((IDREG like '%' || :psearch || '%')";
                textSearch += " OR (FEC_INICIO like '%' || :psearch || '%')";
                textSearch += " OR (FEC_EJECUCION like '%' || :psearch || '%')";
                textSearch += " OR (FEC_FINALIZO like '%' || :psearch || '%')";
                textSearch += " OR (ESTADO like '%' || :psearch || '%'))";
            }
            // get total count of records after search

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
                textOrder = " ORDER BY " + sortColumn + " " + sortColumnDirection;

            _DBContext.Database.OpenConnection();
            String _query = "SELECT * FROM (SELECT IDREG,IDPROC,FEC_INICIO,FEC_EJECUCION,FEC_FINALIZO,ESTADO,"
            +"row_number() over(ORDER BY cr.IDREG  ASC) line_number FROM APP_SCL_ALTAMIRA.CP_REGISTRO cr "
            +" WHERE IDPROC = '"+pidproc+"' "+textfilterby+" " + textSearch + " " + textOrder+") "
            + " WHERE line_number BETWEEN  " + (skip + 1) + " AND " + (skip + pageSize) ;
 
            Console.WriteLine(" EN EJECUCION "+_query);
            OracleCommand oraCommand = new OracleCommand(_query,
            (OracleConnection)_DBContext.Database.GetDbConnection());
            oraCommand.Parameters.Add(new OracleParameter("psearch", searchValue));
            //oraCommand.Parameters.Add(new OracleParameter("idproc", pidproc));
            OracleDataReader oraReader = oraCommand.ExecuteReader();
            
            List<object> registroList = new List<object>();
            var idreg=0;
            var idproc=0;
            DateTime? fecha_inicio = null;
            DateTime?  fecha_finalizo = null;
            DateTime? fecha_ejecucion = null;
            var estado=""; 
        
            if (oraReader.HasRows)
            {
                while (oraReader.Read())
                {
                    var cpregistros =new object();
                    idreg= oraReader.GetInt32(0);
                    //idproc= oraReader.GetInt32(1);
                if (!oraReader.IsDBNull(2))
                {
                    fecha_inicio=oraReader.GetDateTime(2);
                }
                if (!oraReader.IsDBNull(3))
                {
                    fecha_ejecucion=oraReader.GetDateTime(3);
                }  
                if (!oraReader.IsDBNull(4))
                {
                    fecha_finalizo=oraReader.GetDateTime(4);
                }     
                if (!oraReader.IsDBNull(5))
                {
                    estado=oraReader.GetString(5);
                }
                
                var c = new { IDREG=idreg,IDPROC=idproc,FEC_INICIO=fecha_inicio,FEC_EJECUCION=fecha_ejecucion,
                    FEC_FINALIZO=fecha_finalizo,ESTADO=estado
                };
                registroList.Add(c);  
            }
                filterRecord = registroList.Count();
            }
            else
            {
                return Json(new {
                    draw = draw, 
                    iTotalRecords = 0,
                    iDisplayLength = 0,
                    iTotalDisplayRecords = 0,
                    aaData=new {}
                });
            }

            oraReader.Close();
            _DBContext.Database.CloseConnection();
            return Json(new
            {
                draw = draw,
                iTotalRecords = totalRecord,
                iDisplayLength = 10,
                iTotalDisplayRecords = totalRecord,
                aaData = registroList,
            });
        }



       [HttpPost]
        public JsonResult GetNodeData(int prcId)
        {

        List<object> nodeDataList = new List<object>(); 
        List<CP_DEPENDENCIAS> data = (from s in _DBContext.CP_DEPENDENCIAS.Where(x => x.IDPROC == prcId)
                                select s).ToList().AsQueryable().ToList();
        
        var conectionsFirst = new ArrayList(); 
        var conections = new ArrayList();
        
        foreach(var item in data)
            {
                Console.WriteLine("esto es un dato "+item.IDPROC_DEP);
                conectionsFirst.Add(item.IDPROC_DEP);
                conections = new ArrayList();
                var c=new {id=item.IDPROC_DEP,connections=conections,level=1};
                nodeDataList.Add(c);   
                
            }                     
         
 
        nodeDataList.Add(new { x=0,y=0,id=prcId,connections=conectionsFirst,level=0});
    
 

           return Json(new { 
            Data = nodeDataList

           });  
        }
    }
}