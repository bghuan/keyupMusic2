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
            // Huan
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(329, 26);
            Controls.Add(label1);
            Location = new Point(2170, 100);
            Margin = new Padding(4, 4, 4, 4);
            Name = "Huan";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Text = "幻";
            TopMost = true;
            Load += Form1_Load;
            DoubleClick += Huan_ResizeEnd;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private NotifyIcon notifyIcon1;
    }
}
