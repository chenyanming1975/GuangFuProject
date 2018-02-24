using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DefectDetection
{
    public partial class Report : Form
    {
        private string timeStart = null, timeStop = null;
        private string tableName = null,rdlcName = null;
        public Report()
        {
            InitializeComponent();
        }
        
        private void Report_Load(object sender, EventArgs e)
        {
            // TODO: 这行代码将数据加载到表“ServerGlassInfoDataSet1.surfaceDetect_info”中。您可以根据需要移动或删除它。
            //this.surfaceDetect_infoTableAdapter.Fill(this.ServerGlassInfoDataSet1.surfaceDetect_info);
            //this.reportViewer1.LocalReport.ReportEmbeddedResource = "DefectDetection." + rdlcName + ".rdlc";
            //this.reportViewer1.RefreshReport();
            timeStart = DateTime.Now.ToShortDateString();//默认起始日期时当前日期
            timeStop = DateTime.Now.ToShortDateString();//默认结束日期时当前日期
            
        }

        private void tsBtnSearch_Click(object sender, EventArgs e)
        {
            string sql = null;
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "DefectDetection." + rdlcName + ".rdlc";
            this.reportViewer1.RefreshReport();
            #region ///查询时间设置
            string hoursStart = tboxStartHour.Text.ToString();
            string minuteStart = tboxStartMinite.Text.ToString();
            string secondStart = tboxStartSeconds.Text.ToString();
            string hoursStop = tboxEndHour.Text.ToString();
            string minuteStop = tboxEndMinets.Text.ToString();
            string secondStop = tboxEndSeconds.Text.ToString();
            timeStart += " " + hoursStart;
            timeStart += ":" + minuteStart;
            timeStart += ":" + secondStart;

            timeStop += " " + hoursStop;
            timeStop += ":" + minuteStop;
            timeStop += ":" + secondStop;
            #endregion
            SqlConnection connection = new SqlConnection(FrmMain.strConnection);//创建connection对象
            //打开数据库
            connection.Open();
            //创建sql语句
            if (tableName != "glassGuality_info")
                sql = "select * from " + tableName + " where create_time between " + "'" + timeStart + "'" + " and " + "'" + timeStop + "'";//'1905-06-16 ' and '1905-06-18'";
            else
                sql = "select * from " + tableName;
            //创建SqlCommand对象
            SqlCommand cmd = new SqlCommand(sql, connection);
            //创建DataAdapter对象
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            //创建DataSet对象
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);

            //dataGridView1.DataSource = ds.Tables[0];

            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", ds.Tables[0]));
            this.reportViewer1.RefreshReport();
            timeStart = DateTime.Now.ToShortDateString();//默认起始日期时当前日期
            timeStop = DateTime.Now.ToShortDateString();//默认结束日期时当前日期
        }

        private void dateTimePickerStart_ValueChanged(object sender, EventArgs e)
        {
            timeStart = dateTimePickerStart.Value.ToShortDateString();
            
        }

        private void dateTimePickerStop_ValueChanged(object sender, EventArgs e)
        {
            timeStop = dateTimePickerStop.Value.ToShortDateString();
        }

        private void cmBoxReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            rdlcName = cmBoxReport.SelectedItem.ToString();
            if (rdlcName == "surfaceDetect_info")
            {
                tableName = "surfaceDetect_info";
            }
            else if (rdlcName == "glassGuality_info")
            {
                tableName = "glassGuality_info";
            }
        }

      
    }
}
