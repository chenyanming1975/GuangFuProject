using HalconDotNet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DefectDetection
{
    public class surfaceDetect
    {
        private HObject region = null, connectedRegions = null, selectedRegions = null, selectObject = null;
        public HTuple Width = null, Height = null, Row = null, Col = null,Size = null,Area = null;
        private string msg = null;
        public string msgInfo
        {
            set { msg = value; }
            get { return msg; }
        }
       // public HTuple ThresholdMin { set { thresholdMin = value; } }
        object locker = new object();
        public void detect(HObject Img, int thresholdMin, int thresholdMax, out Hashtable[] ht,out HTuple number,bool isLeft)
        {
            try
            {
                #region
                //lock (locker)
                //{
                    //HOperatorSet.GenEmptyObj(out selectObject);
                    HTuple area = null, row = null, col = null,sorted = null,inversSorted = null;
                    HOperatorSet.Threshold(Img, out region, thresholdMin, thresholdMax);
                    HOperatorSet.Connection(region, out connectedRegions);
                    HOperatorSet.SelectShape(connectedRegions, out selectedRegions, "area", "and", 5, 8000);
                    HOperatorSet.AreaCenter(selectedRegions,out area,out row,out col);
                    HOperatorSet.TupleSortIndex(area, out sorted);
                    //区域面积由大到小排序
                    HOperatorSet.TupleInverse(sorted, out inversSorted); 
                    HOperatorSet.CountObj(selectedRegions, out number);
                    ht = new Hashtable[number];
                    for (int i = 0; i < number; i++)
                    {
                        HOperatorSet.SelectObj(selectedRegions, out selectObject, inversSorted[i] + 1);
                        HOperatorSet.RegionFeatures(selectObject, "area", out Area);
                        HOperatorSet.RegionFeatures(selectObject, "width", out Width);
                        HOperatorSet.RegionFeatures(selectObject, "height", out Height);
                        HOperatorSet.RegionFeatures(selectObject, "row", out Row);
                        HOperatorSet.RegionFeatures(selectObject, "column", out Col);
                        Width = (HTuple)Math.Round((double)Width, 2);
                        Height = (HTuple)Math.Round((double)Height, 2);
                        Size = (Width + Height) / 2;
                        //区域的（长 + 宽）< 0.5不计入到缺陷
                        if ( Size < 0.5)
                        {
                            number--;
                            i--;
                            continue;
                        }
                        Row = (HTuple)Math.Round((double)Row, 2);
                        Col = (HTuple)Math.Round((double)Col, 2);
                        if (!isLeft)
                        {
                            Col += 7296;
                        }
                        ht[i] = new Hashtable();
                        ht[i].Add("width", Width);
                        ht[i].Add("height", Height);
                        ht[i].Add("row", Row);
                        ht[i].Add("column", Col);
                        ht[i].Add("size",Size);
                        ht[i].Add("area",Area);
                        

                    }
                    dispose();
                #endregion


            }
           

            catch (Exception exc)
            {
                ht = null;
                number = null;
                msg = DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "detect";
            }
        }
        /// <summary>
        /// 绘制缺陷
        /// </summary>
        /// <param name="hwc"></param>
        /// <param name="ht"></param>
        /// <param name="number"></param>
      

        public void dispose()
        {
            try
            {
                if (region != null)
                {
                    region.Dispose();
                    region = null;
                }
                if (connectedRegions != null)
                {
                    connectedRegions.Dispose();
                    connectedRegions = null;
                }
                if (selectedRegions != null)
                {
                    selectedRegions.Dispose();
                    selectedRegions = null;
                }
                if (selectObject != null)
                {
                    selectObject.Dispose();
                    selectObject = null;
                }
            }
            catch (Exception exc)
            {
                msg = DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff-") + exc.Message + "dispose";
            }
        }
    }
}
