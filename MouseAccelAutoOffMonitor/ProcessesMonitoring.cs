using MouseAccelAutoOffMonitor.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MouseAccelAutoOffMonitor
{
    public class ProcessesMonitoring
    {
        private List<string> _processNames;
        private NotifyIcon _notifyIcon;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _task;
        private ProcessNameListContainer _processNameListContainer;


        public ProcessesMonitoring(NotifyIcon notifyIcon)
        {
            _notifyIcon = notifyIcon;
            _processNameListContainer = new ProcessNameListContainer();
        }
        public void Start()
        {
            _processNameListContainer.LoadFile();
            _processNames = _processNameListContainer.GetProcessNameList();
            _task = Task.Run(() => Wait());
        }
        public void Stop()
        {
            _cancellationTokenSource.Cancel(true);
        }

        public void ExitWait()
        {
            _task.Wait();
        }
        private async void Wait()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            try
            {
                await ExecuteMonitoring(_cancellationTokenSource.Token);
            }
            catch(OperationCanceledException e)
            {
                MouseSettingApi.ToggleEnhancePointerPrecision(true);
            }
            _cancellationTokenSource = null;
        }
        private async Task ExecuteMonitoring(CancellationToken cancellationToken)
        {
            bool current = MouseSettingApi.GetMouseAccelaration();
            bool last = current;
            _notifyIcon.ChangeNotifyIcon(current);

            while (true) 
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (FindProcessNames(_processNames))
                {
                    current = false;
                }
                else
                {
                    current = true;
                }
                if (last != current)
                {
                    MouseSettingApi.ToggleEnhancePointerPrecision(current);
                    last = current;
                    _notifyIcon.ChangeNotifyIcon(current);
                }
                cancellationToken.WaitHandle.WaitOne(5000);
            }
            return;
        }
        public bool FindProcessNames(IReadOnlyCollection<string> pnames)
        {
            foreach(var pname in pnames)
            {
                var processList = Process.GetProcessesByName(pname).ToList();
                if (processList.Any())
                {
                    return true;
                }
            }
            return false;
        }
    }
}