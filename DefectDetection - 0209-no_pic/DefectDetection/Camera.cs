using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using HalconDotNet;
using System.Diagnostics;
using DefectDetection.MultiCam;
namespace DefectDetection
{
    public class Camera
    {
        #region ///变量
        // The Mutex object that will protect image objects during processing
        public static Mutex imageMutex = new Mutex();
        // The MultiCam object that controls the acquisition
        UInt32 channel;
        UInt32 channelR;
        private UInt32 currentSurface;
        MC.CALLBACK multiCamCallback;//左相机回调
        MC.CALLBACK multiCamRCallback;//右相机回调
        public  HObject tmpImage = null;
        //public  static Communication comm = new Communication();
        Stopwatch watchTakeImg = new Stopwatch();//时间记时类。
        public string msg = null;//采像过程消息显示
        #endregion

        #region///委托 
        public delegate void ImageReadyEventHandler(string recData, IntPtr adress, Int32 gElapse, Int32 width, Int32 height);
        public event ImageReadyEventHandler ImageReadyEvent;
        #endregion

        /// <summary>
        /// 采像信息
        /// </summary>
        public String Msg
        {
            set { msg = value; }
            get { return msg; }
        }
        
        public Camera() { }

        object lockerOpenCam = new object();
        
        /// <summary>
        /// 相机参数加载
        /// </summary>
        /// <param name="CamID"></param>
        public void OpenCam(int CamID)
        {
              try
            {
                lock (lockerOpenCam)
                {
                    if (CamID == 0)//左相机
                    {
                        MC.OpenDriver();
                        MC.SetParam(MC.CONFIGURATION, "ErrorLog", "error.log");
                        MC.Create("CHANNEL", out channel);
                        MC.SetParam(channel, "DriverIndex", CamID);
                        MC.SetParam(channel, "Connector", "M");
                        MC.SetParam(channel, "CamFile", Application.StartupPath + "//CameraSet.cam");  //"//" + cameraFilePath);
                        MC.SetParam(channel, "ColorFormat", "RGB24");
                        MC.SetParam(channel, "AcquisitionMode", "LONGPAGE");
                        MC.SetParam(channel, "TrigMode", "HARD");
                        MC.SetParam(channel, "TrigLine", "IIN1");
                        MC.SetParam(channel, "TrigEdge", "GOHIGH");
                        MC.SetParam(channel, "TrigFilter", "STRONG");
                        MC.SetParam(channel, "EndPageDelay_Ln", FrmMain.cameraP.endPageDelay_Ln.ToString());
                        MC.SetParam(channel, "PageDelay_Ln", FrmMain.cameraP.pageDelay_Ln.ToString());
                        MC.SetParam(channel, "SeqLength_Ln", FrmMain.cameraP.seqLength_Ln.ToString());
                        MC.SetParam(channel, "PageLength_Ln", FrmMain.cameraP.pageLength_Ln.ToString());
                        MC.SetParam(channel, "SeqLength_Fr", MC.INDETERMINATE);
                        multiCamCallback = new MC.CALLBACK(MultiCamCallback);
                        MC.RegisterCallback(channel, multiCamCallback, channel);
                        MC.SetParam(channel, MC.SignalEnable + MC.SIG_SURFACE_PROCESSING, "ON");
                        MC.SetParam(channel, MC.SignalEnable + MC.SIG_ACQUISITION_FAILURE, "ON");
                        MC.SetParam(channel, MC.SignalEnable + MC.SIG_SURFACE_FILLED, "ON");
                        MC.SetParam(channel, MC.SignalEnable + MC.SIG_START_ACQUISITION_SEQUENCE, "ON");
                        MC.SetParam(channel, MC.SignalEnable + MC.SIG_END_ACQUISITION_SEQUENCE, "ON");
                        MC.SetParam(channel, "ChannelState", "READY");
                    }
                    else
                    {
                        MC.OpenDriver();
                        MC.SetParam(MC.CONFIGURATION, "ErrorLog", "error.log");
                        MC.Create("CHANNEL", out channelR);
                        MC.SetParam(channelR, "DriverIndex", CamID);
                        MC.SetParam(channelR, "Connector", "M");
                        MC.SetParam(channelR, "CamFile", Application.StartupPath + "//CameraSet.cam");  //"//" + cameraFilePath);
                        MC.SetParam(channelR, "ColorFormat", "RGB24");
                        MC.SetParam(channelR, "AcquisitionMode", "LONGPAGE");
                        MC.SetParam(channelR, "TrigMode", "HARD");
                        MC.SetParam(channelR, "TrigLine", "IIN1");
                        MC.SetParam(channelR, "TrigEdge", "GOHIGH");
                        MC.SetParam(channelR, "TrigFilter", "STRONG");
                        MC.SetParam(channelR, "EndPageDelay_Ln", FrmMain.cameraP.endPageDelay_Ln.ToString());
                        MC.SetParam(channelR, "PageDelay_Ln", FrmMain.cameraP.pageDelay_Ln.ToString());
                        MC.SetParam(channelR, "SeqLength_Ln", FrmMain.cameraP.seqLength_Ln.ToString());
                        MC.SetParam(channelR, "PageLength_Ln", FrmMain.cameraP.pageLength_Ln.ToString());
                        MC.SetParam(channelR, "SeqLength_Fr", MC.INDETERMINATE);  
                        multiCamRCallback = new MC.CALLBACK(MultiCamCallback);
                        MC.RegisterCallback(channelR, multiCamRCallback, channelR);
                        MC.SetParam(channelR, MC.SignalEnable + MC.SIG_SURFACE_PROCESSING, "ON");
                        MC.SetParam(channelR, MC.SignalEnable + MC.SIG_ACQUISITION_FAILURE, "ON");
                        MC.SetParam(channelR, MC.SignalEnable + MC.SIG_SURFACE_FILLED, "ON");
                        MC.SetParam(channelR, MC.SignalEnable + MC.SIG_START_ACQUISITION_SEQUENCE, "ON");
                        MC.SetParam(channelR, MC.SignalEnable + MC.SIG_END_ACQUISITION_SEQUENCE, "ON");
                        MC.SetParam(channelR, "ChannelState", "READY");
                    }
                }

            }
              catch (DefectDetection.MultiCamException exc)
            {
                FrmMain.T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "OpenCam");
            }
        }
        object lockerMultiCam = new object();
        public void MultiCamCallback(ref MC.SIGNALINFO signalInfo)
        {
            lock (lockerMultiCam)
            {
                switch (signalInfo.Signal)
                {
                    case MC.SIG_START_ACQUISITION_SEQUENCE:
                        watchTakeImg.Reset();
                        watchTakeImg.Start();
                        #region /////如果1号机发送过来的产品规格信息改变，停止采相，设置相机采相帧数，再启动相机，根据玻璃长度设置采像线数  //添加重新加载所有的参数by cxx
                        if (FrmMain.comm.productInfoChanged)
                        {
                            //左相机关闭后重启
                            Stop(true);
                            SetPageLines(true);
                            GrabImage(true);
                            //右相机关闭后重启
                            Stop(false);
                            SetPageLines(false);                       
                            GrabImage(false);

                            //重新加载不同规格玻璃的参数
                            FrmMain.ini.ReadParam(Convert.ToInt32(FrmMain.frmMain.tboxGlassLength), Convert.ToInt32(FrmMain.frmMain.tboxGlassWidth), Convert.ToDouble(FrmMain.frmMain.tboxGlassThick));
                            //int length = (int)(double.Parse(comm.StrMsgRec[1]) / FrmMain.systemP.mmPerPix + FrmMain.cameraP.correctLine);
                            //FrmMain.cameraP.pageLength_Ln = length;
                            //FrmMain.cameraP.seqLength_Ln = length;
                        }
                        #endregion
                        FrmMain.T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + "下幅图采像开始");
                        break;
                    case MC.SIG_SURFACE_PROCESSING:
                        //ProcessingCallback(signalInfo);
                        break;
                    case MC.SIG_ACQUISITION_FAILURE:
                        AcqFailureCallback(signalInfo);
                        FrmMain.T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + "采像失败");
                        break;
                    case MC.SIG_SURFACE_FILLED:
                        ProcessingCallback(signalInfo);
                        break;
                    case MC.SIG_END_ACQUISITION_SEQUENCE:
                        watchTakeImg.Stop();
                        FrmMain.T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + "采像结束，采像时间：" + watchTakeImg.ElapsedMilliseconds.ToString() + "ms");
                        break;
                    default:
                        throw new DefectDetection.MultiCamException("Unknown signal");
                }
            }
        }
        object lockerProcess = new object();
        public void ProcessingCallback(MC.SIGNALINFO signalInfo)
        {
            lock (lockerProcess)
            {
                UInt32 currentChannel = (UInt32)signalInfo.Context;
                //statusBar.Text = "Processing";
                currentSurface = signalInfo.SignalInfo;
                try
                {
                    // Update the image with the acquired image buffer data 
                    Int32 width, height, bufferPitch, gElapse;
                    IntPtr bufferAddress;
                    MC.GetParam(currentChannel, "ImageSizeX", out width);
                    MC.GetParam(currentChannel, "ImageSizeY", out height);
                    ////MC.GetParam(currentChannel, "BufferPitch", out bufferPitch);
                    MC.GetParam(currentSurface, "SurfaceAddr", out bufferAddress);
                    MC.GetParam(currentChannel, "Elapsed_Ln", out gElapse);
                    //try
                    //{
                        //imageMutex.WaitOne();
                        //if (tmpImage != null)
                        //{
                        //    tmpImage.Dispose();
                        //    tmpImage = null;
                        //}
                        ////获取彩色图像;    !!!!此行耗时可移动位置到委托中
                        //HOperatorSet.GenImageInterleaved(out tmpImage, bufferAddress, "bgr", width, gElapse, -1, "byte", width, gElapse, 0, 0, -1, 0);
                       
                    //}
                    //finally
                    //{
                    //    imageMutex.ReleaseMutex();
                    //    //Stop();
                    //}

                    if (ImageReadyEvent != null)
                    {
                        ImageReadyEvent("Pass", bufferAddress, gElapse, width,height);//进入图像分析，实际函数m_ImageReadyEvent_GrabImage;
                    }
                }

                catch (DefectDetection.MultiCamException exc)
                {
                    FrmMain.T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "ProcessingCallback");
                }
                catch (System.Exception exc)
                {
                    FrmMain.T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "ProcessingCallback");
                }
                // - GrablinkSnapshotTrigger Sample Program
            }
        }
        private void AcqFailureCallback(MC.SIGNALINFO signalInfo)
        {
            UInt32 currentChannel = (UInt32)signalInfo.Context;

            // + GrablinkSnapshotTrigger Sample Program

            try
            {
                // Display frame rate and channel state
                //statusBar.Text = String.Format("Acquisition Failure, Channel State: IDLE");
           //   this.BeginInvoke(new ImageReadyEventHandler(m_ImageReadyEvent_GrabImage));// new object[1] { CreateGraphics() });
            }
            catch (System.Exception exc)
            {
                msg = DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "System Exception";
               // FrmMain.T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "System Exception");
            }

            // - GrablinkSnapshotTrigger Sample Program
        }
        #region///开始抓取
        object lockerStart = new object();
        public void GrabImage(bool isLeft)
        {
            lock (lockerStart)
            {
                // + GrablinkSnapshotTrigger Sample Program
                // Start an acquisition sequence by activating the channel
                String channelState = null;
                if (isLeft)
                   MC.GetParam(channel, "ChannelState", out channelState);
                else
                    MC.GetParam(channelR, "ChannelState", out channelState);
                if (channelState != "ACTIVE")
                {
                    if (isLeft)
                    {
                        MC.SetParam(channel, "ChannelState", "ACTIVE");
                        // Generate a soft trigger event
                        MC.SetParam(channel, "ForceTrig", "TRIG");
                        FrmMain.T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + "左相机启动成功");
                    }
                    else
                    {
                        MC.SetParam(channelR, "ChannelState", "ACTIVE");
                        // Generate a soft trigger event
                        MC.SetParam(channelR, "ForceTrig", "TRIG");
                        FrmMain.T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + "右相机启动成功");
                    }
                }
                // Refresh();
                // - GrablinkSnapshotTrigger Sample Program
            }
        }
        #endregion 

        #region/// 打开相机
        //public void Start()
        //{
        //    String channelState;
        //    MC.GetParam(channel, "ChannelState", out channelState);
        //    if (channelState != "ACTIVE")
        //    {
        //        MC.SetParam(channel, "ChannelState", "ACTIVE");
        //        MC.SetParam(channel, "ForceTrig", "TRIG");
        //    }
        //    if (channelState == "ACTIVE")
        //        FrmMain.T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + "相机开始工作");
        //}
        #endregion

        #region/// 关闭相机
        object lockerStop = new object();
        public void Stop(bool isLeft)
        {
            lock (lockerStop)
            {
                String channelState = null;
                if (isLeft)
                {
                    MC.GetParam(channel, "ChannelState", out channelState);
                    if (channelState == "ACTIVE")
                        MC.SetParam(channel, "ChannelState", "IDLE");
                    if (channelState == "IDLE")
                       FrmMain.T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + "左相机暂停工作");
                }
                else
                {
                    MC.GetParam(channelR, "ChannelState", out channelState);
                    if (channelState == "ACTIVE")
                       MC.SetParam(channelR, "ChannelState", "IDLE");
                    if (channelState == "IDLE")
                       FrmMain.T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + "右相机暂停工作");
                }
              
             }
        }
        #endregion

        #region///设置采像线数
        public void SetPageLines(bool isLeft)
        {
            String channelState = null;
            if (isLeft)
            {
                MC.GetParam(channel, "ChannelState", out channelState);
                if (channelState == "ACTIVE")
                {
                    Stop(true);
                }
                MC.SetParam(channel, "SeqLength_Ln", FrmMain.cameraP.pageLength_Ln);
                MC.SetParam(channel, "PageLength_Ln", FrmMain.cameraP.seqLength_Ln);
            }
            else
            {
                MC.GetParam(channelR, "ChannelState", out channelState);
                if (channelState == "ACTIVE")
                {
                    Stop(false);
                }
                MC.SetParam(channelR, "SeqLength_Ln", FrmMain.cameraP.pageLength_Ln);
                MC.SetParam(channelR, "PageLength_Ln", FrmMain.cameraP.seqLength_Ln);
            }
           
         

        }
        #endregion
    }
}
