using System.Windows.Forms;

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
            label1 = new SmartLabel();
            notifyIcon1 = new NotifyIcon(components);
            contextMenuStrip1 = new ContextMenuStrip(components);
            timerMove = new System.Windows.Forms.Timer(components);
            label2 = new Label();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(7, 0);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(63, 24);
            label1.TabIndex = 1;
            label1.Text = "label1";
            label1.Click += label1_Click;
            // 
            // notifyIcon1
            // 
            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "幻";
            notifyIcon1.Visible = true;
            notifyIcon1.DoubleClick += notifyIcon1_DoubleClick;
            notifyIcon1.MouseClick += notifyIcon1_MouseClick;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(24, 24);
            string[] menuKeys = new string[]
            {
                "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P",
                "A", "S", "D", "F", "G", "H", "J", "K", "L",
                "Z", "X", "C", "V", "B", "N", "M"
            };
            letterMenuItems = new ToolStripMenuItem[menuKeys.Length];
            for (int i = 0; i < menuKeys.Length; i++)
            {
                letterMenuItems[i] = new ToolStripMenuItem();
                letterMenuItems[i].Name = $"{menuKeys[i].ToLower()}ToolStripMenuItem";
                letterMenuItems[i].Size = new Size(170, 30);
                letterMenuItems[i].Text = menuKeys[i];
            }
            // 特殊项单独处理
            letterMenuItems[9].Text = "PPPPPPPP"; // P
            letterMenuItems[18].Text = "LLLLLLLLLL"; // L

            contextMenuStrip1.Items.AddRange(letterMenuItems);
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(171, 784);
            contextMenuStrip1.ItemClicked += contextMenuStrip1_ItemClicked;

            // 在 contextMenuStrip1 初始化后添加
            contextMenuStrip1.Items.Add(new ToolStripSeparator());

            var vkMenuItem1 = new ToolStripMenuItem("月亮表(显示隐藏)");
            vkMenuItem1.Click += MoonTime.vkMenuItem_Click;
            vkMenuItem1.Name ="moontimmeshow";
            var vkMenuItem2 = new ToolStripMenuItem("月亮表(重启)");
            vkMenuItem2.Click += MoonTime.vkMenuItem_DoubleClick;
            var vkMenuItem3 = new ToolStripMenuItem("微亮键盘(显示隐藏)");
            vkMenuItem3.Click += VirtualKeyboardForm.vkMenuItem_Click;
            vkMenuItem3.Name = "keyboardlightshow";
            var vkMenuItem4 = new ToolStripMenuItem("微亮键盘(重启)");
            vkMenuItem4.Click += VirtualKeyboardForm.vkMenuItem_DoubleClick;

            letterMenuItems2= new ToolStripMenuItem[] { vkMenuItem1 , vkMenuItem2 , vkMenuItem3 , vkMenuItem4 };
            contextMenuStrip1.Items.AddRange(letterMenuItems2);
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(180, 0);
            label2.Name = "label2";
            label2.Size = new Size(63, 24);
            label2.TabIndex = 2;
            label2.Text = "label2";
            // 
            // Huan
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            ClientSize = new Size(266, 26);
            ContextMenuStrip = contextMenuStrip1;
            Controls.Add(label2);
            Controls.Add(label1);
            DoubleBuffered = true;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Location = new Point(2560, 100);
            Margin = new Padding(4);
            Name = "Huan";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Text = "幻";
            TopMost = true;
            Load += Form1_Load;
            DoubleClick += Huan_ResizeEnd;
            Resize += Huan_Resize;
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        public SmartLabel label1;
        private NotifyIcon notifyIcon1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem[] letterMenuItems;
        private ToolStripMenuItem[] letterMenuItems2;
        public System.Windows.Forms.Timer timerMove;
        public Label label2;
    }
}
