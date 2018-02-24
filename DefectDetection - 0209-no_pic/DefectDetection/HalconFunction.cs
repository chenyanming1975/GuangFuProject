using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HalconDotNet;
using System.IO;
namespace DefectDetection
{
    
    class HalconFunction
    {
        //public static HObject ho_AnalysisImage;

        // Local control variables 

        public HTuple hv_Value = null, hv_Value_row1 = null, hv_Value_column1 = null, hv_Value_row2 = null, hv_Value_column2 = null;
        public HTuple  hv_Value_width = null, hv_Value_height = null;
        private string msg = null;
        public string msgInfo
        {
            set { msg = value; }
            get { return msg; }
        }


        public void readImage(out HObject ho_Image, HTuple hv_ImagePath, HTuple hv_DefaultWinHandle)
        {
            setWindow(hv_DefaultWinHandle);
            HOperatorSet.GenEmptyObj(out ho_Image);
            ho_Image.Dispose();
            HOperatorSet.ReadImage(out ho_Image, hv_ImagePath);
            setWindow(hv_DefaultWinHandle);
            displayImageOnWindow(ho_Image, hv_DefaultWinHandle);
            return;
        }
        #region
        private void setWindow(HTuple hv_DefaultWinHandle)
        {

            HOperatorSet.SetDraw(hv_DefaultWinHandle, "margin");
            HOperatorSet.SetLineWidth(hv_DefaultWinHandle, 3);
        }
        #endregion

        public void displayImageOnWindow(HObject hoImage, HTuple hv_DefaultWinHandle)
        {
            HTuple hv_Width, hv_Height;
            HOperatorSet.GetImageSize(hoImage, out hv_Width, out hv_Height);
            HOperatorSet.SetPart(hv_DefaultWinHandle, 0, 0, hv_Height, hv_Width);
            HOperatorSet.DispObj(hoImage, hv_DefaultWinHandle);
            return;
        }
        public void displayCenterLine(HObject image, HTuple hv_DefaultWinHandle)
        {
            HTuple h, w;
            HObject line_x, line_y;
            HOperatorSet.SetColor(hv_DefaultWinHandle, "blue");
            HOperatorSet.GetImageSize(image, out w, out h);
            HOperatorSet.GenRegionLine(out line_x, h / 2, w / w, h / 2, w);
            HOperatorSet.GenRegionLine(out line_y, 0, w / 2, h, w / 2);
            HOperatorSet.DispObj(line_x, hv_DefaultWinHandle);
            HOperatorSet.DispObj(line_y, hv_DefaultWinHandle);
        }
        //Model = 1 代表异常图片; Model = 0 代表普通存图;
        public void SaveImage(HObject img, string filePath, string fileName,int Model)
        {
            try
            {
                string mpathErr = "";
                DirectoryInfo folider;
                mpathErr = CreatDifo(filePath);
                folider = new DirectoryInfo(mpathErr);
                FileSystemInfo[] fileInfoSys = folider.GetFileSystemInfos();
                if (fileInfoSys.Length < 20 && (Model == 1))//Model=1表示存储异常图片
                {
                    //bug未解决
                   // HOperatorSet.WriteImage(img, "tiff", 0, mpathErr + "/" + fileName + ".tif");
                }
                else if (fileInfoSys.Length >= 20 && (Model == 1))//异常图片存储超过50片则，删除后重新存储
                {
                    folider.Delete(true);
                    HOperatorSet.WriteImage(img, "tiff", 0, mpathErr + "/" + fileName + ".tif");
                }
                else //正常运行情况下保存图像
                {
                    HOperatorSet.WriteImage(img, "tiff", 0, filePath + fileName + ".tif");
                }
            }
            catch(Exception exc)
            {
                msg = DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "SaveImage";
               // FrmMain.T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + "异常图像太多");
            }
        }
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string CreatDifo(string filePath)
        {
            string configpath = filePath + DateTime.Now.ToString("yyyyMMdd");
            System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(configpath);
            if (!dirInfo.Exists)
            {
                System.IO.DirectoryInfo DInfo = new System.IO.DirectoryInfo(configpath);
                DInfo.Create();
            }
            return configpath;
        }

      //  public void HalconCutEdge(HWindowControl viewPort, HObject MyImage)
      //  {
      //      try
      //      {
      //          // Initialize local and output iconic variables 
      //          HTuple number;
      //          HOperatorSet.GenEmptyObj(out ho_ImageGauss);
      //          HOperatorSet.GenEmptyObj(out ho_EdgeAmplitude);
      //          if (ho_ImageGauss != null)
      //          {
      //              ho_ImageGauss.Dispose();
      //              ho_ImageGauss = null;
      //          }
      //          HOperatorSet.GaussFilter(MyImage, out ho_ImageGauss, 3);
      //          //sobel_amp (ImageGauss, EdgeAmplitude, 'sum_abs', 5)
      //          if (ho_EdgeAmplitude != null)
      //          {
      //              ho_EdgeAmplitude.Dispose();
      //              ho_EdgeAmplitude = null;
      //          }
      //          HOperatorSet.SobelAmp(ho_ImageGauss, out ho_EdgeAmplitude, "sum_abs", 5);

