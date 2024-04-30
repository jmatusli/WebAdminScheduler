using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebAdminScheduler.Models;
using Oracle.ManagedDataAccess.Client;
 
namespace WebAdminScheduler.helpers
{
   public static class WACustomHelper
   {
      public static int GetLasIdCRON(WebAdminSchedulerContext WAContext)
      {
         CP_CRONTAB data = (from s in WAContext.CP_CRONTABS.OrderByDescending(x => x.IDCRONTAB)
                           select s).ToList().AsQueryable().FirstOrDefault();
                  int LasIdcrontab=data.IDCRONTAB+1;
                  return LasIdcrontab;
      }
      public static int GetLastIdPROC(WebAdminSchedulerContext WAContext)
      {
         CP_PROCESOS data = (from s in WAContext.CP_PROCESOS.OrderByDescending(x => x.IDPROC)
                           select s).ToList().AsQueryable().FirstOrDefault();
                  int LastIdproc=data.IDPROC+1;
                  return LastIdproc;
      }
   }
}