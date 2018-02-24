namespace DefectDetection
{
    partial class Communication
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSeverSendData = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.btnStartServer = new System.Windows.Forms.Button();
            this.tBoxServerPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tBoxServerIp = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnClientRecivData = new System.Windows.Forms.Button();
            this.btnClientSendData = new System.Windows.Forms.Button();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.tBoxClientPort = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tBoxClientIp = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(490, 276);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSeverSendData);
            this.groupBox1.Controls.Add(this.richTextBox1);
            this.groupBox1.Controls.Add(this.btnStartServer);
            this.groupBox1.Controls.Add(this.tBoxServerPort);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tBoxServerIp);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(239, 270);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "服务端";
            // 
            // btnSeverSendData
            // 
            this.btnSeverSendData.Location = new System.Drawing.Point(103, 108);
            this.btnSeverSendData.Name = "btnSeverSendData";
            this.btnSeverSendData.Size = new System.Drawing.Size(67, 23);
            this.btnSeverSendData.TabIndex = 6;
            this.btnSeverSendData.Text = "发送数据";
            this.btnSeverSendData.UseVisualStyleBackColor = true;
            this.btnSeverSendData.Click += new System.EventHandler(this.btnSeverSendData_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.richTextBox1.Location = new System.Drawing.Point(3, 154);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(233, 113);
            this.richTextBox1.TabIndex = 5;
            this.richTextBox1.Text = "";
            // 
            // btnStartServer
            // 
            this.btnStartServer.Location = new System.Drawing.Point(9, 108);
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(75, 23);
            this.btnStartServer.TabIndex = 4;
            this.btnStartServer.Text = "开启服务器";
            this.btnStartServer.UseVisualStyleBackColor = true;
            this.btnStartServer.Click += new System.EventHandler(this.btnStartServer_Click);
            // 
            // tBoxServerPort
            // 
            this.tBoxServerPort.Location = new System.Drawing.Point(70, 70);
            this.tBoxServerPort.Name = "tBoxServerPort";
            this.tBoxServerPort.Size = new System.Drawing.Size(100, 21);
            this.tBoxServerPort.TabIndex = 3;
            this.tBoxServerPort.Text = "2600";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Port";
            // 
            // tBoxServerIp
            // 
            this.tBoxServerIp.Location = new System.Drawing.Point(70, 32);
            this.tBoxServerIp.Name = "tBoxServerIp";
            this.tBoxServerIp.Size = new System.Drawing.Size(100, 21);
            this.tBoxServerIp.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnClientRecivData);
            this.groupBox2.Controls.Add(this.btnClientSendData);
            this.groupBox2.Controls.Add(this.richTextBox2);
            this.groupBox2.Controls.Add(this.tBoxClientPort);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.tBoxClientIp);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(248, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(239, 270);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "客户端";
            // 
            // btnClientRecivData
            // 
            this.btnClientRecivData.Location = new System.Drawing.Point(132, 108);
            this.btnClientRecivData.Name = "btnClientRecivData";
            this.btnClientRecivData.Size = new System.Drawing.Size(71, 23);
            this.btnClientRecivData.TabIndex = 8;
            this.btnClientRecivData.Text = "接收数据";
            this.btnClientRecivData.UseVisualStyleBackColor = true;
            // 
            // btnClientSendData
            // 
            this.btnClientSendData.Location = new System.Drawing.Point(37, 108);
            this.btnClientSendData.Name = "btnClientSendData";
            this.btnClientSendData.Size = new System.Drawing.Size(75, 23);
            this.btnClientSendData.TabIndex = 7;
            this.btnClientSendData.Text = "发送数据";
            this.btnClientSendData.UseVisualStyleBackColor = true;
            this.btnClientSendData.Click += new System.EventHandler(this.btnClientSendData_Click);
            // 
            // richTextBox2
            // 
            this.richTextBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.richTextBox2.Location = new System.Drawing.Point(3, 154);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(233, 113);
            this.richTextBox2.TabIndex = 6;
            this.richTextBox2.Text = "";
            // 
            // tBoxClientPort
            // 
            this.tBoxClientPort.Location = new System.Drawing.Point(56, 73);
            this.tBoxClientPort.Name = "tBoxClientPort";
            this.tBoxClientPort.Size = new System.Drawing.Size(100, 21);
            this.tBoxClientPort.TabIndex = 4;
            this.tBoxClientPort.Text = "2600";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "Port";
            // 
            // tBoxClientIp
            // 
            this.tBoxClientIp.Location = new System.Drawing.Point(56, 32);
            this.tBoxClientIp.Name = "tBoxClientIp";
            this.tBoxClientIp.Size = new System.Drawing.Size(100, 21);
            this.tBoxClientIp.TabIndex = 5;
            this.tBoxClientIp.Text = "192.168.1.105 ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "IP";
            // 
            // Communication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(490, 276);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Communication";
            this.Text = "Communication";
            this.Load += new System.EventHandler(this.Communication_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSeverSendData;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button btnStartServer;
        private System.Windows.Forms.TextBox tBoxServerPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tBoxServerIp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnClientRecivData;
        private System.Windows.Forms.Button btnClientSendData;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.TextBox tBoxClientPort;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tBoxClientIp;
        private System.Windows.Forms.Label label3;
    }
}