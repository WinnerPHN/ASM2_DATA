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

namespace ASM2_DATA
{
    public partial class ViewLibraryForm : Form
    {
        SqlConnection connection;
        private Form currentFormChild;
        public ViewLibraryForm(string role)
        {
            InitializeComponent();
            var builder = new SqlConnectionStringBuilder(@"Server=WINNERN\SQLEXPRESS;Database=LibraryManagement1;");
            builder.Encrypt = true;
            builder.TrustServerCertificate = true;
            builder.IntegratedSecurity = true;
            connection = new SqlConnection(builder.ConnectionString);
        }
        private void OpenChildForm(Form childForm) //Tạo phương thức OpenChildForm để mở form con.
        {
            if (currentFormChild != null)
            {
                currentFormChild.Close();
            }
            currentFormChild = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            this.Controls.Add(childForm);
            this.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void bookManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new BookManagement());
        }
        public void Filldata()
        {
            string query = "select * from Books";
            DataTable tbl = new DataTable();
            SqlDataAdapter ad = new SqlDataAdapter(query, connection);
            ad.Fill(tbl);
            dataGridView1.DataSource = tbl;
            connection.Close();
        }
        private void ViewLibraryForm_Load(object sender, EventArgs e)
        {
            connection.Open();
            Filldata();
        }

        private void borrowerManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new BorrowerManagement());
        }

        private void ticketManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new TicketManagement());
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
