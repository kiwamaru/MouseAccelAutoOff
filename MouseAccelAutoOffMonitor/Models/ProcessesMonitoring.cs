using MouseAccelAutoOffMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MouseAccelAutoOffMonitor.Models
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
        private ProcessNameListFile _processNameListContainer;

        public ProcessesMonitoring(NotifyIcon notifyIcon)
        {
            _notifyIcon = notifyIcon;
            _processNameListContainer = new ProcessNameListFile();
        }

        /// <summary>
        /// 監視スレッド開始
        /// </summary>
        public void Start()
        {
            _processNames = _processNameListContainer.GetProcessNameList();
            if (_processNames != null && _processNames.Any())
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
                UnmanagedAccess.MouseSetting.SetMouseEnhancePointerPrecision(true);
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
            bool current = UnmanagedAccess.MouseSetting.GetMouseEnhancePointerPrecision();
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
                    UnmanagedAccess.MouseSetting.SetMouseEnhancePointerPrecision(current);
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
        public bool FindProcessNames(List<string> pnames)
        {
            var activepname = UnmanagedAccess.ProcessName.GetActiveProcessFileNameWithoutExtension();
            return pnames.Contains(activepname);
        }
    }
}