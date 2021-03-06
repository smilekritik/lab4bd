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

namespace WindowsFormsApp1
{ 
    public partial class Form1 : Form
    {
        static string connection = @"Server=KRITIK;Database=lab2;Trusted_Connection=True;TrustServerCertificate=True";//
        static public SqlConnection sql_Connection = new SqlConnection(connection);
        static DataSet dataset = new DataSet();
        SqlDataAdapter adapter = new SqlDataAdapter();

        SqlCommandBuilder command_Builder = new SqlCommandBuilder();


        public Form1()
        {
            InitializeComponent();

            sql_Connection.Open();
            string sql_Select = "SELECT * FROM Aviary";
            dataset = new DataSet("Some");

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = true;

            adapter = new SqlDataAdapter(sql_Select, sql_Connection);
            dataset.Tables.Add("Aviary");
            adapter.Fill(dataset, "Aviary");
            dataset.Tables.Add("Animals");
            dataGridView1.DataSource = dataset.Tables["Aviary"];

            sql_Connection.Close();
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewTextBoxCell cell = (DataGridViewTextBoxCell)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (e.ColumnIndex == 0)
            {
                sql_Connection.Open();
                string sql_Select = "SELECT * FROM Animal WHERE Aviary_ID =" + cell.Value;

                dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView2.AllowUserToAddRows = true;
                dataset.Tables["Animals"].Clear();
                adapter = new SqlDataAdapter(sql_Select, sql_Connection);
                adapter.Fill(dataset, "Animals");
                dataGridView2.DataSource = dataset.Tables["Animals"];

                sql_Connection.Close();
            }
        }
        private void Add_aninal_Click(object sender, EventArgs e)
        {
            sql_Connection.Open();

            string sql_Scalar = "SELECT MAX(ID) FROM Animal";

            SqlCommand sqlCommand = new SqlCommand(sql_Scalar, sql_Connection);
            object max_ID = sqlCommand.ExecuteScalar();

            string sql_Select = "SELECT * FROM Animal";
            adapter = new SqlDataAdapter(sql_Select, sql_Connection);
            command_Builder = new SqlCommandBuilder(adapter);
            command_Builder.GetInsertCommand();
            command_Builder.GetUpdateCommand();
            command_Builder.GetDeleteCommand();

            DataRow row = dataset.Tables["Animals"].NewRow();

            for (int i = (int)max_ID; i < dataGridView2.Rows.Count - 2; i++)
            {
                row["Aviary_ID"] = dataGridView2.Rows[i].Cells["Aviary_ID"].Value;
                row["Name"] = dataGridView2.Rows[i].Cells["Name"].Value;
                row["Age"] = dataGridView2.Rows[i].Cells["Age"].Value;
                row["Weight"] = dataGridView2.Rows[i].Cells["Weight"].Value;
                row["Sex"] = dataGridView2.Rows[i].Cells["Sex"].Value;
                dataset.Tables["Animals"].Rows.Add(row);
                dataGridView2.Rows.RemoveAt(i);
            }

            adapter.Update(dataset, "Animals");

            sql_Connection.Close();
        }

        private void insert_Click(object sender, EventArgs e)
        {
            sql_Connection.Open();

            string sql_Insert = "INSERT INTO Animal VALUES(7, 'insert', 10, 12.2, 'male')";
            string sql_Select = "SELECT * FROM Animal";

            SqlCommand sqlCommand = new SqlCommand(sql_Insert, sql_Connection);
            sqlCommand.ExecuteNonQuery();
            /*SqlCommand command = new SqlCommand(sql_Select, sql_Connection);
            SqlDataReader reader = command.ExecuteReader();
            if(reader.HasRows) 
                {
                while (reader.Read()) // построчно считываем данные
                 {
                    object id = reader.GetValue(0);
                    object age = reader.GetValue(2);
                 }
                }
             */
        adapter = new SqlDataAdapter(sql_Select, connection);
            dataset = new DataSet();
            adapter.Fill(dataset);
            dataGridView1.DataSource = dataset.Tables[0];

            sql_Connection.Close();
        }

