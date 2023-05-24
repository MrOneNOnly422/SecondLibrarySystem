using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SecondLibrarySystem
{
    public partial class Form1 : Form
    {
         private string connectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=SampleLibrary;Integrated Security=True";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'sampleLibraryDataSet1.LibrarySystem' table. You can move, or remove it, as needed.
            this.librarySystemTableAdapter.Fill(this.sampleLibraryDataSet1.LibrarySystem);
            LoadDataGrid();

        }

        private void LoadDataGrid()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sqlQuery = "SELECT * FROM LibrarySystem";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        grid1.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string accessionNumber = txtno.Text;
                    string title = txttitle.Text;
                    string author = txtauthor.Text;

                    string sqlQuery = "INSERT INTO LibrarySystem (AccessionNumber, Title, Author) VALUES (@AccessionNumber, @Title, @Author)";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@AccessionNumber", accessionNumber);
                        command.Parameters.AddWithValue("@Title", title);
                        command.Parameters.AddWithValue("@Author", author);

                        command.ExecuteNonQuery();

                        MessageBox.Show("Data added!!");

                        LoadDataGrid(); // Refresh the DataGridView
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string accessionNumber = txtno.Text;
                    string newTitle = txttitle.Text;
                    string newAuthor = txtauthor.Text;

                    string sqlQuery = "UPDATE LibrarySystem SET Title = @NewTitle, Author = @NewAuthor WHERE AccessionNumber = @AccessionNumber";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@NewTitle", newTitle);
                        command.Parameters.AddWithValue("@NewAuthor", newAuthor);
                        command.Parameters.AddWithValue("@AccessionNumber", accessionNumber);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Data updated !");

                            LoadDataGrid(); // Refresh the DataGridView
                        }
                        else
                        {
                            MessageBox.Show("Nothing!!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string searchInput = txtSearch.Text;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sqlQuery = "SELECT * FROM LibrarySystem WHERE AccessionNumber LIKE @SearchInput OR Title LIKE @SearchInput OR Author LIKE @SearchInput";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@SearchInput", "%" + searchInput + "%");

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        grid1.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void grid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string accessionNumber = grid1.Rows[e.RowIndex].Cells["AccessionNumber"].Value.ToString();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sqlQuery = "SELECT * FROM LibrarySystem WHERE AccessionNumber = @AccessionNumber";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@AccessionNumber", accessionNumber);

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            txtno.Text = reader["AccessionNumber"].ToString();
                            txttitle.Text = reader["Title"].ToString();
                            txtauthor.Text = reader["Author"].ToString();
                        }

                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string accessionNumber = txtno.Text;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sqlQuery = "DELETE FROM LibrarySystem WHERE AccessionNumber = @AccessionNumber";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@AccessionNumber", accessionNumber);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record deleted successfully!");

                            LoadDataGrid(); // Refresh the DataGridView
                        }
                        else
                        {
                            MessageBox.Show("No matching record found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