      //          HOperatorSet.GenEmptyObj(out ho_Region);
      //          HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
      //          HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
      //          HOperatorSet.GenEmptyObj(out ho_SelectedRegions1);
      //          HOperatorSet.GenEmptyObj(out ho_ImagePartCut);
      //          if (ho_Region != null)
      //          {
      //              ho_Region.Dispose();
      //              ho_Region = null;
      //          }
      //          HOperatorSet.Threshold(ho_EdgeAmplitude, out ho_Region, FrmMain.edgePara.CutEdgeThreshold_Min, FrmMain.edgePara.CutEdgeThreshold_Max);
      //          if (ho_ConnectedRegions != null)
      //          {
      //              ho_ConnectedRegions.Dispose();
      //              ho_ConnectedRegions = null;
      //          }
      //          HOperatorSet.Connection(ho_Region, out ho_ConnectedRegions);
      //          if (ho_RegionFillUp != null)
      //          {
      //              ho_RegionFillUp.Dispose();
      //              ho_RegionFillUp = null;
      //          }
      //          HOperatorSet.FillUp(ho_ConnectedRegions, out ho_RegionFillUp);
      //          if (hv_Area != null)
      //          {
      //              hv_Area = null;
      //              hv_Row = null;
      //              hv_Column = null;
      //          }
      //          HOperatorSet.AreaCenter(ho_RegionFillUp, out hv_Area, out hv_Row, out hv_Column);
      //          if (ho_SelectedRegions != null)
      //          {
      //              ho_SelectedRegions.Dispose();
      //              ho_SelectedRegions = null;
      //          }
      //          HOperatorSet.SelectShape(ho_RegionFillUp, out ho_SelectedRegions, "area", "and",
      //     hv_Area.TupleMax(), 9999999999);
      //          //if (ho_SelectedRegions != null)
      //          //{
      //          //    ho_SelectedRegions.Dispose();
      //          //    ho_SelectedRegions = null;
      //          //}
      //          //if (ho_SelectedRegions1 != null)
      //          //{
      //          //    ho_SelectedRegions1.Dispose();
      //          //    ho_SelectedRegions1 = null;
      //          //}
      //          //HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area",
      //          //    "and",90000, 9999999999);
      //          //HOperatorSet.SelectShape(ho_SelectedRegions, out ho_SelectedRegions1, "rb",
      //          //    "and", 20, 9999999999); //防止3号机最左边的噪声影响
      //          HOperatorSet.CountObj(ho_SelectedRegions, out number);
      //          if (number != 0)
      //          {
      //             // HOperatorSet.SortRegion(ho_SelectedRegions1, out ho_SortRegions, "upper_left", "true", "row");
      //              HOperatorSet.SelectObj(ho_SelectedRegions, out ho_SelectObject, 1);
      //              //HTuple num;
      //              //HOperatorSet.CountObj(ho_SelectedRegions, out num);
      //              HOperatorSet.RegionFeatures(ho_SelectObject, "row1", out hv_Value_row1);
      //              HOperatorSet.RegionFeatures(ho_SelectObject, "column1", out hv_Value_column1);
      //              HOperatorSet.RegionFeatures(ho_SelectObject, "row2", out hv_Value_row2);
      //              HOperatorSet.RegionFeatures(ho_SelectObject, "column2", out hv_Value_column2);
      //              HOperatorSet.RegionFeatures(ho_SelectObject, "width", out hv_Value_width);
      //              HOperatorSet.RegionFeatures(ho_SelectObject, "height", out hv_Value_height);

      ////              HOperatorSet.SmallestRectangle1(ho_SelectObject, out hv_Value_row1, out hv_Value_column1,
      ////out hv_Value_row2, out hv_Value_column2);
      //              FrmMain.glassWidth = hv_Value_width;//保留玻璃宽度，发给1号机时，如果是4号机，则加上宽度;
      //              //row代表y坐标，column代表x坐标
      //              //hv_x = (hv_Value_column - (hv_Value_width / 2)) - 50;
      //              //hv_y = (hv_Value_row - (hv_Value_height / 2)) - 50;
      //              //hv_width = hv_Value_width + 100;
      //              //hv_height = hv_Value_height + 500;
      //              if (ho_ImagePartCut != null)
      //              {
      //                  ho_ImagePartCut.Dispose();
      //                  ho_ImagePartCut = null;
      //              }
      //              if (hv_Value_row1 > 0)
      //              { 
      //              hv_Value_row1 = hv_Value_row1 - 20;
      //              }
      //              if (hv_Value_column1 > 0)
      //              { 
      //               hv_Value_column1 = hv_Value_column1 - 20;
      //              }
                   
