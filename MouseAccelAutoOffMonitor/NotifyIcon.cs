using MouseAccelAutoOffMonitor.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;

namespace MouseAccelAutoOffMonitor
{
    public partial class NotifyIcon : Component
    {
        private System.Drawing.Icon _accelofficon;

        public NotifyIcon()
        {
            InitializeComponent();
            var iconStream = Application.GetResourceStream(new Uri("/Resources/Notify.ico", UriKind.Relative)).Stream;
            _accelofficon = new System.Drawing.Icon(iconStream);
            iconStream.Close();
            iconStream.Dispose();
            toolStripMenuItemExitApp.Click += delegate (object sender, EventArgs e)
            {
                Application.Current.Shutdown();
            };
            toolStripMenuItemStartUpRegistration.Click += ToolStripMenuItemStartUpRegistration_Click;
            toolStripMenuItemStartUpRegistration.Checked = StartUpRegistration.IsRegistrated();
            toolStripMenuItemRegistrationProcess.Click += ToolStripMenuItemRegistrationProcess_Click;
        }

        private void ToolStripMenuItemRegistrationProcess_Click(object sender, EventArgs e)
        {
            MainWindowViewModel.ViewModel.showSettingDialogCommand.Execute();
        }

        private void ToolStripMenuItemStartUpRegistration_Click(object sender, EventArgs e)
        {
            if (toolStripMenuItemStartUpRegistration.Checked)
            {
                toolStripMenuItemStartUpRegistration.Checked = false;
                StartUpRegistration.Remove();
            }
            else
            {
                toolStripMenuItemStartUpRegistration.Checked = true;
                StartUpRegistration.Registration();
            }
        }

        public NotifyIcon(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
        }

        private void notifyIcon1_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
        }

        public void ChangeNotifyIcon(bool accel_on)
        {
            if (accel_on)
            {
                MouseAccelAutoOffMonitor.Icon = ((System.Drawing.Icon)(_resources.GetObject("MouseAccelAutoOffMonitor.Icon")));
                MouseAccelAutoOffMonitor.Text = "ポインターの精度を高める On";
            }
            else
            {
                MouseAccelAutoOffMonitor.Icon = _accelofficon;
                MouseAccelAutoOffMonitor.Text = "ポインターの精度を高める Off";
            }
        }
    }
}