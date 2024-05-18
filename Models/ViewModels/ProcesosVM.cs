using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebAdminScheduler.Models.ViewModels
{
    public class ProcesosVM
    {
        public CP_PROCESOS oProcesos { get; set; }

     
        public List<CP_DEPENDENCIAS> oDependencias { get; set; }
    }
}