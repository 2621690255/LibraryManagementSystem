using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 图书管理系统
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            richTextBox1.Text = "稍等片刻，数据库自动连接中...\n此过程预计需要0~20秒。";
            //richTextBox1.Text = System.IO.File.ReadAllText("数据库连接说明.txt");
            radioButton1.Checked = true;
        }

        //数据库连接
        public bool ConnectSQL()
        {
            ClassSQL classSQL = new ClassSQL();
            if (classSQL.JudgeConnect())
            {
                this.Hide();
                Form2 form2 = new Form2();
                form2.ShowDialog();
                this.Close();
                return true;
            }
            else
            {
                richTextBox1.Text = System.IO.File.ReadAllText("数据库连接说明.txt");
                return false;
                //MessageBox.Show("数据库自动连接失败，将进行手动配置界面！", "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b' || Char.IsDigit(e.KeyChar)) e.Handled = false;
            else e.Handled = true;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            richTextBox1.Text = "稍等片刻，正在尝试重新连接数据库...\n此过程预计需要0~20秒。";

            string ServerAddress = "server=3222205ch0.zicp.vip,37537;database=text1;uid=sa;pwd=lgc2621690255";
            if (radioButton1.Checked) ServerAddress = "server=3222205ch0.zicp.vip,";
            else ServerAddress = "server=server.natappfree.cc,";
            ServerAddress += textBox1.Text + ";database=text1;uid=sa;pwd=lgc2621690255";
            ClassSQL classSQL = new ClassSQL();
            classSQL.SetServerAddress(ServerAddress);
            if (classSQL.JudgeConnect())
            {
                this.Hide();
                Form2 form2 = new Form2();
                form2.ShowDialog();
                this.Close();
            }
            else richTextBox1.Text = System.IO.File.ReadAllText("数据库连接说明.txt");
            //else MessageBox.Show("输入端口号不正确 或 端口号与端口类型不匹配！", "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);          
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            MessageBox.Show("按下确定键，系统将尝试自动连接数据库。","连接信息");

            //Thread.Sleep(1000);
            ConnectSQL();
        }

    }
}
