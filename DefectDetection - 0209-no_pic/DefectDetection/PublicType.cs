using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DefectDetection
{
    class PublicType
    {
    }
    /// <summary>
    /// 系统参数
    /// </summary>
    public struct SystemP
    {
        public int wireControlport;
        public string wireControlIp;
        public int localPort;
        public int localIp;
        public int pcNum;
        public float mmPerPix;
        ///.........
    }

    /// <summary>
    /// 相机参数
    /// </summary>
    public struct CameraP
    {
        public string CamareID;//相机ID
        public string cameraFilePath;
        public int seqLength_Ln;
        public int pageLength_Ln;
        public int pageDelay_Ln;
        public int endPageDelay_Ln;
        public int correctLine;//校正采集的线数
        //public double GlassLeftStarX;
        //public int GlassSide;
        //public double OverGlassWidth;


    }

    /// <summary>
    /// 表面检测参数
    /// </summary>
    public struct SurfaceP
    {
        public int RedThreshold;
        public int RedAreaMin;
        public int RedAreaMax;
        public int BlueThreshold;
        public int BlueAreaMin;
        public int BlueAreaMax;
        public int DisPixleRed;
        public int DisPixleBlue;
        public int DefectCount;
    }
    /// <summary>
    ///线控信息
    /// </summary>
    public struct WireControlInfo
    {
        public volatile bool R_flag;     //接收标志
        public double R_GlassWidth;
        public double R_GlassHeight;
        public double R_GlassThickness;
        public double R_Speed;
        public int R_productionID;  //线控生产ID
        public int R_progressiveID; //过程ID
        public int R_Rank; //玻璃等级 

        public volatile bool S_flag;         //发送标志
        public volatile int S_productionID;  //线控生产ID
        public volatile int S_progressiveID; //过程ID
        public volatile int S_GlassX;  //玻璃中心位置
        public volatile int S_Juggment; //判断玻璃好坏
        public volatile int S_GlassAngle; //玻璃角度
    }
    /// <summary>
    /// 每个缺陷的特征
    /// </summary>
    public struct DefectFeature
    {
        public double area;
        public double width;
        public double height;
        public double row;
        public double col;
    }
}
