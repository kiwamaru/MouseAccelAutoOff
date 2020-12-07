using System;
using System.ComponentModel;
using System.Windows;

namespace MouseAccelAutoOffMonitor
{
    public partial class NotifyIcon : Component
    {
        public NotifyIcon()
        {
            InitializeComponent();
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
    }
}