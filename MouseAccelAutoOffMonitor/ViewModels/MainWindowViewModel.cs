using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MouseAccelAutoOffMonitor.ViewModels
{
    public class MainWindowViewModel
    {
        public static MainWindowViewModel ViewModel;
        public DelegateCommand showSettingDialogCommand;
        public DelegateCommand StartProcessesMonitoringCommand;
        public DelegateCommand PauseProcessesMonitoringCommand;
        public DelegateCommand ExitProcessesMonitoringCommand;

        private NotifyIcon _notifyIcon;
        private IDialogService _dialogService;
        private Dispatcher _dispather = null;
        private ProcessesMonitoring _processesMonitoring;


        public MainWindowViewModel(IDialogService dialogService,NotifyIcon notifyIcon)
        {
            ViewModel = this;
            _notifyIcon = notifyIcon;
            _dialogService = dialogService;
            _dispather = Dispatcher.CurrentDispatcher;

            showSettingDialogCommand = new DelegateCommand(ShowProcessNameListDialog);
            StartProcessesMonitoringCommand = new DelegateCommand(()=> 
            {
                _processesMonitoring = new ProcessesMonitoring(_notifyIcon);
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
        public void ShowProcessNameListDialog()
        {
            _dialogService.Show(nameof(Views.ProcessNameListDialog), null, null);
        }
    }
}
