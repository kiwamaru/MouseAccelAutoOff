using MouseAccelAutoOffMonitor.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MouseAccelAutoOffMonitor.ViewModels
{
    public class RunningAppListDialogViewModel : BindableBase, IDialogAware
    {
        private string _title = "プロセス名登録";

        /// <summary>
        /// タイトル
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private ProcessAppNameItem _selectAppName;

        /// <summary>
        /// 選択中のプロセス名
        /// </summary>
        public ProcessAppNameItem SelectAppName
        {
            get { return this._selectAppName; }
            set
            {
                SetProperty(ref this._selectAppName, value);
            }
        }

        public DelegateCommand OKCommand { get; }
        public DelegateCommand CancelCommand { get; }

        public ObservableCollection<ProcessAppNameItem> AppList { get; private set; }

        public event Action<IDialogResult> RequestClose;

        private ProcessAppNamesContainer _processAppNamesContainer;

        public RunningAppListDialogViewModel()
        {
            AppList = new ObservableCollection<ProcessAppNameItem>();
            OKCommand = new DelegateCommand(OnOK);
            CancelCommand = new DelegateCommand(Cancel);

            _processAppNamesContainer = new ProcessAppNamesContainer();

            _processAppNamesContainer.GetAppProcesses();
            AppList.AddRange(_processAppNamesContainer.AppList);
        }

        public void OnOK()
        {
            Debug.WriteLine(SelectAppName.AppName);
            IDialogParameters param = new DialogParameters();
            param.Add("AppName", SelectAppName.AppName);
            this.RequestClose?.Invoke(new Prism.Services.Dialogs.DialogResult(ButtonResult.OK, param));
        }

        public void Cancel()
        {
            this.RequestClose?.Invoke(new Prism.Services.Dialogs.DialogResult(ButtonResult.Cancel));
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }
    }
}