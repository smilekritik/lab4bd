using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4_BD
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
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

namespace Task
{
    public partial class Quiz : Form
    {
        static string connectionString = @"Server=WIN-QDFDQARV5RR;Database=BuildingCompanyDB;Trusted_Connection=True;TrustServerCertificate=True";
        string sqlSelectBrigades = "SELECT * FROM Brigades";
        string sqlSelectWorkersBrigades = "SELECT * FROM WorkersBrigades";
        string sqlSelectWorkers = "SELECT * FROM Workers";
        static public SqlConnection sqlConnection = new SqlConnection(connectionString);
        static DataSet ds = new DataSet();
        SqlDataAdapter adapter = new SqlDataAdapter();
        SqlCommandBuilder commandBuilder = new SqlCommandBuilder();

        public void ViewTables()
        {
            sqlConnection.Open();

            ds = new DataSet("Task");

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = true;

            adapter = new SqlDataAdapter(sqlSelectBrigades, sqlConnection);
            ds.Tables.Add("Brigades");
            adapter.Fill(ds, "Brigades");
            dataGridView1.DataSource = ds.Tables["Brigades"];

            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.AllowUserToAddRows = true;

            adapter = new SqlDataAdapter(sqlSelectWorkersBrigades, sqlConnection);
            ds.Tables.Add("WorkersBrigades");
            adapter.Fill(ds, "WorkersBrigades");
            dataGridView2.DataSource = ds.Tables["WorkersBrigades"];

            dataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView3.AllowUserToAddRows = true;

            adapter = new SqlDataAdapter(sqlSelectWorkers, sqlConnection);
            ds.Tables.Add("Workers");
            adapter.Fill(ds, "Workers");
            dataGridView3.DataSource = ds.Tables["Workers"];

            sqlConnection.Close();
        }

        public Quiz()
        {
            InitializeComponent();

            ViewTables();
        }


        private void Task_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void SaveBrigades_Click(object sender, EventArgs e)
        {
            sqlConnection.Open();

            adapter = new SqlDataAdapter(sqlSelectBrigades, sqlConnection);
            commandBuilder = new SqlCommandBuilder(adapter);
            commandBuilder.GetInsertCommand();
            commandBuilder.GetUpdateCommand();
            commandBuilder.GetDeleteCommand();

            int rowIndex = dataGridView1.Rows.Count - 2;
            DataRow row = ds.Tables["Brigades"].NewRow();

            row["BuildingCompanyID"] = dataGridView1.Rows[rowIndex].Cells["BuildingCompanyID"].Value;
            row["WorkersAmmount"] = dataGridView1.Rows[rowIndex].Cells["WorkersAmmount"].Value;

            ds.Tables["Brigades"].Rows.Add(row);
            dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 2);

            adapter.Update(ds, "Brigades");

            sqlConnection.Close();

            ViewTables();
        }

        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {

        }

        private void dataGridView2_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {

        }

        private void dataGridView3_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {

        }

        private void SaveWorker_Click(object sender, EventArgs e)
        {
            sqlConnection.Open();

            adapter = new SqlDataAdapter(sqlSelectWorkers, sqlConnection);
            commandBuilder = new SqlCommandBuilder(adapter);
            commandBuilder.GetInsertCommand();
            commandBuilder.GetUpdateCommand();
            commandBuilder.GetDeleteCommand();

            int rowIndex = dataGridView3.Rows.Count - 2;
            DataRow row = ds.Tables["Workers"].NewRow();

            row["Speciality"] = dataGridView3.Rows[rowIndex].Cells["Speciality"].Value;
            row["Professionalism"] = dataGridView3.Rows[rowIndex].Cells["Professionalism"].Value;
            row["DateStart"] = dataGridView3.Rows[rowIndex].Cells["DateStart"].Value;
            row["DateEnd"] = dataGridView3.Rows[rowIndex].Cells["DateEnd"].Value;

            ds.Tables["Workers"].Rows.Add(row);
            dataGridView3.Rows.RemoveAt(dataGridView3.Rows.Count - 2);

            adapter.Update(ds, "Workers");

            sqlConnection.Close();

            ViewTables();
        }

        private void WB_Create_Click(object sender, EventArgs e)
        {
            sqlConnection.Open();

            adapter = new SqlDataAdapter(sqlSelectWorkersBrigades, sqlConnection);
            commandBuilder = new SqlCommandBuilder(adapter);
            commandBuilder.GetInsertCommand();

            int rowIndexWB = dataGridView2.Rows.Count - 2;
            int rowIndexW = dataGridView3.Rows.Count - 2;
            int rowIndexB = dataGridView1.Rows.Count - 2;
            DataRow row = ds.Tables["WorkersBrigades"].NewRow();

            if ((int)dataGridView2.Rows[rowIndexWB].Cells["WorkerID"].Value != (int)dataGridView3.Rows[rowIndexW].Cells["WorkerID"].Value)
            {
                row["WorkerID"] = dataGridView3.Rows[rowIndexW].Cells["WorkerID"].Value;
                row["BrigadeID"] = dataGridView1.Rows[rowIndexB].Cells["BrigadeID"].Value;

                ds.Tables["WorkersBrigades"].Rows.Add(row);

                adapter.Update(ds, "WorkersBrigades");
            }

            sqlConnection.Close();

            ViewTables();
        }
    }
}