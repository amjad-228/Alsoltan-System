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
    public partial class frmAddUser : Form
    {
        public frmAddUser()
        {
            InitializeComponent();
        }

        private void frmAddUser_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // التحقق من صحة البيانات
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("الرجاء ملء جميع الحقول المطلوبة");
                return;
            }

            // تشفير كلمة المرور
            string hashedPassword = SecurityHelper.HashPassword(txtPassword.Text.Trim());

            // حفظ المستخدم في قاعدة البيانات
            try
            {
                using (SqlConnection con = Database.GetConnection())
                {
                    string query = "INSERT INTO users (Username, FirstName, LastName, Password) VALUES (@username, @firstName, @lastName, @password)";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim());
                    cmd.Parameters.AddWithValue("@firstName", txtFirstName.Text.Trim());
                    cmd.Parameters.AddWithValue("@lastName", txtLastName.Text.Trim());
                    cmd.Parameters.AddWithValue("@password", hashedPassword);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("تمت إضافة المستخدم بنجاح");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء إضافة المستخدم: " + ex.Message);
            }
    }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();  
        }
    }
}
