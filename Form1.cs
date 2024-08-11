using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace ASM2_DATA
{
    public partial class Form1 : Form
    {
        SqlConnection connection;
        
        public Form1()
        {
            InitializeComponent();
            var builder = new SqlConnectionStringBuilder(@"Server=WINNERN\SQLEXPRESS;Database=LibraryManagement1;");
            builder.Encrypt = true;
            builder.TrustServerCertificate = true;
            builder.IntegratedSecurity = true;
            connection = new SqlConnection(builder.ConnectionString);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string query = "select * from Accounts where username =@username and user_password =@password";

            connection.Open();
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@username", SqlDbType.VarChar);
            cmd.Parameters["@username"].Value = username;
            cmd.Parameters.AddWithValue("@password", SqlDbType.VarChar);
            cmd.Parameters["@password"].Value = password;
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string role = reader["user_role"].ToString();
                MessageBox.Show(this, "Login successful!", "Result", MessageBoxButtons.OK, MessageBoxIcon.None);
                this.Hide();
                ViewLibraryForm viewLibraryForm = new ViewLibraryForm(role);
                viewLibraryForm.ShowDialog();
                this.Dispose();
            }
            else
            {
                lblError.Text = "Wrong username or password";
            }
            connection.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Do you want to exit ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
