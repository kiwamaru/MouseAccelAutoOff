﻿
namespace MouseAccelAutoOffMonitor
{
    partial class NotifyIcon
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotifyIcon));
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemExitApp = new System.Windows.Forms.ToolStripMenuItem();
            this.MouseAccelAutoOffMonitor = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip.SuspendLayout();
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemExitApp});
            this.contextMenuStrip.Name = "contextMenuStrip1";
            this.contextMenuStrip.Size = new System.Drawing.Size(99, 26);
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
            // 
            // toolStripMenuItemExitApp
            // 
            this.toolStripMenuItemExitApp.Name = "toolStripMenuItemExitApp";
            this.toolStripMenuItemExitApp.Size = new System.Drawing.Size(98, 22);
            this.toolStripMenuItemExitApp.Text = "終了";
            // 
            // MouseAccelAutoOffMonitor
            // 
            this.MouseAccelAutoOffMonitor.ContextMenuStrip = this.contextMenuStrip;
            this.MouseAccelAutoOffMonitor.Icon = ((System.Drawing.Icon)(resources.GetObject("MouseAccelAutoOffMonitor.Icon")));
            this.MouseAccelAutoOffMonitor.Text = "notifyIcon1";
            this.MouseAccelAutoOffMonitor.Visible = true;
            this.MouseAccelAutoOffMonitor.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            this.contextMenuStrip.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.NotifyIcon MouseAccelAutoOffMonitor;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExitApp;
    }
}