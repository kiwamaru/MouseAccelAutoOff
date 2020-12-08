using System;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace MouseAccelAutoOffMonitor
{
    public partial class NotifyIcon : Component
    {
        System.Drawing.Icon _accelofficon;
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
                MouseAccelAutoOffMonitor.Text = "Accel On";
            }
            else 
            { 
                MouseAccelAutoOffMonitor.Icon = _accelofficon;
                MouseAccelAutoOffMonitor.Text = "Accel Off";
            }
        }

    }
}