      //             // hv_Value_width = hv_Value_width + 50;
      //             // hv_Value_height = hv_Value_height + 50;
      //              HOperatorSet.CropPart(ho_EdgeAmplitude, out ho_ImagePartCut, hv_Value_row1, hv_Value_column1, hv_Value_width, hv_Value_height);
      //              //HOperatorSet.WriteImage(ho_ImagePartCut, "bmp", 0, "E:\\ImagePartCut.bmp");
      //              //  ho_EdgeAmplitude.DispObj(viewPort.HalconWindow);//不显示图像
      //              // ho_AnalysisImage = Form1.ho_ImagePart;
      //          }
      //          else
      //          {
      //              ho_ImagePartCut = null;
      //             ////System.Windows.Forms.MessageBox.Show("玻璃采集异常");
      //          }

      //      }
      //      catch (Exception e)
      //      {
      //          FrmMain.T.queueIn(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + e.Message + "HalconCutEdge");
      //      }
      //  }
       // public void HalconBorkenEdge(HWindowControl viewPort, HObject MyImage)
       // {
       //     // Initialize local and output iconic variables   
       //     HTuple number;
       //     if (ho_Image != null)
       //     {
       //         ho_Image.Dispose();
       //         ho_Image = null;
       //     }
            
       //     HOperatorSet.GenEmptyObj(out ho_Region);
       //     HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
       //     HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
       //     HOperatorSet.GenEmptyObj(out ho_SelectedRegions1);
       //     HOperatorSet.GenEmptyObj(out ho_ImagePartBroken);
       //     if (ho_Region != null)
       //     {
       //         ho_Region.Dispose();
       //         ho_Region = null;
       //     }
       //     HOperatorSet.Threshold(MyImage, out ho_Region, FrmMain.edgePara.BrokenEdgeThreshold_Min, FrmMain.edgePara.BrokenEdgeThreshold_Max);
       //     if (ho_ConnectedRegions != null)
       //     {
       //         ho_ConnectedRegions.Dispose();
       //         ho_ConnectedRegions = null;
       //     }
       //     HOperatorSet.Connection(ho_Region, out ho_ConnectedRegions);
       //     if (ho_SelectedRegions != null)
       //     {
       //         ho_SelectedRegions.Dispose();
       //         ho_SelectedRegions = null;
       //     }
       //     if (ho_RegionFillUp != null)
       //     {
       //         ho_RegionFillUp.Dispose();
       //         ho_RegionFillUp = null;
       //     }
       //     HOperatorSet.FillUp(ho_ConnectedRegions, out ho_RegionFillUp);
       //     if (hv_Area != null)
       //     {
       //         hv_Area = null;
       //         hv_Row = null;
       //         hv_Column = null;
       //     }
       //     HOperatorSet.AreaCenter(ho_RegionFillUp, out hv_Area, out hv_Row, out hv_Column);
       //     if (ho_SelectedRegions != null)
       //     {
       //         ho_SelectedRegions.Dispose();
       //         ho_SelectedRegions = null;
       //     }
       //     HOperatorSet.SelectShape(ho_RegionFillUp, out ho_SelectedRegions, "area", "and",
       //hv_Area.TupleMax(), 9999999999);
       //     //if (ho_SelectedRegions1 != null)
       //     //{
       //     //    ho_SelectedRegions1.Dispose();
       //     //    ho_SelectedRegions1 = null;
       //     //}
       //     //HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area",
       //     //    "and", 90000, 9999999999);
       //     //HOperatorSet.SelectShape(ho_SelectedRegions, out ho_SelectedRegions1, "rb",
       //     //      "and", 20, 9999999999); //防止3号机最左边的噪声影响
       //     HOperatorSet.CountObj(ho_SelectedRegions,out number);
       //     if (number == 1)
       //     {
       //        // HOperatorSet.SortRegion(ho_SelectedRegions, out ho_SortRegions, "upper_left", "true", "row");
       //         HOperatorSet.SelectObj(ho_SelectedRegions, out ho_SelectObject, 1);
       //         HTuple hv_row1 = null, hv_column1 = null, hv_row2 = null, hv_column2 = null;
       //         HOperatorSet.RegionFeatures(ho_SelectObject, "row1", out hv_row1);
       //         HOperatorSet.RegionFeatures(ho_SelectObject, "column1", out hv_column1);
       //         HOperatorSet.RegionFeatures(ho_SelectObject, "width", out hv_row2);
       //         HOperatorSet.RegionFeatures(ho_SelectObject, "height", out hv_column2);

       //         if (ho_ImagePartBroken != null)
       //         {
       //             ho_ImagePartBroken.Dispose();
       //             ho_ImagePartBroken = null;
       //         }
       //         if (hv_row1 > 0)
       //         {
       //             hv_row1 = hv_row1 - 20;
       //         }
       //         if (hv_column1 > 0)
       //         {
       //             hv_column1 = hv_column1 - 20;
       //         }
       //        // hv_row2 = hv_row2 + 50;
       //        // hv_column2 = hv_column2 + 50;
       //         HOperatorSet.CropPart(MyImage, out ho_ImagePartBroken, hv_row1, hv_column1, hv_row2, hv_column2);
       //     }
       //     else
       //     {
       //         ho_ImagePartBroken = null;
       //         return;
       //     }
          
       // }
        //释放资源
      
    }
}
