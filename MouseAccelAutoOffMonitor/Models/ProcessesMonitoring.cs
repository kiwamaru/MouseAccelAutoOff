using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MouseAccelAutoOffMonitor
{
    /// <summary>
    /// プロセス監視用スレッド
    /// </summary>
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

        /// <summary>
        /// 監視スレッド開始
        /// </summary>
        public void Start()
        {
            _processNames = _processNameListContainer.GetProcessNameList();
            if(_processNames != null && _processNames.Any()) 
            { 
                _task = Task.Run(() => ExecuteMonitoringManager());
            }
        }

        /// <summary>
        /// 監視スレッド停止
        /// </summary>
        public void Stop()
        {
            _cancellationTokenSource?.Cancel(true);
        }

        /// <summary>
        /// 監視スレッド停止待ち
        /// </summary>
        public void ExitWait()
        {
            _task?.Wait();
        }

        /// <summary>
        /// 監視スレッドの起動と例外監視スレッド
        /// </summary>
        private async void ExecuteMonitoringManager()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            try
            {
                await ExecuteMonitoring(_cancellationTokenSource.Token);
            }
            catch (OperationCanceledException e)
            {
                MouseSettingApi.SetMouseEnhancePointerPrecision(true);
            }
            _cancellationTokenSource = null;
        }

        /// <summary>
        /// 監視スレッド
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task ExecuteMonitoring(CancellationToken cancellationToken)
        {
            bool current = MouseSettingApi.GetMouseEnhancePointerPrecision();
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
                    MouseSettingApi.SetMouseEnhancePointerPrecision(current);
                    last = current;
                    _notifyIcon.ChangeNotifyIcon(current);
                }
                cancellationToken.WaitHandle.WaitOne(5000);
            }
            return;
        }

        /// <summary>
        /// 監視対象プロセス名が、現在実行中のプロセスにあるかどうかを返す
        /// </summary>
        /// <param name="pnames">プロセス名リスト</param>
        /// <returns></returns>
        public bool FindProcessNames(IReadOnlyCollection<string> pnames)
        {
            foreach (var pname in pnames)
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