using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;

namespace MouseAccelAutoOffMonitor
{
    /// <summary>
    /// プロセス名の設定データファイルを管理するコンテナ
    /// </summary>
    public class ProcessNameListFile
    {
        private static string filepath = @"MonitoringProcessNames.xml";

        private List<string> _processNameList;

        /// <summary>
        /// XMLファイルからプロセス名リストを取得
        /// </summary>
        public List<string> GetProcessNameList()
        {
            if (_processNameList == null)
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
                return null;
            }
            catch (System.IO.IOException)
            {
                return null;
            }
            return _processNameList;
        }

        /// <summary>
        /// プロセス名リストを登録し、ファイルに書き込む
        /// </summary>
        /// <param name="processNameList"></param>
        public void SetProcessNameList(List<string> processNameList)
        {
            _processNameList.Clear();
            _processNameList.AddRange(processNameList);
            WriteFile();
        }

        /// <summary>
        /// プロセス名リストをファイルに書き込む
        /// </summary>
        private void WriteFile()
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