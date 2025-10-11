namespace keyupMusic2.fantasy
{
    partial class Read
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
            textBox1 = new TextBox();
            ((System.ComponentModel.ISupportInitialize)webView21).BeginInit();
            SuspendLayout();
            // 
            // webView21
            // 
            webView21.AllowExternalDrop = true;
            webView21.CreationProperties = null;
            webView21.DefaultBackgroundColor = Color.White;
            webView21.Dock = DockStyle.Fill;
            webView21.Location = new Point(0, 0);
            webView21.Name = "webView21";
            webView21.Size = new Size(800, 450);
            webView21.TabIndex = 0;
            webView21.ZoomFactor = 1D;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(8, 8);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(530, 38);
            textBox1.TabIndex = 1;
            // 
            // Read
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(textBox1);
            Controls.Add(webView21);
            Name = "Read";
            Text = "Read";
            Load += Read_Load;
            ((System.ComponentModel.ISupportInitialize)webView21).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 webView21;
        private TextBox textBox1;
    }
}