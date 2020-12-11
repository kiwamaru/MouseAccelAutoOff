using MouseAccelAutoOffMonitor.Models.UnmanagedAccess;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MouseAccelAutoOffMonitor.Models
{
    public class RunningAppItem
    {
        public int Index { get; set; }
        public string AppName { get; set; }
        public string WindowTitle { get; set; }
        public BitmapSource Icon { get; set; }
        public RunningAppItem(int index,string appname,string windowTitle, BitmapSource icon)
        {
            Index = index;
            AppName = appname;
            WindowTitle = windowTitle;
            Icon = icon;
        }
    }
    public class RunningAppList
    {
        public List<RunningAppItem> AppList { get; set; } = new List<RunningAppItem>();

        public bool GetAppProcesses()
        {
            AppList.Clear();
            var processes = Process.GetProcesses();
            var appProcesses = processes.Where(x => 
                x.MainWindowHandle != IntPtr.Zero && !String.IsNullOrEmpty(x.MainWindowTitle) && ProcessName.IsWindowVisible(x.MainWindowHandle)
            ).ToList();//アプリのみ取得
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
                    string[] delimiter = { "-" };
                    var bitmap = Icon.ExtractAssociatedIcon(app.MainModule.FileName).ToBitmap();
                    var rawWinTitle = app.MainWindowTitle;
                    var splitedTitles = rawWinTitle.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                    var winTitle = "";
                    if (splitedTitles.Count() > 0) { 
                        winTitle = splitedTitles[splitedTitles.Count() - 1].Trim();
                    }
                    if (String.IsNullOrWhiteSpace(winTitle)) winTitle = app.ProcessName;
                    AppList.Add(new RunningAppItem(i++, app.ProcessName, winTitle, BitmapConverter.ConvBitmapSource(bitmap)));
                }
            }
            return true;
        }
    }
}
