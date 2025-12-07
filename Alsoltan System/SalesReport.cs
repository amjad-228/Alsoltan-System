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
    public partial class SalesReport : Form
    {
        public SalesReport()
        {
            InitializeComponent();
            // تعيين التواريخ الافتراضية
            dtpFromDate.Value = DateTime.Now.AddDays(-30);
            dtpToDate.Value = DateTime.Now;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void dgvSalesReport_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void SalesRepoert_Load(object sender, EventArgs e)
        {
            LoadSalesReport();
        }

        private void LoadSalesReport()
        {
            try
            {
                using (SqlConnection con = Database.GetConnection())
                {
                    string query = @"select i.InvoiceID AS 'رقم الفاتورة', i.InvoiceDate AS 'تاريخ الفاتورة', i.TotalAmount AS 'المبلغ الإجمالي', i.CreatedBy AS 'البائع' from invoices i where i.invoiceDate between @fromDate AND @toDate order by i.InvoiceDate DESC";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@fromDate", dtpFromDate.Value.Date);
                    cmd.Parameters.AddWithValue("@toDate", dtpToDate.Value.Date.AddDays(1).AddSeconds(-1)); // نهاية اليوم

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvSalesReport.DataSource = dt;

                    // حساب إجمالي المبيعات
                    decimal totalSales = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        totalSales += Convert.ToDecimal(row["المبلغ الإجمالي"]);
                    }
                    lblTotalSales.Text = "إجمالي المبيعات: " + totalSales.ToString("C");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء تحميل التقرير: " + ex.Message);
            }
        }

        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            LoadSalesReport();
        }

        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            LoadSalesReport();
        }
    }
}
