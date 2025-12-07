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
    // نموذج تقرير فواتير المبيعات
    // يعرض تقرير بفواتير المبيعات مع إمكانية التصفية حسب التاريخ والعميل
    public partial class frmSalesReport : Form
    {
        public frmSalesReport()
        {
            InitializeComponent();
        }

        // تحميل قائمة العملاء في القائمة المنسدلة
        private void LoadCustomers()
        {
            // إضافة خيار "جميع العملاء" أولاً
            cmbCustomers.Items.Add("جميع العملاء");
            cmbCustomers.SelectedIndex = 0;

            using (SqlConnection con = Database.GetConnection())
            {
                string query = "SELECT CustomerID, CustomerName FROM Customers ORDER BY CustomerName";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    cmbCustomers.Items.Add(new CustomerItem
                    {
                        CustomerID = Convert.ToInt32(row["CustomerID"]),
                        CustomerName = row["CustomerName"].ToString()
                    });
                }
            }
        }

        // تحميل تقرير فواتير المبيعات
        private void LoadSalesReport()
        {
            try
            {
                using (SqlConnection con = Database.GetConnection())
                {
                    // استعلام لجلب فواتير المبيعات مع تفاصيل العميل
                    string query = @"SELECT 
                                    si.InvoiceID,
                                    si.InvoiceNumber,
                                    si.InvoiceDate,
                                    c.CustomerName,
                                    si.SubTotal,
                                    si.Discount,
                                    si.TotalAmount,
                                    si.AmountPaid,
                                    si.Balance,
                                    si.PaymentType,
                                    si.Notes
                                 FROM SalesInvoices si
                                 LEFT JOIN Customers c ON si.CustomerID = c.CustomerID
                                 WHERE si.InvoiceDate BETWEEN @fromDate AND @toDate";

                    // إضافة شرط تصفية العميل إذا تم اختيار عميل محدد
                    if (cmbCustomers.SelectedIndex > 0)
                    {
                        CustomerItem selectedCustomer = (CustomerItem)cmbCustomers.SelectedItem;
                        query += " AND si.CustomerID = @customerID";
                    }

                    query += " ORDER BY si.InvoiceDate DESC";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@fromDate", dtpFromDate.Value.Date);
                    cmd.Parameters.AddWithValue("@toDate", dtpToDate.Value.Date.AddDays(1).AddSeconds(-1));

                    // إضافة معامل العميل إذا تم اختيار عميل محدد
                    if (cmbCustomers.SelectedIndex > 0)
                    {
                        CustomerItem selectedCustomer = (CustomerItem)cmbCustomers.SelectedItem;
                        cmd.Parameters.AddWithValue("@customerID", selectedCustomer.CustomerID);
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvReport.DataSource = dt;

                    // تغيير أسماء الأعمدة لتكون بالعربية
                    if (dgvReport.Columns["InvoiceID"] != null)
                        dgvReport.Columns["InvoiceID"].HeaderText = "رقم الفاتورة";
                    if (dgvReport.Columns["InvoiceNumber"] != null)
                        dgvReport.Columns["InvoiceNumber"].HeaderText = "رقم الفاتورة";
                    if (dgvReport.Columns["InvoiceDate"] != null)
                        dgvReport.Columns["InvoiceDate"].HeaderText = "تاريخ الفاتورة";
                    if (dgvReport.Columns["CustomerName"] != null)
                        dgvReport.Columns["CustomerName"].HeaderText = "العميل";
                    if (dgvReport.Columns["SubTotal"] != null)
                        dgvReport.Columns["SubTotal"].HeaderText = "الإجمالي الفرعي";
                    if (dgvReport.Columns["Discount"] != null)
                        dgvReport.Columns["Discount"].HeaderText = "الخصم";
                    if (dgvReport.Columns["TotalAmount"] != null)
                        dgvReport.Columns["TotalAmount"].HeaderText = "الإجمالي النهائي";
                    if (dgvReport.Columns["AmountPaid"] != null)
                        dgvReport.Columns["AmountPaid"].HeaderText = "المبلغ المدفوع";
                    if (dgvReport.Columns["Balance"] != null)
                        dgvReport.Columns["Balance"].HeaderText = "الرصيد المتبقي";
                    if (dgvReport.Columns["PaymentType"] != null)
                        dgvReport.Columns["PaymentType"].HeaderText = "نوع الدفع";
                    if (dgvReport.Columns["Notes"] != null)
                        dgvReport.Columns["Notes"].HeaderText = "الملاحظات";

                    // حساب إجمالي المبالغ
                    decimal totalAmount = 0;
                    decimal totalPaid = 0;
                    decimal totalBalance = 0;

                    foreach (DataRow row in dt.Rows)
                    {
                        totalAmount += Convert.ToDecimal(row["TotalAmount"]);
                        totalPaid += Convert.ToDecimal(row["AmountPaid"]);
                        totalBalance += Convert.ToDecimal(row["Balance"]);
                    }

                    lblTotalAmount.Text = $"الإجمالي: {totalAmount:F2} | المدفوع: {totalPaid:F2} | المتبقي: {totalBalance:F2}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء تحميل التقرير: " + ex.Message);
            }
        }

        // حدث تحميل النموذج
        private void frmSalesReport_Load(object sender, EventArgs e)
        {
            // تعيين التواريخ الافتراضية (آخر 30 يوم)
            dtpFromDate.Value = DateTime.Now.AddDays(-30);
            dtpToDate.Value = DateTime.Now;

            // تحميل قائمة العملاء
            LoadCustomers();

            // تحميل التقرير تلقائياً
            LoadSalesReport();
        }

        // زر عرض التقرير
        private void btnShowReport_Click(object sender, EventArgs e)
        {
            LoadSalesReport();
        }

        // زر طباعة التقرير
        private void btnPrintReport_Click(object sender, EventArgs e)
        {
            // هنا يمكن إضافة كود الطباعة لاحقاً
            MessageBox.Show("ميزة الطباعة ستُضاف لاحقاً");
        }

        // حدث تغيير تاريخ البداية
        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            LoadSalesReport();
        }

        // حدث تغيير تاريخ النهاية
        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            LoadSalesReport();
        }

        // حدث تغيير اختيار العميل
        private void cmbCustomers_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSalesReport();
        }
    }

    // فئة لتمثيل عنصر العميل في القائمة المنسدلة
    public class CustomerItem
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }

        public override string ToString()
        {
            return CustomerName;
        }
    }
}
