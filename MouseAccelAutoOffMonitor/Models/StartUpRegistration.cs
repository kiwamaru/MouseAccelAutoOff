using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MouseAccelAutoOffMonitor
{
    public class StartUpRegistration
    {
        public static void Registration()
        {
            var regkey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            regkey.SetValue(Application.ProductName, Application.ExecutablePath);
            regkey.Close();
        }
        public static void Remove()
        {
            var regkey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            regkey.DeleteValue(Application.ProductName);
            regkey.Close();
        }
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
