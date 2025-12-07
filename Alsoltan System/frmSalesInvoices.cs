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
    // نموذج فواتير المبيعات
    // يتيح إنشاء وحفظ فواتير المبيعات للعملاء
    // مع دعم الدفع النقدي والآجل
    public partial class frmSalesInvoices : Form
    {
        public frmSalesInvoices()
        {
            InitializeComponent();
        }

        // قائمة لتخزين عناصر الفاتورة
        private List<SalesInvoiceItem> invoiceItems = new List<SalesInvoiceItem>();

        // تحميل قائمة العملاء في القائمة المنسدلة
        private void LoadCustomers()
        {
            using (SqlConnection con = Database.GetConnection())
            {
                string query = "SELECT CustomerID, CustomerName FROM Customers ORDER BY CustomerName";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbCustomers.DataSource = dt;
                cmbCustomers.DisplayMember = "CustomerName";
                cmbCustomers.ValueMember = "CustomerID";

                // عدم تحديد أي عميل تلقائياً
                cmbCustomers.SelectedIndex = -1;
            }
        }

        // تحميل قائمة المنتجات
        private void LoadProducts()
        {
            using (SqlConnection con = Database.GetConnection())
            {
                string query = @"SELECT ID as ProductID, Name as ProductName, Price, Quantity as AvailableQuantity 
                               FROM Products 
                               WHERE Quantity > 0 
                               ORDER BY Name";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbProducts.DataSource = dt;
                cmbProducts.DisplayMember = "ProductName";
                cmbProducts.ValueMember = "ProductID";

                // عدم تحديد أي منتج تلقائياً
                cmbProducts.SelectedIndex = -1;
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

            // حساب المبلغ المدفوع والرصيد المتبقي
            decimal amountPaid = 0;
            decimal balance = 0;

            if (rbCash.Checked)
            {
                // في حالة الدفع النقدي، المبلغ المدفوع يساوي الإجمالي النهائي
                amountPaid = totalAmount;
            }
            else
            {
                // في حالة الدفع الآجل، المبلغ المدفوع صفر والرصيد يساوي الإجمالي النهائي
                amountPaid = 0;
                balance = totalAmount;
            }

            lblAmountPaid.Text = "المبلغ المدفوع: " + amountPaid.ToString("F2");
            lblBalance.Text = "الرصيد المتبقي: " + balance.ToString("F2");
        }

        // توليد رقم فاتورة تلقائي
        private string GenerateInvoiceNumber()
        {
            // يمكن تحسين هذا الكود لاحقاً لتوليد أرقام فواتير متسلسلة
            return "SI-" + DateTime.Now.ToString("yyyyMMdd-HHmmss");
        }

        // تفعيل حقول إدخال البيانات للمنتجات
        private void EnableProductFields()
        {
            cmbProducts.Enabled = true;
            txtQuantity.Enabled = true;
            btnAddProduct.Enabled = true;
        }

        // تعطيل حقول إدخال البيانات للمنتجات
        private void DisableProductFields()
        {
            cmbProducts.Enabled = false;
            txtQuantity.Text = "1";
            txtPrice.Text = "";
            lblAvailableQuantity.Text = "الكمية المتوفرة: 0";
            btnAddProduct.Enabled = false;
        }

        // التحقق من صحة بيانات الفاتورة
        private bool ValidateInvoice()
        {
            // في حالة الدفع الآجل، يجب اختيار عميل
            if (rbCredit.Checked && cmbCustomers.SelectedIndex == -1)
            {
                MessageBox.Show("الرجاء اختيار العميل للدفع الآجل");
                cmbCustomers.Focus();
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

            // التحقق من توفر الكمية المطلوبة
            DataRowView selectedProduct = (DataRowView)cmbProducts.SelectedItem;
            int availableQuantity = Convert.ToInt32(selectedProduct["AvailableQuantity"]);

            if (quantity > availableQuantity)
            {
                MessageBox.Show("الكمية المطلوبة أكبر من الكمية المتوفرة (" + availableQuantity + ")");
                txtQuantity.Focus();
                return false;
            }

            return true;
        }

        // حدث تحميل النموذج
        private void frmSalesInvoices_Load(object sender, EventArgs e)
        {
            // تعيين التاريخ الحالي كتاريخ الفاتورة
            dtpInvoiceDate.Value = DateTime.Now;
            dtpInvoiceDate.Enabled = false; // تعطيل تعديل التاريخ

            // توليد رقم فاتورة تلقائي
            txtInvoiceNumber.Text = GenerateInvoiceNumber();
            txtInvoiceNumber.ReadOnly = true; // منع تعديل رقم الفاتورة

            // تحميل العملاء والمنتجات
            LoadCustomers();
            LoadProducts();

            // تعطيل حقول المنتجات حتى يتم اختيار منتج
           // DisableProductFields();

            // تعيين نوع الدفع الافتراضي إلى نقدي
            rbCash.Checked = true;

            // تعيين الخصم الافتراضي إلى صفر
            txtDiscount.Text = "0";
        }

        // حدث تغيير اختيار المنتج
        private void cmbProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProducts.SelectedIndex != -1 && cmbProducts.SelectedValue != null)
            {
                // عند اختيار منتج، نجلب سعره والكمية المتوفرة
                DataRowView selectedProduct = (DataRowView)cmbProducts.SelectedItem;

                txtPrice.Text = selectedProduct["Price"].ToString();
                txtPrice.ReadOnly = true; // منع تعديل السعر

                lblAvailableQuantity.Text = "الكمية المتوفرة: " + selectedProduct["AvailableQuantity"].ToString();

                // تفعيل حقول الإدخال
                txtQuantity.Enabled = true;
                btnAddProduct.Enabled = true;
                txtQuantity.Focus();
            }
            else
            {
                txtPrice.Text = "";
                lblAvailableQuantity.Text = "الكمية المتوفرة: 0";
                txtQuantity.Enabled = false;
                btnAddProduct.Enabled = false;
            }
        }

        // زر إضافة منتج للفاتورة
        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            if (!ValidateProduct())
                return;

            // إنشاء عنصر فاتورة جديد
            SalesInvoiceItem item = new SalesInvoiceItem();
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
            lblAvailableQuantity.Text = "الكمية المتوفرة: 0";
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
                            string invoiceQuery = @"INSERT INTO SalesInvoices 
                                                    (InvoiceNumber, InvoiceDate, CustomerID, PaymentType, 
                                                     SubTotal, Discount, TotalAmount, AmountPaid, Balance, Notes, CreatedBy) 
                                                    VALUES (@invoiceNumber, @invoiceDate, @customerID, @paymentType, 
                                                            @subTotal, @discount, @totalAmount, @amountPaid, @balance, @notes, @createdBy);
                                                    SELECT SCOPE_IDENTITY();";

                            SqlCommand invoiceCmd = new SqlCommand(invoiceQuery, con, transaction);
                            invoiceCmd.Parameters.AddWithValue("@invoiceNumber", txtInvoiceNumber.Text);
                            invoiceCmd.Parameters.AddWithValue("@invoiceDate", dtpInvoiceDate.Value);

                            // إذا كان الدفع نقدياً، قد لا يكون هناك عميل
                            if (cmbCustomers.SelectedValue != null)
                            {
                                invoiceCmd.Parameters.AddWithValue("@customerID", cmbCustomers.SelectedValue);
                            }
                            else
                            {
                                invoiceCmd.Parameters.AddWithValue("@customerID", DBNull.Value);
                            }

                            invoiceCmd.Parameters.AddWithValue("@paymentType", rbCash.Checked ? "Cash" : "Credit");
                            invoiceCmd.Parameters.AddWithValue("@subTotal", decimal.Parse(lblSubTotal.Text.Replace("الإجمالي الفرعي: ", "")));
                            invoiceCmd.Parameters.AddWithValue("@discount", string.IsNullOrWhiteSpace(txtDiscount.Text) ? 0 : decimal.Parse(txtDiscount.Text));
                            invoiceCmd.Parameters.AddWithValue("@totalAmount", decimal.Parse(lblTotalAmount.Text.Replace("الإجمالي النهائي: ", "")));
                            invoiceCmd.Parameters.AddWithValue("@amountPaid", decimal.Parse(lblAmountPaid.Text.Replace("المبلغ المدفوع: ", "")));
                            invoiceCmd.Parameters.AddWithValue("@balance", decimal.Parse(lblBalance.Text.Replace("الرصيد المتبقي: ", "")));
                            invoiceCmd.Parameters.AddWithValue("@notes", txtNotes.Text.Trim());
                            invoiceCmd.Parameters.AddWithValue("@createdBy", "المستخدم الحالي"); // يجب استبدال هذا باسم المستخدم الفعلي

                            int invoiceId = Convert.ToInt32(invoiceCmd.ExecuteScalar());

                            // حفظ عناصر الفاتورة
                            foreach (var item in invoiceItems)
                            {
                                string itemQuery = @"INSERT INTO SalesInvoiceItems 
                                                     (InvoiceID, ProductID, Quantity, UnitPrice, Total) 
                                                     VALUES (@invoiceId, @productId, @quantity, @unitPrice, @total)";

                                SqlCommand itemCmd = new SqlCommand(itemQuery, con, transaction);
                                itemCmd.Parameters.AddWithValue("@invoiceId", invoiceId);
                                itemCmd.Parameters.AddWithValue("@productId", item.ProductID);
                                itemCmd.Parameters.AddWithValue("@quantity", item.Quantity);
                                itemCmd.Parameters.AddWithValue("@unitPrice", item.UnitPrice);
                                itemCmd.Parameters.AddWithValue("@total", item.Total);
                                itemCmd.ExecuteNonQuery();

                                // تحديث كمية المنتج في المخزون
                                string updateProductQuery = @"UPDATE Products 
                                                           SET Quantity = Quantity - @soldQuantity 
                                                           WHERE ID = @productId";

                                SqlCommand updateProductCmd = new SqlCommand(updateProductQuery, con, transaction);
                                updateProductCmd.Parameters.AddWithValue("@soldQuantity", item.Quantity);
                                updateProductCmd.Parameters.AddWithValue("@productId", item.ProductID);
                                updateProductCmd.ExecuteNonQuery();
                            }

                            // إذا كان الدفع آجل، تحديث رصيد العميل
                            if (rbCredit.Checked && cmbCustomers.SelectedValue != null)
                            {
                                string updateCustomerQuery = @"UPDATE Customers 
                                                             SET CurrentBalance = CurrentBalance + @amount 
                                                             WHERE CustomerID = @customerID";

                                SqlCommand updateCustomerCmd = new SqlCommand(updateCustomerQuery, con, transaction);
                                updateCustomerCmd.Parameters.AddWithValue("@amount", decimal.Parse(lblTotalAmount.Text.Replace("الإجمالي النهائي: ", "")));
                                updateCustomerCmd.Parameters.AddWithValue("@customerID", cmbCustomers.SelectedValue);
                                updateCustomerCmd.ExecuteNonQuery();
                            }

                            transaction.Commit();

                            MessageBox.Show("تم حفظ فاتورة المبيعات بنجاح");

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
                MessageBox.Show("حدث خطأ أثناء حفظ فاتورة المبيعات: " + ex.Message);
            }
        }

        // إعادة تعيين النموذج لفاتورة جديدة
        private void ResetForm()
        {
            txtInvoiceNumber.Text = GenerateInvoiceNumber();
            dtpInvoiceDate.Value = DateTime.Now;
            cmbCustomers.SelectedIndex = -1;
            rbCash.Checked = true;
            txtDiscount.Text = "0";
            txtNotes.Text = "";

            invoiceItems.Clear();
            RefreshInvoiceItemsGrid();
            CalculateTotals();

            // إعادة تحميل المنتجات لتحديث الكميات المتوفرة
            LoadProducts();

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

        // حدث تغيير نوع الدفع
        private void rbCash_CheckedChanged(object sender, EventArgs e)
        {
            CalculateTotals();
        }

        private void rbCredit_CheckedChanged(object sender, EventArgs e)
        {
            CalculateTotals();
        }

        private void lblAvailableQuantity_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }

    // فئة لتمثيل عنصر فاتورة البيع
    public class SalesInvoiceItem
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
    }
}
