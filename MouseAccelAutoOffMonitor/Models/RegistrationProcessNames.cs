using System;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace MouseAccelAutoOffMonitor
{
    public class RegistrationProcessNames
    {

        public string ChooseExePath()
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.InitialDirectory = @"C:\";
            ofd.Filter = "exeファイル(*.exe)|*.exe";
            ofd.Title = "実行中に「ポインターの精度を高める」をOFFしたいexeファイルを選択してください";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                return ofd.FileName;
            }
            return null;
        }
    }
}