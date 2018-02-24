using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using ViewROI;
using HalconDotNet;
using System.Collections;
using System.Threading;
using System.IO;
using System.Windows.Documents;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
namespace DefectDetection
{
    public partial class FrmMain : Form
    {
        /// <summary>
        /// 委托
        /// </summary>
        delegate void SetMsgCallBack(string text);
        private delegate void showListView(ListViewItem list,bool isClear);
        private delegate void picBoxShow();
        private delegate void chatShow();
        private delegate void tboxAllNumShow(int num);
        private delegate void tboxOKShow(int num);
        private delegate void tboxOKProportionShow(double num);
      
        /// <summary>
        /// 图像显示相关变量
        /// </summary>
        /// 左窗体显示
        public HWndCtrl viewControllerL;
        public ROIController roiControllerL;
        public ROIRectangle1 rectangle1L = null;
        /// 右窗体显示
        public HWndCtrl viewControllerR;
        public ROIController roiControllerR;
        public ROIRectangle1 rectangle1R = null;

        public HObject testImg = null,imgRed = null,imgGreen = null,imgBlue = null;
        public HObject imgRedR = null, imgGreenR = null, imgBlueR = null;
        public HImage hImgLeft = null, hImgRight = null;
        public HObject hoImageR = null, hoImage = null;//存放从相机采集到的图像

        private String LeftImgPath = Application.StartupPath + "\\LeftImg\\";
        private String RightImgPath = Application.StartupPath + "\\RightImg\\";

        /// <summary>
        /// 画布中显示玻璃图像,假设画布代表实际大小为880mm * 400mm
        /// </summary>
        private static int canvasWidth = 0; //画布宽
        private static int canvasHeight =0;//画布高
        //private Bitmap m_bmp = null ;//画布中图像
        float m_nScale = 1.0F;//缩放比例
        string m_strMousePt;//鼠标当前位置对应坐标
        Point m_ptCanvas;           //画布原点在设备上的坐标
        Point m_ptCanvasBuf;        //重置画布坐标计算时用的临时变量
        Point m_ptBmp;   //图像位于画布坐标系中的坐标
        Point m_ptMouseDown;        //鼠标点下是在设备坐标上的坐标
        float k = 0;//玻璃实际尺寸与画布尺寸的转换比例

        //记录相对于画布玻璃左上角点（orgX，orgY），后边用做坐标系转换
        int orgX = 0;
        int orgY = 0;
        
         //创建位图
        System.Drawing.Bitmap imageBmp = null;
        
           
        ListViewItem list = new ListViewItem();
        /// <summary>
        /// 图像处理数据相关变量
        /// </summary>
        public Hashtable[] ht = null;//手动测试变量
        surfaceDetect surfdetect = new surfaceDetect();//手动测试变量
        public surfaceDetect leftProcess = new surfaceDetect();
        public surfaceDetect rightProcess = new surfaceDetect();
        public static Hashtable[] leftHt = null;
        public static Hashtable[] rightHt = null;
        public static bool leftFlag = false;//左边图像处理完成后置为true
        public static bool rightFlag = false;//右边图像处理完成后置为true
        public HTuple leftNum = null;
        public HTuple rightNum = null;
        public DefectFeature defectFeature = new DefectFeature();
        /// <summary>
        /// 线程相关变量
        /// </summary>
        public Thread LeftImgThread = null;
        public Thread RightImgThread = null;
        private Thread tCameraSet = null;//左相机图像获取线程;
        private Thread tCameraRSet = null;//右相机图像获取线程;
        private Thread saveToDBThread = null;//更新数据库线程

        private object locker = new object();
        
        /// <summary>
        /// 常用变量
        /// </summary>
        public string Model = "";
        private List<String> listL = new List<string>();//存放左边图像路径及名称
        private List<String> listR = new List<string>();//存放右边图像路径及名称
        private Stopwatch watch = new Stopwatch();
        private Stopwatch watchR = new Stopwatch();
        private double glassOK = 0, glassNG = 0;
        private int glassCount = 0;

        public static QueList T = new QueList(); //消息队列;
        private textFile txtFile = new textFile(Application.StartupPath + "\\log.txt");//log文件;
        public static SystemP systemP = new SystemP();
        public static CameraP cameraP = new CameraP();
        public static SurfaceP surfaceP = new SurfaceP();
        public static IniFunction ini = new IniFunction();
        public static Camera mycamera = new Camera();//左相机配置;
        public static Camera mycameraR = new Camera();//右相机配置;

        public int glassRealCanvasWidth = 0;
        public int glassRealCanvasHeight = 0;
        public int glassPixelWidth = 0;
        public int glassPixelHeight = 0;

        private string now = null;//表示当前时间
        private List<string> txData = null;//表示折线图X集合
        private List<int> tyData = null;//表示折线图Y集合
        private Random ra = new Random();//随机产生数值，测试用

        ArrayList lstItm = new ArrayList();
        private int ii = 0;//消息框显示使用
        private bool processingL = false;//左窗体线程是否在加载处理图像
        private bool processingR = false;//右窗体线程是否在加载处理图像
        private static bool udpStart = false;
        public static FrmMain frmMain;
        /// <summary>
        /// 数据库
        /// </summary>
        public static string strConnection = "initial catalog=ServerGlassInfo;integrated security=SSPI";
        /// <summary>
        /// 通信
        /// </summary>
        public static Communication comm = new Communication();

