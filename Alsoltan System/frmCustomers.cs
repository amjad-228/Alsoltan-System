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
    // نموذج إدارة العملاء
    // يتيح إضافة وعرض وتعديل العملاء في النظام
    public partial class frmCustomers : Form
    {
        public frmCustomers()
        {
            InitializeComponent();
        }

        // تحميل قائمة العملاء من قاعدة البيانات
        private void LoadCustomers(string searchTerm = "")
        {
            using (SqlConnection con = Database.GetConnection())
            {
                string query = "SELECT CustomerID, CustomerName, Phone, CurrentBalance, CreatedDate FROM Customers";

                // إذا كان هناك مصطلح بحث، نضيف شرط WHERE
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query += " WHERE CustomerName LIKE @searchTerm";
                }

                query += " ORDER BY CustomerName";

                SqlDataAdapter da = new SqlDataAdapter(query, con);

                // إذا كان هناك مصطلح بحث، نضيف المعامل
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    da.SelectCommand.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
                }

                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvCustomers.DataSource = dt;
            }
        }

        // تفعيل حقول إدخال البيانات للعميل
        private void EnableDataSection()
        {
            txtCustomerName.Enabled = true;
            txtPhone.Enabled = true;
            txtCurrentBalance.Enabled = true;
            btnSave.Visible = true;
            btnCancel.Visible = true;
        }

        // تعطيل حقول إدخال البيانات للعميل ومسحها
        private void DisableDataSection()
        {
            txtCustomerID.Text = "";
            txtCustomerName.Text = "";
            txtPhone.Text = "";
            txtCurrentBalance.Text = "";

            txtCustomerName.Enabled = false;
            txtPhone.Enabled = false;
            txtCurrentBalance.Enabled = false;
            btnSave.Visible = false;
            btnCancel.Visible = false;
        }

        // التحقق من صحة بيانات العميل
        private bool ValidateCustomerData()
        {
            if (string.IsNullOrWhiteSpace(txtCustomerName.Text))
            {
                MessageBox.Show("الرجاء إدخال اسم العميل");
                txtCustomerName.Focus();
                return false;
            }

            // التحقق من صحة الرصيد إذا تم إدخاله
            if (!string.IsNullOrWhiteSpace(txtCurrentBalance.Text))
            {
                decimal balance;
                if (!decimal.TryParse(txtCurrentBalance.Text, out balance))
                {
                    MessageBox.Show("الرجاء إدخال قيمة صحيحة للرصيد");
                    txtCurrentBalance.Focus();
                    return false;
                }
            }

            return true;
        }

        // حدث تحميل النموذج
        private void frmCustomers_Load(object sender, EventArgs e)
        {
            LoadCustomers();
            DisableDataSection();
        }

        // زر إضافة عميل جديد
        private void btnAdd_Click(object sender, EventArgs e)
        {
            DisableDataSection();
            EnableDataSection();
            txtCustomerName.Focus();
        }

        // زر حفظ بيانات العميل
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateCustomerData())
                return;

            try
            {
                using (SqlConnection con = Database.GetConnection())
                {
                    string query;
                    SqlCommand cmd;

                    // إذا كان هناك رقم عميل، نقوم بالتحديث، وإلا نقوم بالإضافة
                    if (!string.IsNullOrEmpty(txtCustomerID.Text))
                    {
                        // تحديث بيانات العميل الموجود
                        query = "UPDATE Customers SET CustomerName = @name, Phone = @phone, CurrentBalance = @balance WHERE CustomerID = @id";
                        cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@id", txtCustomerID.Text);
                    }
                    else
                    {
                        // إضافة عميل جديد
                        query = "INSERT INTO Customers (CustomerName, Phone, CurrentBalance, CreatedBy) VALUES (@name, @phone, @balance, @createdBy)";
                        cmd = new SqlCommand(query, con);
                    }

                    cmd.Parameters.AddWithValue("@name", txtCustomerName.Text.Trim());
                    cmd.Parameters.AddWithValue("@phone", txtPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@balance", string.IsNullOrWhiteSpace(txtCurrentBalance.Text) ? 0 : decimal.Parse(txtCurrentBalance.Text));
                    cmd.Parameters.AddWithValue("@createdBy", "المستخدم الحالي"); // يجب استبدال هذا باسم المستخدم الفعلي

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("تم حفظ بيانات العميل بنجاح");
                    LoadCustomers();
                    DisableDataSection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء حفظ بيانات العميل: " + ex.Message);
            }
        }

        // زر إلغاء العملية
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DisableDataSection();
        }

        // حدث النقر على خلية في جدول العملاء
        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCustomers.Rows[e.RowIndex];
                txtCustomerID.Text = row.Cells["CustomerID"].Value.ToString();
                txtCustomerName.Text = row.Cells["CustomerName"].Value.ToString();
                txtPhone.Text = row.Cells["Phone"].Value?.ToString() ?? "";
                txtCurrentBalance.Text = row.Cells["CurrentBalance"].Value?.ToString() ?? "";
            }
        }

        // حدث البحث التلقائي أثناء الكتابة
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadCustomers(txtSearch.Text.Trim());
        }

        private void pnlData_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgvCustomers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
