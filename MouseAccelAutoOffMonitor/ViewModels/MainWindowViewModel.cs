using Prism.Commands;
using Prism.Services.Dialogs;
using System.Linq;
using System.Windows;

namespace MouseAccelAutoOffMonitor.ViewModels
{
    /// <summary>
    /// メインWindow 起動時にすぐ隠される
    /// </summary>
    public class MainWindowViewModel
    {
        public static MainWindowViewModel ViewModel;

        public DelegateCommand showSettingDialogCommand;
        public DelegateCommand StartProcessesMonitoringCommand;
        public DelegateCommand PauseProcessesMonitoringCommand;
        public DelegateCommand ExitProcessesMonitoringCommand;

        private NotifyIcon _notifyIcon;
        private IDialogService _dialogService;
        private Models.ProcessesMonitoring _processesMonitoring;

        public MainWindowViewModel(IDialogService dialogService, NotifyIcon notifyIcon)
        {
            ViewModel = this;
            _notifyIcon = notifyIcon;
            _dialogService = dialogService;

            showSettingDialogCommand = new DelegateCommand(() =>
            {
                //ダイアログの多重起動防止
                if(null == Application.Current.Windows.OfType<DialogWindow>().FirstOrDefault(x=>x.Content.GetType() == typeof(Views.ProcessNameListDialog)))
                { 
                    _dialogService.ShowDialog(nameof(Views.ProcessNameListDialog), null, null);
                }
            });
            StartProcessesMonitoringCommand = new DelegateCommand(() =>
            {
                _processesMonitoring = new Models.ProcessesMonitoring(_notifyIcon);
                _processesMonitoring.Start();
            });

            PauseProcessesMonitoringCommand = new DelegateCommand(() =>
            {
                _processesMonitoring.Stop();
            });

            ExitProcessesMonitoringCommand = new DelegateCommand(() =>
            {
                _processesMonitoring.Stop();
                _processesMonitoring.ExitWait();
                _notifyIcon.Dispose();
            });
        }
    }
}