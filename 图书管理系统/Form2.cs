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
    public partial class Form2 : Form
    {
        public Form2()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(button1.Text == "确认注册")
            {
                if (textBox1.Text.Length == 0 || textBox2.Text.Length == 0 || textBox3.Text.Length == 0)
                {
                    MessageBox.Show("账号、密码均不能为空！", "注册提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if(textBox2.Text != textBox3.Text)
                {
                    MessageBox.Show("两次密码输入不一致！", "注册提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                //INSERT users VALUES('541807010119', 'zhimakaimeng', '刘广城', '男', 0, 1)
                string str = "INSERT INTO users VALUES ('" + textBox1.Text + "','" + textBox2.Text + "',null, null, 0, 0)";
              //  MessageBox.Show(str);
                ClassSQL classSQL = new ClassSQL();
                int is_ok = classSQL.ExecuteUpdate(str);
                if (is_ok != 0)
                {
                    MessageBox.Show("注册成功！即将为您跳转用户菜单...", "注册信息");
                    this.Hide();
                    Form4 form4 = new Form4(textBox1.Text, "用户");
                    form4.ShowDialog();
                    textBox1.Clear(); textBox2.Clear(); textBox3.Clear();
                    this.Show();
                }
                else MessageBox.Show("注册失败！", "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else if(button1.Text == "立即登录")
            {
                if (textBox1.Text.Length == 0 || textBox2.Text.Length == 0)
                {
                    MessageBox.Show("账号、密码均不能为空！", "登录提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ClassSQL classSQL = new ClassSQL();
                string str = "SELECT * FROM users WHERE 账号 = '" + textBox1.Text + "'";
                DataTable dataTable = classSQL.ExecuteQuery(str);
               // MessageBox.Show(dataTable.Rows[0][1].ToString(), textBox2.Text);
                if (dataTable.Rows[0]["密码"].ToString() == textBox2.Text.ToString())
                {
                    string UserOrAdmin = "用户";
                    if (dataTable.Rows[0]["管理权限"].ToString() == "True")
                    {
                       DialogResult dialogResult = MessageBox.Show("检测到管理权限，是否进入管理页面？", "登录提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialogResult == DialogResult.Yes) UserOrAdmin = "管理";
                    }
                    this.Hide();
                    Form4 form4 = new Form4(textBox1.Text, UserOrAdmin);                    
                    form4.ShowDialog();
                    textBox1.Clear(); textBox2.Clear(); textBox3.Clear();
                    this.Show();
                }
                else
                {
                    MessageBox.Show("输入账号与输入密码不匹配！", "登录提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            else
            {
                if (textBox1.Text.Length == 0 || textBox2.Text.Length == 0 || textBox3.Text.Length == 0)
                {
                    MessageBox.Show("账号、密码均不能为空！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000; timer1.Start();

            textBox2.Enabled = false;
            textBox3.Enabled = false;
            label5.Visible = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox2.Clear(); textBox3.Clear();
            if (textBox1.Text.Length == 0)
            {              
                textBox2.Enabled = false;           
                textBox3.Enabled = false;
                textBox3.Visible = true;

                label3.Visible = true;
                label4.Text = "请输入您的账号！";
                label5.Visible = false;

                button1.Text = "注册/登录";
                return;
            }
            else
            {
                label5.Visible = true;
                textBox2.Enabled = true;
            }

            if (textBox1.Text.Length == textBox1.MaxLength)
            {
                MessageBox.Show("账号长度最长为" + textBox1.MaxLength.ToString() + "(当前:" + textBox1.Text.Length + ")", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            string str = "SELECT * FROM users WHERE 账号 = '" + textBox1.Text + "'";
            ClassSQL classSQL = new ClassSQL(); //SQL类实例化
            DataTable dataTable =  classSQL.ExecuteQuery(str);
            bool is_register = true; //注册用户?
            if (dataTable != null && dataTable.Rows.Count > 0) is_register = false;
            
            if (!is_register) //账号已存在
            {              
                label3.Visible = false;
                textBox3.Enabled = false;
                textBox3.Visible = false;
                label4.Text = "账号已存在，用户可直接登录！";
                button1.Text = "立即登录";
            }

            else
            {               
                label3.Visible = true;
                textBox3.Enabled = true;
                textBox3.Visible = true;
                label4.Text = "账号不存在，系统将自动为您注册！";
                button1.Text = "确认注册";
            }

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox3.Clear();
            if (textBox2.Text.Length == 0)
            {
                label5.Text = "请输入您的密码！";
            }
            else if (textBox2.Text.Length == textBox2.MaxLength)
            {
                MessageBox.Show("密码长度最长为" + textBox2.MaxLength.ToString() + "(当前:" + textBox2.Text.Length + ")", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (textBox3.Text.Length != 0 && textBox3.Text != textBox2.Text) label5.Text = "两次密码输入不一致！";
            else label5.Text = "";
           // textBox3.Text = textBox2.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text.Length == 0) label5.Text = "";
            else if (textBox3.Text != textBox2.Text) label5.Text = "两次密码输入不一致！";
            else label5.Text = "";
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsUpper(e.KeyChar) || Char.IsLower(e.KeyChar) || Char.IsDigit(e.KeyChar) || e.KeyChar == '\b')
            {
                e.Handled = false;
            }
            else e.Handled = true;
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
            else e.Handled = false;
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
            else e.Handled = false;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //this.toolStripStatusLabel1.Text = "欢迎使用本系统！" + "当前时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.toolStripStatusLabel1.Text = "欢迎使用本系统！" + "当前时间：" + DateTime.Now.ToString();
        }

        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3("注册和登录.txt");
            form3.ShowDialog();
            form3.Dispose();
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.textBox2.Focus();
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if(textBox3.Enabled == true) this.textBox3.Focus();
                else
                {
                    this.button1.Focus();
                    button1_Click(sender, e);   //调用登录按钮的事件处理代码
                }
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.button1.Focus();
                button1_Click(sender, e);   //调用登录按钮的事件处理代码
            }
        }
    }
}
