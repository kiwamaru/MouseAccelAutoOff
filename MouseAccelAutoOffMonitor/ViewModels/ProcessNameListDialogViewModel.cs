using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MouseAccelAutoOffMonitor.ViewModels
{
    public class ProcessNameListDialogViewModel : BindableBase, IDialogAware
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

        /// <summary>
        /// 選択中のプロセス名
        /// </summary>
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

        private ProcessNameListContainer _processNameListContainer;

        public event Action<IDialogResult> RequestClose;

        public ProcessNameListDialogViewModel()
        {
            ProcessList = new ObservableCollection<string>();
            AddCommand = new DelegateCommand(AddProcessName);
            DelCommand = new DelegateCommand(DeleteProcessName, () => !String.IsNullOrEmpty(SelectProcessName)).ObservesProperty(() => SelectProcessName);
            OKCommand = new DelegateCommand(OnOK);
            CancelCommand = new DelegateCommand(DeleteProcessName);

            _processNameListContainer = new ProcessNameListContainer();
            var pnames = _processNameListContainer.GetProcessNameList();
            if (pnames != null && pnames.Any())
            {
                ProcessList.AddRange(pnames);
            }
        }

        /// <summary>
        /// プロセス名を追加
        /// </summary>
        public void AddProcessName()
        {
            var fpath = this.ChooseProcessName();
            if (!String.IsNullOrWhiteSpace(fpath))
            {
                var pname = Path.GetFileName(fpath);
                if (!ProcessList.Contains(pname))
                {
                    ProcessList.Add(pname);
                }
            }
        }

        /// <summary>
        /// プロセス名を削除
        /// </summary>
        public void DeleteProcessName()
        {
            if (SelectProcessName != null)
            {
                ProcessList.Remove(SelectProcessName);
            }
        }

        public void OnOK()
        {
            _processNameListContainer.SetProcessNameList(ProcessList.ToList());
            this.RequestClose?.Invoke(new Prism.Services.Dialogs.DialogResult(ButtonResult.OK));
        }

        public void Cencel()
        {
            this.RequestClose?.Invoke(new Prism.Services.Dialogs.DialogResult(ButtonResult.Cancel));
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            ///監視スレッド再開
            MainWindowViewModel.ViewModel.StartProcessesMonitoringCommand.Execute();
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            //監視スレッド停止
            MainWindowViewModel.ViewModel.PauseProcessesMonitoringCommand.Execute();
        }

        /// <summary>
        /// コモンファイルダイアログにてプロセス名を選択する
        /// </summary>
        /// <returns></returns>
        public string ChooseProcessName()
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.InitialDirectory = @"C:\";
            ofd.Filter = "exeファイル(*.exe)|*.exe";
            ofd.Title = "実行中に「ポインターの精度を高める」をOFFしたいexeファイルを選択してください";

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return ofd.FileName;
            }
            return null;
        }
    }
}