        public FrmMain()
        {
            frmMain = this;
            InitializeComponent();
            ///窗体显示
            this.StartPosition = FormStartPosition.CenterScreen;
            this.pictureBox2.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBox2.BackColor = Color.Black;
            this.pictureBox2.MouseWheel += new MouseEventHandler(pictureBox2_MouseWheel);
         

        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMain_Load(object sender, EventArgs e)
        {
            #region ///设置窗体，显示图像
            ///设置左窗体，显示图像
            if (viewControllerL != null)
            {
                viewControllerL.setViewState(HWndCtrl.MODE_VIEW_NONE);
            }
            viewControllerL = new HWndCtrl(hwcLeft);
            roiControllerL = new ROIController();
            viewControllerL.useROIController(roiControllerL);
            viewControllerL.setViewState(HWndCtrl.MODE_VIEW_NONE);

           ///设置右窗体，显示图像
            if (viewControllerR != null)
            {
                viewControllerR.setViewState(HWndCtrl.MODE_VIEW_NONE);
            }
            viewControllerR = new HWndCtrl(hwcRight);
            roiControllerR = new ROIController();
            viewControllerR.useROIController(roiControllerR);
            viewControllerR.setViewState(HWndCtrl.MODE_VIEW_NONE);
            #endregion

            #region //chart控件初始化
            chart1.Legends.Clear();
            tyData = new List<int>() { 0 };
            txData = new List<string>() { DateTime.Now.ToString("HH-mm-ss-f") };
            //axChartSpace1
            chart1.Titles.Add("良品率");
            chart1.ChartAreas[0].Axes[0].MajorGrid.Enabled = false;//x轴上的网格
            chart1.ChartAreas[0].Axes[0].MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            chart1.ChartAreas[0].Axes[0].MajorTickMark.Enabled = true;
            //chart1.ChartAreas[0].AxisX.AxisName
            chart1.Series.Add(new Series());
            //chart1.Series[1].Label = "###";
            chart1.Series[0].ToolTip = "zzz";
            chart1.Series[0].ChartType = SeriesChartType.Line;
            chart1.Series[0].Points.DataBindXY(txData, tyData);

            chart1.Series[0].Color = Color.Blue;               //线条颜色
            chart1.Series[0].BorderWidth = 2;                   //线条粗细
            chart1.Series[0].MarkerBorderColor = Color.Black;   //标记点边框颜色
            chart1.Series[0].MarkerBorderWidth = 3;             //标记点边框大小
            chart1.Series[0].MarkerColor = Color.Black;         //标记点中心颜色
            chart1.Series[0].MarkerSize = 2;                    //标记点大小
            chart1.Series[0].MarkerStyle = MarkerStyle.Circle;  //标记点类型
            #endregion

            #region//读取Ini文件获取参数
            ini.ReadParam(350, 300, 0.1);
            #endregion

            #region ///加载listView1
            listView1.GridLines = true;
            listView1.View = View.Details;
            //listView1.CheckBoxes = true;
            //listView1.Bounds = new Rectangle(new Point(10, 10), new Size(1800, 200));;
            listView1.Columns.Add("lstGlassID", "玻璃ID");
            listView1.Columns.Add("lstLevel ", "等级");
            listView1.Columns.Add("lstDefectID", "缺陷ID");

            listView1.Columns.Add("lstSize", "大小");
            listView1.Columns.Add("lstArea", "面积");
            listView1.Columns.Add("lstWidth", "宽");
            listView1.Columns.Add("lstxHeight", "高");
            listView1.Columns.Add("lstCntX", "中心X");
            listView1.Columns.Add("lstCntY", "中心Y");
            lvwColumnSorter = new ListViewColumnSorter();//listview中内容排序
            #endregion
           

            #region//绘制玻璃
            //一幅图片大小是7296*17000，两幅大小为14592*17000转换为实际尺寸为436*510(此处mm/pix = 0.03)
            //画布大小为880*400，现允许画布能显示上述玻璃
            //玻璃尺寸转换到画布上为：
            //height: 380(假设在画布上显示高为380，实际是510)则转换比例K = 380/510 = 0.745
            //width:436*k = 325
            k = 0.745f;//玻璃1mm在画布上相当于0.745mm
            glassPixelWidth = 7296 * 2;
            glassPixelHeight = 17000;
            draw(glassPixelWidth, glassPixelHeight, systemP.mmPerPix, k);
            cMiddle = (int)(Math.Round(glassPixelWidth / 2 * systemP.mmPerPix * k,0));
            #endregion

           
            lblProjectName.Text = "test01";

            #region ///获取采集到的图像
            mycamera.ImageReadyEvent += new Camera.ImageReadyEventHandler(m_ImageReadyEvent_GrabImage);
            mycameraR.ImageReadyEvent += new Camera.ImageReadyEventHandler(m_ImageReadyEvent_GrabImageR);
            #endregion

            comm.ShowMessage += this.glassInfoShow;//订阅事件 udp接收到的玻璃信息传递给主窗体

            #region ///加载相机参数
            //加载相机参数(左相机)
            //if (tCameraSet != null)
                //{
                //    tCameraSet.Abort();
                //    tCameraSet.DisableComObjectEagerCleanup();
                //    tCameraSet = null;
                //}
                //tCameraSet = new Thread(CameraSet);
                //tCameraSet.IsBackground = true;
                //tCameraSet.Start();

                ////加载相机参数(右相机)
                //if (tCameraRSet != null)
                //{
                //    tCameraRSet.Abort();
                //    tCameraRSet.DisableComObjectEagerCleanup();
                //    tCameraRSet = null;
                //}
                //tCameraRSet = new Thread(CameraSet);
                //tCameraRSet.IsBackground = true;
            //tCameraRSet.Start();
            #endregion
  
            #region//启动时间
            tsTboxRunTime.Text = DateTime.Now.ToString("MM-dd HH:mm:ss");
            #endregion

            #region ///页面初始化权限
            //tsMenuItemVision.Enabled = false;
            //tsMenuItemGetReport.Enabled = false;
            //tsMenuItemHelp.Enabled = false;
            //tsBtnRun.Enabled = false;
            //tsBtnStop.Enabled = false;
            //tsBtnLoadImg.Enabled = false;
            //tsBtnParaSet.Enabled = false;
            #endregion
            //暂时不用
            tsMenuItemLeftCam.Enabled = false;
            tsMenuItemCamRight.Enabled = false;

            //初始化 坐标
            //m_ptCanvas = new Point(pictureBox2.Width / 2, pictureBox2.Height / 2);
            m_ptCanvas = new Point(1, 1);
            //玻璃在画布上的实际尺寸
            glassRealCanvasWidth = (int)(glassPixelWidth * systemP.mmPerPix * k);
            glassRealCanvasHeight = (int)(glassPixelHeight * systemP.mmPerPix * k);
            //记录相对于画布玻璃左上角点（orgX，orgY），后边用做坐标系转换
            orgX = canvasWidth / 2 - glassRealCanvasWidth / 2;
            orgY = canvasHeight / 2 - glassRealCanvasHeight / 2;
                     
        }

        /// <summary>
        /// 开始运行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsBtnRun_Click(object sender, EventArgs e)
        {
            Model = "run";
            #region//UDP通信
            initalUDP();
            #endregion
            LeftImgThread = new Thread(new ParameterizedThreadStart(LeftImgProcess));
            LeftImgThread.IsBackground = true;
            LeftImgThread.Start(LeftImgPath);

            RightImgThread = new Thread(new ParameterizedThreadStart(RightImgProcess));
            RightImgThread.IsBackground = true;
            RightImgThread.Start(RightImgPath);
        }

        /// <summary>
        /// 停止运行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsBtnStop_Click(object sender, EventArgs e)
        {
            //左相机
            if (tCameraSet != null)
            {
                tCameraSet.Abort();
                tCameraSet.DisableComObjectEagerCleanup();
                tCameraSet = null;
            }
            tCameraSet = new Thread(CameraStop);
            tCameraSet.IsBackground = true;
            tCameraSet.Start();

            //右相机
            if (tCameraRSet != null)
            {
                tCameraRSet.Abort();
                tCameraRSet.DisableComObjectEagerCleanup();
                tCameraRSet = null;
            }
            tCameraRSet = new Thread(CameraRStop);
            tCameraRSet.IsBackground = true;
            tCameraRSet.Start();
        }

        /// <summary>
        /// 手动加载图像并测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsBtnLoadImg_Click(object sender, EventArgs e)
        {
            Model = "manual";
             openFileDialog1.Filter = "所有文件|*.*";
             if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
             {
                 Thread thread1 = new Thread(loadImg);
                 thread1.IsBackground = true;
                 thread1.Start();
             }
           
        }

        private void loadImg()
        {
           
                //glassRealHeight = 380;
                //glassRealWidth = 325;
                //draw(glassRealWidth, glassRealHeight,systemP.mmPerPix,k);
                draw(7296*2, 17000, systemP.mmPerPix, k);
                string fileName = openFileDialog1.FileName;
                lstItm.Clear();//清空存放缺陷位置信息
                HOperatorSet.ReadImage(out testImg, fileName);
                HalconFunction halcon = new HalconFunction();
                if (imgBlue != null)
                {
                    imgBlue.Dispose();
                    imgBlue = null;
                }
                HOperatorSet.Decompose3(testImg, out imgRed, out imgGreen, out imgBlue);
                imgRed.Dispose();
                imgGreen.Dispose();
                HImage imgshow = new HImage(imgBlue);
                viewControllerR.addIconicVar(imgshow);
                viewControllerR.repaint();
                //表面检测
                surfdetect.detect(imgBlue, surfaceP.BlueThreshold, 255, out ht, out rightNum,false);
                //清空ListView1
                listViewShow(null, true);
                //pictureBox上绘制缺陷，并在listView1上显示
                defectShow( ht, rightNum, false);
                //刷新显示
                pictureBoxShow();

        }
        /// <summary>
        /// 左窗体图像放大
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsMenuItemZoomIn_Click(object sender, EventArgs e)
        {
            viewControllerL.setViewState(HWndCtrl.MODE_VIEW_ZOOM);
        }

        /// <summary>
        /// 左窗体还原
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsMenuItemOriginal_Click(object sender, EventArgs e)
        {
            viewControllerL.resetAll();
            viewControllerL.repaint();
        }

        /// <summary>
        /// 左窗体移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsMenuItemMove_Click(object sender, EventArgs e)
        {
            viewControllerL.setViewState(HWndCtrl.MODE_VIEW_MOVE);
        }

        /// <summary>
        /// 右窗体图像放大
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ts2MenuItem1_Click(object sender, EventArgs e)
        {
            viewControllerR.setViewState(HWndCtrl.MODE_VIEW_ZOOM);
        }

        /// <summary>
        /// 右窗体还原
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ts2MenuItem2_Click(object sender, EventArgs e)
        {
            viewControllerR.resetAll();
            viewControllerR.repaint();
        }

        /// <summary>
        /// 右窗体移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ts2MenuItem3_Click(object sender, EventArgs e)
        {
            viewControllerR.setViewState(HWndCtrl.MODE_VIEW_MOVE);
        }

        /// <summary>
        /// 左相机获取图像，处理图像
        /// </summary>
        /// void m_ImageReadyEvent_GrabImage(string recData, HObject recImage)
        void m_ImageReadyEvent_GrabImage(string recData, IntPtr adrass, Int32 gElapse, Int32 width, Int32 height)
        {
            try
            {
                this.BeginInvoke(new MethodInvoker(delegate
                {
                    #region ///图像获取及处理
                    switch (recData)
                    {
                        case "Pass":
                            processingL = true;
                            watch.Reset();
                            watch.Start();
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                            #region //该行代码在两个线程中应该应该只执行一次。此处需修改
                            draw(glassPixelWidth, glassPixelHeight, systemP.mmPerPix, k);
                            #endregion
                            leftFlag = false;
                            HOperatorSet.GenImageInterleaved(out hoImage, adrass, "bgr", width, gElapse, -1, "byte", width, gElapse, 0, 0, -1, 0);
                            if (imgBlue!= null)
                            {
                                imgBlue.Dispose();
                                imgBlue = null;
                            }
                            HOperatorSet.Decompose3(hoImage, out imgRed, out imgGreen, out imgBlue);
                            watch.Stop();
                            T.queueIn(DateTime.Now.ToString("yy-MM-dd HH-mm-ss f") + "左窗口获取图像所用时间：" + watch.ElapsedMilliseconds.ToString() + "ms");
                            hImgLeft = new HImage(imgBlue);
                            viewControllerL.addIconicVar(hImgLeft);
                            viewControllerL.repaint();
                            //处理图像
                            if (imgBlue != null)
                            {
                                watch.Reset();
                                watch.Start();
                                leftProcess.detect(imgBlue, surfaceP.BlueThreshold, 255, out leftHt, out leftNum, true);
                                leftFlag = true;
                                watch.Stop();
                                T.queueIn(DateTime.Now.ToString("yy-MM-dd HH-mm-ss f") + "左窗口处理图像所用时间：" + watch.ElapsedMilliseconds.ToString() + "ms");
                                #region//将当前玻璃缺陷画出来，并显示在listView上
                                if (leftFlag && rightFlag)
                                {
                                    UdpSend();
                                    //清除上一块玻璃内容
                                    listViewShow(null, true);
                                    lstItm.Clear();//清除保存玻璃位置信息的list
                                    defectShow(leftHt, leftNum, true);
                                    defectShow(rightHt, rightNum, false);

                                    pictureBoxShow(); //刷新显示
                                    processingL = false;
                                    updateTextBox();//更新textBox
                                    updateChart();//更新折线图
                                    ////更新数据库
                                    if (saveToDBThread != null)
                                    {
                                        saveToDBThread.Abort();
                                        saveToDBThread = null;
                                    }
                                    saveToDBThread = new Thread(GlassStatistics);
                                    saveToDBThread.IsBackground = false;
                                    saveToDBThread.Start();
                                }
                                #endregion
                            }
                            ImgDispose();
                            GC.Collect();
                            break;
                        default:
                            break;
                    }
                    #endregion
                }
               ));
            }
            catch (Exception exc)
            {
              T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "m_ImageReadyEvent_GrabImage");
            }
        }

