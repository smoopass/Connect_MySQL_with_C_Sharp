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
        //打开
        private void Button1_Click(object sender, EventArgs e)
        {
            databaseServer = textBox1.Text;
            databasePort = textBox5.Text;
            databaseUid = textBox2.Text;
            databasePassword = textBox3.Text;
            databaseName = textBox4.Text;
            Initialize(databaseServer, databasePort, databaseUid, databasePassword, databaseName);
            if (OpenConnection())
            {
                MessageBox.Show("Connection succeeded!");
            }
            else
            {
                MessageBox.Show("Cannot connect, please check your password/database.");
            }
        }
        //查所有
        private void Button2_Click(object sender, EventArgs e)
        {
            tableName = textBox6.Text;
            string sqlDML = "select * from " + tableName;
            DataSet dataSet = GetDataSet(sqlDML, tableName);
            dataGridView1.DataSource = dataSet;
            dataGridView1.DataMember = tableName;
            for (int i = 0; i < dataGridView1.ColumnCount; ++i)
            {
                dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }
        //增
        private void Button4_Click(object sender, EventArgs e)
        {
            List<string> dataString = new List<string>();
            dataString.Add(textBox7.Text);
            dataString.Add(textBox8.Text);
            dataString.Add(textBox9.Text);
            dataString.Add(textBox10.Text);
            dataString.Add(textBox11.Text);
            dataString.Add(textBox12.Text);
            dataString.Add(textBox13.Text);
            string sqlDML = "show columns from " + tableName;
            MySqlCommand mySqlCommand = CreateCommand(sqlDML);
            MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
            mySqlDataReader.Read();
            string tableNameSring = mySqlDataReader.GetString(0);
            int tableNameCount = 1;
            while (mySqlDataReader.Read())
            {
                tableNameSring += "," + mySqlDataReader.GetString(0);
                ++tableNameCount;
            }
            mySqlDataReader.Close();
            sqlDML = "insert into " + tableName + "(" + tableNameSring + ") values(" + dataString[0];
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
            AlterRow(sqlDML);
        }
        //删
        private void Button5_Click(object sender, EventArgs e)
        {
            string rowName = textBox14.Text;
            string deleteKey = textBox15.Text;
            string sqlDML;
            if (IsInteger(deleteKey))
            {
                sqlDML = "delete from " + tableName + " where " + rowName + " = " + deleteKey;
            }
            else
            {
                sqlDML = "delete from " + tableName + " where " + rowName + " = '" + deleteKey + "'";
            }
            AlterRow(sqlDML);
        }
        //改
        private void Button6_Click(object sender, EventArgs e)
        {
            string rowName = textBox16.Text;
            string oldData = textBox17.Text;
            string newData = textBox18.Text;
            string sqlDML;
            if (IsInteger(oldData))
            {
                sqlDML = "update " + tableName + " set " + rowName + " = " + newData + " where " + rowName + " = " + oldData;
            }
            else
            {
                sqlDML = "update " + tableName + " set " + rowName + " = '" + newData + "' where " + rowName + " = '" + oldData + "'";
            }
            AlterRow(sqlDML);
        }
        //查
        private void Button7_Click(object sender, EventArgs e)
        {
            string rowName = textBox19.Text;
            string condition = textBox20.Text;
            string sqlDML = "select " + rowName + " from " + tableName + " where " + condition;
            DataSet dataSet = GetDataSet(sqlDML, tableName);
            dataGridView2.DataSource = dataSet;
            dataGridView2.DataMember = tableName;
            for (int i = 0; i < dataGridView2.ColumnCount; ++i)
            {
                dataGridView2.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }
        //关闭
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
        //创建command
        public MySqlCommand CreateCommand(string sql)
        {
            MySqlCommand mySqlCmmmand = new MySqlCommand(sql, mySqlConnection);
            return mySqlCmmmand;
        }
        //创建adapter
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
