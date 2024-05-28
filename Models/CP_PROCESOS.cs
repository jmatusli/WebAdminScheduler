using System;
using System.Collections.Generic;

namespace WebAdminScheduler.Models
{
    public partial class CP_PROCESOS
    {
        public int IDPROC { get; set; }
        public int IDCONEX { get; set; }
        public string? NOMBRE { get; set; }
        public string? DESCRIPCION { get; set; }
        public string? PATH { get; set; }
        public string? PARAMETRO1 { get; set; }
        public string? PARAMETRO2 { get; set; }
        public string? PARAMETRO3 { get; set; }
        public string? PARAMETRO4 { get; set; }
		public int DEPENDENCIA { get; set; }
		public int INTENTOS { get; set; }
		public int ESPERA_INTENTO { get; set; }
        public string? ESTADO { get; set; }
		public int FTP { get; set; }
		public int IDHOST { get; set; }
		public int COMPRESION { get; set; }
		public int IDCRONTAB { get; set; }
        public string? NODE { get; set; }
		public int IDNOTIF { get; set; }
    }
}