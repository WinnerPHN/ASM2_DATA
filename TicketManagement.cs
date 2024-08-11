using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ASM2_DATA
{
    public partial class TicketManagement : Form
    {
        SqlConnection connection;

        public TicketManagement()
        {
            InitializeComponent();
            connection = new SqlConnection(@"Data Source=V\SQLEXPRESS;Initial Catalog=LibraryManagement1;Integrated Security=True;");
        }

        private void TicketManagement_Load(object sender, EventArgs e)
        {
            FillData();
        }

        private void FillData()
        {
            try
            {
                string query = "SELECT * FROM Tickets"; // Adjust the query based on your table schema
                DataTable tbl = new DataTable();
                SqlDataAdapter ad = new SqlDataAdapter(query, connection);
                ad.Fill(tbl);
                dgvTicket.DataSource = tbl;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvTicket_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dgvTicket.Rows[e.RowIndex];
                // Populate your form fields based on selected row
                // e.g., txtTicketID.Text = row.Cells["ticket_id"].Value.ToString();
            }
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            FillData();
        }

        private void TicketManagement_Load_1(object sender, EventArgs e)
        {

        }
    }
}
