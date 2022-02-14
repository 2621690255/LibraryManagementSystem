using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 图书管理系统
{
    public partial class Form4 : Form
    {
        string userid, UserOrAdmin;
        public Form4()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
        }
        public Form4(string userid, string UserOrAdmin)
        {
            this.userid = userid; this.UserOrAdmin = UserOrAdmin;
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000; timer1.Start();

            this.Text = UserOrAdmin + "菜单";
            groupBox1.Text = UserOrAdmin + "信息";
            if (UserOrAdmin == "管理")
            {
                label6.Visible = false; textBox5.Visible = false;
                button1.Text = "增加/修改书籍";
                button2.Text = "删除书籍";
            }
            else
            {
                button1.Text = "借阅书籍";
                button2.Text = "归还书籍/历史记录";
            }

            Load_Infor();
        }

        private void Load_Infor()
        {
            ClassSQL classSQL = new ClassSQL();
            string str = "SELECT * FROM users WHERE 账号 = '" + userid + "'";
            DataTable dataTable = classSQL.ExecuteQuery(str);

            textBox1.Text = dataTable.Rows[0]["账号"].ToString();
            textBox2.Text = dataTable.Rows[0]["密码"].ToString();
            textBox3.Text = dataTable.Rows[0]["姓名"].ToString();

            string Sex = dataTable.Rows[0]["性别"].ToString();
            if (Sex == "男 ") comboBox1.SelectedIndex = 0;
            else if (Sex == "女 ") comboBox1.SelectedIndex = 1;
            else comboBox1.SelectedIndex = 2;

            textBox5.Text = dataTable.Rows[0]["借阅情况"].ToString() + "/10";
            textBox4.Text = dataTable.Rows[0]["管理权限"].ToString() == "True" ? "管理员" : "无";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            string str = UserOrAdmin == "用户" ? "借阅" : "添加";
            Form5 form5 = new Form5(userid, str);
            form5.ShowDialog();
            Load_Infor();
            this.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            string str = UserOrAdmin == "用户" ? "归还" : "删除";
            Form5 form5 = new Form5(userid, str);
            form5.ShowDialog();
            Load_Infor();
            this.Show();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
           if( e.KeyChar > 32 && e.KeyChar < 127 || e.KeyChar == '\b')
           {             
                e.Handled = false;
           }
           else e.Handled = true;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length == textBox2.MaxLength)
            {
                MessageBox.Show("密码长度最长为" + textBox2.MaxLength.ToString() + "(当前:" + textBox2.Text.Length + ")", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text.Length == textBox3.MaxLength)
            {
                MessageBox.Show("姓名长度最长为" + textBox3.MaxLength.ToString() + "(当前:" + textBox3.Text.Length + ")", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ClassSQL classSQL = new ClassSQL();
            string str = "SELECT * FROM users WHERE 账号 = '" + userid + "'";
            DataTable dataTable = classSQL.ExecuteQuery(str);
            if(dataTable != null && dataTable.Rows.Count > 0)
            {
                if((int)dataTable.Rows[0]["借阅情况"] != 0)
                {
                    MessageBox.Show("当前账号有书籍未归还！", "注销提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DialogResult dialogResult = MessageBox.Show("是否要注销当前账号？", "注销提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if(dialogResult == DialogResult.Yes)
                {
                    classSQL = new ClassSQL();
                    str = "DELETE FROM records WHERE 借阅账号 = '" + userid + "'";
                    int is_ok = classSQL.ExecuteUpdate(str);

                    classSQL = new ClassSQL();
                    str = "DELETE FROM users WHERE 账号 = '" + userid + "'";
                    is_ok |= classSQL.ExecuteUpdate(str);

                    if(is_ok > 0)
                    {
                        MessageBox.Show("注销成功！", "注销提示");
                        this.Close();
                        return;
                    }
                    else MessageBox.Show("注销失败！", "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.toolStripStatusLabel1.Text = "登入账号：" + userid + ", 当前时间：" + DateTime.Now.ToString();
            //this.toolStripStatusLabel1.Text = "欢迎使用本系统！" + "当前时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3(UserOrAdmin + "手册.txt");
            form3.ShowDialog();
            form3.Dispose();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if(textBox2.Text.Length == 0)
            {
                MessageBox.Show("密码不能为空！", "修改提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ClassSQL classSQL = new ClassSQL();
            string[] Sex = { "男", "女", "保密" };
            string str = "UPDATE users SET ";
            str += "密码 = '" + textBox2.Text + "',";
            str += "姓名 = '" + textBox3.Text + "',";
            str += "性别 = '" + Sex[comboBox1.SelectedIndex] + "'";
            str += " WHERE 账号 = '" + userid + "'";
           // MessageBox.Show(str);
            int is_ok = classSQL.ExecuteUpdate(str);

            if(is_ok > 0)
            {
                MessageBox.Show("用户信息修改成功！", "修改信息");
            }
            else MessageBox.Show("用户信息修改失败！", "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
