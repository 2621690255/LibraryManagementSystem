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
    public partial class Form5 : Form
    {
        string userid, Operator;
        public Form5()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
        }
        public Form5(string userid, string Operator)
        {            
            this.userid = userid; this.Operator = Operator;
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000; timer1.Start();

            this.Text = Operator + "菜单";
            button1.Text = "确认" + Operator;

            if (Operator == "借阅" || Operator == "删除")
            {
                textBox5.ReadOnly = true; textBox7.ReadOnly = true;
                ClassSQL classSQL = new ClassSQL();
                string str = "SELECT * FROM books";
                DataTable dataTable = classSQL.ExecuteQuery(str);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dataTable;
                    dataGridView1_CellClick(sender,new DataGridViewCellEventArgs(0, 0));
                }
                else
                {
                    MessageBox.Show("馆内暂无书籍资料，请联系管理员添加书籍！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close(); return;
                }
            }

            else if(Operator == "归还")
            {
                label1.Text = "书号"; textBox1.ReadOnly = true;
                label2.Text = "书名"; textBox2.ReadOnly = true;
                label3.Text = "借阅日期"; textBox3.ReadOnly = true;
                label4.Text = "应还日期"; textBox4.ReadOnly = true;
                label5.Text = "应缴费用"; textBox5.ReadOnly = true; textBox5.TextChanged -= new EventHandler(textBox5_TextChanged);
                label6.Visible = false; textBox6.Enabled = false; textBox6.Visible = false;
                label7.Visible = false; textBox7.Enabled = false; textBox7.Visible = false;
                
                label8.Visible = true;                
                radioButton1.Enabled = true; radioButton2.Enabled = true;
                radioButton1.Visible= true; radioButton2.Visible = true;
                radioButton1.Checked = true; radioButton2.Checked = false;
                
                dataGridView1.CellClick -= new DataGridViewCellEventHandler(dataGridView1_CellClick);
                dataGridView1.CellClick += new DataGridViewCellEventHandler(dataGridView1_CellClick2);
            }

            else if (Operator == "添加")
            {
                ClassSQL classSQL = new ClassSQL();
                string str = "SELECT * FROM books WHERE 书号 = null";
                DataTable dataTable = classSQL.ExecuteQuery(str);
                dataGridView1.DataSource = dataTable;

                textBox1.TextChanged += new EventHandler(textBox1_TextChanged);
                textBox2.TextChanged += new EventHandler(textBox2_TextChanged);
                textBox3.TextChanged += new EventHandler(textBox2_TextChanged);
                textBox4.TextChanged += new EventHandler(textBox2_TextChanged);
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // MessageBox.Show("QQQQQQQ");
            if (textBox1.Text.Length == textBox1.MaxLength) MessageBox.Show("书号长度最长为" + textBox1.MaxLength.ToString() + "(当前:" + textBox1.Text.Length + ")", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            ClassSQL classSQL = new ClassSQL();
            string str = "SELECT * FROM books WHERE 书号 = '" + textBox1.Text + "'";
            DataTable dataTable = classSQL.ExecuteQuery(str);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                MessageBox.Show("检测到 书号[" + textBox1.Text + "] 已存在，添加操作将变成修改书籍信息", "修改提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dataGridView1.DataSource = dataTable;
                textBox2.Text = dataTable.Rows[0]["书名"].ToString();
                textBox3.Text = dataTable.Rows[0]["作者"].ToString();
                textBox4.Text = dataTable.Rows[0]["出版社"].ToString();
                textBox6.Text = dataTable.Rows[0]["借阅数量"].ToString();
                textBox7.Text = dataTable.Rows[0]["库存"].ToString();
                textBox5.Text = dataTable.Rows[0]["超时收费"].ToString();
                button1.Text = "确认修改";
            }
            else
            {
                dataGridView1.DataSource = dataTable;
                textBox2.Text = null;
                textBox3.Text = null;
                textBox4.Text = null;
                textBox6.Text = "0";
                textBox7.Text = null;
                textBox5.Text = null;
                button1.Text = "确认添加";
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length == textBox2.MaxLength) 
                MessageBox.Show("书名长度最长为" + textBox2.MaxLength.ToString() + "(当前:" + textBox2.Text.Length + ")", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (textBox3.Text.Length == textBox3.MaxLength) 
                MessageBox.Show("作者长度最长为" + textBox3.MaxLength.ToString() + "(当前:" + textBox3.Text.Length + ")", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (textBox4.Text.Length == textBox4.MaxLength) 
                MessageBox.Show("出版社长度最长为" + textBox4.MaxLength.ToString() + "(当前:" + textBox4.Text.Length + ")", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void textBoxAll_TextChanged(object sender, EventArgs e)
        {
            if (Operator == "借阅" || Operator == "删除")
            {
                string str = "SELECT * FROM books";
                bool first = true;

                if (textBox1.Text.Length != 0)
                {
                    if (first)
                    {
                        str += " WHERE "; first = false;
                    }
                    else str += " AND ";
                    str += "书号 LIKE " + "'%" + textBox1.Text + "%'";
                }

                if (textBox2.Text.Length != 0)
                {
                    if (first)
                    {
                        str += " WHERE "; first = false;
                    }
                    else str += " AND ";
                    str += "书名 LIKE " + "'%" + textBox2.Text + "%'";
                }

                if (textBox3.Text.Length != 0)
                {
                    if (first)
                    {
                        str += " WHERE "; first = false;
                    }
                    else str += " AND ";
                    str += "作者 LIKE " + "'%" + textBox3.Text + "%'";
                }

                if (textBox4.Text.Length != 0)
                {
                    if (first)
                    {
                        str += " WHERE "; first = false;
                    }
                    else str += " AND ";
                    str += "出版社 LIKE " + "'%" + textBox4.Text + "%'";
                }

                // MessageBox.Show(str);
                ClassSQL classSQL = new ClassSQL();
                DataTable dataTable = classSQL.ExecuteQuery(str);
                if (dataTable == null || dataTable.Rows.Count <= 0)
                {
                    MessageBox.Show("暂未找到到符合条件的书籍！", "查询提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dataGridView1.DataSource = dataTable;
                    textBox6.Text = textBox7.Text = textBox5.Text = null;
                }
                else
                {
                    dataGridView1.DataSource = dataTable;
                    dataGridView1_CellClick(sender, new DataGridViewCellEventArgs(0, 0));
                }
            }

            else if (Operator == "归还")
            {

            }

            //else if (Operator == "删除")
            //{

            //}

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClassSQL classSQL = null;
            string str = null;
            DataTable dataTable = null;
            int is_ok = 0;

            if (Operator == "借阅" || Operator == "删除")
            {
                if (dataGridView1.CurrentRow != null && dataGridView1.CurrentRow.Index > -1)
                {
                    if (dataGridView1.CurrentRow == null) return;
                    string bookid = dataGridView1.CurrentRow.Cells["书号"].Value.ToString();
                    string op = "[书号:" + bookid + "] 书名《" + dataGridView1.CurrentRow.Cells["书名"].Value.ToString() + "》\n 是否需要" + Operator + "该书?";

                    DialogResult dialogResult = MessageBox.Show(op, Operator + "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.No) return;

                    if (Operator == "借阅")
                    {
                        if ((int)dataGridView1.CurrentRow.Cells["库存"].Value == (int)dataGridView1.CurrentRow.Cells["借阅数量"].Value)
                        {
                            MessageBox.Show("该书库存不足！", "借阅提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        str = "SELECT * FROM records WHERE 借阅账号 = '" + userid + "' AND 借阅书号 = '" + bookid + "' AND 是否归还 = 0";
                        classSQL = new ClassSQL();
                        dataTable = classSQL.ExecuteQuery(str);
                        if (dataTable != null && dataTable.Rows.Count > 0)
                        {
                            MessageBox.Show("您已借阅该书且尚未归还，请勿重复借阅！", "借阅提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        str = "SELECT * FROM users WHERE 账号 = '" + userid + "'";
                        classSQL = new ClassSQL();
                        dataTable = classSQL.ExecuteQuery(str);
                        if (dataTable != null && dataTable.Rows.Count > 0 && (int)dataTable.Rows[0]["借阅情况"] == 10)
                        {
                            MessageBox.Show("一个账号最多同时借阅10本书籍，请先归还一些书籍！", "借阅提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        str = "UPDATE books SET 借阅数量= 借阅数量 + 1 WHERE 书号 = '" + bookid + "'";
                        classSQL = new ClassSQL();
                        is_ok = classSQL.ExecuteUpdate(str);

                        str = "UPDATE users SET 借阅情况= 借阅情况 + 1 WHERE 账号 = '" + userid + "'";
                        classSQL = new ClassSQL();
                        is_ok &= classSQL.ExecuteUpdate(str);

                        //MessageBox.Show(DateTime.Now.ToString());
                        //INSERT INTO records VALUES('1','000003','2020-6-13 3:10:22',null,0,90)
                        str = "INSERT INTO records VALUES('" + userid + "','" + bookid + "','" + DateTime.Now.ToString() + "',null,0,30)";
                        classSQL = new ClassSQL();
                        is_ok &= classSQL.ExecuteUpdate(str);                   
                    }

                    else if (Operator == "删除")
                    {
                        if ((int)dataGridView1.CurrentRow.Cells["借阅数量"].Value != 0)
                        {
                            MessageBox.Show("该书尚有用户未归还！", "删除提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        str = "DELETE FROM records WHERE 借阅书号 = '" + bookid + "'";
                        //MessageBox.Show(str);
                        classSQL = new ClassSQL();
                        is_ok = classSQL.ExecuteUpdate(str);

                        str = "DELETE FROM books WHERE 书号 = '" + bookid + "'";
                        //MessageBox.Show(str);
                        classSQL = new ClassSQL();
                        is_ok |= classSQL.ExecuteUpdate(str);
                    }
               
                    if (is_ok > 0)
                    {
                        classSQL = new ClassSQL();
                        str = "SELECT * FROM books";
                        dataTable = classSQL.ExecuteQuery(str);
                        if (dataTable != null && dataTable.Rows.Count > 0)
                        {
                            dataGridView1.DataSource = dataTable;
                            dataGridView1_CellClick(sender, new DataGridViewCellEventArgs(0, 0));
                        }
                        else
                        {
                            textBox6.Text = null; textBox7.Text = null; textBox5.Text = null;
                        }
                        MessageBox.Show(Operator + "成功！", Operator + "提示");
                    }
                    else MessageBox.Show(Operator + "失败！", "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else MessageBox.Show("当前未选中任何书籍！", Operator + "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            else if(Operator == "归还")
            {
                if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.Index < 0) 
                {
                    if(radioButton1.Checked) MessageBox.Show("当前未选中任何书籍！", "归还提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else MessageBox.Show("当前未选中任何记录！", "删除提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string bookid = dataGridView1.CurrentRow.Cells["书号"].Value.ToString();
                //归还书籍
                if (radioButton1.Checked)
                {
                    //MessageBox.Show(textBox5.Text);
                    //MessageBox.Show(dataGridView1.CurrentRow.Cells[2].Value.ToString());
                    //MessageBox.Show(Convert.ToDouble(textBox5.Text).ToString());
                    //MessageBox.Show(Convert.ToDouble(dataGridView1.CurrentRow.Cells[2].Value.ToString()).ToString());
                    int OverDueDays = Convert.ToInt32(Convert.ToDouble(textBox5.Text) / Convert.ToDouble(dataGridView1.CurrentRow.Cells[2].Value.ToString()));
                    if(OverDueDays != 0) 
                        MessageBox.Show("当前书籍已逾期，逾期 " + OverDueDays + "天，应缴费用 " + textBox5.Text + "元！", "归还提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                   
                    str = "UPDATE books SET 借阅数量= 借阅数量 - 1 WHERE 书号 = '" + bookid + "'";
                   // MessageBox.Show(str);
                    classSQL = new ClassSQL();
                    is_ok = classSQL.ExecuteUpdate(str);

                    str = "UPDATE users SET 借阅情况= 借阅情况 - 1 WHERE 账号 = '" + userid + "'";
                    //MessageBox.Show(str);
                    classSQL = new ClassSQL();
                    is_ok &= classSQL.ExecuteUpdate(str);

                    //MessageBox.Show(DateTime.Now.ToString());
                    //INSERT INTO records VALUES('1','000003','2020-6-13 3:10:22',null,0,90)
                    str = "UPDATE records SET 归还日期 = '" + DateTime.Now.ToString() + "', 是否归还 = 1";
                    str += " WHERE 借阅账号 = '" + userid + "' AND 借阅书号 = '" + bookid + "' AND 是否归还 = 0";
                    //MessageBox.Show(str);
                    classSQL = new ClassSQL();
                    is_ok &= classSQL.ExecuteUpdate(str);

                    if(is_ok > 0)
                    {
                        MessageBox.Show("归还成功！", "归还提示");
                        radioButton1.Checked = false; radioButton1.Checked = true;
                    }
                    else MessageBox.Show("归还失败！", "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                //删除记录
                else
                {
                    DialogResult dialogResult = MessageBox.Show("是否要删除该条记录？", "删除提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if(dialogResult == DialogResult.Yes)
                    {

                        str = "DELETE FROM [records]";
                        str += " WHERE 借阅账号 = '" + userid + "' AND 借阅书号 = '" + bookid + "' AND 是否归还 = 1";
                        str += " AND 借阅日期 = '"+ Convert.ToDateTime(textBox3.Text).ToString("yyyy-MM-dd HH:mm:ss") +"' ";
                      //  MessageBox.Show(str);
                        classSQL = new ClassSQL();
                        is_ok = classSQL.ExecuteUpdate(str);
                        
                        if (is_ok > 0)
                        {
                            MessageBox.Show("记录删除成功！", "删除提示");
                            radioButton2.Checked = false; radioButton2.Checked = true;
                        }
                        else MessageBox.Show("记录删除失败！", "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            else if (Operator == "添加")
            {
                if (textBox1.Text.Length == 0 || textBox2.Text.Length == 0 || textBox3.Text.Length == 0 || textBox4.Text.Length == 0 || textBox7.Text.Length == 0 || textBox5.Text.Length == 0)
                {
                    MessageBox.Show("请确保输入项均不为空！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (button1.Text == "确认修改")
                {
                   classSQL = new ClassSQL();
                    //UPDATE books SET 书名 = 'C语言入门', 作者 = '李老师', 出版社 = '铁道出版社', 借阅数量 = 12, 库存 = 88, 超时收费 = 1.99 WHERE 书号 = '000003'
                    str = "UPDATE  books SET ";
                    str += "书名 = '" + textBox2.Text + "',";
                    str += "作者 = '" + textBox3.Text + "',";
                    str += "出版社 = '" + textBox4.Text + "',";
                    str += "借阅数量 = " + textBox6.Text + ",";
                    str += "库存 = " + textBox7.Text + ",";
                    str += "超时收费 = " + textBox5.Text + " ";
                    str += "WHERE 书号 = '" + textBox1.Text + "'";

                   // MessageBox.Show(str);
                    is_ok = classSQL.ExecuteUpdate(str);
                    if (is_ok > 0) MessageBox.Show("修改成功！", "添加信息");
                    else MessageBox.Show("修改失败！", "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                else if(button1.Text == "确认添加")
                {                 
                    classSQL = new ClassSQL();
                    //INSERT INTO books VALUES('000003','数据库系统原理3','佚名','清华大学出版社', 0, 100, .234);
                    str = "INSERT INTO books VALUES('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "',";
                    str += "0, " + textBox7.Text + "," + textBox5.Text + ")";

                    is_ok = classSQL.ExecuteUpdate(str);
                    if (is_ok > 0)
                    {
                        MessageBox.Show("添加成功！", "添加信息");
                        button1.Text = "确认修改";
                    }
                    else MessageBox.Show("添加失败！", "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                classSQL = new ClassSQL();
                str = "SELECT * FROM books WHERE 书号 = '" + textBox1.Text + "'";
                dataTable = classSQL.ExecuteQuery(str);
                dataGridView1.DataSource = dataTable;

            }

        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || e.KeyChar == '\b') e.Handled = false;
            else e.Handled = true;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            double price; double.TryParse(textBox5.Text, out price);
            if (price < 0 || price > 10)
            {
                MessageBox.Show("请输入一个 0~10 的实数(精度0.01)！", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox5.Text = "9.99";
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || e.KeyChar == '\b') e.Handled = false;
            else if (e.KeyChar == '.' && ((TextBox)sender).Text.IndexOf('.') == -1) e.Handled = false;
            else e.Handled = true;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex > -1)
            {
                textBox6.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                textBox7.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                textBox5.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            }
            else
            {
                textBox6.Text = null; textBox7.Text = null; textBox5.Text = null;
            }
        }

        private void dataGridView1_CellClick2(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                textBox1.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                textBox2.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                textBox3.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                textBox4.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();

                if (radioButton1.Checked)
                {
                    double tax = Convert.ToDouble(dataGridView1.CurrentRow.Cells[2].Value.ToString());
                    DateTime dateTime = Convert.ToDateTime(dataGridView1.CurrentRow.Cells[1].Value.ToString());
                    TimeSpan timeSpan =DateTime.Now.Subtract(dateTime);
                    if (timeSpan.Days > 0) textBox5.Text = (timeSpan.Days * tax).ToString();
                    else textBox5.Text = "0.00";
                    //MessageBox.Show((DateTime.Now - dateTime).ToString());
                }
                else textBox5.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            }
        }

        private void radioButtonAll_CheckedChanged(object sender, EventArgs e)
        {
            ClassSQL classSQL = new ClassSQL();
            string str = null;
            if (radioButton1.Checked)
            {
                str = "SELECT 借阅日期, 应还日期, [超时收费(元/天)],借阅书号 AS 书号, 书名, 作者, 出版社 FROM borrows";
                label5.Text = "应缴费用"; label4.Text = "应还日期"; button1.Text = "确认归还";
            }
            else
            {
                str = "SELECT 借阅日期, 归还日期, 已缴费用,借阅书号 AS 书号, 书名, 作者, 出版社 FROM [returns]";
                label5.Text = "已缴费用"; label4.Text = "归还日期"; button1.Text = "删除记录";
            }
            str += " WHERE 借阅账号 = '" + userid + "'";
            
            DataTable dataTable = classSQL.ExecuteQuery(str);
            dataGridView1.DataSource = dataTable;
            if (dataTable != null && dataTable.Rows.Count > 0) dataGridView1_CellClick2(sender, new DataGridViewCellEventArgs(0, 0));
            else textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = textBox5.Text = null;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.toolStripStatusLabel1.Text = "当前时间：" + DateTime.Now.ToString();
            //this.toolStripStatusLabel1.Text = "当前时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3(Operator + "书籍.txt");
            form3.ShowDialog();
            form3.Dispose();
        }

        private void textBox7_Leave(object sender, EventArgs e)
        {
            int cnt = 0; int.TryParse(textBox7.Text, out cnt);
            int min = 0; int.TryParse(textBox6.Text, out min);
            if (cnt < min || cnt > 1000)
            {
                MessageBox.Show("请输入一个 " + min.ToString() + "~" + 1000 + "的整数！", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox7.Text = "1000";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