        private void update_Click(object sender, EventArgs e)
        {
            sql_Connection.Open();

            string sql_Insert = "UPDATE Ticket SET Cost = 1111 WHERE Cost = 1";
            string sql_Select = "SELECT * FROM Ticket";

            SqlCommand sqlCommand = new SqlCommand(sql_Insert, sql_Connection);
            sqlCommand.ExecuteNonQuery();

            adapter = new SqlDataAdapter(sql_Select, connection);
            dataset = new DataSet();

            adapter.Fill(dataset);
            dataGridView1.DataSource = dataset.Tables[0];

            sql_Connection.Close();
        }

        private void delete_Click(object sender, EventArgs e)
        {
            sql_Connection.Open();

            string sql_Insert = "DELETE FROM Animal WHERE age = 10";
            string sql_Select = "SELECT * FROM Animal";

            SqlCommand sqlCommand = new SqlCommand(sql_Insert, sql_Connection);
            sqlCommand.ExecuteNonQuery();

            adapter = new SqlDataAdapter(sql_Select, connection);
            dataset = new DataSet();

            adapter.Fill(dataset);
            dataGridView1.DataSource = dataset.Tables[0];

            sql_Connection.Close();
        }

        private void scalar_Click(object sender, EventArgs e)
        {
            sql_Connection.Open();

            string sql_Scalar = "SELECT MAX(qwe.Animals_counted) as an_top FROM(SELECT Aviary.ID as num, COUNT(Aviary.ID) as Animals_counted FROM Aviary RIGHT JOIN Animal ON Aviary.ID = Animal.Aviary_ID GROUP BY Aviary.ID) as qwe";

            SqlCommand sqlCommand = new SqlCommand(sql_Scalar, sql_Connection);
            sqlCommand.ExecuteScalar();

            adapter = new SqlDataAdapter(sql_Scalar, connection);
            dataset = new DataSet();

            adapter.Fill(dataset);
            dataGridView1.DataSource = dataset.Tables[0];

            sql_Connection.Close();
        }

        private void procedure_Click(object sender, EventArgs e)
        {
            sql_Connection.Open();

            SqlParameter parametr = new SqlParameter
            {
                ParameterName = "@max_av",
                SqlDbType = SqlDbType.Money,
                Direction = ParameterDirection.Output
            };

            SqlCommand sqlCommand = new SqlCommand("Top_Aviary", sql_Connection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.Add(parametr);
            sqlCommand.ExecuteScalar();

            object sum = sqlCommand.Parameters["@max_av"].Value;
            MessageBox.Show($"Max animals count = {sum}");

            sql_Connection.Close();
        }

        private void transaction_Click(object sender, EventArgs e)
        {
            sql_Connection.Open();

            string sql_Insert = "INSERT INTO Animal VALUES(7, 'transaction', 10, 12.2, 'male')";
            string sql_Select = "SELECT * FROM Animal";

            SqlTransaction sql_Transaction = sql_Connection.BeginTransaction();
            SqlCommand sql_Command1 = new SqlCommand(sql_Insert, sql_Connection);
            SqlCommand sql_Command2 = new SqlCommand(sql_Insert, sql_Connection);

            sql_Command1.Transaction = sql_Transaction;
            sql_Command2.Transaction = sql_Transaction;

            try
            {
                sql_Command1.ExecuteNonQuery();
                sql_Command2.ExecuteNonQuery();
                sql_Transaction.Commit();
                MessageBox.Show("Transaction done");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                sql_Transaction.Rollback();
            }

            adapter = new SqlDataAdapter(sql_Select, connection);
            dataset = new DataSet();

            adapter.Fill(dataset);
            dataGridView1.DataSource = dataset.Tables[0];

            sql_Connection.Close();
        }

        
    }
}
