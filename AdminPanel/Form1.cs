using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdminPanel
{
    public partial class Form1 : Form
    {
        SqlConnection con;
        SqlCommand command;
        DataTable dt;
        SqlDataAdapter adapter;
        SqlParameter param1;
        SqlParameter param2;

        public Form1()
        {
            InitializeComponent();
            con = new SqlConnection();
            con.ConnectionString = @"Integrated Security = SSPI; data source = DESKTOP-LT9TUC4\SQLEXPRESS; Initial Catalog = Bot";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            con.Open();
            command = new SqlCommand();
            command.Connection = con;
            command.CommandText = "SELECT ID_USER_TELEGRAM, NICK, USER_POSITION FROM USERS";

            dt = new DataTable();
            adapter = new SqlDataAdapter(command);
            adapter.Fill(dt);

            dataGridView1.DataSource = dt;

            command.Dispose();
            con.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            con.Open();
            command = new SqlCommand();
            command.Connection = con;
            command.CommandText = "SELECT ID_USER_TELEGRAM, NICK, USER_POSITION FROM USERS";

            dt = new DataTable();
            adapter = new SqlDataAdapter(command);
            adapter.Fill(dt);

            dataGridView1.DataSource = dt;

            command.Dispose();
            con.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            try
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[index];
                textBox3.Text = selectedRow.Cells[0].Value.ToString();
                //textBox2.Text = selectedRow.Cells[3].Value.ToString();
                textBox4.Text = selectedRow.Cells[1].Value.ToString();
                textBox2.Text = selectedRow.Cells[2].Value.ToString();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            con.Open();
            command = new SqlCommand();
            dt = new DataTable();
            command.Connection = con;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_USERS_UPDATE";

            string str = textBox2.Text;
            string id = textBox1.Text;
            param1 = new SqlParameter("@user_position", str);
            param2 = new SqlParameter("@id", id);
            param1.Direction = ParameterDirection.Input;
            param1.DbType = DbType.String;
            param2.Direction = ParameterDirection.Input;
            param2.DbType = DbType.String;
            command.Parameters.Add(param1);
            command.Parameters.Add(param2);

            adapter = new SqlDataAdapter(command);
            adapter.Fill(dt);

            command.Dispose();
            con.Close();
        }
    }
}
