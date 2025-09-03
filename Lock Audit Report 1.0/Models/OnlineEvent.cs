using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lock_Audit_Report_1._0.Models
{
    public class OnlineEvent
    {
        public DateTime Timestamp { get; set; }
        public string GuestName { get; set; }
        public string KeyStatus { get; set; }
        public string RoomNumber { get; set; }
        public string CredentialClass { get; set; }
    }
}
