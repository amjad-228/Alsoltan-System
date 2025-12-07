using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alsoltan_System
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        bool sidebarExpand = false;
        private string currentPage = "المنتجات";

        private void pnlSidebar_MouseEnter(object sender, EventArgs e)
        {
            sidebarExpand = true;
            sidebarTimer.Start();
        }

        private void pnlSidebar_MouseLeave(object sender, EventArgs e)
        {
            sidebarExpand = false;
            sidebarTimer.Start();
        }

        private void CollapseMenuItems()
        {
            foreach (Control ctrl in pnlSidebar.Controls)
            {
                if (ctrl is Button btn)
                {
                    btn.Text = ""; // إزالة النص
                    btn.ImageAlign = ContentAlignment.MiddleCenter;
                }
            }
        }

        private void ExpandMenuItems()
        {
            foreach (Control ctrl in pnlSidebar.Controls)
            {
                if (ctrl is Button btn)
                {
                    btn.Text = btn.Tag?.ToString(); // النص محفوظ في Tag
                    btn.ImageAlign = ContentAlignment.MiddleLeft;
                    btn.TextImageRelation = TextImageRelation.ImageBeforeText;
                    btn.TextAlign = ContentAlignment.MiddleLeft;
                }
            }
        }

        private void sidebarTimer_Tick(object sender, EventArgs e)
        {
            if (sidebarExpand)
            {
                if(pnlSidebar.Width < 200)
                    pnlSidebar.Width += 100;                
                else
                    sidebarTimer.Stop();
                ExpandMenuItems();
            }
            else
            {
                if (pnlSidebar.Width > 60)
                    pnlSidebar.Width -= 100;
                else
                    sidebarTimer.Stop();
                CollapseMenuItems(); 
            }
        }

        private void HighlightCurrentPage ()
        {
            foreach (Control ctrl in pnlSidebar.Controls)
            {
                if (ctrl is Button btn)
                {
                    if (btn.Tag != null && btn.Tag.ToString() == currentPage)
                    {
                        btn.BackColor = Color.DarkOrange;
                    }
                    else
                    {
                        btn.BackColor = Color.Transparent;
                    }
                }
            }
        }

        private Form activeForm = null;
        private void OpenChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            this.pnlContent.Controls.Add(childForm);
            this.pnlContent.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        // حدث فتح نموذج المنتجات الجديد
        private void btnProducts_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmProductsNew());
            currentPage = "المنتجات";
            HighlightCurrentPage();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmAddUser addUserForm = new frmAddUser();
            addUserForm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmSuppliers());
            currentPage = "الموردين";
            HighlightCurrentPage();
        }

        // حدث فتح نموذج العملاء
        private void btnCustomers_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmCustomers());
            currentPage = "العملاء";
            HighlightCurrentPage();
        }

        // حدث فتح نموذج فواتير المشتريات
        private void btnPurchaseInvoices_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmPurchaseInvoices());
            currentPage = "فواتير المشتريات";
            HighlightCurrentPage();
        }

        // حدث فتح نموذج فواتير المبيعات
        private void btnSalesInvoices_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmSalesInvoices());
            currentPage = "فواتير المبيعات";
            HighlightCurrentPage();
        }

        // حدث فتح تقرير فواتير المشتريات
        private void btnPurchaseReport_Click(object sender, EventArgs e)
        {
            OpenChildForm(new frmPurchaseReport());
            currentPage = "تقرير المشتريات";
            HighlightCurrentPage();
        }

        // حدث فتح تقرير فواتير المبيعات
        private void btnSalesReport_Click_1(object sender, EventArgs e)
        {
            OpenChildForm(new frmSalesReport());
            currentPage = "تقرير المبيعات";
            HighlightCurrentPage();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            OpenChildForm(new frmDashboard());
            currentPage = "الرئيسية";
            HighlightCurrentPage();
        }
    }
}
