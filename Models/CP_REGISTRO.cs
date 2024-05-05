using System;
using System.Collections.Generic;

namespace WebAdminScheduler.Models
{
    public partial class CP_REGISTRO
    {
        public int IDREG { get; set; }
        public int IDPROC { get; set; }
        public string? FEC_INICIO { get; set; }
        public DateTime? FEC_EJECUCION { get; set; }
        public string? FEC_FINALIZO { get; set; }
        public string? ESTADO { get; set; }
        public int FLAG_ALARMA { get; set; }
    }
}