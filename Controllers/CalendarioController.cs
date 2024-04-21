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
        public JsonResult ListarCrontabss()
        {
                int totalRecord = 0;
    int filterRecord = 0;
    var draw = Request.Form["draw"].FirstOrDefault();
    var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
    var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
    var searchValue = Request.Form["search[value]"].FirstOrDefault();
    int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
    int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
    IQueryable<CP_CRONTAB>  data = _DBContext.Set < CP_CRONTAB > ().AsQueryable();
    //get total count of data in table
    totalRecord = data.Count();
    // search data when search value found
    if (!string.IsNullOrEmpty(searchValue)) {
        data = data.Where(x => x.FECHA.ToLower().Contains(searchValue.ToLower()) );
    }
    // get total count of records after search
    filterRecord = data.Count();
     
    
    if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection)) data = data.OrderBy(sortColumn+" "+sortColumnDirection);
    
    //pagination
    var crontabList = data.Skip(skip).Take(pageSize).ToList();
    var returnObj = new {
        draw = draw, recordsTotal = totalRecord, recordsFiltered = filterRecord, data = crontabList
    };
  
        return Json(returnObj);
    }
}

}