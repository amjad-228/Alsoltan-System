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
    // نموذج شاشة البداية (Dashboard)
    // يعرض إحصائيات عامة للنظام ومخططات بيانية
    public partial class frmDashboard : Form
    {
        public frmDashboard()
        {
            InitializeComponent();
        }

        // تحميل إحصائيات النظام
        private void LoadSystemStatistics()
        {
            try
            {
                using (SqlConnection con = Database.GetConnection())
                {
                    // عدد المنتجات
                    string productsQuery = "SELECT COUNT(*) FROM Products";
                    SqlCommand productsCmd = new SqlCommand(productsQuery, con);
                    con.Open();
                    int productsCount = (int)productsCmd.ExecuteScalar();
                    lblProductsCount.Text = productsCount.ToString();
                    con.Close();

                    // عدد الموردين
                    string suppliersQuery = "SELECT COUNT(*) FROM Suppliers";
                    SqlCommand suppliersCmd = new SqlCommand(suppliersQuery, con);
                    con.Open();
                    int suppliersCount = (int)suppliersCmd.ExecuteScalar();
                    lblSuppliersCount.Text = suppliersCount.ToString();
                    con.Close();

                    // عدد العملاء
                    string customersQuery = "SELECT COUNT(*) FROM Customers";
                    SqlCommand customersCmd = new SqlCommand(customersQuery, con);
                    con.Open();
                    int customersCount = (int)customersCmd.ExecuteScalar();
                    lblCustomersCount.Text = customersCount.ToString();
                    con.Close();

                    // إجمالي المبيعات (آخر 30 يوم)
                    string salesQuery = @"SELECT ISNULL(SUM(TotalAmount), 0) 
                                        FROM SalesInvoices 
                                        WHERE InvoiceDate >= DATEADD(day, -30, GETDATE())";
                    SqlCommand salesCmd = new SqlCommand(salesQuery, con);
                    con.Open();
                    decimal salesTotal = (decimal)salesCmd.ExecuteScalar();
                    lblSalesTotal.Text = salesTotal.ToString("F2");
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء تحميل الإحصائيات: " + ex.Message);
            }
        }

        // رسم مخطط شريطي بسيط للإحصائيات
        private void DrawBarChart()
        {
            try
            {
                pnlBarChart.Controls.Clear();

                // إنشاء أشرطة للمخطط
                int barWidth = 40;
                int spacing = 20;
                int maxHeight = pnlBarChart.Height - 40;

                // بيانات المخطط
                Dictionary<string, int> chartData = new Dictionary<string, int>
                {
                    {"المنتجات", int.Parse(lblProductsCount.Text)},
                    {"الموردين", int.Parse(lblSuppliersCount.Text)},
                    {"العملاء", int.Parse(lblCustomersCount.Text)}
                };

                // حساب القيمة القصوى لتحديد ارتفاع الأشرطة
                int maxValue = chartData.Values.Max();
                if (maxValue == 0) maxValue = 1; // لتجنب القسمة على صفر

                int xPosition = 20;

                foreach (var item in chartData)
                {
                    // حساب ارتفاع الشريط
                    int barHeight = (int)((double)item.Value / maxValue * maxHeight);

                    // إنشاء شريط
                    Panel bar = new Panel();
                    bar.BackColor = GetBarColor(item.Key);
                    bar.Size = new Size(barWidth, barHeight);
                    bar.Location = new Point(xPosition, maxHeight - barHeight + 20);

                    // إضافة الشريط إلى لوحة المخطط
                    pnlBarChart.Controls.Add(bar);

                    // إضافة مسمى للقيمة
                    Label valueLabel = new Label();
                    valueLabel.Text = item.Value.ToString();
                    valueLabel.AutoSize = true;
                    valueLabel.Location = new Point(xPosition, maxHeight - barHeight);
                    valueLabel.ForeColor = Color.Black;
                    valueLabel.Font = new Font("Arial", 8);
                    pnlBarChart.Controls.Add(valueLabel);

                    // إضافة مسمى للاسم
                    Label nameLabel = new Label();
                    nameLabel.Text = item.Key;
                    nameLabel.AutoSize = true;
                    nameLabel.Location = new Point(xPosition - 10, maxHeight + 25);
                    nameLabel.ForeColor = Color.Black;
                    nameLabel.Font = new Font("Arial", 8);
                    pnlBarChart.Controls.Add(nameLabel);

                    xPosition += barWidth + spacing;
                }
            }
            catch (Exception ex)
            {
                // لا نعرض رسالة خطأ للمخططات لعدم تأثيرها على وظائف النظام الأساسية
            }
        }

        // الحصول على لون الشريط حسب النوع
        private Color GetBarColor(string category)
        {
            switch (category)
            {
                case "المنتجات":
                    return Color.Blue;
                case "الموردين":
                    return Color.Green;
                case "العملاء":
                    return Color.Orange;
                default:
                    return Color.Gray;
            }
        }

        // رسم مخطط دائري بسيط
        private void DrawPieChart()
        {
            try
            {
                pnlPieChart.Controls.Clear();

                // إنشاء ملصق بسيط يوضح أن المخطط الدائري سيُضاف لاحقاً
                Label pieLabel = new Label();
                pieLabel.Text = "المخطط الدائري سيُضاف في النسخة القادمة";
                pieLabel.AutoSize = true;
                pieLabel.Location = new Point(50, 100);
                pieLabel.ForeColor = Color.Gray;
                pieLabel.Font = new Font("Arial", 12, FontStyle.Italic);
                pnlPieChart.Controls.Add(pieLabel);
            }
            catch (Exception ex)
            {
                // لا نعرض رسالة خطأ للمخططات لعدم تأثيرها على وظائف النظام الأساسية
            }
        }


        // حدث تحميل النموذج
        private void frmDashboard_Load(object sender, EventArgs e)
        {
            // تحميل إحصائيات النظام
            LoadSystemStatistics();

            // رسم المخططات
            DrawBarChart();
            DrawPieChart();

            // تحديث حقوق النسخ مع السنة الحالية
            lblCopyright.Text = $"© {DateTime.Now.Year} نظام مشروبات السلطان لإدارة المبيعات والمشتريات";
        }
    }
}
