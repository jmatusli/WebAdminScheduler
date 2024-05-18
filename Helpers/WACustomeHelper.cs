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
                  int LasIdcrontab=(data?.IDCRONTAB?? 0)+1;
                  return LasIdcrontab;
      }
      public static int GetLastIdPROC(WebAdminSchedulerContext WAContext)
      {
         CP_PROCESOS data = (from s in WAContext.CP_PROCESOS.OrderByDescending(x => x.IDPROC)
                           select s).ToList().AsQueryable().FirstOrDefault();
                  int LastIdproc=(data?.IDPROC?? 0)+1;
                  return LastIdproc;
      }

       public static int GetLastIDDEP(WebAdminSchedulerContext WAContext)
      {
         CP_DEPENDENCIAS data = (from s in WAContext.CP_DEPENDENCIAS.OrderByDescending(x => x.IDDEP)
                           select s).ToList().AsQueryable().FirstOrDefault();
                  int LastIddep= (data?.IDDEP ?? 0)+1;
                  return LastIddep;
      }
   }
}