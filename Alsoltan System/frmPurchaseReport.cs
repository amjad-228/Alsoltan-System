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
    // نموذج تقرير فواتير المشتريات
    // يعرض تقرير بفواتير المشتريات مع إمكانية التصفية حسب التاريخ والمورد
    public partial class frmPurchaseReport : Form
    {
        public frmPurchaseReport()
        {
            InitializeComponent();
        }

        // تحميل قائمة الموردين في القائمة المنسدلة
        private void LoadSuppliers()
        {
            // إضافة خيار "جميع الموردين" أولاً
            cmbSuppliers.Items.Add("جميع الموردين");
            cmbSuppliers.SelectedIndex = 0;

            using (SqlConnection con = Database.GetConnection())
            {
                string query = "SELECT SupplierID, SupplierName FROM Suppliers ORDER BY SupplierName";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    cmbSuppliers.Items.Add(new SupplierItem
                    {
                        SupplierID = Convert.ToInt32(row["SupplierID"]),
                        SupplierName = row["SupplierName"].ToString()
                    });
                }
            }
        }

        // تحميل تقرير فواتير المشتريات
        private void LoadPurchaseReport()
        {
            try
            {
                using (SqlConnection con = Database.GetConnection())
                {
                    // استعلام لجلب فواتير المشتريات مع تفاصيل المورد
                    string query = @"SELECT 
                                    pi.InvoiceID,
                                    pi.InvoiceNumber,
                                    pi.InvoiceDate,
                                    s.SupplierName,
                                    pi.SubTotal,
                                    pi.Discount,
                                    pi.TotalAmount,
                                    pi.PaymentType,
                                    pi.Notes
                                 FROM PurchaseInvoices pi
                                 LEFT JOIN Suppliers s ON pi.SupplierID = s.SupplierID
                                 WHERE pi.InvoiceDate BETWEEN @fromDate AND @toDate";

                    // إضافة شرط تصفية المورد إذا تم اختيار مورد محدد
                    if (cmbSuppliers.SelectedIndex > 0)
                    {
                        SupplierItem selectedSupplier = (SupplierItem)cmbSuppliers.SelectedItem;
                        query += " AND pi.SupplierID = @supplierID";
                    }

                    query += " ORDER BY pi.InvoiceDate DESC";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@fromDate", dtpFromDate.Value.Date);
                    cmd.Parameters.AddWithValue("@toDate", dtpToDate.Value.Date.AddDays(1).AddSeconds(-1));

                    // إضافة معامل المورد إذا تم اختيار مورد محدد
                    if (cmbSuppliers.SelectedIndex > 0)
                    {
                        SupplierItem selectedSupplier = (SupplierItem)cmbSuppliers.SelectedItem;
                        cmd.Parameters.AddWithValue("@supplierID", selectedSupplier.SupplierID);
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
                    if (dgvReport.Columns["SupplierName"] != null)
                        dgvReport.Columns["SupplierName"].HeaderText = "المورد";
                    if (dgvReport.Columns["SubTotal"] != null)
                        dgvReport.Columns["SubTotal"].HeaderText = "الإجمالي الفرعي";
                    if (dgvReport.Columns["Discount"] != null)
                        dgvReport.Columns["Discount"].HeaderText = "الخصم";
                    if (dgvReport.Columns["TotalAmount"] != null)
                        dgvReport.Columns["TotalAmount"].HeaderText = "الإجمالي النهائي";
                    if (dgvReport.Columns["PaymentType"] != null)
                        dgvReport.Columns["PaymentType"].HeaderText = "نوع الدفع";
                    if (dgvReport.Columns["Notes"] != null)
                        dgvReport.Columns["Notes"].HeaderText = "الملاحظات";

                    // حساب إجمالي المبالغ
                    decimal totalAmount = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        totalAmount += Convert.ToDecimal(row["TotalAmount"]);
                    }

                    lblTotalAmount.Text = "الإجمالي: " + totalAmount.ToString("F2");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء تحميل التقرير: " + ex.Message);
            }
        }

        // حدث تحميل النموذج
        private void frmPurchaseReport_Load(object sender, EventArgs e)
        {
            // تعيين التواريخ الافتراضية (آخر 30 يوم)
            dtpFromDate.Value = DateTime.Now.AddDays(-30);
            dtpToDate.Value = DateTime.Now;

            // تحميل قائمة الموردين
            LoadSuppliers();

            // تحميل التقرير تلقائياً
            LoadPurchaseReport();
        }

        // زر عرض التقرير
        private void btnShowReport_Click(object sender, EventArgs e)
        {
            LoadPurchaseReport();
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
            LoadPurchaseReport();
        }

        // حدث تغيير تاريخ النهاية
        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            LoadPurchaseReport();
        }

        // حدث تغيير اختيار المورد
        private void cmbSuppliers_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPurchaseReport();
        }
    }

    // فئة لتمثيل عنصر المورد في القائمة المنسدلة
    public class SupplierItem
    {
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }

        public override string ToString()
        {
            return SupplierName;
        }
    }
}
