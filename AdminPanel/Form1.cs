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

        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            con = new SqlConnection();
            con.ConnectionString = @"Integrated Security = SSPI; data source = DESKTOP-LT9TUC4\SQLEXPRESS; Initial Catalog = Bot";

            command = new SqlCommand();
            command.Connection = con;
            command.CommandText = "SELECT * FROM USERS";

            dt = new DataTable();
            adapter = new SqlDataAdapter(command);
            adapter.Fill(dt);

            dataGridView1.DataSource = dt;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //con.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            con = new SqlConnection();
            con.ConnectionString = @"Integrated Security = SSPI; data source = DESKTOP-LT9TUC4\SQLEXPRESS; Initial Catalog = Bot";

            command = new SqlCommand();
            command.Connection = con;
            command.CommandText = "SELECT * FROM USERS";

            dt = new DataTable();
            adapter = new SqlDataAdapter(command);
            adapter.Fill(dt);

            dataGridView1.DataSource = dt;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            try
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[index];
                textBox1.Text = selectedRow.Cells[0].Value.ToString();
                textBox2.Text = selectedRow.Cells[3].Value.ToString();
                textBox3.Text = selectedRow.Cells[1].Value.ToString();
                textBox4.Text = selectedRow.Cells[2].Value.ToString();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
