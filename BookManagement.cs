﻿    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Windows.Forms;

    namespace ASM2_DATA
    {
        public partial class BookManagement : Form
        {
            SqlConnection connection;
            public BookManagement()
            {
                InitializeComponent();
                connection = new SqlConnection(@"Data Source=V\SQLEXPRESS;Initial Catalog=LibraryManagement1;Integrated Security=True;");
            }

            public void Filldata()
            {
                string query = "select * from Books";
                DataTable tbl = new DataTable();
                SqlDataAdapter ad = new SqlDataAdapter(query, connection);
                ad.Fill(tbl);
                dgvBooks.DataSource = tbl;
            }

            private void ViewLibraryFrom_Load(object sender, EventArgs e)
            {
                Filldata();
                GetPublishers();
                GetAuthors();
                GetCategories();
            }

            public void GetAuthors()
            {
                string query = "select author_id, author_name from Authors";
                DataTable table = new DataTable();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                dataAdapter.Fill(table);
                cboAuthor.DataSource = table;
                cboAuthor.DisplayMember = "author_name";
                cboAuthor.ValueMember = "author_id";
            }

            public void GetCategories()
            {
                string query = "select category_id, category_name from Categories";
                DataTable table = new DataTable();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                dataAdapter.Fill(table);
                cboCategory.DataSource = table;
                cboCategory.DisplayMember = "category_name";
                cboCategory.ValueMember = "category_id";
            }

            public void GetPublishers()
            {
                string query = "select publisher_id, publisher_name from Publishers";
                DataTable table = new DataTable();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                dataAdapter.Fill(table);
                cboPublisher.DataSource = table;
                cboPublisher.DisplayMember = "publisher_name";
                cboPublisher.ValueMember = "publisher_id";
            }

            private void BookManagement_Load(object sender, EventArgs e)
            {
                // Ensuring all data is loaded when the form loads
                ViewLibraryFrom_Load(sender, e);
            }

            private void dgvBooks_CellContentClick(object sender, DataGridViewCellEventArgs e)
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = this.dgvBooks.Rows[e.RowIndex];
                    txtID.Text = row.Cells["book_id"].Value.ToString();
                    txtName.Text = row.Cells["book_name"].Value.ToString();
                    cboAuthor.SelectedValue = row.Cells["author_id"].Value.ToString();
                    cboCategory.SelectedValue = row.Cells["category_id"].Value.ToString();
                    cboPublisher.SelectedValue = row.Cells["publisher_id"].Value.ToString();
                    dtpPublishingDate.Value = (DateTime)row.Cells["publishing_date"].Value;
                }
            }

            public void FillDataBooks()
            {
                string query = "SELECT * FROM Books";

                DataTable dataTable = new DataTable();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, connection);
                sqlDataAdapter.Fill(dataTable);
                dgvBooks.DataSource = dataTable;
            }

            public void ClearDataBooks()
            {
                txtID.Text = "";
                txtName.Text = "";
                cboAuthor.SelectedIndex = -1;
                cboCategory.SelectedIndex = -1;
                cboPublisher.SelectedIndex = -1;
            }

            private void btnAdd_Click(object sender, EventArgs e)
            {
                string name = txtName.Text;
                string authorID = cboAuthor.SelectedValue.ToString();
                string categoryID = cboCategory.SelectedValue.ToString();
                string publisherID = cboPublisher.SelectedValue.ToString();
                DateTime publishingDate = dtpPublishingDate.Value;

                string insert = "INSERT INTO Books (book_name, author_id, category_id, publisher_id, publishing_date) VALUES (@book_name, @author_id, @category_id, @publisher_id, @publishing_date)";

                try
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(insert, connection))
                    {
                        cmd.Parameters.AddWithValue("@book_name", name);
                        cmd.Parameters.AddWithValue("@author_id", authorID);
                        cmd.Parameters.AddWithValue("@category_id", categoryID);
                        cmd.Parameters.AddWithValue("@publisher_id", publisherID);
                        cmd.Parameters.AddWithValue("@publishing_date", publishingDate);
                        cmd.ExecuteNonQuery();
                    }
                    FillDataBooks();
                    ClearDataBooks();
                    MessageBox.Show(this, "Inserted successfully", "Result", MessageBoxButtons.OK, MessageBoxIcon.None);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, $"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }

            private void btnUpdate_Click(object sender, EventArgs e)
            {
                if (MessageBox.Show(this, "Do you want to edit?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (txtID.Text == "")
                    {
                        MessageBox.Show("Book ID is not provided.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (txtName.Text == "" || cboAuthor.SelectedValue == null || cboCategory.SelectedValue == null || cboPublisher.SelectedValue == null)
                    {
                        MessageBox.Show("Please fill in all the required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string update = "UPDATE Books SET book_name = @book_name, author_id = @author_id, category_id = @category_id, publisher_id = @publisher_id, publishing_date = @publishing_date WHERE book_id = @book_id";

                    try
                    {
                        connection.Open();
                        using (SqlCommand cmd = new SqlCommand(update, connection))
                        {
                            cmd.Parameters.AddWithValue("@book_id", int.Parse(txtID.Text));
                            cmd.Parameters.AddWithValue("@book_name", txtName.Text);
                            cmd.Parameters.AddWithValue("@author_id", int.Parse(cboAuthor.SelectedValue.ToString()));
                            cmd.Parameters.AddWithValue("@category_id", int.Parse(cboCategory.SelectedValue.ToString()));
                            cmd.Parameters.AddWithValue("@publisher_id", int.Parse(cboPublisher.SelectedValue.ToString()));
                            cmd.Parameters.AddWithValue("@publishing_date", dtpPublishingDate.Value);
                            cmd.ExecuteNonQuery();
                        }
                        FillDataBooks();
                        ClearDataBooks();
                        MessageBox.Show(this, "Updated successfully", "Result", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, $"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            private void btnDelete_Click(object sender, EventArgs e)
            {
                if (MessageBox.Show(this, "Do you want to delete?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (txtID.Text == "")
                    {
                        MessageBox.Show("Book ID is not provided.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string delete = "DELETE FROM Books WHERE book_id = @book_id";

                    try
                    {
                        connection.Open();
                        using (SqlCommand cmd = new SqlCommand(delete, connection))
                        {
                            cmd.Parameters.AddWithValue("@book_id", int.Parse(txtID.Text));
                            cmd.ExecuteNonQuery();
                        }
                        FillDataBooks();
                        ClearDataBooks();
                        MessageBox.Show(this, "Deleted successfully", "Result", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, $"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            private void btnClearData_Click(object sender, EventArgs e)
            {
                ClearDataBooks();
            }

            private void btnDisplay_Click(object sender, EventArgs e)
            {
                Filldata();
            }
        }
    }
