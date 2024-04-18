using System;
using System.Collections.Generic;

namespace WebAdminScheduler.Models
{
    public partial class CP_CRONTAB
    {
        public int IDCRONTAB { get; set; }
        public string? FECHA { get; set; }
        public string? HORA_INICIO { get; set; }
        public string? HORA_FIN { get; set; }
        public string? RECURRENCIA { get; set; }
        public string? WDAY_M2S_EX { get; set; }
        public string? DAY_EX { get; set; }
        public string? MONTH_EX { get; set; }
		public int REPEAT_EVERY_MINS { get; set; }
		public int REPEAT_AFTER_FINISH { get; set; }
    }
}