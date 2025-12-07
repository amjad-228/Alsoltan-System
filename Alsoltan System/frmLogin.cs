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

namespace Alsoltan_System
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private bool CheckLoginDataIsValid(string username, string password)
        {
            if (username.Equals("") || password.Equals(""))
            {
                return false;
            }
            return true;
        }

        private bool CheckLogin(string username, string password)
        {
            using (SqlConnection con = Database.GetConnection())
            {
                // تشفير كلمة المرور قبل المقارنة
                string hashedPassword = SecurityHelper.HashPassword(password);
                string query = "SELECT COUNT(*) FROM users WHERE Username = @u AND Password = @p";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@p", hashedPassword);
                con.Open();
                int count = (int)cmd.ExecuteScalar();

                return count > 0;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (!CheckLoginDataIsValid(username, password))
            {
                MessageBox.Show("الرجاء ادحال اسم المستخدم وكلمة المرور");
                return;
            }


            if (CheckLogin(txtUsername.Text, txtPassword.Text))
            {
                Form Main = new frmMain();
                Main.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("اسم المستخدم او كلمة مرور غير صحيحة");
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }
    }
}
