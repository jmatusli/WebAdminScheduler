using System;
using System.Collections.Generic;

namespace WebAdminScheduler.Models
{
    public partial class CP_CONEXION
    {
        public int IDCONEX { get; set; }
        public string? USUARIO { get; set; }
        public string? PASSWORD { get; set; }
        public string? SERVICIO { get; set; }
        public string? DLINK { get; set; }
        public string? TIPO { get; set; }
        
    }
}