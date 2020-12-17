using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace MouseAccelAutoOffMonitor.Models
{
    public class ProcessAppNameItem
    {
        public int Index { get; set; }
        public string AppName { get; set; }
        public BitmapSource Icon { get; set; }
        public ProcessAppNameItem(int index,string appname, BitmapSource icon)
        {
            Index = index;
            AppName = appname;
            Icon = icon;
        }
    }
    public class ProcessAppNamesContainer
    {
        public List<ProcessAppNameItem> AppList { get; set; } = new List<ProcessAppNameItem>();

        public bool GetAppProcesses()
        {
            AppList.Clear();
            var processes = Process.GetProcesses();
            var appProcesses = processes.Where(x => x.MainWindowHandle != IntPtr.Zero).ToList();//アプリのみ取得
            if(!appProcesses.Any())
            {
                return false;
            }
            var myproc = Process.GetCurrentProcess();
            appProcesses.RemoveAll(x => x.ProcessName == myproc.ProcessName);
            int i = 0;
            foreach (var app in appProcesses)
            {
                if (!AppList.Select(x=>x.AppName).Contains(app.ProcessName)) 
                {
                    using (Icon ico = Icon.ExtractAssociatedIcon(app.MainModule.FileName))
                    {
                        var Source = Imaging.CreateBitmapSourceFromHIcon(ico.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                        AppList.Add(new ProcessAppNameItem(i++, app.ProcessName, Source));
                    }
                }
            }
            return true;
        }
    }
}
