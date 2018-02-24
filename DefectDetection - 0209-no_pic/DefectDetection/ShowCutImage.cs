using HalconDotNet;
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
    public partial class ShowCutImage : Form
    {
        private HObject image = null;
        HalconFunction halcon = new HalconFunction();
        public ShowCutImage(HObject img)
        {
            InitializeComponent();
            this.image = img;
        }

        private void ShowCutImage_Load(object sender, EventArgs e)
        {
            halcon.displayImageOnWindow(image,hWindowControl1.HalconWindow);
         
        }
    }
}
