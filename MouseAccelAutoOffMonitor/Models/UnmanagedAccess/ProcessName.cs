using System;
using System.Runtime.InteropServices;
using System.Text;

namespace MouseAccelAutoOffMonitor.Models.UnmanagedAccess
{
    public class ProcessName
    {
        #region Win32API Methods

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr handle);

        [DllImport("psapi.dll", CharSet = CharSet.Ansi)]
        private static extern uint GetModuleBaseName(IntPtr hWnd, IntPtr hModule, [MarshalAs(UnmanagedType.LPStr), Out] StringBuilder lpBaseName, uint nSize);

        #endregion Win32API Methods

        /// <summary>
        /// プロセス名の取得
        /// </summary>
        /// <returns></returns>
        public static string GetActiveProcessFileName()
        {
            try
            {
                var handle = (IntPtr)GetWindowThreadProcessId(GetForegroundWindow(), out var processid);
                if (handle != IntPtr.Zero)
                {
                    var hnd = OpenProcess(0x0400 | 0x0010, false, processid);
                    if (hnd != null)
                    {
                        var buffer = new StringBuilder(255);
                        GetModuleBaseName(hnd, IntPtr.Zero, buffer, (uint)buffer.Capacity);
                        CloseHandle(hnd);

                        return buffer.ToString();
                    }
                }
                return null;
            }
            catch (System.ComponentModel.Win32Exception e)
            {
                return null;
            }
        }
    }
}