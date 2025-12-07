namespace Alsoltan_System
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.btnSalesInvoices = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnPurchaseInvoices = new System.Windows.Forms.Button();
            this.btnCustomers = new System.Windows.Forms.Button();
            this.btnSuppliers = new System.Windows.Forms.Button();
            this.btnSalesReportt = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnProducts = new System.Windows.Forms.Button();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.sidebarTimer = new System.Windows.Forms.Timer(this.components);
            this.btnPurchaseReport = new System.Windows.Forms.Button();
            this.btnSalesReport = new System.Windows.Forms.Button();
            this.pnlSidebar.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSidebar
            // 
            this.pnlSidebar.BackColor = System.Drawing.Color.White;
            this.pnlSidebar.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlSidebar.Controls.Add(this.btnSalesReport);
            this.pnlSidebar.Controls.Add(this.btnPurchaseReport);
            this.pnlSidebar.Controls.Add(this.btnSalesInvoices);
            this.pnlSidebar.Controls.Add(this.btnPurchaseInvoices);
            this.pnlSidebar.Controls.Add(this.btnCustomers);
            this.pnlSidebar.Controls.Add(this.btnSuppliers);
            this.pnlSidebar.Controls.Add(this.btnSalesReportt);
            this.pnlSidebar.Controls.Add(this.button1);
            this.pnlSidebar.Controls.Add(this.btnProducts);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 0);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(60, 867);
            this.pnlSidebar.TabIndex = 0;
            this.pnlSidebar.MouseEnter += new System.EventHandler(this.pnlSidebar_MouseEnter);
            this.pnlSidebar.MouseLeave += new System.EventHandler(this.pnlSidebar_MouseLeave);
            // 
            // btnSalesInvoices
            // 
            this.btnSalesInvoices.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSalesInvoices.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSalesInvoices.FlatAppearance.BorderSize = 0;
            this.btnSalesInvoices.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalesInvoices.Font = new System.Drawing.Font("Niagara Engraved", 13F, System.Drawing.FontStyle.Bold);
            this.btnSalesInvoices.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSalesInvoices.ImageKey = "sales1.png";
            this.btnSalesInvoices.ImageList = this.imageList1;
            this.btnSalesInvoices.Location = new System.Drawing.Point(0, 468);
            this.btnSalesInvoices.Name = "btnSalesInvoices";
            this.btnSalesInvoices.Size = new System.Drawing.Size(56, 78);
            this.btnSalesInvoices.TabIndex = 6;
            this.btnSalesInvoices.Tag = "فواتير المبيعات";
            this.btnSalesInvoices.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSalesInvoices.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSalesInvoices.UseVisualStyleBackColor = true;
            this.btnSalesInvoices.Click += new System.EventHandler(this.btnSalesInvoices_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "products.png");
            this.imageList1.Images.SetKeyName(1, "report.png");
            this.imageList1.Images.SetKeyName(2, "sales.png");
            this.imageList1.Images.SetKeyName(3, "supplier.png");
            this.imageList1.Images.SetKeyName(4, "customers.png");
            this.imageList1.Images.SetKeyName(5, "purchase.png");
            this.imageList1.Images.SetKeyName(6, "purchases.png");
            this.imageList1.Images.SetKeyName(7, "sales1.png");
            // 
            // btnPurchaseInvoices
            // 
            this.btnPurchaseInvoices.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPurchaseInvoices.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnPurchaseInvoices.FlatAppearance.BorderSize = 0;
            this.btnPurchaseInvoices.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPurchaseInvoices.Font = new System.Drawing.Font("Niagara Engraved", 13F, System.Drawing.FontStyle.Bold);
            this.btnPurchaseInvoices.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPurchaseInvoices.ImageKey = "purchases.png";
            this.btnPurchaseInvoices.ImageList = this.imageList1;
            this.btnPurchaseInvoices.Location = new System.Drawing.Point(0, 390);
            this.btnPurchaseInvoices.Name = "btnPurchaseInvoices";
            this.btnPurchaseInvoices.Size = new System.Drawing.Size(56, 78);
            this.btnPurchaseInvoices.TabIndex = 5;
            this.btnPurchaseInvoices.Tag = "فواتير المشتريات";
            this.btnPurchaseInvoices.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPurchaseInvoices.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnPurchaseInvoices.UseVisualStyleBackColor = true;
            this.btnPurchaseInvoices.Click += new System.EventHandler(this.btnPurchaseInvoices_Click);
            // 
            // btnCustomers
            // 
            this.btnCustomers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCustomers.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCustomers.FlatAppearance.BorderSize = 0;
            this.btnCustomers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCustomers.Font = new System.Drawing.Font("Niagara Engraved", 13F, System.Drawing.FontStyle.Bold);
            this.btnCustomers.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCustomers.ImageKey = "customers.png";
            this.btnCustomers.ImageList = this.imageList1;
            this.btnCustomers.Location = new System.Drawing.Point(0, 312);
            this.btnCustomers.Name = "btnCustomers";
            this.btnCustomers.Size = new System.Drawing.Size(56, 78);
            this.btnCustomers.TabIndex = 4;
            this.btnCustomers.Tag = "إدارة العملاء";
            this.btnCustomers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCustomers.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCustomers.UseVisualStyleBackColor = true;
            this.btnCustomers.Click += new System.EventHandler(this.btnCustomers_Click);
            // 
            // btnSuppliers
            // 
            this.btnSuppliers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSuppliers.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSuppliers.FlatAppearance.BorderSize = 0;
            this.btnSuppliers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSuppliers.Font = new System.Drawing.Font("Niagara Engraved", 13F, System.Drawing.FontStyle.Bold);
            this.btnSuppliers.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSuppliers.ImageKey = "supplier.png";
            this.btnSuppliers.ImageList = this.imageList1;
            this.btnSuppliers.Location = new System.Drawing.Point(0, 234);
            this.btnSuppliers.Name = "btnSuppliers";
            this.btnSuppliers.Size = new System.Drawing.Size(56, 78);
            this.btnSuppliers.TabIndex = 3;
            this.btnSuppliers.Tag = "إدارة الموردين";
            this.btnSuppliers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSuppliers.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSuppliers.UseVisualStyleBackColor = true;
            this.btnSuppliers.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnSalesReportt
            // 
            this.btnSalesReportt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSalesReportt.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSalesReportt.FlatAppearance.BorderSize = 0;
            this.btnSalesReportt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalesReportt.Font = new System.Drawing.Font("Niagara Engraved", 13F, System.Drawing.FontStyle.Bold);
            this.btnSalesReportt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSalesReportt.ImageKey = "report.png";
            this.btnSalesReportt.ImageList = this.imageList1;
            this.btnSalesReportt.Location = new System.Drawing.Point(0, 156);
            this.btnSalesReportt.Name = "btnSalesReportt";
            this.btnSalesReportt.Size = new System.Drawing.Size(56, 78);
            this.btnSalesReportt.TabIndex = 2;
            this.btnSalesReportt.Tag = "تقرير المبيعات";
            this.btnSalesReportt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSalesReportt.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSalesReportt.UseVisualStyleBackColor = true;
            this.btnSalesReportt.Click += new System.EventHandler(this.btnSalesReport_Click);
            // 
            // button1
            // 
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Niagara Engraved", 13F, System.Drawing.FontStyle.Bold);
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.ImageKey = "sales.png";
            this.button1.ImageList = this.imageList1;
            this.button1.Location = new System.Drawing.Point(0, 78);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(56, 78);
            this.button1.TabIndex = 1;
            this.button1.Tag = "المبيعات";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnProducts
            // 
            this.btnProducts.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnProducts.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnProducts.FlatAppearance.BorderSize = 0;
            this.btnProducts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProducts.Font = new System.Drawing.Font("Niagara Engraved", 13F, System.Drawing.FontStyle.Bold);
            this.btnProducts.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProducts.ImageKey = "products.png";
            this.btnProducts.ImageList = this.imageList1;
            this.btnProducts.Location = new System.Drawing.Point(0, 0);
            this.btnProducts.Name = "btnProducts";
            this.btnProducts.Size = new System.Drawing.Size(56, 78);
            this.btnProducts.TabIndex = 0;
            this.btnProducts.Tag = "المنتجات";
            this.btnProducts.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnProducts.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnProducts.UseVisualStyleBackColor = true;
            this.btnProducts.Click += new System.EventHandler(this.btnProducts_Click);
            // 
            // pnlContent
            // 
            this.pnlContent.BackColor = System.Drawing.Color.White;
            this.pnlContent.Controls.Add(this.button2);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(60, 0);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(1168, 867);
            this.pnlContent.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(652, 150);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(153, 46);
            this.button2.TabIndex = 0;
            this.button2.Text = "إضافة مستخدمم";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // sidebarTimer
            // 
            this.sidebarTimer.Interval = 1;
            this.sidebarTimer.Tick += new System.EventHandler(this.sidebarTimer_Tick);
            // 
            // btnPurchaseReport
            // 
            this.btnPurchaseReport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPurchaseReport.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnPurchaseReport.FlatAppearance.BorderSize = 0;
            this.btnPurchaseReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPurchaseReport.Font = new System.Drawing.Font("Niagara Engraved", 13F, System.Drawing.FontStyle.Bold);
            this.btnPurchaseReport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPurchaseReport.ImageKey = "sales1.png";
            this.btnPurchaseReport.ImageList = this.imageList1;
            this.btnPurchaseReport.Location = new System.Drawing.Point(0, 546);
            this.btnPurchaseReport.Name = "btnPurchaseReport";
            this.btnPurchaseReport.Size = new System.Drawing.Size(56, 78);
            this.btnPurchaseReport.TabIndex = 7;
            this.btnPurchaseReport.Tag = "تقرير المشتريات";
            this.btnPurchaseReport.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPurchaseReport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnPurchaseReport.UseVisualStyleBackColor = true;
            this.btnPurchaseReport.Click += new System.EventHandler(this.btnPurchaseReport_Click);
            // 
            // btnSalesReport
            // 
            this.btnSalesReport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSalesReport.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSalesReport.FlatAppearance.BorderSize = 0;
            this.btnSalesReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalesReport.Font = new System.Drawing.Font("Niagara Engraved", 13F, System.Drawing.FontStyle.Bold);
            this.btnSalesReport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSalesReport.ImageKey = "sales1.png";
            this.btnSalesReport.ImageList = this.imageList1;
            this.btnSalesReport.Location = new System.Drawing.Point(0, 624);
            this.btnSalesReport.Name = "btnSalesReport";
            this.btnSalesReport.Size = new System.Drawing.Size(56, 78);
            this.btnSalesReport.TabIndex = 8;
            this.btnSalesReport.Tag = "تقرير المبيعات";
            this.btnSalesReport.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSalesReport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSalesReport.UseVisualStyleBackColor = true;
            this.btnSalesReport.Click += new System.EventHandler(this.btnSalesReport_Click_1);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1228, 867);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlSidebar);
            this.Name = "frmMain";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "مشروبات السلطان - الرئيسية";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.pnlSidebar.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Button btnProducts;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Timer sidebarTimer;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnSalesReportt;
        private System.Windows.Forms.Button btnSuppliers;
        private System.Windows.Forms.Button btnCustomers;
        private System.Windows.Forms.Button btnPurchaseInvoices;
        private System.Windows.Forms.Button btnSalesInvoices;
        private System.Windows.Forms.Button btnPurchaseReport;
        private System.Windows.Forms.Button btnSalesReport;
    }
}