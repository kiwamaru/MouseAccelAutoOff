using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace MouseAccelAutoOffMonitor
{
    public class ProcessNameListContainer
    {
        static string filepath = @"MonitoringProcessNames.xml";

        private List<string> _processNameList;

        public void LoadFile()
        {
            if(_processNameList == null)
            {
                _processNameList = new List<string>();
            }

            try
            {
                var xDocument = XDocument.Load(filepath);
                XElement xMonitoringProcesses = xDocument.Element("MonitoringProcessTable");
                IEnumerable<XElement> boxedLunchRow = xMonitoringProcesses.Elements("MonitoringProcess");
                foreach (var p in boxedLunchRow)
                {
                    _processNameList.Add(p.Element("Name").Value);
                }
            }
            catch (XmlException e) 
            {
                return;
            }
        }
        public List<string> GetProcessNameList()
        {
            return _processNameList;
        }
        public void SetProcessNameList(List<string> processNameList)
        { 
            _processNameList.Clear();
            _processNameList.AddRange(processNameList);
            WriteFile();
        }
        public void WriteFile()
        {
            Debug.Assert(_processNameList != null);

            var XDoc = new XDocument(new XDeclaration("1.0", "utf-8", "true"));
            XElement xTable = new XElement("MonitoringProcessTable");
            foreach (var pname in _processNameList)
            {
                XElement xItem = new XElement("MonitoringProcess");
                xItem.Add(new XElement("Name", pname));
                xTable.Add(xItem);
            }
            XDoc.Add(xTable);

            XDoc.Save(filepath);
        }
    }
}
