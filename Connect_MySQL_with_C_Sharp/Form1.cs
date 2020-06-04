using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace MySQL_ex
{
    public partial class Form1 : Form
    {
        private MySqlConnection mySqlConnection;
        private string databaseServer;
        private string databasePort;
        private string databaseUid;
        private string databasePassword;
        private string databaseName;
        private string tableName;
        
        public Form1()
        {
            InitializeComponent();
        }
        /*Button1-打开程序与数据库连接*/
        private void Button1_Click(object sender, EventArgs e)
        {
            //接收文本框输入
            databaseServer = textBox1.Text;
            databasePort = textBox5.Text;
            databaseUid = textBox2.Text;
            databasePassword = textBox3.Text;
            databaseName = textBox4.Text;
            //初始化连接信息，组成连接信息字符串
            Initialize(databaseServer, databasePort, databaseUid, databasePassword, databaseName);
            //打开连接
            if (OpenConnection())
            {
                MessageBox.Show("Connection succeeded!");
            }
            else
            {
                MessageBox.Show("Cannot connect, please check your password/database.");
            }
        }
        /*Button2-查询表中所有数据*/
        private void Button2_Click(object sender, EventArgs e)
        {
            //接收文本框输入
            tableName = textBox6.Text;
            //组成select语句字符串
            string sqlDML = "select * from " + tableName;
            //执行语句，查询数据
            DataSet dataSet = GetDataSet(sqlDML, tableName);
            //设置表名、数据来源
            dataGridView1.DataSource = dataSet;
            dataGridView1.DataMember = tableName;
            //单元格列宽自适应
            for (int i = 0; i < dataGridView1.ColumnCount; ++i)
            {
                dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }
        /*Button4-插入数据*/
        private void Button4_Click(object sender, EventArgs e)
        {
            //接收文本框输入，保存在List中
            List<string> dataString = new List<string>
            {
                textBox7.Text,
                textBox8.Text,
                textBox9.Text,
                textBox10.Text,
                textBox11.Text,
                textBox12.Text,
                textBox13.Text
            };
            //组成show语句字符串
            string sqlDML = "show columns from " + tableName;
            //读取列名
            MySqlCommand mySqlCommand = CreateCommand(sqlDML);
            MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
            //读取到的列名组成字符串，并统计列数
            mySqlDataReader.Read();
            string tableNameSring = mySqlDataReader.GetString(0);
            int tableNameCount = 1;
            while (mySqlDataReader.Read())
            {
                tableNameSring += "," + mySqlDataReader.GetString(0);
                ++tableNameCount;
            }
            //关闭DataReader连接
            mySqlDataReader.Close();
            //根据列数组成insert语句字符串
            //文本框输入的多余的值忽略处理
            sqlDML = "insert into " + tableName + "(" + tableNameSring + ") values(" + dataString[0];
            //判断插入的值是否为纯数字，不是需要加''
            for(int i = 1; i < tableNameCount; ++i)
            {
                if (IsInteger(dataString[i]))
                {
                    sqlDML += "," + dataString[i];
                }
                else
                {
                    sqlDML += ",'" + dataString[i] + "'";
                }
            }
            sqlDML += ")";
            //执行语句，插入数据
            AlterRow(sqlDML);
        }
        /*Button5-删除数据*/
        private void Button5_Click(object sender, EventArgs e)
        {
            //接收文本框输入
            string rowName = textBox14.Text;
            string deleteKey = textBox15.Text;
            //组成delete语句字符串
            string sqlDML;
            //判断删除的值是否为纯数字，不是需要加''
            if (IsInteger(deleteKey))
            {
                sqlDML = "delete from " + tableName + " where " + rowName + " = " + deleteKey;
            }
            else
            {
                sqlDML = "delete from " + tableName + " where " + rowName + " = '" + deleteKey + "'";
            }
            //执行语句，删除数据
            AlterRow(sqlDML);
        }
        /*Button6-更改数据*/
        private void Button6_Click(object sender, EventArgs e)
        {
            //接收文本框输入
            string rowName = textBox16.Text;
            string oldData = textBox17.Text;
            string newData = textBox18.Text;
            //组成update语句字符串
            string sqlDML;
            //判断要更改的值和新值是否为纯数字，不是需要加''
            if (IsInteger(oldData))
            {
                sqlDML = "update " + tableName + " set " + rowName + " = " + newData + " where " + rowName + " = " + oldData;
            }
            else
            {
                sqlDML = "update " + tableName + " set " + rowName + " = '" + newData + "' where " + rowName + " = '" + oldData + "'";
            }
            //执行语句，更改数据
            AlterRow(sqlDML);
        }
        /*Button7-查询数据*/
        private void Button7_Click(object sender, EventArgs e)
        {
            //接收文本框输入
            string rowName = textBox19.Text;
            string condition = textBox20.Text;
            //组成select语句字符串
            string sqlDML = "select " + rowName + " from " + tableName + " where " + condition;
            //执行语句，查询数据
            DataSet dataSet = GetDataSet(sqlDML, tableName);
            //设置表名、数据来源
            dataGridView2.DataSource = dataSet;
            dataGridView2.DataMember = tableName;
            //单元格列宽自适应
            for (int i = 0; i < dataGridView2.ColumnCount; ++i)
            {
                dataGridView2.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }
        /*Button3-关闭程序与数据库连接*/
        private void Button3_Click(object sender, EventArgs e)
        {
            CloseConnection();
        }
        //初始化连接信息
        public void Initialize(string server, string port, string uid, string password, string database)
        {
            this.databaseServer = server;
            this.databasePort = port;
            this.databaseUid = uid;
            this.databasePassword = password;
            this.databaseName = database;
            string connectionString = "server=" + server + ";port=" + port + ";user=" + uid + ";password=" + password + ";database=" + database + ";charset=utf8";
            mySqlConnection = new MySqlConnection(connectionString);
        }
        //打开连接
        public bool OpenConnection()
        {
            try
            {
                mySqlConnection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        //实例化MySqlCommang类
        public MySqlCommand CreateCommand(string sql)
        {
            MySqlCommand mySqlCmmmand = new MySqlCommand(sql, mySqlConnection);
            return mySqlCmmmand;
        }
        //实例化MySqlDataAdapter类
        public MySqlDataAdapter CreateAdapter(MySqlCommand mySqlCmmmand)
        {
            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCmmmand);
            return mySqlDataAdapter;
        }
        //查询记录
        public DataSet GetDataSet(string sql, string table)
        {
            MySqlCommand mySqlCommand = CreateCommand(sql);
            MySqlDataAdapter mySqlDataAdapter = CreateAdapter(mySqlCommand);
            DataSet dataSet = new DataSet();
            try
            {
                mySqlDataAdapter.Fill(dataSet, table);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dataSet;
        }
        //增加、删除、修改记录
        public void AlterRow(string sql)
        {
            MySqlCommand mySqlCommand = CreateCommand(sql);
            try
            {
                mySqlCommand.ExecuteNonQuery();
                MessageBox.Show("Insertion/Deletion/Renewal succeed!");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //关闭连接
        public void CloseConnection()
        {
            try
            {
                mySqlConnection.Close();
                MessageBox.Show("Connection closed!");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        //判断字符串是否为纯数字
        public bool IsInteger(string str)
        {
            bool boolResult = true;
            if (str == "")
            {
                boolResult = false;
            }
            else
            {
                foreach (char c in str)
                {
                    if (char.IsNumber(c))
                    {
                        continue;
                    }
                    else
                    {
                        boolResult = false;
                        break;
                    }
                }
            }
            return boolResult;
        }
    }
}
