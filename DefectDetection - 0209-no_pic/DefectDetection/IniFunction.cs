using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace DefectDetection
{
    public class IniFunction
    {
        /// <summary>
        /// ini文件操作
        /// </summary>
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string FilePath);
        [DllImport("kernel32")]
        private static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string FilePath);
        public string IniPath = Application.StartupPath + "\\FileConfig.ini ";
        //public string strCameraSec = "Camera";
        //public string strSystemSec = "System";
        //public string strSurfaceSec = "Surface";
        //public string strEdgeSec = "cutEdge";
        //public string strBrokeEdge = "brokeEdge";

        public string strCameraSec;
        public string strSystemSec;
        public string strSurfaceSec;
        public string strEdgeSec;
        public string strBrokeEdge;
        StringBuilder temp = new StringBuilder(1024);

        //读取本地Ini文件函数
        public string ReadValue(string Section, string key)
        {
            GetPrivateProfileString(Section, key, "", temp, 1024, IniPath);
            return temp.ToString();
        }
        //写入ini文件
        public void WriteValue(string Section,string key,string value,string IniPath)
        {
            WritePrivateProfileString(Section, key, value, IniPath);
        }
       //读取本地参数
        public void ReadParam(int glassLength,int glassWidth,double glassThick)
        {
           
            strSystemSec = "System";
            switch (glassLength * glassWidth)
            {
                case 406 * 355:
                    strCameraSec = "Camera406_355";
                    strSurfaceSec = "Surface406_355";
                    strEdgeSec = "cutEdge406_355";
                    strBrokeEdge = "brokeEdge406_355";
                    break;
                case 400 * 500:
                    strCameraSec = "Camera500_400";
                    strSurfaceSec = "Surface500_400";
                    strEdgeSec = "cutEdge500_400";
                    strBrokeEdge = "brokeEdge500_400";
                    break;
                case 350 * 420:
                    strCameraSec = "Camera420_350";
                    strSurfaceSec = "Surface420_350";
                    strEdgeSec = "cutEdge420_350";
                    strBrokeEdge = "brokeEdge420_350";
                    break;
                case 370 * 400:
                    strCameraSec = "Camera400_370";
                    strSurfaceSec = "Surface400_370";
                    strEdgeSec = "cutEdge400_370";
                    strBrokeEdge = "brokeEdge400_370";
                    break;
                case 370 * 470:
                    strCameraSec = "Camera470_370";
                    strSurfaceSec = "Surface470_370";
                    strEdgeSec = "cutEdge470_370";
                    strBrokeEdge = "brokeEdge470_370";
                    break;
                case 355 * 430:
                    strCameraSec = "Camera430_355";
                    strSurfaceSec = "Surface430_355";
                    strEdgeSec = "cutEdge430_355";
                    strBrokeEdge = "brokeEdge430_355";
                    break;
                case 300 * 360:
                    strCameraSec = "Camera360_300";
                    strSurfaceSec = "Surface360_300";
                    strEdgeSec = "cutEdge360_300";
                    strBrokeEdge = "brokeEdge360_300";
                    break;
                case 300 * 350:
                    strCameraSec = "Camera350_300";
                    strSurfaceSec = "Surface350_300";
                    //strEdgeSec = "cutEdge350_300";
                    //strBrokeEdge = "brokeEdge350_300";
                    break;
                case 367 * 420:
                    strCameraSec = "Camera420_367";
                    strSurfaceSec = "Surface420_367";
                    strEdgeSec = "cutEdge420_367";
                    strBrokeEdge = "brokeEdge420_367";
                    break;
                case 406 * 430:
                    strCameraSec = "Camera430_406";
                    strSurfaceSec = "Surface430_406";
                    strEdgeSec = "cutEdge430_406";
                    strBrokeEdge = "brokeEdge430_406";
                    break;

            }
            ///系统参数
            FrmMain.systemP.pcNum = int.Parse(ReadValue(strSystemSec, "PcNum"));
            FrmMain.systemP.mmPerPix = float.Parse(ReadValue(strSystemSec, "MmPerPix"));
            FrmMain.systemP.wireControlIp = ReadValue(strSystemSec, "WireControlIp");
            FrmMain.systemP.wireControlport = int.Parse(ReadValue(strSystemSec, "WireControlPort"));
            ///相机参数
            FrmMain.cameraP.CamareID = ReadValue(strCameraSec, "CamareID");
            FrmMain.cameraP.cameraFilePath = ReadValue(strCameraSec, "cameraFilePath");
            FrmMain.cameraP.seqLength_Ln = int.Parse(ReadValue(strCameraSec, "SeqLength_Ln"));
            FrmMain.cameraP.pageLength_Ln = int.Parse(ReadValue(strCameraSec, "PageLength_Ln"));
            FrmMain.cameraP.pageDelay_Ln = int.Parse(ReadValue(strCameraSec, "PageDelay_Ln"));
            FrmMain.cameraP.endPageDelay_Ln =int.Parse( ReadValue(strCameraSec, "EndPageDelay_Ln"));
            FrmMain.cameraP.correctLine = int.Parse(ReadValue(strCameraSec, "CorrectLine"));
            ///表面检测参数
            FrmMain.surfaceP.BlueThreshold = int.Parse(ReadValue(strSurfaceSec, "BlueThreshold"));
            FrmMain.surfaceP.RedThreshold = int.Parse(ReadValue(strSurfaceSec, "RedThreshold"));
            FrmMain.surfaceP.BlueAreaMin = int.Parse(ReadValue(strSurfaceSec, "BlueAreaMin"));
            FrmMain.surfaceP.RedAreaMin = int.Parse(ReadValue(strSurfaceSec, "RedAreaMin"));
            FrmMain.surfaceP.BlueAreaMax = int.Parse(ReadValue(strSurfaceSec, "BlueAreaMax"));
            FrmMain.surfaceP.RedAreaMax = int.Parse(ReadValue(strSurfaceSec, "RedAreaMax"));
            FrmMain.surfaceP.DisPixleRed = int.Parse(ReadValue(strSurfaceSec, "DisPixleRed"));
            FrmMain.surfaceP.DisPixleBlue = int.Parse(ReadValue(strSurfaceSec, "DisPixleBlue"));
            FrmMain.surfaceP.DefectCount = int.Parse(ReadValue(strSurfaceSec, "DefectCount"));
             
           
       
           
        }
        //写入参数
        public void WriteParam(int glassLength, int glassWidth, double glassThick)
        {
            strSystemSec = "System";
            switch (glassLength * glassWidth)
            {
                case 406 * 355:
                    strCameraSec = "Camera406_355";
                    strSurfaceSec = "Surface406_355";
                    strEdgeSec = "cutEdge406_355";
                    strBrokeEdge = "brokeEdge406_355";
                    break;
                case 400 * 500:
                    strCameraSec = "Camera500_400";
                    strSurfaceSec = "Surface500_400";
                    strEdgeSec = "cutEdge500_400";
                    strBrokeEdge = "brokeEdge500_400";
                    break;
                case 350 * 420:
                    strCameraSec = "Camera420_350";
                    strSurfaceSec = "Surface420_350";
                    strEdgeSec = "cutEdge420_350";
                    strBrokeEdge = "brokeEdge420_350";
                    break;
                case 370 * 400:
                    strCameraSec = "Camera400_370";
                    strSurfaceSec = "Surface400_370";
                    strEdgeSec = "cutEdge400_370";
                    strBrokeEdge = "brokeEdge400_370";
                    break;
                case 370 * 470:
                    strCameraSec = "Camera470_370";
                    strSurfaceSec = "Surface470_370";
                    strEdgeSec = "cutEdge470_370";
                    strBrokeEdge = "brokeEdge470_370";
                    break;
                case 355 * 430:
                    strCameraSec = "Camera430_355";
                    strSurfaceSec = "Surface430_355";
                    strEdgeSec = "cutEdge430_355";
                    strBrokeEdge = "brokeEdge430_355";
                    break;
                case 300 * 360:
                    strCameraSec = "Camera360_300";
                    strSurfaceSec = "Surface360_300";
                    strEdgeSec = "cutEdge360_300";
                    strBrokeEdge = "brokeEdge360_300";
                    break;
                case 300 * 350:
                    strCameraSec = "Camera350_300";
                    strSurfaceSec = "Surface350_300";
                    strEdgeSec = "cutEdge350_300";
                    strBrokeEdge = "brokeEdge350_300";
                    break;
                case 367 * 420:
                    strCameraSec = "Camera420_367";
                    strSurfaceSec = "Surface420_367";
                    strEdgeSec = "cutEdge420_367";
                    strBrokeEdge = "brokeEdge420_367";
                    break;
                case 406 * 430:
                    strCameraSec = "Camera430_406";
                    strSurfaceSec = "Surface430_406";
                    strEdgeSec = "cutEdge430_406";
                    strBrokeEdge = "brokeEdge430_406";
                    break;

            }
            //系统参数
            WriteValue(strSystemSec, "PcNum", FrmMain.systemP.pcNum.ToString(), IniPath);
            WriteValue(strSystemSec, "MmPerPix", FrmMain.systemP.mmPerPix.ToString(), IniPath);
            WriteValue(strSystemSec, "WireControlPort", FrmMain.systemP.wireControlport.ToString(), IniPath);
            WriteValue(strSystemSec, "WireControlIp", FrmMain.systemP.wireControlIp.ToString(), IniPath);
            //相机参数
            WriteValue(strCameraSec, "CamareID", FrmMain.cameraP.CamareID, IniPath);
            WriteValue(strCameraSec, "SeqLength_Ln", FrmMain.cameraP.seqLength_Ln.ToString(), IniPath);
            WriteValue(strCameraSec, "PageLength_Ln", FrmMain.cameraP.pageLength_Ln.ToString(), IniPath);
            WriteValue(strCameraSec, "PageDelay_Ln", FrmMain.cameraP.pageDelay_Ln.ToString(), IniPath);
            WriteValue(strCameraSec, "EndPageDelay_Ln", FrmMain.cameraP.endPageDelay_Ln.ToString(), IniPath);
            WriteValue(strCameraSec, "CorrectLine", FrmMain.cameraP.correctLine.ToString(), IniPath);
            //表面检测参数
            WriteValue(strSurfaceSec, "BlueThreshold", FrmMain.surfaceP.BlueThreshold.ToString(), IniPath);
            WriteValue(strSurfaceSec, "RedThreshold", FrmMain.surfaceP.RedThreshold.ToString(), IniPath);
            WriteValue(strSurfaceSec, "BlueAreaMin", FrmMain.surfaceP.BlueAreaMin.ToString(), IniPath);
            WriteValue(strSurfaceSec, "RedAreaMin", FrmMain.surfaceP.RedAreaMin.ToString(), IniPath);
            WriteValue(strSurfaceSec, "BlueAreaMax", FrmMain.surfaceP.BlueAreaMax.ToString(), IniPath);
            WriteValue(strSurfaceSec, "RedAreaMax", FrmMain.surfaceP.RedAreaMax.ToString(), IniPath);
            WriteValue(strSurfaceSec, "DisPixleRed", FrmMain.surfaceP.DisPixleRed.ToString(), IniPath);
            WriteValue(strSurfaceSec, "DisPixleBlue", FrmMain.surfaceP.DisPixleBlue.ToString(), IniPath);
            WriteValue(strSurfaceSec, "DefectCount", FrmMain.surfaceP.DefectCount.ToString(), IniPath);
          
          
        }
    }
}
