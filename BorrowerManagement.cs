using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ASM2_DATA
{
    public partial class BorrowerManagement : Form
    {
        SqlConnection connection;
        public BorrowerManagement()
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
            cboBooks.DataSource = tbl;
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
            cboAddress.DataSource = table;
            cboAddress.DisplayMember = "author_name";
            cboAddress.ValueMember = "author_id";
        }

        public void GetCategories()
        {
            string query = "select category_id, category_name from Categories";
            DataTable table = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
            dataAdapter.Fill(table);
            cboPhone.DataSource = table;
            cboPhone.DisplayMember = "category_name";
            cboPhone.ValueMember = "category_id";
        }

        public void GetPublishers()
        {
            string query = "select publisher_id, publisher_name from Publishers";
            DataTable table = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
            dataAdapter.Fill(table);
            cboEmail.DataSource = table;
            cboEmail.DisplayMember = "publisher_name";
            cboEmail.ValueMember = "publisher_id";
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
                DataGridViewRow row = this.cboBooks.Rows[e.RowIndex];
                txtBorrowerID.Text = row.Cells["book_id"].Value.ToString();
                txtBorrowerName.Text = row.Cells["book_name"].Value.ToString();
                cboAddress.SelectedValue = row.Cells["address_id"].Value.ToString();
                cboPhone.SelectedValue = row.Cells["phone_id"].Value.ToString();
                cboEmail.SelectedValue = row.Cells["email_id"].Value.ToString();
                dtpBorrowerDOB.Value = (DateTime)row.Cells["publishing_date"].Value;
            }
        }

        public void FillDataBooks()
        {
            string query = "SELECT * FROM Books";

            DataTable dataTable = new DataTable();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, connection);
            sqlDataAdapter.Fill(dataTable);
            cboBooks.DataSource = dataTable;
        }

        public void ClearDataBooks()
        {
            txtBorrowerID.Text = "";
            txtBorrowerName.Text = "";
            cboAddress.SelectedIndex = -1;
            cboPhone.SelectedIndex = -1;
            cboEmail.SelectedIndex = -1;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string name = txtBorrowerName.Text;
            string authorID = cboAddress.SelectedValue.ToString();
            string categoryID = cboPhone.SelectedValue.ToString();
            string publisherID = cboEmail.SelectedValue.ToString();
            DateTime publishingDate = dtpBorrowerDOB.Value;

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
                if (txtBorrowerID.Text == "")
                {
                    MessageBox.Show("Book ID is not provided.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (txtBorrowerName.Text == "" || cboAddress.SelectedValue == null || cboPhone.SelectedValue == null || cboEmail.SelectedValue == null)
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
                        cmd.Parameters.AddWithValue("@book_id", int.Parse(txtBorrowerID.Text));
                        cmd.Parameters.AddWithValue("@book_name", txtBorrowerName.Text);
                        cmd.Parameters.AddWithValue("@author_id", int.Parse(cboAddress.SelectedValue.ToString()));
                        cmd.Parameters.AddWithValue("@category_id", int.Parse(cboPhone.SelectedValue.ToString()));
                        cmd.Parameters.AddWithValue("@publisher_id", int.Parse(cboEmail.SelectedValue.ToString()));
                        cmd.Parameters.AddWithValue("@publishing_date", dtpBorrowerDOB.Value);
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
                if (txtBorrowerID.Text == "")
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
                        cmd.Parameters.AddWithValue("@book_id", int.Parse(txtBorrowerID.Text));
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
