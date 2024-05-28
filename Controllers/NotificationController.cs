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
    public class NotificationController : Controller
    {
        private readonly WebAdminSchedulerContext _DBContext;
        private readonly ILogger<NotificationController> _logger;
        public NotificationController(WebAdminSchedulerContext context)
        {
            _DBContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            Int32 result = WACustomHelper.GetLastIDNOTIF(_DBContext);
            ViewBag.LastIdNotif=result;
            return View();
        }
        
		[HttpPost]
		public JsonResult Save([FromBody] NotificationVM dtonotification)
		{
           CP_NOTIFICATIONS notification = new CP_NOTIFICATIONS();
           DateTime fechaActual = DateTime.Today;
        
            notification.IDNOTIF=WACustomHelper.GetLastIDNOTIF(_DBContext);
            notification.RECIPIENTS=dtonotification.oNotification.RECIPIENTS;
            notification.NOTIFYSUCCESS=dtonotification.oNotification.NOTIFYSUCCESS;
            notification.NOTIFYFAILURE=dtonotification.oNotification.NOTIFYFAILURE;
            notification.NAME=dtonotification.oNotification.NAME;
            _DBContext.CP_NOTIFICATIONS.Add(notification);
            _DBContext.SaveChanges();
            Console.WriteLine("ejemplo "+notification.IDNOTIF);
            return Json(notification);
		}
        public IActionResult Detalle(int id)
        {
            CP_NOTIFICATIONS data = (from s in _DBContext.CP_NOTIFICATIONS.Where(x => x.IDNOTIF == id)
            select s).ToList().AsQueryable().FirstOrDefault();
			ViewBag.IDNOTIF = id;  
            return View(data);
        }
        public IActionResult Edit(int id)
        {
            CP_NOTIFICATIONS data = (from s in _DBContext.CP_NOTIFICATIONS.Where(x => x.IDNOTIF == id)
            select s).ToList().AsQueryable().FirstOrDefault();  
            ViewBag.IDNOTIF = id;  
            //ViewBag.data=data;
            return View(data);
        }
       
        [HttpPost]
        public JsonResult Update([FromBody] NotificationVM dtonotification)
        {
          
            CP_NOTIFICATIONS notification = (from s in _DBContext.CP_NOTIFICATIONS.Where(x => x.IDNOTIF == dtonotification.oNotification.IDNOTIF)
                                select s).ToList().AsQueryable().FirstOrDefault();
            notification.RECIPIENTS=dtonotification.oNotification.RECIPIENTS;
            notification.NOTIFYSUCCESS=dtonotification.oNotification.NOTIFYSUCCESS;
            notification.NOTIFYFAILURE=dtonotification.oNotification.NOTIFYFAILURE;
            notification.NAME=dtonotification.oNotification.NAME;
            _DBContext.CP_NOTIFICATIONS.Update(notification);
            _DBContext.SaveChanges();
        
            return Json(notification);
        }
        private bool NotificationsExists(int id)
        {
          return (_DBContext.CP_NOTIFICATIONS?.Any(e => e.IDNOTIF == id)).GetValueOrDefault();
        }

	    [HttpPost]
        public JsonResult ListarNotifications()
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
			IQueryable<CP_NOTIFICATIONS>  data = _DBContext.Set < CP_NOTIFICATIONS > ().AsQueryable();
			//Obtener el total de los datos de la tabla
			totalRecord = data.Count();
			// Buscar datos cuando se encuentre el valor de búsqueda
            if (!string.IsNullOrEmpty(searchValue)) {  
				textSearch +=" AND ((RECIPIENTS like '%' || :psearch || '%')";
				textSearch +=" OR (NOTIFYSUCCESS like '%' || :psearch || '%')";
				textSearch +=" OR (NOTIFYFAILURE like '%' || :psearch || '%')";
				textSearch +=" OR (NAME like '%' || :psearch || '%'))";
			}
			// get total count of records after search
			  
			if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection)) 
			textOrder=" ORDER BY "+sortColumn+" "+sortColumnDirection;
			 
			_DBContext.Database.OpenConnection();
            String _query="SELECT * FROM (SELECT cc.*,row_number() over "
            +"(ORDER BY cc.idnotif DESC) line_number FROM APP_SCL_ALTAMIRA.CP_NOTIFICATIONS cc ) "
            +" WHERE line_number BETWEEN  "+(skip+1)+" AND "+(skip+pageSize)+" "+textSearch+" "+textOrder;
	
			OracleCommand oraCommand = new OracleCommand(_query, 
			(OracleConnection)_DBContext.Database.GetDbConnection());
            oraCommand.Parameters.Add(new OracleParameter("psearch", searchValue));
            OracleDataReader oraReader = oraCommand.ExecuteReader();
            List<object> cp_notificationList = new List<object>();
            var idnotif = 0;
            var recipients = "";
            var notifysuccess = 0;
            var notifyfailure = 0;
            var name="";

			if (oraReader.HasRows)
			{
				while (oraReader.Read())
				{
					CP_NOTIFICATIONS cp_notifications = new CP_NOTIFICATIONS();
					/*cp_notifications.IDNOTIF*/ idnotif = oraReader.GetInt32(0);
					/*cp_notifications.RECIPIENTS */recipients = oraReader.GetString(1);
					/*cp_notifications.NOTIFYSUCCESS*/ notifysuccess = oraReader.GetInt32(2);
					/*cp_notifications.NOTIFYFAILURE */notifyfailure = oraReader.GetInt32(3);
					/*cp_notifications.NAME */name = oraReader.GetString(4);
                   
                var n = new { IDNOTIF = idnotif, RECIPIENTS=recipients, NOTIFYSUCCESS = notifysuccess, 
                    NOTIFYFAILURE = notifyfailure, NAME = name
                };
                cp_notificationList.Add(n);
            }
            filterRecord = cp_notificationList.Count();
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
            aaData = cp_notificationList,
        });
    }
}
}