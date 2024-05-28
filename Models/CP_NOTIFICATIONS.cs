using System;
using System.Collections.Generic;

namespace WebAdminScheduler.Models
{
    public partial class CP_NOTIFICATIONS
    {
		public int IDNOTIF { get; set; }
        public string? RECIPIENTS { get; set; }
        public int? NOTIFYSUCCESS { get; set; }
        public int? NOTIFYFAILURE { get; set; }
        public string? NAME { get; set; }
    }
}