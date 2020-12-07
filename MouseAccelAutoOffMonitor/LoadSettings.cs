using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MouseAccelAutoOffMonitor
{
    public class LoadSettings
    {
        static string filepath = @"MonitoringProcessNames.xml";
        public static List<string> LoadMonitoringProcessNames()
        {
            var list = XDocument.Load(filepath);
            var names = list.Descendants("MonitoringProcess").Select(p => p.Element("Name")?.Value);
            return names.ToList();
        }
    }
}
