using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace DefectDetection
{
    public partial class Communication : Form
    {
        public UdpClient udpServer = null;
        public IPEndPoint ipEndPoint = null;
        public int ipPort;
        private static string hostIp;//本地Ip
        public string wireControlIp;//线控IP  
        public int wireControlPort;//线控Port
        private static string hostName;  //本地主机名
        private IPHostEntry ipentry;
        public WireControlInfo wireControlPara = new WireControlInfo();//线控通讯信息
        private string msg = null;
        private string Model = null;
        private bool udpReStart = false;
        public bool productInfoChanged = false;//生产信息是否更改
        //委托
        public delegate void ShowMessageEventHandler(Object sender, int []array);
        public event ShowMessageEventHandler ShowMessage;
        /// <summary>
        /// 委托，显示信息
        /// </summary>
        delegate void SetMsgCallBack(string text);
        public string msgInfo
        {
            set { msg = value; }
            get { return msg; }
        }
        public Communication()
        {

            InitializeComponent();
            hostName = Dns.GetHostName();
            ipentry = Dns.GetHostEntry(hostName);
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    hostIp = _IPAddress.ToString();//lhw 标记Win7系统，默认IP地址为IPv6，应找到IPv4;
                    tBoxServerIp.Text = hostIp;
                 
                }
            }
         
            
        }
        /* 远程端口与本地端口一致*/
        public void SetPara()
        {
            try
            {
                if (udpServer != null)
                {
                    udpServer.Close();
                    udpServer = null;
                }
                //本机节点
                //ipEndPoint = new IPEndPoint(IPAddress.Parse(hostIp), ipPort);
                //本地测试
                ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"),FrmMain.systemP.wireControlport);
                //绑定服务器的ip和端口号
                udpServer = new UdpClient(ipEndPoint);
                UdpState s = new UdpState(udpServer, ipEndPoint);
                udpServer.BeginReceive(EndReceive, s);
               
            }
            catch (Exception exc)
            {
                FrmMain.T.queueIn(DateTime.Now.ToString("yy-MM-dd HH-mm-ss f") + exc.ToString() + "SetPara");
            }
        }
        /// <summary>
        /// 异步接收
        /// </summary>
        /// <param name="ar"></param>
        public void EndReceive(IAsyncResult ar) 
        {
            try
            {
                if (Model == "test")
                    msgShow1(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff---") + "开始接收数据......");
                else
                {
                    //FrmMain.T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff---") + "开始接收数据......");
                }
                UdpState s = ar.AsyncState as UdpState;
                if (s != null)
                {
                    UdpClient udpClient = s.udpclient;
                    IPEndPoint ip = s.IP;
                    if(true)
                    {
                        Byte[] receiveBytes = udpClient.EndReceive(ar, ref ip);
                        int length = receiveBytes.Length / 4;
                        Int32[] array = new Int32[length];
                        bool N = ConvertByteAToIntA(receiveBytes, ref array);
                        if (!N)
                        {
                            FrmMain.T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff---") + "接收的数据错误");
                        }
                        else
                        {
                            if (Model == "test")
                                msgShow1(array[0].ToString() + ";" + array[1].ToString() + ";" + array[2].ToString() + ";" + array[3].ToString());
                            if (array[0] != 0 & array[1] != 0 & array[2] != 0 & array[3] != 0)
                            {
                                FrmMain.T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff---") + "数据接收成功：" + array[0].ToString() + ";" + array[1].ToString() + ";" + array[2].ToString() + ";" + array[3].ToString());
                                //FrmMain.frmMain.tBoxGlassId.Text = array[0].ToString();
                                if (array[1].ToString() != FrmMain.frmMain.tboxGlassLength.Text || array[2].ToString() != FrmMain.frmMain.tboxGlassWidth.Text || array[3].ToString() != FrmMain.frmMain.tboxGlassThick.Text)
                                {
                                    if(ShowMessage != null)//说明有人订阅了事件
                                    {
                                        ShowMessage(this,array);//将udp接收到的玻璃信息传递给FrmMain
                                    }   
                                    productInfoChanged = true;
                                }
                            }
                            else
                            {
                                FrmMain.T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff---") + "数据接收失败：");
                            }
                          

                            //Program.frmain.UDPStatue.Text = "通讯状态：数据已接收";
                            //Program.frmain.GlassId.Text = "当前玻璃ID:" + Convert.ToString(FloatPara.R_progressiveID);

                        }
                        array = null;//释放;

                    }

                    udpClient.BeginReceive(EndReceive, s);
                }
            }
            catch (Exception ex)
            {
                FrmMain.T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff---") + "数据接收错误:" + ex);
               // T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff---") + "数据接收错误:" + ex);
            }
        }
        /// <summary>
        /// 发送数据，从这里发送数据
        /// </summary>
        /// <param name="s"></param>
        /// <param name="S_Juggment"></param>
        public void udpSend(Int32[] s, Int16 S_Juggment)  
        {
            bool checksendStatue = false;
            try
            {
                byte[] sendData = new byte[4 * s.Length];
                byte[] sendData32 = new byte[4 * s.Length];
                byte[] sendData16 = new byte[2];

                ConvertInt32ToByte(s, ref sendData32);
             
                udpServer.Send(sendData32, sendData.Length, IPAddress.Parse(wireControlIp).ToString(),FrmMain.systemP.wireControlport);//给线控发送数据;
                if (Model == "test")
                {
                    msgShow2(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff---") + "\r" + s[0] + "\r" + s[1] + "\r" + s[2] + "\r" + s[3] + "\r" + "数据发送成功:");
                }
                else
                {
                    //FrmMain.T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff---") + "数据发送成功:");
                }
                checksendStatue = true;
            }
            catch (Exception ex)
            {
                FrmMain.T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff---") + "数据发送失败:" + ex);
            }
            if (checksendStatue)
            {
               // Program.frmain.UDPStatue.Text = "通讯状态：数据已发送";
            }
            else
            {
               // Program.frmain.UDPStatue.Text = "通讯状态：数据发送失败";
            }

        }
        private void btnStartServer_Click(object sender, EventArgs e)
        {
            Model = "test";
            wireControlIp = FrmMain.systemP.wireControlIp;
            wireControlPort = FrmMain.systemP.wireControlport;
            SetPara();
        }

       
        /// <summary>
        /// 先高低位掉头，然后转换
        /// </summary>
        /// <param name="m"></param>
        /// <param name="arry"></param>
        /// <returns></returns>
        static bool ConvertInt32ToByte(Int32[] m, ref byte[] arry)
        {
            try
            {
                if (arry == null) return false;
                if (arry.Length < 4 * m.Length) return false;
                for (int i = 0; i < m.Length; i++)
                {
                    arry[3 + 4 * i] = (byte)(m[i] & 0xFF);
                    arry[2 + 4 * i] = (byte)((m[i] & 0xFF00) >> 8);
                    arry[1 + 4 * i] = (byte)((m[i] & 0xFF0000) >> 16);
                    arry[0 + 4 * i] = (byte)((m[i] & 0xFF000000) >> 24);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 先高低位掉头，然后转换
        /// </summary>
        /// <param name="m"></param>
        /// <param name="arry"></param>
        /// <returns></returns>
        static bool ConvertInt16ToByte(Int16 m, ref byte[] arry)
        {
            try
            {
                if (arry == null) return false;
                arry[1] = (byte)(m & 0xFF);
                arry[0] = (byte)((m & 0xFF00) >> 8);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// byte[]型数组先高低位排序再转换成int32数组
        /// </summary>
        /// <param name="byteArray"></param>
        /// <param name="intArray"></param>
        /// <returns></returns>
        static bool ConvertByteAToIntA(byte[] byteArray, ref Int32[] intArray)
        {
            try
            {
                if (intArray == null) return false;
                byte[] newbyte = new byte[4];
                for (int i = 0; i < (byteArray.Length / 4); i++)
                {
                    newbyte[3] = byteArray[0 + 4 * i];
                    newbyte[2] = byteArray[1 + 4 * i];
                    newbyte[1] = byteArray[2 + 4 * i];
                    newbyte[0] = byteArray[3 + 4 * i];
                    intArray[i] = BitConverter.ToInt32(newbyte, 0);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void btnSeverSendData_Click(object sender, EventArgs e)
        {
            try
            {
                    //paramFromResult();  //发送数据赋值
                    Int32[] s = new Int32[4]{3117, 3118, 
                            10,1000};
                    Int16 S_Juggment = Convert.ToInt16(1);
                    udpSend(s, S_Juggment);
                    //udpConnect.FloatPara.S_flag = true;
                    FrmMain.T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff---") + "数据发送完成");
            }
            catch
            {
                FrmMain.T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff---") + "数据发送失败");
            }
        }

        private void btnClientSendData_Click(object sender, EventArgs e)
        {
            try
            {
                wireControlIp = FrmMain.systemP.wireControlIp;
                //paramFromResult();  //发送数据赋值
                Int32[] s = new Int32[4]{3117, 3118, 
                            10000,1000};
                Int16 S_Juggment = Convert.ToInt16(1);
                udpSend(s, S_Juggment);
                //udpConnect.FloatPara.S_flag = true;
                //T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff---") + "数据发送完成");
                //richTextBox2.AppendText(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff---") + "数据发送完成");

            }
            catch
            {
                FrmMain.T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff---") + "数据发送失败");
            }
        }
        private void msgShow1(string text)
        {
            if (this.richTextBox1.InvokeRequired)
            {
                SetMsgCallBack msgshow = new SetMsgCallBack(msgShow1);
                this.Invoke(msgshow, new Object[] { text });
            }
            else
            {
                this.richTextBox1.AppendText(text + "\r\n");
            }
        }
        private void msgShow2(string text)
        {
            if (this.richTextBox2.InvokeRequired)
            {
                SetMsgCallBack msgshow = new SetMsgCallBack(msgShow2);
                this.Invoke(msgshow, new Object[] { text });
            }
            else
            {
                this.richTextBox2.AppendText(text + "\r\n");
            }
        }

        private void Communication_Load(object sender, EventArgs e)
        {

        }
    }
    class UdpState
    {
        public UdpClient udpclient = null;
        public UdpClient UdpClient
        {
            get { return udpclient; }
        }
        public IPEndPoint ip;
        public IPEndPoint IP
        {
            get { return ip; }
        }
        public UdpState(UdpClient udpclient, IPEndPoint ip)
        {
            this.udpclient = udpclient;
            this.ip = ip;
        }
    }
}
