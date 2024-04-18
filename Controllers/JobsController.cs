using Microsoft.AspNetCore.Mvc;
using WebAdminScheduler.Models;
using WebAdminScheduler.Models.ViewModels;
using System.Diagnostics;

namespace WebAdminScheduler.Controllers
{
    public class JobsController : Controller
    {
        public JobsController(IConfiguration config)
        {
        }
        public IActionResult Index()
        {
            return View();
        }
		
   }
}