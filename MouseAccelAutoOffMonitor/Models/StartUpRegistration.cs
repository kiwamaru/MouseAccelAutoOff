using System;
using System.Windows.Forms;

namespace MouseAccelAutoOffMonitor
{
    /// <summary>
    /// スタートアップ関連のクラス
    /// </summary>
    public class StartUpRegistration
    {
        /// <summary>
        /// スタートアップに登録
        /// </summary>
        public static void Registration()
        {
            var regkey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            regkey.SetValue(Application.ProductName, Application.ExecutablePath);
            regkey.Close();
        }

        /// <summary>
        /// スタートアップから削除
        /// </summary>
        public static void Remove()
        {
            var regkey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            regkey.DeleteValue(Application.ProductName);
            regkey.Close();
        }

        /// <summary>
        /// スタートアップに登録済みか
        /// </summary>
        /// <returns></returns>
        public static bool IsRegistrated()
        {
            var regkey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            var b = (string)regkey.GetValue(Application.ProductName);
            regkey.Close();
            if (String.IsNullOrEmpty(b))
            {
                return false;
            }
            return true;
        }
    }
}