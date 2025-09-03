using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lock_Audit_Report_1._0.Models
{
    public class DateRangeConfig
    {
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
    }

    public class AppConfig
    {
        public string ServerURL { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int DefaultDaysBack { get; set; }
        public TimezoneConfig Timezone { get; set; }
        public bool Debug { get; set; }
    }

    public class TimezoneConfig
    {
        public string Name { get; set; }
        public int Offset { get; set; }
        public bool UseInHeaders { get; set; }
    }

}
