using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DefectDetection
{
    public partial class SystemSetting : Form
    {
        private static IniFunction ini = new IniFunction();
        public SystemSetting()
        {
            InitializeComponent();
        }

        private void SystemSetting_Load(object sender, EventArgs e)
        {
            #region///加载参数

            //系统参数
            //tboxPcNum.Text = FrmMain.systemP.pcNum.ToString();
            tboxMmPix.Text = FrmMain.systemP.mmPerPix.ToString();
            tboxRemortPort.Text = FrmMain.systemP.wireControlport.ToString();
            tboxLocalPortUdp.Text = FrmMain.systemP.localPort.ToString();
            //相机参数
            tboxCameraNum.Text = FrmMain.cameraP.CamareID;
            tboxPageLength.Text = FrmMain.cameraP.pageLength_Ln.ToString();
            tboxPageDelay.Text = FrmMain.cameraP.pageDelay_Ln.ToString();
            tboxEndPageDelay.Text = FrmMain.cameraP.endPageDelay_Ln.ToString();
            tboxLineCorrect.Text = FrmMain.cameraP.correctLine.ToString();
            //表面检测参数
            tboxblueThreshold.Text = FrmMain.surfaceP.BlueThreshold.ToString();
            tboxBlueAreaMin.Text = FrmMain.surfaceP.BlueAreaMin.ToString();
            tboxBlueAreaMax.Text = FrmMain.surfaceP.BlueAreaMax.ToString();
            textBox3.Text = FrmMain.surfaceP.DefectCount.ToString();
            #endregion
        }

        private void tsBtnLoad_Click(object sender, EventArgs e)
        {
            string[] str = tsComboxGlassType.SelectedItem.ToString().Split('*');
            int length = int.Parse(str[0]);
            int width = int.Parse(str[1]);
            double thick = 1.1;
            ini.ReadParam(length, width, thick);
            //系统参数
            //tboxPcNum.Text = FrmMain.systemP.pcNum.ToString();
            tboxMmPix.Text = FrmMain.systemP.mmPerPix.ToString();
            tboxRemortPort.Text = FrmMain.systemP.wireControlport.ToString();
            tboxLocalPortUdp.Text = FrmMain.systemP.localPort.ToString();
            //相机参数
            tboxCameraNum.Text = FrmMain.cameraP.CamareID;
            tboxPageLength.Text = FrmMain.cameraP.pageLength_Ln.ToString();
            tboxPageDelay.Text = FrmMain.cameraP.pageDelay_Ln.ToString();
            tboxEndPageDelay.Text = FrmMain.cameraP.endPageDelay_Ln.ToString();
            tboxLineCorrect.Text = FrmMain.cameraP.correctLine.ToString();
            //表面检测参数
            tboxblueThreshold.Text = FrmMain.surfaceP.BlueThreshold.ToString();
            tboxBlueAreaMin.Text = FrmMain.surfaceP.BlueAreaMin.ToString();
            tboxBlueAreaMax.Text = FrmMain.surfaceP.BlueAreaMax.ToString();
            textBox3.Text = FrmMain.surfaceP.DefectCount.ToString();
        }

        private void tsBtnWritePara_Click(object sender, EventArgs e)
        {
            if (tsComboxGlassType.SelectedItem != null)
            {
                ///修改变量值
                apply();
                ///写入到ini文件
                string[] str = tsComboxGlassType.SelectedItem.ToString().Split('*');
                int length = int.Parse(str[0]);
                int width = int.Parse(str[1]);
                double thick = 0.1;
                DialogResult result = MessageBox.Show("是否修改" + tsComboxGlassType.SelectedItem.ToString() + "规格玻璃参数", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    FrmMain.ini.WriteParam(length, width, thick);
                    MessageBox.Show("写入成功！");
                }
                else
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show("请加载玻璃规格");
            }
        }

        private void apply()
        {
            //系统参数
            //tboxPcNum.Text = FrmMain.systemP.pcNum.ToString();
            FrmMain.systemP.mmPerPix = float.Parse(tboxMmPix.Text);
            FrmMain.systemP.wireControlport = int.Parse(tboxRemortPort.Text);
            FrmMain.systemP.localPort = int.Parse(tboxLocalPortUdp.Text);
            //相机参数
            FrmMain.cameraP.CamareID = tboxCameraNum.Text;
            FrmMain.cameraP.seqLength_Ln = int.Parse(tboxPageLength.Text);
            FrmMain.cameraP.pageLength_Ln = int.Parse(tboxPageLength.Text);
            FrmMain.cameraP.pageDelay_Ln = int.Parse(tboxPageDelay.Text);
            FrmMain.cameraP.endPageDelay_Ln = int.Parse(tboxEndPageDelay.Text);
            FrmMain.cameraP.correctLine = int.Parse(tboxLineCorrect.Text);
            //表面检测参数
            FrmMain.surfaceP.BlueThreshold = int.Parse(tboxblueThreshold.Text);
            FrmMain.surfaceP.BlueAreaMin = int.Parse(tboxBlueAreaMin.Text);
            FrmMain.surfaceP.BlueAreaMax = int.Parse(tboxBlueAreaMax.Text);
            FrmMain.surfaceP.DefectCount = int.Parse(textBox3.Text);
        }
        private void tsBtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
    }
}
