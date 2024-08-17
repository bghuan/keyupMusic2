namespace keyupMusic2
{
    partial class Huan
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing && (components != null))
        //    {
        //        components.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Huan));
            label1 = new Label();
            notifyIcon1 = new NotifyIcon(components);
            contextMenuStrip1 = new ContextMenuStrip(components);
            toolStripMenuItem2 = new ToolStripMenuItem();
            toolStripMenuItem3 = new ToolStripMenuItem();
            toolStripMenuItem4 = new ToolStripMenuItem();
            rToolStripMenuItem = new ToolStripMenuItem();
            tToolStripMenuItem = new ToolStripMenuItem();
            yToolStripMenuItem = new ToolStripMenuItem();
            uToolStripMenuItem = new ToolStripMenuItem();
            iToolStripMenuItem = new ToolStripMenuItem();
            oToolStripMenuItem = new ToolStripMenuItem();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(7, -5);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(63, 24);
            label1.TabIndex = 1;
            label1.Text = "label1";
            label1.Click += label1_Click;
            // 
            // notifyIcon1
            // 
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "幻";
            notifyIcon1.Visible = true;
            notifyIcon1.Click += notifyIcon1_Click;
            notifyIcon1.DoubleClick += notifyIcon1_DoubleClick;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(24, 24);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { toolStripMenuItem2, toolStripMenuItem3, toolStripMenuItem4, rToolStripMenuItem, tToolStripMenuItem, yToolStripMenuItem, uToolStripMenuItem, iToolStripMenuItem, oToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(241, 307);
            contextMenuStrip1.ItemClicked += contextMenuStrip1_ItemClicked;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(240, 30);
            toolStripMenuItem2.Text = "Q";
            toolStripMenuItem2.Click += toolStripMenuItem2_Click;
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new Size(240, 30);
            toolStripMenuItem3.Text = "W";
            // 
            // toolStripMenuItem4
            // 
            toolStripMenuItem4.Name = "toolStripMenuItem4";
            toolStripMenuItem4.Size = new Size(240, 30);
            toolStripMenuItem4.Text = "E";
            // 
            // rToolStripMenuItem
            // 
            rToolStripMenuItem.Name = "rToolStripMenuItem";
            rToolStripMenuItem.Size = new Size(240, 30);
            rToolStripMenuItem.Text = "R";
            // 
            // tToolStripMenuItem
            // 
            tToolStripMenuItem.Name = "tToolStripMenuItem";
            tToolStripMenuItem.Size = new Size(240, 30);
            tToolStripMenuItem.Text = "T";
            // 
            // yToolStripMenuItem
            // 
            yToolStripMenuItem.Name = "yToolStripMenuItem";
            yToolStripMenuItem.Size = new Size(240, 30);
            yToolStripMenuItem.Text = "Y";
            // 
            // uToolStripMenuItem
            // 
            uToolStripMenuItem.Name = "uToolStripMenuItem";
            uToolStripMenuItem.Size = new Size(240, 30);
            uToolStripMenuItem.Text = "U";
            // 
            // iToolStripMenuItem
            // 
            iToolStripMenuItem.Name = "iToolStripMenuItem";
            iToolStripMenuItem.Size = new Size(240, 30);
            iToolStripMenuItem.Text = "I";
            // 
            // oToolStripMenuItem
            // 
            oToolStripMenuItem.Name = "oToolStripMenuItem";
            oToolStripMenuItem.Size = new Size(240, 30);
            oToolStripMenuItem.Text = "O";
            // 
            // Huan
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(329, 26);
            ContextMenuStrip = contextMenuStrip1;
            Controls.Add(label1);
            Location = new Point(2170, 100);
            Margin = new Padding(4);
            Name = "Huan";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Text = "幻";
            TopMost = true;
            Load += Form1_Load;
            DoubleClick += Huan_ResizeEnd;
            MouseLeave += Huan_MouseLeave;
            MouseHover += Huan_MouseHover;
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private NotifyIcon notifyIcon1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem toolStripMenuItem4;
        private ToolStripMenuItem rToolStripMenuItem;
        private ToolStripMenuItem tToolStripMenuItem;
        private ToolStripMenuItem yToolStripMenuItem;
        private ToolStripMenuItem uToolStripMenuItem;
        private ToolStripMenuItem iToolStripMenuItem;
        private ToolStripMenuItem oToolStripMenuItem;
    }
}
