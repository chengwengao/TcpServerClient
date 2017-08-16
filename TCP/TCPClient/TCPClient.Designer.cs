namespace TCPClient
{
    partial class TCPClient
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.contentBox = new System.Windows.Forms.Label();
            this.inputBox = new System.Windows.Forms.TextBox();
            this.sendButton = new System.Windows.Forms.Button();
            this.ipAddressBox = new System.Windows.Forms.TextBox();
            this.ipAddressLabel = new System.Windows.Forms.Label();
            this.portLabel = new System.Windows.Forms.Label();
            this.portBox = new System.Windows.Forms.TextBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // contentBox
            // 
            this.contentBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.contentBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.contentBox.Location = new System.Drawing.Point(12, 50);
            this.contentBox.Name = "contentBox";
            this.contentBox.Size = new System.Drawing.Size(688, 319);
            this.contentBox.TabIndex = 0;
            this.contentBox.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // inputBox
            // 
            this.inputBox.Location = new System.Drawing.Point(12, 372);
            this.inputBox.Multiline = true;
            this.inputBox.Name = "inputBox";
            this.inputBox.Size = new System.Drawing.Size(539, 79);
            this.inputBox.TabIndex = 1;
            // 
            // sendButton
            // 
            this.sendButton.Location = new System.Drawing.Point(557, 372);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(82, 79);
            this.sendButton.TabIndex = 2;
            this.sendButton.Text = "Send";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
            // 
            // ipAddressBox
            // 
            this.ipAddressBox.Location = new System.Drawing.Point(35, 12);
            this.ipAddressBox.Name = "ipAddressBox";
            this.ipAddressBox.Size = new System.Drawing.Size(193, 21);
            this.ipAddressBox.TabIndex = 3;
            // 
            // ipAddressLabel
            // 
            this.ipAddressLabel.AutoSize = true;
            this.ipAddressLabel.Location = new System.Drawing.Point(12, 15);
            this.ipAddressLabel.Name = "ipAddressLabel";
            this.ipAddressLabel.Size = new System.Drawing.Size(17, 12);
            this.ipAddressLabel.TabIndex = 4;
            this.ipAddressLabel.Text = "ip";
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(250, 15);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(29, 12);
            this.portLabel.TabIndex = 5;
            this.portLabel.Text = "port";
            // 
            // portBox
            // 
            this.portBox.Location = new System.Drawing.Point(285, 12);
            this.portBox.Name = "portBox";
            this.portBox.Size = new System.Drawing.Size(100, 21);
            this.portBox.TabIndex = 6;
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(413, 12);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(75, 23);
            this.connectButton.TabIndex = 7;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // TCPClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(715, 463);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.portBox);
            this.Controls.Add(this.portLabel);
            this.Controls.Add(this.ipAddressLabel);
            this.Controls.Add(this.ipAddressBox);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.inputBox);
            this.Controls.Add(this.contentBox);
            this.Name = "TCPClient";
            this.Text = "TCPClient";
            this.Load += new System.EventHandler(this.Form_Load);
            this.FormClosing += Form_Closing;
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.Label contentBox;
        private System.Windows.Forms.TextBox inputBox;
        private System.Windows.Forms.Button sendButton;
        private System.Windows.Forms.TextBox ipAddressBox;
        private System.Windows.Forms.Label ipAddressLabel;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.TextBox portBox;
        private System.Windows.Forms.Button connectButton;
    }
}

