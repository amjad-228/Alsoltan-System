using System;
using System.Collections;
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
    // نموذج إدارة الموردين
    // يتيح إضافة وعرض وتعديل الموردين في النظام
    public partial class frmSuppliers : Form
    {
        public frmSuppliers()
        {
            InitializeComponent();
        }

        // تحميل قائمة الموردين من قاعدة البيانات
        private void LoadSuppliers(string searchTerm = "")
        {
            using (SqlConnection con = Database.GetConnection())
            {
                string query = "SELECT SupplierID, SupplierName, Phone, CreatedDate FROM Suppliers";
                // إذا كان هناك مصطلح بحث، نضيف شرط WHERE
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query += " WHERE SupplierName LIKE @searchTerm";
                }
                query += " ORDER BY SupplierName";

                SqlDataAdapter  da = new SqlDataAdapter(query, con);

                // إذا كان هناك مصطلح بحث، نضيف المعامل
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    da.SelectCommand.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
                }

                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvSuppliers.DataSource = dt;
            }
        }

        // تفعيل حقول إدخال البيانات للمورد
        private void EnableDataSection()
        {
            txtSupplierName.Enabled = true;
            txtPhone.Enabled = true;
            btnSave.Visible = true;
            btnCancel.Visible = true;
        }

        // تعطيل حقول إدخال البيانات للمورد ومسحها
        private void DisableDataSection()
        {
            txtSupplierID.Text = "";
            txtSupplierName.Text = "";
            txtPhone.Text = "";

            txtSupplierName.Enabled = false;
            txtPhone.Enabled = false;
            btnSave.Visible = false;
            btnCancel.Visible = false;
        }

        // التحقق من صحة بيانات المورد
        private bool ValidateSupplierData()
        {
            if (string.IsNullOrWhiteSpace(txtSupplierName.Text))
            {
                MessageBox.Show("الرجاء إدخال اسم المورد");
                txtSupplierName.Focus();
                return false;
            }
            return true;
        }


        private void frmSuppliers_Load(object sender, EventArgs e)
        {
            LoadSuppliers();
            DisableDataSection();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DisableDataSection();
            EnableDataSection();
            txtSupplierName.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateSupplierData())
                return;

            try
            {
                using (SqlConnection con = Database.GetConnection())
                {
                    string query;
                    SqlCommand cmd;

                    // إذا كان هناك رقم مورد، نقوم بالتحديث، وإلا نقوم بالإضافة
                    if (!string.IsNullOrEmpty(txtSupplierID.Text))
                    {
                        // تحديث بيانات المورد الموجود
                        query = "UPDATE Suppliers SET SupplierName = @name, Phone = @phone WHERE SupplierID = @id";
                        cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@id", txtSupplierID.Text);
                    }
                    else
                    {
                        // إضافة مورد جديد
                        query = "INSERT INTO Suppliers (SupplierName, Phone, CreatedBy) VALUES (@name, @phone, @createdBy)";
                        cmd = new SqlCommand(query, con);
                    }

                    cmd.Parameters.AddWithValue("@name", txtSupplierName.Text.Trim());
                    cmd.Parameters.AddWithValue("@phone", txtPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@createdBy", "المستخدم الحالي"); // يجب استبدال هذا باسم المستخدم الفعلي

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("تم حفظ بيانات المورد بنجاح");
                    LoadSuppliers();
                    DisableDataSection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء حفظ بيانات المورد: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DisableDataSection();
        }

        private void dgvSuppliers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSuppliers.Rows[e.RowIndex];
                txtSupplierID.Text = row.Cells["SupplierID"].Value.ToString();
                txtSupplierName.Text = row.Cells["SupplierName"].Value.ToString();
                txtPhone.Text = row.Cells["Phone"].Value?.ToString() ?? "";
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadSuppliers(txtSearch.Text.Trim());
        }
    }
}
