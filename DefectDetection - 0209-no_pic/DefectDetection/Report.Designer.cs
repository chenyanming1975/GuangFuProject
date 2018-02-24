namespace DefectDetection
{
    partial class Report
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
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.surfaceDetect_infoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ServerGlassInfoDataSet1 = new DefectDetection.ServerGlassInfoDataSet1();
            this.glassGuality_infoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ServerGlassInfoDataSet = new DefectDetection.ServerGlassInfoDataSet();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tboxEndSeconds = new System.Windows.Forms.TextBox();
            this.tboxEndMinets = new System.Windows.Forms.TextBox();
            this.tboxEndHour = new System.Windows.Forms.TextBox();
            this.dateTimePickerStop = new System.Windows.Forms.DateTimePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tboxStartSeconds = new System.Windows.Forms.TextBox();
            this.tboxStartMinite = new System.Windows.Forms.TextBox();
            this.tboxStartHour = new System.Windows.Forms.TextBox();
            this.dateTimePickerStart = new System.Windows.Forms.DateTimePicker();
            this.tsBtnSearch = new System.Windows.Forms.Button();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.glassGuality_infoTableAdapter = new DefectDetection.ServerGlassInfoDataSetTableAdapters.glassGuality_infoTableAdapter();
            this.surfaceDetect_infoTableAdapter = new DefectDetection.ServerGlassInfoDataSet1TableAdapters.surfaceDetect_infoTableAdapter();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cmBoxReport = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.surfaceDetect_infoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ServerGlassInfoDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.glassGuality_infoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ServerGlassInfoDataSet)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // surfaceDetect_infoBindingSource
            // 
            this.surfaceDetect_infoBindingSource.DataMember = "surfaceDetect_info";
            this.surfaceDetect_infoBindingSource.DataSource = this.ServerGlassInfoDataSet1;
            // 
            // ServerGlassInfoDataSet1
            // 
            this.ServerGlassInfoDataSet1.DataSetName = "ServerGlassInfoDataSet1";
            this.ServerGlassInfoDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // glassGuality_infoBindingSource
            // 
            this.glassGuality_infoBindingSource.DataMember = "glassGuality_info";
            this.glassGuality_infoBindingSource.DataSource = this.ServerGlassInfoDataSet;
            // 
            // ServerGlassInfoDataSet
            // 
            this.ServerGlassInfoDataSet.DataSetName = "ServerGlassInfoDataSet";
            this.ServerGlassInfoDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.80981F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 63.19019F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.reportViewer1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 48.3871F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 51.6129F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 117F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(677, 417);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.LightBlue;
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.tboxEndSeconds);
            this.groupBox2.Controls.Add(this.tboxEndMinets);
            this.groupBox2.Controls.Add(this.tboxEndHour);
            this.groupBox2.Controls.Add(this.dateTimePickerStop);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 166);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(243, 167);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "结束时间";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 126);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(17, 12);
            this.label8.TabIndex = 13;
            this.label8.Text = "秒";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 94);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 12);
            this.label7.TabIndex = 12;
            this.label7.Text = "分";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 67);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "时";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "日期";
            // 
            // tboxEndSeconds
            // 
            this.tboxEndSeconds.Location = new System.Drawing.Point(65, 123);
            this.tboxEndSeconds.Name = "tboxEndSeconds";
            this.tboxEndSeconds.Size = new System.Drawing.Size(100, 21);
            this.tboxEndSeconds.TabIndex = 6;
            // 
            // tboxEndMinets
            // 
            this.tboxEndMinets.Location = new System.Drawing.Point(65, 91);
            this.tboxEndMinets.Name = "tboxEndMinets";
            this.tboxEndMinets.Size = new System.Drawing.Size(100, 21);
            this.tboxEndMinets.TabIndex = 5;
            // 
            // tboxEndHour
            // 
            this.tboxEndHour.Location = new System.Drawing.Point(65, 64);
            this.tboxEndHour.Name = "tboxEndHour";
            this.tboxEndHour.Size = new System.Drawing.Size(100, 21);
            this.tboxEndHour.TabIndex = 4;
            // 
            // dateTimePickerStop
            // 
            this.dateTimePickerStop.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dateTimePickerStop.Location = new System.Drawing.Point(65, 28);
            this.dateTimePickerStop.Name = "dateTimePickerStop";
            this.dateTimePickerStop.Size = new System.Drawing.Size(101, 21);
            this.dateTimePickerStop.TabIndex = 3;
            this.dateTimePickerStop.ValueChanged += new System.EventHandler(this.dateTimePickerStop_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.LightBlue;
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tboxStartSeconds);
            this.groupBox1.Controls.Add(this.tboxStartMinite);
            this.groupBox1.Controls.Add(this.tboxStartHour);
            this.groupBox1.Controls.Add(this.dateTimePickerStart);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(243, 157);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "开始时间";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "秒";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "分";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "时";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "日期";
            // 
            // tboxStartSeconds
            // 
            this.tboxStartSeconds.Location = new System.Drawing.Point(65, 114);
            this.tboxStartSeconds.Name = "tboxStartSeconds";
            this.tboxStartSeconds.Size = new System.Drawing.Size(101, 21);
            this.tboxStartSeconds.TabIndex = 5;
            // 
            // tboxStartMinite
            // 
            this.tboxStartMinite.Location = new System.Drawing.Point(65, 75);
            this.tboxStartMinite.Name = "tboxStartMinite";
            this.tboxStartMinite.Size = new System.Drawing.Size(101, 21);
            this.tboxStartMinite.TabIndex = 4;
            // 
            // tboxStartHour
            // 
            this.tboxStartHour.Location = new System.Drawing.Point(65, 48);
            this.tboxStartHour.Name = "tboxStartHour";
            this.tboxStartHour.Size = new System.Drawing.Size(101, 21);
            this.tboxStartHour.TabIndex = 3;
            // 
            // dateTimePickerStart
            // 
            this.dateTimePickerStart.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dateTimePickerStart.Location = new System.Drawing.Point(65, 17);
            this.dateTimePickerStart.Name = "dateTimePickerStart";
            this.dateTimePickerStart.Size = new System.Drawing.Size(101, 21);
            this.dateTimePickerStart.TabIndex = 2;
            this.dateTimePickerStart.ValueChanged += new System.EventHandler(this.dateTimePickerStart_ValueChanged);
            // 
            // tsBtnSearch
            // 
            this.tsBtnSearch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tsBtnSearch.Location = new System.Drawing.Point(173, 34);
            this.tsBtnSearch.Name = "tsBtnSearch";
            this.tsBtnSearch.Size = new System.Drawing.Size(54, 23);
            this.tsBtnSearch.TabIndex = 0;
            this.tsBtnSearch.Text = "查询";
            this.tsBtnSearch.UseVisualStyleBackColor = true;
            this.tsBtnSearch.Click += new System.EventHandler(this.tsBtnSearch_Click);
            // 
            // reportViewer1
            // 
            this.reportViewer1.BackColor = System.Drawing.Color.Gray;
            this.reportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportViewer1.DocumentMapWidth = 63;
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = this.surfaceDetect_infoBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.Location = new System.Drawing.Point(252, 3);
            this.reportViewer1.Name = "reportViewer1";
            this.tableLayoutPanel1.SetRowSpan(this.reportViewer1, 3);
            this.reportViewer1.Size = new System.Drawing.Size(422, 411);
            this.reportViewer1.TabIndex = 6;
            // 
            // glassGuality_infoTableAdapter
            // 
            this.glassGuality_infoTableAdapter.ClearBeforeFill = true;
            // 
            // surfaceDetect_infoTableAdapter
            // 
            this.surfaceDetect_infoTableAdapter.ClearBeforeFill = true;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.LightBlue;
            this.groupBox3.Controls.Add(this.cmBoxReport);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.tsBtnSearch);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 339);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(243, 75);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "查询";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 39);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 1;
            this.label9.Text = "选择报表";
            // 
            // cmBoxReport
            // 
            this.cmBoxReport.FormattingEnabled = true;
            this.cmBoxReport.Items.AddRange(new object[] {
            "surfaceDetect_info",
            "glassGuality_info"});
            this.cmBoxReport.Location = new System.Drawing.Point(65, 34);
            this.cmBoxReport.Name = "cmBoxReport";
            this.cmBoxReport.Size = new System.Drawing.Size(91, 20);
            this.cmBoxReport.TabIndex = 2;
            this.cmBoxReport.SelectedIndexChanged += new System.EventHandler(this.cmBoxReport_SelectedIndexChanged);
            // 
            // Report
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 417);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Report";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "报表";
            this.Load += new System.EventHandler(this.Report_Load);
            ((System.ComponentModel.ISupportInitialize)(this.surfaceDetect_infoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ServerGlassInfoDataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.glassGuality_infoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ServerGlassInfoDataSet)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DateTimePicker dateTimePickerStart;
        private System.Windows.Forms.DateTimePicker dateTimePickerStop;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource glassGuality_infoBindingSource;
        private ServerGlassInfoDataSet ServerGlassInfoDataSet;
        private ServerGlassInfoDataSetTableAdapters.glassGuality_infoTableAdapter glassGuality_infoTableAdapter;
        private System.Windows.Forms.BindingSource surfaceDetect_infoBindingSource;
        private ServerGlassInfoDataSet1 ServerGlassInfoDataSet1;
        private ServerGlassInfoDataSet1TableAdapters.surfaceDetect_infoTableAdapter surfaceDetect_infoTableAdapter;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tboxEndSeconds;
        private System.Windows.Forms.TextBox tboxEndMinets;
        private System.Windows.Forms.TextBox tboxEndHour;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tboxStartSeconds;
        private System.Windows.Forms.TextBox tboxStartMinite;
        private System.Windows.Forms.TextBox tboxStartHour;
        private System.Windows.Forms.Button tsBtnSearch;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cmBoxReport;
        private System.Windows.Forms.Label label9;
    }
}