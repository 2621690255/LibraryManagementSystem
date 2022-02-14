using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data; //SQL
using System.Data.SqlClient;
using System.Windows.Forms;

namespace 图书管理系统
{
    class ClassSQL
    {
        private static string MySqlCon = "server=3222205ch0.zicp.vip,37537;database=text1;uid=text1_users;pwd=lgc2621690255"; //sa登录
        //private string MySqlCon = "server=.;database=text1;integrated security = SSPI"; //Windows登录

        //判断是否数据库连接成功
        public bool JudgeConnect()
        {
            //int result = -1;
            bool result = false;
            //创建连接对象
            SqlConnection SqlConnection = new SqlConnection(@MySqlCon);
            try
            {
                SqlConnection.Open();
                if (SqlConnection.State == ConnectionState.Open)
                {
                    MessageBox.Show("连接成功！","连接信息");
                    result = true;
                }
                else
                {
                   // result = 0;
                    MessageBox.Show("连接失败！","错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                //result = 0;
                MessageBox.Show("连接失败！", "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SqlConnection.Close();
            }
            //return Convert.ToBoolean(result);
            return result;
        }
        //public bool JudgeConnect()
        //{
        //    SqlConnection con = new SqlConnection(@MySqlCon);
        //    //con.Open();
        //    if (con.State != ConnectionState.Open) return false;
        //    else
        //    {
        //       // con.Close();
        //        return true;
        //    }
        //}
        //设置数据库地址
        public void SetServerAddress(string NewSqlCon)
        {
            MySqlCon = NewSqlCon;
        }
        
        //用于查询；其实是相当于提供一个可以传参的函数，到时候写一个sql语句，存在string里，传给这个函数，就会自动执行。
        public DataTable ExecuteQuery(string sqlStr)      
        {
            SqlConnection con = new SqlConnection(@MySqlCon);          
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sqlStr;
            DataTable dt = new DataTable();
            SqlDataAdapter msda;
            msda = new SqlDataAdapter(cmd);
            msda.Fill(dt);
            con.Close();
            return dt;
        }

        //用于增删改;
        public int ExecuteUpdate(string sqlStr)      
        {
            SqlConnection con = new SqlConnection(@MySqlCon);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sqlStr;
            int iud = 0;
            iud = cmd.ExecuteNonQuery();
            con.Close();
            return iud;
        }
    }
}