        /// <summary>
        /// 右相机获取图像，处理图像
        /// </summary>
        /// <param name="recData"></param>
        public void m_ImageReadyEvent_GrabImageR(string recData, IntPtr adrass, Int32 gElapse, Int32 width, Int32 height)
        {
            try
            {
                this.BeginInvoke(new MethodInvoker(delegate
                {
                        #region ///图像获取及处理
                        switch (recData)
                        {
                            case "Pass":
                            processingR = true;
                            watchR.Reset();
                            watchR.Start();
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                             #region //该行代码在两个线程中应该只执行一次。？？？
                            draw(glassPixelWidth, glassPixelHeight, systemP.mmPerPix, k);
                            #endregion
                            rightFlag = false;
                            HOperatorSet.GenImageInterleaved(out hoImageR, adrass, "bgr", width, gElapse, -1, "byte", width, gElapse, 0, 0, -1, 0);
                            if (imgBlueR != null)
                            {
                                imgBlueR.Dispose();
                                imgBlueR = null;
                            }
                            HOperatorSet.Decompose3(hoImageR, out imgRedR, out imgGreenR, out imgBlueR);
                            watchR.Stop();
                            T.queueIn(DateTime.Now.ToString("yy-MM-dd HH-mm-ss f") + "右窗口获取图像所用时间：" + watchR.ElapsedMilliseconds.ToString() + "ms");
                            hImgRight = new HImage(imgBlueR);
                            viewControllerR.addIconicVar(hImgRight);
                            viewControllerR.repaint();
                            //处理图像
                            if (imgBlueR != null)
                            {
                                    watchR.Reset();
                                    watchR.Start();
                                    rightProcess.detect(imgBlueR, surfaceP.BlueThreshold, 255, out rightHt,out rightNum,false);
                                    rightFlag = true;
                                    watchR.Stop();
                                    T.queueIn(DateTime.Now.ToString("yy-MM-dd HH-mm-ss f") + "右窗口处理图像所用时间：" + watch.ElapsedMilliseconds.ToString() + "ms");
                                    #region//将当前玻璃缺陷画出来，并显示在listView上
                                    if (leftFlag && rightFlag)
                                    {
                                        UdpSend();
                                        //清除上一块玻璃内容
                                        listViewShow(null, true);
                                        lstItm.Clear();//清除保存玻璃位置信息的list
                                        defectShow(leftHt, leftNum, true);
                                        defectShow(rightHt, rightNum, false);

                                        pictureBoxShow(); //刷新显示
                                        processingR = false;
                                        updateTextBox();//更新textBox
                                        updateChart();//更新折线图
                                        ////更新数据库
                                        if (saveToDBThread != null)
                                        {
                                            saveToDBThread.Abort();
                                            saveToDBThread = null;
                                        }
                                        saveToDBThread = new Thread(GlassStatistics);
                                        saveToDBThread.IsBackground = false;
                                        saveToDBThread.Start();
                                    }
                                  #endregion
                            }
                           ImgDisposeR();
                           GC.Collect();
                                break;
                            default:
                                break;
                        }
                        #endregion
                }
                ));
            }
            catch (Exception exc)
            {
              T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "m_ImageReadyEvent_GrabImageR");
            }
        }

        public void glassInfoShow(Object sender, int [] array)
        {
            this.BeginInvoke(new MethodInvoker(delegate
                {
                    tBoxGlassId.Text = array[0].ToString();
                    tboxGlassLength.Text = array[1].ToString();
                    tboxGlassWidth.Text = array[2].ToString();
                    tboxGlassThick.Text = array[3].ToString();
                }
                ));
        }

        /// <summary>
        /// Udp发送玻璃检测结果
        /// </summary>
        public void UdpSend()
        {
                if (leftNum > 0 || rightNum > 0)
                {
                    Int32[] s = new Int32[4]{3117, 3118, 
                                10,1000};
                    Int16 S_Juggment = Convert.ToInt16(1);
                    comm.udpSend(s, S_Juggment);
                    T.queueIn(DateTime.Now.ToString("yy-MM-dd HH-mm-ss ff") + "----数据发送成功：" + s[0].ToString() + ";" + s[1].ToString() + ";" + s[2].ToString() + ";" + s[3].ToString());
                }
                else
                {
                }
        }

        /// <summary>
        /// 玻璃信息写入数据库
        /// </summary>
        public void GlassStatistics()
        {
           // 写入数据库
                if (leftNum > 0 || rightNum > 0)
                {
                    saveGlassQualityInfoToDb();
                    if (Model == "manual")
                    {
                        saveDefectInfoToDb(ht, leftNum);
                    }
                    else
                    {
                        //保存左边玻璃缺陷信息
                        saveDefectInfoToDb(leftHt, leftNum);
                        //保存右边玻璃缺陷信息
                        saveDefectInfoToDb(rightHt, rightNum);
                    }
                }
        }

        /// <summary>
        /// 将玻璃质量信息保存在表[glassGuality_info]
        /// </summary>
        public void saveGlassQualityInfoToDb()
        {
            //记录每个项目玻璃的NG和OK数量
            using (SqlConnection con = new SqlConnection(strConnection))
            {

                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "update [glassGuality_info] set glass_NG = " + glassNG + ",glass_OK = " + glassOK + " where projectName= '" + lblProjectName.Text + "'";
                int QueryResult = cmd.ExecuteNonQuery();
                if (QueryResult == 0)
                {
                    //如果无法更新，说明表中没有该项目的信息，需要添加信息
                    cmd.CommandText = "insert into [glassGuality_info] (projectName,glass_NG,glass_OK) values ('" + lblProjectName.Text + "','" + glassNG + "','" + glassOK + "')";
                    QueryResult = cmd.ExecuteNonQuery();
                    if (QueryResult == 0)
                    {
                        T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff---") + "更新数据失败");
                        return;
                    }
                    else
                        T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff---") + "添加数据成功");
                }
                else
                {
                   T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff---") + "更新数据成功");
                }
                con.Close();
            }
                   
        }

