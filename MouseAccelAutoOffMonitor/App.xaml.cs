using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MouseAccelAutoOffMonitor
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private NotifyIcon notifyIcon = new NotifyIcon();
        private ProcessesMonitoring _processesMonitoring;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            _processesMonitoring = new ProcessesMonitoring();
            _processesMonitoring.Start();

        }

        protected override void OnExit(ExitEventArgs e)
        {
            _processesMonitoring.Stop();
            _processesMonitoring.ExitWait();
            base.OnExit(e);
            notifyIcon.Dispose();
        }
    }
}
