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

        private CancellationTokenSource _cancellationTokenSource;
        private Task _task;

        public void Start()
        {
            _processNames = LoadSettings.LoadMonitoringProcessNames();
            _task = Task.Run(() => Wait());
        }
        public void ExitWait()
        {
            _task.Wait();
        }
        public async void Wait()
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
        public void Stop()
        {
            _cancellationTokenSource.Cancel(true);
        }
        public async Task ExecuteMonitoring(CancellationToken cancellationToken)
        {
            bool current = MouseSettingApi.GetMouseAccelaration();
            bool last = current;

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