        /// <summary>
        /// 将缺陷的详细信息记录在数据表[surfaceDetect_info]中
        /// </summary>
        /// <param name="ht"></param>
        /// <param name="number"></param>
        public void saveDefectInfoToDb(Hashtable[] ht, HTuple number)
        {
            try
            {
                double row = 0, col = 0, width = 0, height = 0;
                for (int i = 0; i < number; i++)
                {
                    foreach (var item in ht[i].Keys)
                    {
                        switch ((string)item)
                        {
                            case "row":
                                row = double.Parse(ht[i][item].ToString());
                                break;
                            case "column":
                                col = double.Parse(ht[i][item].ToString());
                                break;
                            case "width":
                                width = double.Parse(ht[i][item].ToString());
                                break;
                            case "height":
                                height = double.Parse(ht[i][item].ToString());
                                break;
                            default:
                                break;
                        }
                    }
                    //写入数据库
                    using (SqlConnection con = new SqlConnection(FrmMain.strConnection))
                    {

                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = con;
                        cmd.CommandText = "insert into [surfaceDetect_info] (projectName,region_area,region_row,region_cloumn,region_width,region_height) values ('" + lblProjectName.Text + "'," + 0 + "," + row + "," + col + "," + width + "," + height + ")";
                        int QueryResult = cmd.ExecuteNonQuery();
                        if (QueryResult == 0)
                        {
                            T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff---") + "更新数据失败");
                            return;
                        }
                        else
                        {
                            //FrmMain.T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff---") + "更新数据成功");
                        }
                        con.Close();
                    }
                }

            }
            catch (Exception exc)
            {
               T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "saveDefectInfoToDb");
            }
        }

        /// <summary>
        /// 自动加载并计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsMenuItemAutoLoad_Click(object sender, EventArgs e)
        {
            Model = "autoTest";
            #region//UDP通信
            initalUDP();
            #endregion
            LeftImgThread = new Thread(new ParameterizedThreadStart(LeftImgProcess));
            LeftImgThread.IsBackground = true;
            LeftImgThread.Start(LeftImgPath);

            RightImgThread = new Thread(new ParameterizedThreadStart(RightImgProcess));
            RightImgThread.IsBackground = true;
            RightImgThread.Start(RightImgPath);
        }
        HObject glass = null;
        /// <summary>
        /// 左图加载及分析过程
        /// </summary>
        /// <param name="path"></param>
        private void LeftImgProcess(object path)
        {
            try
            {
                //在线运行
                if (Model == "run")
                {
                    //左相机
                    if (tCameraSet != null)
                    {
                        tCameraSet.Abort();
                        tCameraSet.DisableComObjectEagerCleanup();
                        tCameraSet = null;
                    }
                    tCameraSet = new Thread(CameraStart);
                    tCameraSet.IsBackground = true;
                    tCameraSet.Start();
                  
                }
                //本地加载图像
                if (Model == "autoTest")
                {
                    DirectoryInfo folider = new DirectoryInfo((string)path);
                    FileInfo[] fileInfo = folider.GetFiles();
                   
                    listL.Clear();
                    foreach (FileInfo f in fileInfo)
                    {
                        listL.Add(f.Name);
                    }
                    foreach (String s in listL)
                    {
                        processingL = true;
                        watch.Reset();
                        watch.Start();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();

                        #region //这几行代码在两个线程中应该只执行一次。在收到线控信号开始执行
                        draw(glassPixelWidth, glassPixelHeight, systemP.mmPerPix, k);
                        leftFlag = false;
                        //DefectAllNum = 0;
                        #endregion

                        Bitmap image = new Bitmap(path + s);
                        if (image != null)
                        {
                            BitmapData bitMapData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                            HOperatorSet.GenImageInterleaved(out glass, bitMapData.Scan0, "bgr", image.Width, image.Height, -1, "byte", image.Width, image.Height, 0, 0, -1, 0);
                            //HOperatorSet.GenImageInterleaved(out tmpImage, bufferAddress, "bgr", width, gElapse, -1, "byte", width, gElapse, 0, 0, -1, 0);
                            image.UnlockBits(bitMapData);
                            if (image != null)
                            {
                                image.Dispose();
                                image = null;
                            }
                            if (imgBlue != null)
                            {
                                imgBlue.Dispose();
                                imgBlue = null;
                            }
                            HOperatorSet.Decompose3(glass, out imgRed, out imgGreen, out imgBlue);
                            //ImgDispose();
                            watch.Stop();
                            T.queueIn(DateTime.Now.ToString("yy-MM-dd HH-mm-ss f") + "左窗口获取图像所用时间：" + watch.ElapsedMilliseconds.ToString() + "ms");
                            hImgLeft = new HImage(imgBlue);
                            viewControllerL.addIconicVar(hImgLeft);
                            viewControllerL.repaint();
                            //处理图像
                            if (imgBlue != null)
                            {
                                    watch.Reset();
                                    watch.Start();
                                    leftProcess.detect(imgBlue, surfaceP.BlueThreshold, 255, out leftHt,out leftNum,true);
                                    leftFlag = true;
                                    watch.Stop();
                                    T.queueIn(DateTime.Now.ToString("yy-MM-dd HH-mm-ss f") + "左窗口处理图像所用时间：" + watch.ElapsedMilliseconds.ToString() + "ms");
                                    //将当前玻璃缺陷画出来，并显示在listView上
                                    if (leftFlag && rightFlag)
                                    {
                                        UdpSend();
                                        //清除上一块玻璃内容
                                        listViewShow(null, true);
                                        lstItm.Clear();//清除保存玻璃位置信息的list
                                        defectShow(leftHt, leftNum, true);
                                        defectShow(rightHt, rightNum, false);

                                        pictureBoxShow(); //刷新显示
                                        processingL = false;
                                        updateTextBox();//更新textBox
                                        updateChart();//更新折线图
                                        ////更新数据库
                                        if (saveToDBThread != null)
                                        {
                                            saveToDBThread.Abort();
                                            saveToDBThread = null;
                                        }
                                        saveToDBThread = new Thread(GlassStatistics);
                                        saveToDBThread.IsBackground = false;
                                        saveToDBThread.Start();
                                       
                                        
                                    }

                               
                                   
                            }
                        }
                        Thread.Sleep(3000);
                        ImgDispose();
                        GC.Collect();
                    
                       
                    }
                }
            }
            catch (Exception exc)
            {
               T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "LeftImgProcess");
            }
        }


        HObject glassR = null;
        /// <summary>
        /// 右图加载及分析过程
        /// </summary>
        /// <param name="path"></param>
        private void RightImgProcess(object path)
        {
            try
            {
                //在线运行
                if (Model == "run")
                {
                    //右相机
                    if (tCameraSet != null)
                    {
                        tCameraSet.Abort();
                        tCameraSet.DisableComObjectEagerCleanup();
                        tCameraSet = null;
                    }
                    tCameraSet = new Thread(CameraRStart);
                    tCameraSet.IsBackground = true;
                    tCameraSet.Start();
                }
                //本地加载图像
                if (Model == "autoTest")
                {
                    DirectoryInfo folider = new DirectoryInfo((string)path);
                    FileInfo[] fileInfo = folider.GetFiles();
                    //HObject glass = null;
                    listR.Clear();
                   
                    foreach (FileInfo f in fileInfo)
                    {
                        listR.Add(f.Name);
                    }
                    foreach (String s in listR)
                    {
                        processingR = true;
                        watchR.Reset();
                        watchR.Start();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        #region //该代码在两个线程中应该只执行一次。？？？
                        draw(glassPixelWidth, glassPixelHeight, systemP.mmPerPix, k);
                        #endregion
                        rightFlag = false;           
                        Bitmap image = new Bitmap(path + s);
                        if (image != null)
                        {
                            BitmapData bitMapData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                            HOperatorSet.GenImageInterleaved(out glassR, bitMapData.Scan0, "bgr", image.Width, image.Height, -1, "byte", image.Width, image.Height, 0, 0, -1, 0);
                            image.UnlockBits(bitMapData);
                           
                            if (image != null)
                            {
                                image.Dispose();
                                image = null;
                            }
                            if (imgBlueR != null)
                            {
                                imgBlueR.Dispose();
                                imgBlueR = null;
                            }
                            HOperatorSet.Decompose3(glassR, out imgRedR, out imgGreenR, out imgBlueR);
                            //ImgDisposeR();
                            watchR.Stop();
                            T.queueIn(DateTime.Now.ToString("yy-MM-dd HH-mm-ss ff") + "右窗口获取图像所用时间：" + watchR.ElapsedMilliseconds.ToString() + "ms");
                            
                            hImgRight = new HImage(imgBlueR);
                            viewControllerR.addIconicVar(hImgRight);
                            viewControllerR.repaint();
                            //处理图像
                            if (imgBlueR != null)
                            {
                               watchR.Reset();
                               watchR.Start();
                               rightProcess.detect(imgBlueR, surfaceP.BlueThreshold, 255, out rightHt, out rightNum,false);
                               rightFlag = true;
                               watchR.Stop();
                               T.queueIn(DateTime.Now.ToString("yy-MM-dd HH-mm-ss ff") + "右窗口处理图像所用时间：" + watchR.ElapsedMilliseconds.ToString() + "ms");
                               //将当前玻璃缺陷画出来，并显示在listView上
                               if (leftFlag && rightFlag)
                               {
                                   UdpSend();
                                   //清除上一块玻璃内容
                                   listViewShow(null, true);
                                   lstItm.Clear();//清除保存玻璃位置信息的list
                                   defectShow(leftHt, leftNum, true);
                                   defectShow(rightHt, rightNum, false);

                                   pictureBoxShow(); //刷新显示
                                   processingR = false;
                                   updateTextBox();//更新textBox
                                   updateChart();//更新折线图
                                   //更新数据库
                                   if (saveToDBThread != null)
                                   {
                                       saveToDBThread.Abort();
                                       saveToDBThread = null;
                                   }
                                   saveToDBThread = new Thread(GlassStatistics);
                                   saveToDBThread.IsBackground = false;
                                   saveToDBThread.Start();
                                
                               }
                            }
                        }
                        Thread.Sleep(3000);
                        ImgDisposeR();
                        GC.Collect();
                        
                    }
                }
            }
            catch (Exception exc)
            {
                T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "RightImgProcess");
            }
        } 

        /// <summary>
        /// listView显示，使用委托，防止线程间错误操作
        /// </summary>
        /// <param name="list"></param>
        /// 
      
        //int DefectAllNum = 0;
        public void defectShow(Hashtable[] ht, HTuple number,bool isLeft)
        {
            try
            {
                lock (locker)
                {
                    //HOperatorSet.SetColor(hwc.HalconWindow, "yellow");
                    Graphics g = Graphics.FromImage(imageBmp);
                    Pen pen = new Pen(Brushes.Black, 0.01f);
                    Pen pen1 = new Pen(Brushes.Blue, 4);
                    double row = 0, col = 0, width = 0, height = 0,size = 0,area = 0;
                    ////玻璃在画布上的实际尺寸
                    //glassRealCanvasWidth = (int)(glassPixelWidth * systemP.mmPerPix * k);
                    //glassRealCanvasHeight = (int)(glassPixelHeight * systemP.mmPerPix * k);
                    ////记录相对于画布玻璃左上角点（orgX，orgY），后边用做坐标系转换
                    // orgX = canvasWidth / 2 - glassRealCanvasWidth / 2;
                    // orgY = canvasHeight / 2 - glassRealCanvasHeight/2;
                     
                    for (int i = 0; i < number; i++)
                    {
                        foreach (var item in ht[i].Keys)
                        {
                            switch ((string)item)
                            {
                                case "row":
                                    row = double.Parse(ht[i][item].ToString());
                                    break;
                                case "column":
                                    col = double.Parse(ht[i][item].ToString());
                                    break;
                                case "width":
                                    width = double.Parse(ht[i][item].ToString());
                                    break;
                                case "height":
                                    height = double.Parse(ht[i][item].ToString());
                                    break;
                                case "size":
                                    size = double.Parse(ht[i][item].ToString());
                                    break;
                                case "area":
                                    area = double.Parse(ht[i][item].ToString());
                                    break;
                                default:
                                    break;
                            }
                        }
                        float cntX = (float)Math.Round(col * systemP.mmPerPix * k, 2);
                        float cntY = (float)Math.Round(row * systemP.mmPerPix * k, 2);
                        float widthD = (float)Math.Round(width * systemP.mmPerPix * k, 2);
                        float heightD = (float)Math.Round(height * systemP.mmPerPix * k, 2);
                        if (heightD >= 0 && heightD <= 1)
                            heightD = 1f;
                        if (heightD >= 0 && heightD <= 1)
                            widthD = 1f;
                        ////缺陷绘制
                        //if (isLeft)
                        //if(col > 10753.7 && col < 10753.8)
                        g.DrawRectangle(pen, cntX + orgX, cntY + orgY, widthD, heightD);
                        //else
                        //    g.DrawRectangle(pen, cntX + orgX + glassRealCanvasWidth / 2, cntY + orgY, widthD, heightD);
                        
                        //HOperatorSet.DispRectangle1(hwc.HalconWindow, row - height / 2 - 10, col - width / 2 - 10, row + height / 2 + 10, col + width / 2 + 10);
                        //缺陷添加在listViewItem
                        //缺陷列表信息添加
                        list.SubItems.Clear();
                        list.SubItems[0].Text = glassCount.ToString();
                        list.SubItems.Add("NG");
                        list.SubItems.Add(i.ToString());

                        list.SubItems.Add(size.ToString());
                        list.SubItems.Add(area.ToString());
                        list.SubItems.Add(width.ToString());
                        list.SubItems.Add(height.ToString());
                        
                        list.SubItems.Add(col.ToString());
                        list.SubItems.Add(row.ToString());
                        listViewShow((ListViewItem)list.Clone(), false);
                   
                        defectFeature.col = col;
                        defectFeature.row = row;
                        lstItm.Add(defectFeature);
                        //listViewQueue.Enqueue(lstItm[i]);
                    }
                    //DefectAllNum += number;//记录整块玻璃缺陷总数
                    //Debug.Print(DefectAllNum.ToString());
                    //g.DrawRectangle(pen1, orgX, orgY, glassRealCanvasWidth, glassRealCanvasHeight);
                    g.DrawLine(pen1, new Point(cMiddle + orgX, 10), new Point(cMiddle + orgX, 390));
                    g.Dispose();
                    pen.Dispose();
                    pen1.Dispose();
                }
                

            }
            catch (Exception exc)
            {
               T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "defectShow");
            }
        }
       
        /// <summary>
        /// listView1显示、清空
        /// </summary>
        /// <param name="list"></param>
        /// <param name="isClear"></param>
        private void listViewShow(ListViewItem list,bool isClear)
        {
            try
            {
                if (listView1.InvokeRequired)
                {
                    showListView showView = new showListView(listViewShow);
                    listView1.BeginInvoke(showView, new object[] { list,isClear});
                }
                else
                {
                    if (isClear)
                        listView1.Items.Clear();
                    else
                        listView1.Items.Add(list);
                   
                }
            }
            catch (Exception exc)
            {
                T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "listViewShow");
            }
 
        }

        /// <summary>
        /// pictureBox刷新
        /// </summary>
        private void pictureBoxShow()
        {
            try
            {
                if (pictureBox2.InvokeRequired)
                {
                    picBoxShow picShow = new picBoxShow(pictureBoxShow);
                    pictureBox2.BeginInvoke(picShow);
                }
                else
                {
                    //刷新
                    pictureBox2.Refresh();
                }
            }
            catch (Exception exc)
            {
                T.queueIn(DateTime.Now.ToString("yy-MM-dd HH-mm-ss f") + exc.ToString() + "pictureBoxShow");
            }
        }

        private void textBoxAllNumShow(int num)
        {
            try
            {
                if (tboxAllNum.InvokeRequired)
                {
                    tboxAllNumShow boxshow = new tboxAllNumShow(textBoxAllNumShow);
                    tboxAllNum.BeginInvoke(boxshow, new object[] { num });
                }
                else
                {
                    tboxAllNum.Text = num.ToString();

                }
            }
            catch (Exception exc)
            {
                T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "textBoxAllNumShow");
            }
        }

        private void textBoxOKShow(int num)
        {
            try
            {
                if (tboxOKNum.InvokeRequired)
                {
                    tboxOKShow boxshow = new tboxOKShow(textBoxOKShow);
                    tboxOKNum.BeginInvoke(boxshow, new object[] { num });
                }
                else
                {
                    tboxOKNum.Text = num.ToString();

                }
            }
            catch (Exception exc)
            {
                T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "textBoxOKShow");
            }
        }

        private void textBoxOKProportionShow(double num)
        {
            try
            {
                if (tboxOKProportion.InvokeRequired)
                {
                    tboxOKProportionShow boxshow = new tboxOKProportionShow(textBoxOKProportionShow);
                    tboxOKProportion.BeginInvoke(boxshow, new object[] { num });
                }
                else
                {
                    tboxOKProportion.Text = num.ToString();

                }
            }
            catch (Exception exc)
            {
                T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "textBoxOKProportionShow");
            }
        }
        /// <summary>
        /// 左窗体图像处理资源释放
        /// </summary>
        public void ImgDispose()
        {
            try
            {
                if (glass != null)
                {
                    glass.Dispose();
                    glass = null;
                }
                if (imgRed != null)
                {
                    imgRed.Dispose();
                    imgRed = null;
                }
                if (imgGreen != null)
                {
                    imgGreen.Dispose();
                    imgGreen = null;
                }
                //if (imgBlue != null)
                //{
                //    imgBlue.Dispose();
                //    imgBlue = null;
                //}
                if (hImgLeft != null)
                {
                    hImgLeft.Dispose();
                    hImgLeft = null;
                }
            }
            catch (Exception exc)
            {
                T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "ImgDispose");
            }
           
        }

        /// <summary>
        /// 右窗体图像处理资源释放
        /// </summary>
        public void ImgDisposeR()
        {
            try
            {
                if (glassR != null)
                {
                    glassR.Dispose();
                    glassR = null;
                }
                if (imgRedR != null)
                {
                    imgRedR.Dispose();
                    imgRedR = null;
                }
                if (imgGreenR != null)
                {
                    imgGreenR.Dispose();
                    imgGreenR = null;
                }
                //if (imgBlueR != null)
                //{
                //    imgBlueR.Dispose();
                //    imgBlueR = null;
                //}
                if (hImgRight != null)
                {
                    hImgRight.Dispose();
                    hImgRight = null;
                }
            }
            catch (Exception exc)
            {
                T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "ImgDisposeR");
            }
         
        }

        /// <summary>
        /// 委托显示（目前不使用）
        /// </summary>
        /// <param name="text"></param>
        public void msgShow(string text)
        {
            try
            {
                if (this.richTextBox1.InvokeRequired)
                {
                    SetMsgCallBack msgshow = new SetMsgCallBack(msgShow);
                    this.Invoke(msgshow, new object[] { text });
                }
                else
                {
                    this.richTextBox1.AppendText(text + '\r');
                }
            }
            catch (Exception exc)
            {
                T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + " msgShow");
            }
        }
      
        /// <summary>
        /// 加载左相机参数
        /// </summary>
        public void CameraSet()
        {
            try
            {
                mycamera.OpenCam(0);
            }
            catch (Exception exc)
            {
                T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + " CameraSet");
            }
        }

        /// <summary>
        /// 启动左相机图像抓取;
        /// </summary>
        public void CameraStart()
        {
            try
            {
                mycamera.GrabImage(true);
            }
            catch (Exception exc)
            {
                T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "CameraStart");
            }
        }

        /// <summary>
        /// 停止左相机图像抓取;
        /// </summary>
        public void CameraStop()
        {
            try
            {
                mycamera.Stop(true);
            }
            catch (Exception exc)
            {
                T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "CameraStop");
            }
        }

        /// <summary>
        /// 加载右相机参数
        /// </summary>
        public void CameraRSet()
        {
            try
            {
                mycameraR.OpenCam(1);//加载右相机参数
            }
            catch (Exception exc)
            {
                T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "CameraRSet");
            }
        }

        /// <summary>
        /// 启动右相机图像抓取;
        /// </summary>
        public void CameraRStart()
        {
            try
            {
                mycameraR.GrabImage(false);//启动图像抓取;
            }
            catch (Exception exc)
            {
                T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "CameraRStart");
            }
        }

        /// <summary>
        /// 停止右相机图像抓取;
        /// </summary>
        public void CameraRStop()
        {
            try
            {
                mycameraR.Stop(false);//停止图像抓取;
            }
            catch (Exception exc)
            {
                T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "CameraRStop");
            }
        }

        /// <summary>
        /// 启动udp服务器
        /// </summary>
        private void initalUDP()
        {
            try
            {
                comm.wireControlIp = systemP.wireControlIp;
                comm.wireControlPort = systemP.wireControlport;
                comm.SetPara();
            }
            catch (Exception exc)
            {
                T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "initalUDP");
            }
        }
        
        /// <summary>
        /// 定时器每隔100ms刷新，将信息显示到richTextBox1
        /// </summary>
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (T.getCount() != 0)
                {
                    string s = T.queueOut();
                    if (ii < 40)
                    {
                        ii++;
                        richTextBox1.AppendText(s + "\r\n");
                        txtFile.addTextfile(s);
                    }
                    else
                    {
                        ii = 0;
                        richTextBox1.Text = s + "\r\n";
                        txtFile.txtfile(s);
                    }
                    timer1.Start();
                }
            }
            catch (Exception exc)
            {
                T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "timer1_Tick");
            }
        }

        /// <summary>
        /// 生成报表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemReport_Click(object sender, EventArgs e)
        {
            try
            {
                Thread th1 = new Thread(showReportDialog);
                th1.IsBackground = true;
                th1.Start();
            }
            catch (Exception exc)
            {
                T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "MenuItemReport_Click");
            }
        }
        /// <summary>
        /// 显示报表窗体
        /// </summary>
        private void showReportDialog()
        {
            try
            {
                Report rpt = new Report();
                rpt.ShowDialog();
            }
            catch (Exception exc)
            {
                T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "showReportDialog");
            }
        }

        /// <summary>
        /// 窗口移动重新绘制缺陷
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hwcLeft_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (Model == "manual")
                {
                    ////surfdetect.drawDefect(hwcLeft, ht, leftNum);
                    //坐标信息
                    ////HTuple X = new HTuple();
                    ////HTuple Y = new HTuple();
                    ////HTuple row = new HTuple();
                    ////HTuple col = new HTuple();
                    ////HTuple grayval = new HTuple();
                    ////HTuple hv_Width = null, hv_Height = null;
                    ////HObject ho_img = null;
                    ////ho_img = imgBlue;
                    ////HOperatorSet.GetImageSize(ho_img, out hv_Width, out hv_Height);
                    ////int row1 = e.Y / hwcLeft.Height *hv_Height;
                    ////int col1 = e.X / hwcLeft.Width *hv_Width;
                    ////if (col >= 0 && row >= 0)
                    ////{
                    ////    HOperatorSet.GetGrayval(ho_img, col, row, out grayval);
                    ////    tsTboxX.Text = col.D.ToString();
                    ////    tsTboxY.Text = row.D.ToString();
                    ////    tsTboxGrayValue.Text = grayval.D.ToString();
                    ////}

                }
            }
            catch (Exception exc)
            {
               T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc + "hwcLeft_MouseMove");
            }
        }

        /// <summary>
        /// 左相机调试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsMenuItemLeftCam_Click(object sender, EventArgs e)
        {
            try
            {
                if (tCameraSet != null)
                {
                    tCameraSet.Abort();
                    tCameraSet.DisableComObjectEagerCleanup();
                    tCameraSet = null;
                }
                tCameraSet = new Thread(CameraStart);
                tCameraSet.IsBackground = true;
                tCameraSet.Start();
            }
            catch (Exception exc)
            {
                T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "tsMenuItemLeftCam_Click");
            }
        }

        /// <summary>
        /// 右相机调试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsMenuItemCamRight_Click(object sender, EventArgs e)
        {
            try
            {
                if (tCameraSet != null)
                {
                    tCameraSet.Abort();
                    tCameraSet.DisableComObjectEagerCleanup();
                    tCameraSet = null;
                }
                tCameraSet = new Thread(CameraRStart);
                tCameraSet.IsBackground = true;
                tCameraSet.Start();
            }
            catch (Exception exc)
            {
                T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "tsMenuItemCamRight_Click");
            }
        }

        /// <summary>
        /// udp调试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsMenuItemCommunication_Click(object sender, EventArgs e)
        {
            try
            {
                comm.ShowDialog();
            }
            catch (Exception exc)
            {
                T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "tsMenuItemCommunication_Click");
            }
        }

        /// <summary>
        /// 参数设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsButtonParaSet_Click(object sender, EventArgs e)
        {
            try
            {
                SystemSetting setting = new SystemSetting();
                setting.ShowDialog();
            }
            catch (Exception exc)
            {
                T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "tsButtonParaSet_Click");
            }
        }

        /// <summary>
        /// 系统登录、退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsMenuItemLogin_Click(object sender, EventArgs e)
        {
            try
            {
                Login login = new Login(this);
                login.ShowDialog();
            }
            catch (Exception exc)
            {
                T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "tsMenuItemLogin_Click");
            }

        }

        /// <summary>
        /// 绘制玻璃区域
        /// </summary>
        /// <param name="realWidth">玻璃图像宽度 </param>
        /// <param name="realHeight">玻璃图像高度</param>
        private void draw(int pixelWidth,int pixelHeight,double mmpix,double k)
        {
            try
            {
                //定义画布大小
                //假设画布代表实际大小为880mm * 400mm
                canvasWidth = 880;
                canvasHeight = 400;
                //玻璃在画布中尺寸
                float realGlassWidth = (float)(pixelWidth * mmpix * k);
                float realGlassHeight = (float)(pixelHeight * mmpix * k);

                ////创建位图
                if (imageBmp != null)
                {
                    imageBmp.Dispose();
                    imageBmp = null;
                }
                imageBmp = new System.Drawing.Bitmap(canvasWidth, canvasHeight);
                //m_bmp = (Bitmap)imageBmp.Clone();
                //创建Graphics类对象
                Graphics g = Graphics.FromImage(imageBmp);
                //清空图片背景色
                g.Clear(Color.White);
                Font font = new System.Drawing.Font("Arial", 9, FontStyle.Regular);
                Font font1 = new System.Drawing.Font("宋体", 20, FontStyle.Regular);
                Font font2 = new System.Drawing.Font("Arial", 8, FontStyle.Regular);

                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, imageBmp.Width, imageBmp.Height), Color.WhiteSmoke, Color.LightGray, 1.2f, true);
                g.FillRectangle(Brushes.Gray, 0, 0, canvasWidth, canvasHeight);
                //Brush brush = new SolidBrush(Color.WhiteSmoke);
                Brush brush2 = new SolidBrush(Color.SaddleBrown);

                //画玻璃图像
                //g.FillRectangle(Brushes.LightBlue, 1250 - realWidth / 2, 600 - realHeight / 2, realWidth, realHeight);
                g.FillRectangle(Brushes.LightBlue, 440 - realGlassWidth / 2, 200 - realGlassHeight / 2, realGlassWidth, realGlassHeight);
                System.Drawing.Pen mypen = new Pen(brush, 1);

                ////绘制纵向线条
                int x = 0;
                for (int i = 0; i < canvasWidth / 8; i++)
                {
                    g.DrawLine(mypen, x, 0, x, canvasHeight);
                    x = x + canvasWidth / 8;
                }
                //绘制横向线条
                int y = 0;
                for (int i = 0; i < canvasHeight / 6; i++)
                {
                    g.DrawLine(mypen, 0, y, canvasWidth, y);
                    y = y + canvasHeight / 6;
                }

                g.Dispose();
            }
            catch (Exception exc)
            {
                T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "draw");
            }
        }

        /// <summary>
        /// 更新textBox
        /// </summary>
        private void updateTextBox()
        {
            try
            {
                if (leftNum > 0 || rightNum > 0)
                    glassNG++;
                else
                    glassOK++;
                glassCount++;
                textBoxAllNumShow((int)glassCount);
                textBoxOKShow((int)glassOK);

                double proportion = Math.Round(glassOK / glassCount, 2);
                textBoxOKProportionShow(proportion);
            }
            catch (Exception exc)
            {
                T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "updateTextBox");
            }
        }

        /// <summary>
        /// 更新折线图
        /// </summary>
        private void updateChart()
        {
            try
            {
                if (chart1.InvokeRequired)
                {
                    chatShow chartshow = new chatShow(updateChart);
                    chart1.BeginInvoke(chartshow);
                }
                else
                {
                    now = DateTime.Now.ToString("HH-mm-ss-f");
                    tyData.Add(ra.Next(0, 100));
                    txData.Add(now);
                    chart1.Series[0].Points.DataBindXY(txData, tyData);

                }
            }
            catch (Exception exc)
            {
                T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "updateChart");
            }
        }
        /// <summary>
        /// 参数设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsMenuItemSetting_Click(object sender, EventArgs e)
        {
            try
            {
                SystemSetting setting = new SystemSetting();
                setting.ShowDialog();
            }
            catch (Exception exc)
            {
                T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "tsMenuItemSetting_Click");
            }

        }

       
        private void tsMenuItemClear_Click(object sender, EventArgs e)
        {
            try
            {
                glassCount = 0;
                glassNG = 0;
                glassOK = 0;
                textBoxAllNumShow((int)glassCount);
                textBoxOKShow((int)glassOK);
                textBoxOKProportionShow(0);
            }
            catch (Exception exc)
            {
                T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "tsMenuItemClear_Click");
            }
        }
        private bool MoveFlag = false;
        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Middle)
                {      //如果中键点下    初始化计算要用的临时数据
                    m_ptMouseDown = e.Location;
                    m_ptCanvasBuf = m_ptCanvas;
                    MoveFlag = true;
                }
                pictureBox2.Focus();
            }
            catch (Exception exc)
            {
                T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "pictureBox2_MouseDown");
            }
        }
        /// <summary>
        /// 重绘图像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                Graphics g = e.Graphics;
                g.TranslateTransform(m_ptCanvas.X, m_ptCanvas.Y);       //设置坐标偏移
                g.ScaleTransform(m_nScale, m_nScale);                   //设置缩放比
                if (imageBmp != null)
                    g.DrawImage(imageBmp, m_ptBmp);                            //绘制图像
                else
                    return;
                g.ResetTransform();                                     //重置坐标系
                Pen p = new Pen(Color.Cyan, 3);
                //g.DrawLine(p, 0, m_ptCanvas.Y, pictureBox2.Width, m_ptCanvas.Y);
                //g.DrawLine(p, m_ptCanvas.X, 0, m_ptCanvas.X, pictureBox2.Height);
                g.DrawLine(p, 1, 1, pictureBox2.Width, 1);//y坐标轴
                g.DrawLine(p, 1, 1, 1, pictureBox2.Height);//x坐标轴
                p.Dispose();
                #region//绘制网格线
                //float nIncrement = (50 * m_nScale);             //网格间的间隔 根据比例绘制
                //for (float x = m_ptCanvas.X; x > 0; x -= nIncrement)
                //    g.DrawLine(Pens.Cyan, x, 0, x, pictureBox1.Height);
                //for (float x = m_ptCanvas.X; x < pictureBox1.Width; x += nIncrement)
                //    g.DrawLine(Pens.Cyan, x, 0, x, pictureBox1.Height);
                //for (float y = m_ptCanvas.Y; y > 0; y -= nIncrement)
                //    g.DrawLine(Pens.Cyan, 0, y, pictureBox1.Width, y);
                //for (float y = m_ptCanvas.Y; y < pictureBox1.Width; y += nIncrement)
                //    g.DrawLine(Pens.Cyan, 0, y, pictureBox1.Width, y);
                #endregion
                //计算屏幕左上角 和 右下角 对应画布上的坐标 ,//画布在屏幕上的位置
                Size szTemp = pictureBox2.Size - (Size)m_ptCanvas;
                PointF ptCanvasOnShowRectLT = new PointF(
                    -m_ptCanvas.X / m_nScale, -m_ptCanvas.Y / m_nScale);
                PointF ptCanvasOnShowRectRB = new PointF(
                    szTemp.Width / m_nScale, szTemp.Height / m_nScale);
                //显示文字信息
                string strDraw = "Scale: " + m_nScale.ToString("F1") +
                    "\nOrigin: " + m_ptCanvas.ToString() +
                    "\nLT: " + Point.Round(ptCanvasOnShowRectLT).ToString() +
                    "\nRB: " + Point.Round(ptCanvasOnShowRectRB).ToString() +
                    "\n" + ((Size)Point.Round(ptCanvasOnShowRectRB)
                    - (Size)Point.Round(ptCanvasOnShowRectLT)).ToString();
                Size strSize = TextRenderer.MeasureText(strDraw, this.Font);
                //绘制文字信息
                SolidBrush sb = new SolidBrush(Color.FromArgb(125, 0, 0, 0));
                g.FillRectangle(sb, 0, 0, strSize.Width, strSize.Height);
                g.DrawString(strDraw, this.Font, Brushes.Yellow, 0, 0);
                strSize = TextRenderer.MeasureText(m_strMousePt, this.Font);

                g.FillRectangle(sb, pictureBox2.Width - strSize.Width, 0, strSize.Width, strSize.Height);
                g.DrawString(m_strMousePt, this.Font, Brushes.Yellow, pictureBox2.Width - strSize.Width, 0);
                sb.Dispose();
            }
            catch (Exception exc)
            {
                T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "pictureBox2_Paint");
            }
        }

        /// <summary>
        /// 缩放图像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox2_MouseWheel(object sender, MouseEventArgs e)
        {
            try
            {
                if (m_nScale <= 0.3 && e.Delta <= 0) return;        //缩小下线
                if (m_nScale >= 8.9 && e.Delta >= 0) return;        //放大上线
                //获取 当前点到画布坐标原点的距离
                SizeF szSub = (Size)m_ptCanvas - (Size)e.Location;
                //当前的距离差除以缩放比还原到未缩放长度
                float tempX = szSub.Width / m_nScale;           //这里
                float tempY = szSub.Height / m_nScale;          //将画布比例
                //还原上一次的偏移                               //按照当前缩放比还原到
                m_ptCanvas.X -= (int)(szSub.Width - tempX);     //没有缩放
                m_ptCanvas.Y -= (int)(szSub.Height - tempY);    //的状态
                //重置距离差为  未缩放状态                       
                szSub.Width = tempX;
                szSub.Height = tempY;
                m_nScale += e.Delta > 0 ? 0.2F : -0.2F;
                //重新计算 缩放并 重置画布原点坐标
                m_ptCanvas.X += (int)(szSub.Width * m_nScale - szSub.Width);
                m_ptCanvas.Y += (int)(szSub.Height * m_nScale - szSub.Height);
                pictureBox2.Invalidate();
            }
            catch (Exception exc)
            {
                T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "pictureBox2_MouseWheel");
            }

        }
        SizeF szSub = new SizeF();//鼠标当前点对应画布中的坐标
        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Middle)
                {
                    m_ptCanvas = (Point)((Size)m_ptCanvasBuf + ((Size)e.Location - (Size)m_ptMouseDown));
                    pictureBox2.Invalidate();
                }
                //计算 右上角显示的坐标信息
                szSub = (Size)e.Location - (Size)m_ptCanvas;  //计算鼠标当前点对应画布中的坐标
                //System.Windows.Forms.MessageBox.Show(szSub.ToPointF().X + ":" + szSub.ToPointF().Y);
                szSub.Width /= m_nScale;
                szSub.Height /= m_nScale;
                Size sz = TextRenderer.MeasureText(m_strMousePt, this.Font);    //获取上一次的区域并重绘
                pictureBox2.Invalidate(new Rectangle(pictureBox2.Width - sz.Width, 0, sz.Width, sz.Height));
                PointF tmp = new PointF((szSub.ToPointF().X - orgX) / k / systemP.mmPerPix, (szSub.ToPointF().Y - orgY) / k / systemP.mmPerPix);
                m_strMousePt = e.Location.ToString() + "\n" + ((Point)(szSub.ToSize())).ToString() + "\n" + tmp;
                sz = TextRenderer.MeasureText(m_strMousePt, this.Font);         //绘制新的区域
                pictureBox2.Invalidate(new Rectangle(pictureBox2.Width - sz.Width, 0, sz.Width, sz.Height));
            }
            catch (Exception exc)
            {
                T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "pictureBox2_MouseMove");
            }
        }


        HObject imgPart = null;
        
        HTuple rMiddle = null, cMiddle = null;//将图像的实际坐标转换在画布上，绘制玻璃分割线
       
        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                HTuple r = null, c = null;//鼠标选中区域的中心点  
                HTuple r1 = null, c1 = null, r2 = null, c2 = null;
                SizeF szSubIsleft = new SizeF();
                //r,c为缺陷在玻璃的中的位置,已换算为pixel
                r = Math.Round((szSub.ToPointF().Y - orgY) / k / systemP.mmPerPix, 2);
                c = Math.Round((szSub.ToPointF().X - orgX) / k / systemP.mmPerPix, 2);
               

                r1 = r - 50;
                c1 = c - 50;
                r2 = r + 50;
                c2 = c + 50;
                if (r1 < 1) r1 = 1;
                if (c1 < 1) c1 = 1;
                if (c2 < 1) c2 = 1;
                if (r2 < 1) r2 = 1;
               
                if (imgBlue != null && (!processingL || !processingR) && !MoveFlag)
                {  
                    if(c > 7296)
                        HOperatorSet.CropRectangle1(imgBlue, out imgPart, r1, c1 - 7296, r2, c2-7296);
                    else
                        HOperatorSet.CropRectangle1(imgBlue, out imgPart, r1, c1, r2, c2);
                    //HOperatorSet.WriteImage(imgPart, "jpeg", 0, "E:\\aaa.jpg");
                    ShowCutImage showimg = new ShowCutImage(imgPart);
                    showimg.ShowDialog();
                }
                
                //if (!processingL && !processingR)
                //{

                if (lstItm != null && !MoveFlag)
                {
                    
                    for (int i = 0; i < lstItm.Count; i++)
                    {
                        DefectFeature tmp = (DefectFeature)lstItm[i];
                        double c_data = tmp.col;
                        double r_data = tmp.row;
                      
                        if (c_data > c - 30 && c_data < c + 30 && r_data > r - 30 && r_data < r + 30)
                        {
                            listView1.Items[i].BackColor = Color.Red;
                        }              
                        continue;
                    }
                  
                }
                MoveFlag = false;
            }
            catch (Exception exc)
            {
                T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "pictureBox2_MouseUp");
            }
         
        }

        private void tsMenuItemExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

      
        double[] type = new double[6];
        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                HOperatorSet.GenEmptyObj(out imgPart);
                //ShowCutImage defectShow = new ShowCutImage(imgBlue);
                if (listView1.SelectedItems.Count == 0) return;
                else
                {
                    for (int i = 0; i < 6; i++)
                    {
                        type[i] = double.Parse(listView1.SelectedItems[0].SubItems[i+3].Text);
                    }
                }
              
                
                double row1 = type[5] - type[3] / 2 - 20;
                double col1 = type[4] - type[2] / 2 - 20;
                double row2 = type[5] + type[3] / 2 + 20;
                double col2 = type[4] + type[2] / 2 + 20;

                if (col1 < 0) col1 = 0;
                if (col2 < 0) col2 = 0;
                
                if (col1 > 7296) col1 -= 7296;
                if (col2 > 7296) col2 -= 7296;
               
                 HOperatorSet.CropRectangle1(imgBlue, out imgPart, row1, col1, row2, col2);
                 ShowCutImage defectShow = new ShowCutImage(imgPart);
                 defectShow.ShowDialog();
              
                

                //保存缺陷图
                //if (defectShow.chBSaveDefectImg.Checked)
                //{
                //    string fileName = DateTime.Now.ToString("hh-mm-ss");
                //    halcon.SaveImage(imgPart, filePath, fileName,0);
                //   // ShareImageFile = filePath + fileName;                    
                //}
                //defectShow.ShowDialog();//显示窗体;
                //propertyGrid1Show("表面检测");//显示特征值;
            }
            catch (Exception exc)
            {
               T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "listView1_MouseClick");
            }
        }
        private ListViewColumnSorter lvwColumnSorter;
        private void listView1_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            try
            {
                this.listView1.ListViewItemSorter = lvwColumnSorter;
                if (e.Column == lvwColumnSorter.SortColumn)
                {
                    // Reverse the current sort direction for this column.
                    if (lvwColumnSorter.Order == System.Windows.Forms.SortOrder.Ascending)
                    {
                        lvwColumnSorter.Order = System.Windows.Forms.SortOrder.Descending;
                    }
                    else
                    {
                        lvwColumnSorter.Order = System.Windows.Forms.SortOrder.Ascending;
                    }
                }
                else
                {
                    // Set the column number that is to be sorted; default to ascending.
                    lvwColumnSorter.SortColumn = e.Column;
                    lvwColumnSorter.Order = System.Windows.Forms.SortOrder.Ascending;
                }

                // Perform the sort with these new sort options.
                this.listView1.Sort();
            }
            catch (Exception exc)
            {
                T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "listView1_ColumnClick");
            }

        }

      

    }


    public class ListViewColumnSorter : System.Collections.IComparer
    {
        private int ColumnToSort;
        private System.Windows.Forms.SortOrder OrderOfSort;
        private System.Collections.CaseInsensitiveComparer ObjectCompare;
        public ListViewColumnSorter()
        {
            // Initialize the column to '0'
            ColumnToSort = 1;
            // Initialize the sort order to 'none'
            OrderOfSort = System.Windows.Forms.SortOrder.None;
            // Initialize the CaseInsensitiveComparer object
            ObjectCompare = new System.Collections.CaseInsensitiveComparer();
        }

        public int Compare(object x, object y)
        {
            int compareResult = 0;
            ListViewItem listviewX, listviewY;
            listviewX = (ListViewItem)x;
            listviewY = (ListViewItem)y;

            // Compare the two items
            if (listviewX.SubItems[ColumnToSort].Text != "" && ColumnToSort > 1)
                compareResult = ObjectCompare.Compare(Convert.ToDouble(listviewX.SubItems[ColumnToSort].Text.ToString()), Convert.ToDouble(listviewY.SubItems[ColumnToSort].Text.ToString()));
            if (OrderOfSort == System.Windows.Forms.SortOrder.Ascending)
            {
                return compareResult;
            }
            else if (OrderOfSort == System.Windows.Forms.SortOrder.Descending)
            {
                return (-compareResult);
            }
            else
            {
                // Return '0' to indicate they are equal
                return 0;
            }
        }

        /// <summary>
        /// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
        /// </summary>
        public int SortColumn
        {
            set
            {
                ColumnToSort = value;
            }
            get
            {
                return ColumnToSort;
            }
        }

        /// <summary>
        /// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
        /// </summary>
        public System.Windows.Forms.SortOrder Order
        {
            set
            {
                OrderOfSort = value;
            }
            get
            {
                return OrderOfSort;
            }
        }

    }

   
}
