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
    // نموذج إدارة المنتجات
    // يتيح إضافة وعرض وتعديل وحذف المنتجات في النظام
    // مع ربط كل منتج بمورد معين
    public partial class frmProductsNew : Form
    {
        public frmProductsNew()
        {
            InitializeComponent();
        }

        // تحميل قائمة المنتجات من قاعدة البيانات
        private void LoadProducts(string searchTerm = "")
        {
            using (SqlConnection con = Database.GetConnection())
            {
                // استعلام يجلب المنتجات مع اسم المورد
                string query = @"SELECT p.ID, p.Name, p.Price, p.Quantity, s.SupplierName 
                                FROM Products p 
                                LEFT JOIN Suppliers s ON p.SupplierID = s.SupplierID";

                // إذا كان هناك مصطلح بحث، نضيف شرط WHERE
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query += " WHERE p.Name LIKE @searchTerm";
                }

                query += " ORDER BY p.Name";

                SqlDataAdapter da = new SqlDataAdapter(query, con);

                // إذا كان هناك مصطلح بحث، نضيف المعامل
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    da.SelectCommand.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
                }

                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvProducts.DataSource = dt;

                // تغيير أسماء الأعمدة لتكون بالعربية
                if (dgvProducts.Columns["ID"] != null)
                    dgvProducts.Columns["ID"].HeaderText = "رقم المنتج";
                if (dgvProducts.Columns["Name"] != null)
                    dgvProducts.Columns["Name"].HeaderText = "اسم المنتج";
                if (dgvProducts.Columns["Price"] != null)
                    dgvProducts.Columns["Price"].HeaderText = "السعر";
                if (dgvProducts.Columns["Quantity"] != null)
                    dgvProducts.Columns["Quantity"].HeaderText = "الكمية";
                if (dgvProducts.Columns["SupplierName"] != null)
                    dgvProducts.Columns["SupplierName"].HeaderText = "المورد";
            }
        }

        // تحميل قائمة الموردين في القائمة المنسدلة
        private void LoadSuppliers()
        {
            cmbSuppliers.Items.Clear();
            cmbSuppliers.DisplayMember = "SupplierName";
            cmbSuppliers.ValueMember = "SupplierID";

            using (SqlConnection con = Database.GetConnection())
            {
                string query = "SELECT SupplierID, SupplierName FROM Suppliers ORDER BY SupplierName";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmbSuppliers.DataSource = dt;
            }
        }

        // تفعيل حقول إدخال البيانات للمنتج
        private void EnableDataSection()
        {
            txtProductName.Enabled = true;
            txtPrice.Enabled = true;
            txtQuantity.Enabled = true;
            cmbSuppliers.Enabled = true;
            btnSave.Visible = true;
            btnCancel.Visible = true;
        }

        // تعطيل حقول إدخال البيانات للمنتج ومسحها
        private void DisableDataSection()
        {
            txtProductID.Text = "";
            txtProductName.Text = "";
            txtPrice.Text = "";
            txtQuantity.Text = "";
            cmbSuppliers.SelectedIndex = -1;

            txtProductName.Enabled = false;
            txtPrice.Enabled = false;
            txtQuantity.Enabled = false;
            cmbSuppliers.Enabled = false;
            btnSave.Visible = false;
            btnCancel.Visible = false;
        }

        // التحقق من صحة بيانات المنتج
        private bool ValidateProductData()
        {
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                MessageBox.Show("الرجاء إدخال اسم المنتج");
                txtProductName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("الرجاء إدخال السعر");
                txtPrice.Focus();
                return false;
            }

            decimal price;
            if (!decimal.TryParse(txtPrice.Text, out price) || price <= 0)
            {
                MessageBox.Show("الرجاء إدخال قيمة صحيحة للسعر");
                txtPrice.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtQuantity.Text))
            {
                MessageBox.Show("الرجاء إدخال الكمية");
                txtQuantity.Focus();
                return false;
            }

            int quantity;
            if (!int.TryParse(txtQuantity.Text, out quantity) || quantity < 0)
            {
                MessageBox.Show("الرجاء إدخال قيمة صحيحة للكمية");
                txtQuantity.Focus();
                return false;
            }

            if (cmbSuppliers.SelectedIndex == -1)
            {
                MessageBox.Show("الرجاء اختيار المورد");
                cmbSuppliers.Focus();
                return false;
            }

            return true;
        }

        // حدث تحميل النموذج
        private void frmProductsNew_Load(object sender, EventArgs e)
        {
            LoadProducts();
            LoadSuppliers();
            DisableDataSection();
            this.dgvProducts.ClearSelection();
        }

        // زر إضافة منتج جديد
        private void btnAdd_Click(object sender, EventArgs e)
        {
            DisableDataSection();
            EnableDataSection();
            txtProductName.Focus();
        }

        // زر تعديل منتج
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("الرجاء اختيار المنتج المراد تعديله");
                return;
            }

            EnableDataSection();
        }

        // زر حذف منتج
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("الرجاء اختيار المنتج المراد حذفه");
                return;
            }

            DialogResult result = MessageBox.Show("هل أنت متأكد من حذف هذا المنتج؟", "تأكيد الحذف",
                                                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    int productID = Convert.ToInt32(dgvProducts.SelectedRows[0].Cells["ID"].Value);

                    using (SqlConnection con = Database.GetConnection())
                    {
                        string query = "DELETE FROM Products WHERE ID = @id";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@id", productID);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MessageBox.Show("تم حذف المنتج بنجاح");
                        LoadProducts();
                        DisableDataSection();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("حدث خطأ أثناء حذف المنتج: " + ex.Message);
                }
            }
        }

        // زر حفظ بيانات المنتج
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateProductData())
                return;

            try
            {
                using (SqlConnection con = Database.GetConnection())
                {
                    string query;
                    SqlCommand cmd;

                    // إذا كان هناك رقم منتج، نقوم بالتحديث، وإلا نقوم بالإضافة
                    if (!string.IsNullOrEmpty(txtProductID.Text))
                    {
                        // تحديث بيانات المنتج الموجود
                        query = "UPDATE Products SET Name = @name, Price = @price, Quantity = @quantity, SupplierID = @supplierID WHERE ID = @id";
                        cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@id", txtProductID.Text);
                    }
                    else
                    {
                        // إضافة منتج جديد
                        query = "INSERT INTO Products (Name, Price, Quantity, SupplierID) VALUES (@name, @price, @quantity, @supplierID)";
                        cmd = new SqlCommand(query, con);
                    }

                    cmd.Parameters.AddWithValue("@name", txtProductName.Text.Trim());
                    cmd.Parameters.AddWithValue("@price", decimal.Parse(txtPrice.Text));
                    cmd.Parameters.AddWithValue("@quantity", int.Parse(txtQuantity.Text));
                    cmd.Parameters.AddWithValue("@supplierID", cmbSuppliers.SelectedValue);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("تم حفظ بيانات المنتج بنجاح");
                    LoadProducts();
                    DisableDataSection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء حفظ بيانات المنتج: " + ex.Message);
            }
        }

        // زر إلغاء العملية
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DisableDataSection();
            dgvProducts.ClearSelection();

        }

        // حدث النقر على خلية في جدول المنتجات
        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvProducts.Rows[e.RowIndex];
                txtProductID.Text = row.Cells["ID"].Value.ToString();
                txtProductName.Text = row.Cells["Name"].Value.ToString();
                txtPrice.Text = row.Cells["Price"].Value?.ToString() ?? "";
                txtQuantity.Text = row.Cells["Quantity"].Value?.ToString() ?? "";

                // تحديد المورد في القائمة المنسدلة
                string supplierName = row.Cells["SupplierName"].Value?.ToString();
                if (!string.IsNullOrEmpty(supplierName))
                {
                    foreach (DataRowView item in cmbSuppliers.Items)
                    {
                        if (item["SupplierName"].ToString() == supplierName)
                        {
                            cmbSuppliers.SelectedItem = item;
                            break;
                        }
                    }
                }
                else
                {
                    cmbSuppliers.SelectedIndex = -1;
                }
            }
        }

        // حدث البحث التلقائي أثناء الكتابة
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProducts(txtSearch.Text.Trim());
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pnlSearch_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgvProducts_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvProducts.ClearSelection();
        }

        private void lblQuantity_Click(object sender, EventArgs e)
        {

        }

        private void lblSupplier_Click(object sender, EventArgs e)
        {

        }

        private void dgvProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
