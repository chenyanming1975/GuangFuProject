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
    public partial class Login : Form
    {
        FrmMain form;
        public Login(FrmMain form1)
        {
            this.form = form1;
            InitializeComponent();
            
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            login();
        }

        private void login()
        {
            if (txtUserName.Text == "")
            {
                MessageBox.Show("请输入用户名！！");
                return;
            }
            if (txtPwd.Text == "")
            {
                MessageBox.Show("请输入密码！！");
                return;
            }
            if (txtUserName.Text == "emt")
            {
                if (txtPwd.Text == "emt")
                {
                    form.Enabled = true;
                    form.tsMenuItemVision.Enabled = true;
                    form.tsMenuItemGetReport.Enabled = true;
                    form.tsMenuItemHelp.Enabled = true;
                    form.tsBtnRun.Enabled = true;
                    form.tsBtnStop.Enabled = true;
                    form.tsBtnLoadImg.Enabled = true;
                    form.tsBtnParaSet.Enabled = true;
                    MessageBox.Show("登录成功！");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("密码错误！！");
                    txtPwd.Focus();
                    return;
                }
            }
            else if (txtUserName.Text == "debug")
            {
                if (txtPwd.Text == "debug")
                {
                    //form.Enabled = true;
                    //form.项目PToolStripMenuItem.Enabled = true;
                    //form.编辑ToolStripMenuItem.Enabled = true;
                    //form.统计CToolStripMenuItem.Enabled = true;
                    //form.帮助HToolStripMenuItem.Enabled = true;
                    //form.关于AToolStripMenuItem.Enabled = true;
                    //form.toolStripBtnCreateFile.Enabled = true;
                    //form.toolStripBtnDeleteFile.Enabled = true;
                    //form.toolStripBtnEditFile.Enabled = true;
                    //form.toolStripBtnSelectFile.Enabled = true;
                    //form.toolStripButtonHelp.Enabled = true;
                    //form.toolStripButtonAbout.Enabled = true;
                    //form.toolStripBtnClearCount.Enabled = true;
                    //form.toolStripButtonClose.Enabled = true;
                    //form.Name = "调试者界面";
                    //this.Close();
                }
                else
                {
                    MessageBox.Show("密码错误！！");
                    txtPwd.Focus();
                    return;
                }

            }
            else
            {
                MessageBox.Show("用户名错误！！");
                txtUserName.Focus();
                return;
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            txtUserName.Focus();
        }

        private void txtPwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                login();
            }
        }

      

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            if (sender.Equals(txtUserName))
            {
                lblUserName.Visible = txtUserName.Text.Length < 1;
            }
        }

        private void txtPwd_TextChanged(object sender, EventArgs e)
        {
            if (sender.Equals(txtPwd))
            {
                lblPwd.Visible = txtPwd.Text.Length < 1;
            }
        }

        private void lblPwd_Click(object sender, EventArgs e)
        {
            if (sender.Equals(lblPwd))
            {
                txtPwd.Focus();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
    }
}
