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
    // نموذج فواتير المشتريات
    // يتيح إنشاء وحفظ فواتير المشتريات من الموردين
    // مع دعم الدفع النقدي والآجل
    public partial class frmPurchaseInvoices : Form
    {
        public frmPurchaseInvoices()
        {
            InitializeComponent();
        }

        // قائمة لتخزين عناصر الفاتورة
        private List<PurchaseInvoiceItem> invoiceItems = new List<PurchaseInvoiceItem>();

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
            // عدم تحديد أي مورد تلقائياً
            cmbSuppliers.SelectedIndex = -1;
        }

        // تحميل قائمة المنتجات حسب المورد المختار
        private void LoadProductsBySupplier()
        {
            if (cmbSuppliers.SelectedValue != null && cmbSuppliers.SelectedIndex != -1 )
            {
                using (SqlConnection con = Database.GetConnection())
                {
                    string query = "SELECT ID as ProductID, Name as ProductName FROM Products WHERE SupplierID = @supplierID ORDER BY Name";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@supplierID", cmbSuppliers.SelectedValue);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // تعيين مصدر البيانات الجديد
                    cmbProducts.DataSource = null; // مسح المصدر السابق
                    cmbProducts.DataSource = dt;
                    cmbProducts.DisplayMember = "ProductName";
                    cmbProducts.ValueMember = "ProductID";

                    // عدم تحديد أي منتج تلقائياً
                    cmbProducts.SelectedIndex = -1;

                }
            }
            else
            {
                // إذا لم يتم اختيار مورد، نمسح القائمة
                cmbProducts.DataSource = null;
                cmbProducts.Items.Clear();
            }
        }

        // تحديث جدول عناصر الفاتورة
        private void RefreshInvoiceItemsGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("المنتج");
            dt.Columns.Add("الكمية");
            dt.Columns.Add("السعر");
            dt.Columns.Add("الإجمالي");

            foreach (var item in invoiceItems)
            {
                DataRow row = dt.NewRow();
                row["المنتج"] = item.ProductName;
                row["الكمية"] = item.Quantity;
                row["السعر"] = item.UnitPrice.ToString("F2");
                row["الإجمالي"] = item.Total.ToString("F2");
                dt.Rows.Add(row);
            }

            dgvInvoiceItems.DataSource = dt;
        }

        // حساب الإجماليات
        private void CalculateTotals()
        {
            decimal subTotal = 0;
            foreach (var item in invoiceItems)
            {
                subTotal += item.Total;
            }

            decimal discount = 0;
            if (!string.IsNullOrWhiteSpace(txtDiscount.Text))
            {
                decimal.TryParse(txtDiscount.Text, out discount);
            }

            decimal totalAmount = subTotal - discount;

            lblSubTotal.Text = "الإجمالي الفرعي: " + subTotal.ToString("F2");
            lblTotalDiscount.Text = "إجمالي الخصم: " + discount.ToString("F2");
            lblTotalAmount.Text = "الإجمالي النهائي: " + totalAmount.ToString("F2");
        }

        // توليد رقم فاتورة تلقائي
        private string GenerateInvoiceNumber()
        {
            // يمكن تحسين هذا الكود لاحقاً لتوليد أرقام فواتير متسلسلة
            return "PI-" + DateTime.Now.ToString("yyyyMMdd-HHmmss");
        }

        // تفعيل حقول إدخال البيانات للمنتجات
        private void EnableProductFields()
        {
            cmbProducts.Enabled = true;
            txtQuantity.Enabled = true;
            txtPrice.Enabled = true;
            btnAddProduct.Enabled = true;
        }

        // تعطيل حقول إدخال البيانات للمنتجات
        private void DisableProductFields()
        {
            // عدم تحديد أي منتج تلقائياً
            cmbProducts.SelectedIndex = -1;
            cmbProducts.Enabled = false;
            txtQuantity.Text = "1";
            txtPrice.Text = "";
            btnAddProduct.Enabled = false;
        }

        // التحقق من صحة بيانات الفاتورة
        private bool ValidateInvoice()
        {
            if (cmbSuppliers.SelectedIndex == -1)
            {
                MessageBox.Show("الرجاء اختيار المورد");
                cmbSuppliers.Focus();
                return false;
            }

            if (invoiceItems.Count == 0)
            {
                MessageBox.Show("الرجاء إضافة منتجات للفاتورة");
                cmbProducts.Focus();
                return false;
            }

            // التحقق من صحة الخصم إذا تم إدخاله
            if (!string.IsNullOrWhiteSpace(txtDiscount.Text))
            {
                decimal discount;
                if (!decimal.TryParse(txtDiscount.Text, out discount) || discount < 0)
                {
                    MessageBox.Show("الرجاء إدخال قيمة صحيحة للخصم");
                    txtDiscount.Focus();
                    return false;
                }
            }

            return true;
        }

        // التحقق من صحة بيانات المنتج
        private bool ValidateProduct()
        {
            if (cmbProducts.SelectedIndex == -1)
            {
                MessageBox.Show("الرجاء اختيار المنتج");
                cmbProducts.Focus();
                return false;
            }

            int quantity;
            if (!int.TryParse(txtQuantity.Text, out quantity) || quantity <= 0)
            {
                MessageBox.Show("الرجاء إدخال كمية صحيحة");
                txtQuantity.Focus();
                return false;
            }

            decimal price;
            if (!decimal.TryParse(txtPrice.Text, out price) || price <= 0)
            {
                MessageBox.Show("الرجاء إدخال سعر صحيح");
                txtPrice.Focus();
                return false;
            }

            return true;
        }

        // حدث تحميل النموذج
        private void frmPurchaseInvoices_Load(object sender, EventArgs e)
        {
            // تعيين التاريخ الحالي كتاريخ الفاتورة
            dtpInvoiceDate.Value = DateTime.Now;

            // توليد رقم فاتورة تلقائي
            txtInvoiceNumber.Text = GenerateInvoiceNumber();

            // تحميل الموردين
            LoadSuppliers();

            // تعطيل حقول المنتجات حتى يتم اختيار مورد
            DisableProductFields();
        }

        // حدث تغيير اختيار المورد
        private void cmbSuppliers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSuppliers.SelectedIndex != -1)
            {
                LoadProductsBySupplier();
                EnableProductFields();
            }
            else
            {
                DisableProductFields();
            }
        }

        // حدث تغيير اختيار المنتج
        private void cmbProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProducts.SelectedIndex != -1)
            {
                // عند اختيار منتج، نجلب سعره الافتراضي
                DataRowView selectedProduct = (DataRowView)cmbProducts.SelectedItem;
                int productID = Convert.ToInt32(selectedProduct["ProductID"]);

                using (SqlConnection con = Database.GetConnection())
                {
                    string query = "SELECT Price FROM Products WHERE ID = @productID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@productID", productID);

                    con.Open();
                    object result = cmd.ExecuteScalar();
                    con.Close();

                    if (result != null)
                    {
                        txtPrice.Text = result.ToString();
                    }
                }
            }
        }

        // زر إضافة منتج للفاتورة
        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            if (!ValidateProduct())
                return;

            // إنشاء عنصر فاتورة جديد
            PurchaseInvoiceItem item = new PurchaseInvoiceItem();
            DataRowView selectedProduct = (DataRowView)cmbProducts.SelectedItem;

            item.ProductID = Convert.ToInt32(selectedProduct["ProductID"]);
            item.ProductName = selectedProduct["ProductName"].ToString();
            item.Quantity = int.Parse(txtQuantity.Text);
            item.UnitPrice = decimal.Parse(txtPrice.Text);
            item.Total = item.Quantity * item.UnitPrice;

            // إضافة العنصر إلى القائمة
            invoiceItems.Add(item);

            // تحديث واجهة المستخدم
            RefreshInvoiceItemsGrid();
            CalculateTotals();

            // إعادة تعيين حقول الإدخال
            txtQuantity.Text = "1";
            txtPrice.Text = "";
            cmbProducts.SelectedIndex = -1; // عدم تحديد أي منتج تلقائياً
            cmbProducts.Focus();
        }

        // زر حفظ الفاتورة
        private void btnSaveInvoice_Click(object sender, EventArgs e)
        {
            if (!ValidateInvoice())
                return;

            try
            {
                using (SqlConnection con = Database.GetConnection())
                {
                    con.Open();
                    using (SqlTransaction transaction = con.BeginTransaction())
                    {
                        try
                        {
                            // حفظ رأس الفاتورة
                            string invoiceQuery = @"INSERT INTO PurchaseInvoices 
                                                    (InvoiceNumber, InvoiceDate, SupplierID, PaymentType, 
                                                     SubTotal, Discount, TotalAmount, Notes, CreatedBy) 
                                                    VALUES (@invoiceNumber, @invoiceDate, @supplierID, @paymentType, 
                                                            @subTotal, @discount, @totalAmount, @notes, @createdBy);
                                                    SELECT SCOPE_IDENTITY();";

                            SqlCommand invoiceCmd = new SqlCommand(invoiceQuery, con, transaction);
                            invoiceCmd.Parameters.AddWithValue("@invoiceNumber", txtInvoiceNumber.Text);
                            invoiceCmd.Parameters.AddWithValue("@invoiceDate", dtpInvoiceDate.Value);
                            invoiceCmd.Parameters.AddWithValue("@supplierID", cmbSuppliers.SelectedValue);
                            invoiceCmd.Parameters.AddWithValue("@paymentType", rbCash.Checked ? "Cash" : "Credit");
                            invoiceCmd.Parameters.AddWithValue("@subTotal", decimal.Parse(lblSubTotal.Text.Replace("الإجمالي الفرعي: ", "")));
                            invoiceCmd.Parameters.AddWithValue("@discount", string.IsNullOrWhiteSpace(txtDiscount.Text) ? 0 : decimal.Parse(txtDiscount.Text));
                            invoiceCmd.Parameters.AddWithValue("@totalAmount", decimal.Parse(lblTotalAmount.Text.Replace("الإجمالي النهائي: ", "")));
                            invoiceCmd.Parameters.AddWithValue("@notes", txtNotes.Text.Trim());
                            invoiceCmd.Parameters.AddWithValue("@createdBy", "المستخدم الحالي"); // يجب استبدال هذا باسم المستخدم الفعلي

                            int invoiceId = Convert.ToInt32(invoiceCmd.ExecuteScalar());

                            // حفظ عناصر الفاتورة
                            foreach (var item in invoiceItems)
                            {
                                string itemQuery = @"INSERT INTO PurchaseInvoiceItems 
                                                     (InvoiceID, ProductID, Quantity, UnitPrice, Total) 
                                                     VALUES (@invoiceId, @productId, @quantity, @unitPrice, @total)";

                                SqlCommand itemCmd = new SqlCommand(itemQuery, con, transaction);
                                itemCmd.Parameters.AddWithValue("@invoiceId", invoiceId);
                                itemCmd.Parameters.AddWithValue("@productId", item.ProductID);
                                itemCmd.Parameters.AddWithValue("@quantity", item.Quantity);
                                itemCmd.Parameters.AddWithValue("@unitPrice", item.UnitPrice);
                                itemCmd.Parameters.AddWithValue("@total", item.Total);
                                itemCmd.ExecuteNonQuery();
                            }

                            // إذا كان الدفع آجل، تحديث رصيد المورد
                            if (rbCredit.Checked)
                            {
                                string updateSupplierQuery = @"UPDATE Suppliers 
                                                              SET CurrentBalance = ISNULL(CurrentBalance, 0) + @amount 
                                                              WHERE SupplierID = @supplierID";

                                SqlCommand updateSupplierCmd = new SqlCommand(updateSupplierQuery, con, transaction);
                                updateSupplierCmd.Parameters.AddWithValue("@amount", decimal.Parse(lblTotalAmount.Text.Replace("الإجمالي النهائي: ", "")));
                                updateSupplierCmd.Parameters.AddWithValue("@supplierID", cmbSuppliers.SelectedValue);
                                updateSupplierCmd.ExecuteNonQuery();
                            }

                            transaction.Commit();

                            MessageBox.Show("تم حفظ فاتورة المشتريات بنجاح");

                            // إعادة تعيين النموذج لفاتورة جديدة
                            ResetForm();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء حفظ فاتورة المشتريات: " + ex.Message);
            }
        }

        // إعادة تعيين النموذج لفاتورة جديدة
        private void ResetForm()
        {
            txtInvoiceNumber.Text = GenerateInvoiceNumber();
            dtpInvoiceDate.Value = DateTime.Now;
            cmbSuppliers.SelectedIndex = -1;
            rbCash.Checked = true;
            txtDiscount.Text = "0";
            txtNotes.Text = "";

            invoiceItems.Clear();
            RefreshInvoiceItemsGrid();
            CalculateTotals();

            DisableProductFields();
        }

        // زر إلغاء الفاتورة
        private void btnCancelInvoice_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        // حدث تغيير قيمة الخصم
        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            CalculateTotals();
        }

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pnlInvoiceInfo_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblInvoiceDate_Click(object sender, EventArgs e)
        {

        }

        private void lblSubTotal_Click(object sender, EventArgs e)
        {

        }
    }

    // فئة لتمثيل عنصر فاتورة الشراء
    public class PurchaseInvoiceItem
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
    }
}
