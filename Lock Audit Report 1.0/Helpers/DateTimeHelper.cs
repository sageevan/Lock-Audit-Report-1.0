using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lock_Audit_Report_1._0.Helpers
{
    public static class DateTimeHelper
    {
        public static bool TryParseDate(string input, out DateTime result)
        {
            return DateTime.TryParseExact(input, "dd/MM/yyyy HHmm", CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
        }
    }
}
