using MouseAccelAutoOffMonitor.ViewModels;
using MouseAccelAutoOffMonitor.Views;
using Prism.Ioc;
using Prism.Services.Dialogs;
using Prism.Unity;
using System.Windows;
using Unity;

namespace MouseAccelAutoOffMonitor
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            MainWindowViewModel.ViewModel.StartProcessesMonitoringCommand.Execute();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            MainWindowViewModel.ViewModel.ExitProcessesMonitoringCommand.Execute();
            base.OnExit(e);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<Views.ProcessNameListDialog, ViewModels.ProcessNameListDialogViewModel>();
            containerRegistry.RegisterDialog<Views.RunningAppListDialog, ViewModels.RunningAppListDialogViewModel>();
            containerRegistry.Register<NotifyIcon>();
        }

        protected override Window CreateShell()
        {
            return new MainWindow();
        }
    }
}