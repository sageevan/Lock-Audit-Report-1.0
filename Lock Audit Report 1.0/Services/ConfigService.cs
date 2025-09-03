using Lock_Audit_Report_1._0.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;

namespace Lock_Audit_Report_1._0.Services
{
    public static class ConfigService
    {
        private static readonly string ConfigFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");

        public static AppConfig Load()
        {
            if (!File.Exists(ConfigFile))
                throw new FileNotFoundException("Configuration file not found", ConfigFile);

            var json = File.ReadAllText(ConfigFile);
            return JsonConvert.DeserializeObject<AppConfig>(json);
        }
    }
}
