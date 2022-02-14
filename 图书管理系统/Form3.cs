using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; //SQL

namespace 图书管理系统
{
    public partial class Form3 : Form
    {
        private string FileName;
        public Form3()
        {
            InitializeComponent();
        }
        public Form3(string FileName)
        {
            this.FileName = FileName;
            InitializeComponent();
        }       

        private void Form3_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = System.IO.File.ReadAllText(FileName);
            //richTextBox1.LoadFile(FileName, System.Windows.Forms.RichTextBoxStreamType.PlainText);
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
