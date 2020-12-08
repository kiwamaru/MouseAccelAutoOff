using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;

namespace MouseAccelAutoOffMonitor.ViewModels
{
    public class ProcessNameListDialogViewModel : BindableBase,IDialogAware
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

        private string _selectProcessName;
        public string SelectProcessName
        {
            get { return this._selectProcessName; }
            set
            {
                SetProperty(ref this._selectProcessName, value);
            }
        }
        public DelegateCommand AddCommand { get; }
        public DelegateCommand DelCommand { get; }

        public DelegateCommand OKCommand { get; }
        public DelegateCommand CancelCommand { get; }

        public ObservableCollection<string> ProcessList { get; private set; }

        private RegistrationProcessNames _registrationProcessNames;

        private ProcessNameListContainer _processNameListContainer;

        public event Action<IDialogResult> RequestClose;

        public ProcessNameListDialogViewModel()
        {
            ProcessList = new ObservableCollection<string>();
            _registrationProcessNames = new RegistrationProcessNames();
            AddCommand = new DelegateCommand(AddProcessName);
            DelCommand = new DelegateCommand(DeleteProcessName,() => !String.IsNullOrEmpty(SelectProcessName)).ObservesProperty(() => SelectProcessName);
            OKCommand = new DelegateCommand(OnOK);
            CancelCommand = new DelegateCommand(DeleteProcessName);

            _processNameListContainer = new ProcessNameListContainer();
            _processNameListContainer.LoadFile();
            var pnames = _processNameListContainer.GetProcessNameList();
            if(pnames != null && pnames.Any()) 
            { 
                ProcessList.AddRange(pnames);
            }
        }
        public void AddProcessName()
        {
            var fpath = _registrationProcessNames.ChooseExePath();
            if(!String.IsNullOrWhiteSpace(fpath))
            {
                var pname = Path.GetFileNameWithoutExtension(fpath);
                if (!ProcessList.Contains(pname)) 
                { 
                    ProcessList.Add(pname);
                }
            }
        }
        public void DeleteProcessName()
        {
            if(SelectProcessName != null)
            {
                ProcessList.Remove(SelectProcessName);
            }
        }
        public void OnOK()
        {
            _processNameListContainer.SetProcessNameList(ProcessList.ToList());
            this.RequestClose?.Invoke(new DialogResult( ButtonResult.OK));

        }
        public void Cencel()
        {
            this.RequestClose?.Invoke(new DialogResult( ButtonResult.Cancel));
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            MainWindowViewModel.ViewModel.StartProcessesMonitoringCommand.Execute();
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            MainWindowViewModel.ViewModel.PauseProcessesMonitoringCommand.Execute();
        }
    }
}