namespace ERP_Sales
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label labelInvoice;
        private System.Windows.Forms.TextBox txtInvoiceNumber;
        private System.Windows.Forms.DateTimePicker dtDate;

        private System.Windows.Forms.Label labelCustomer;
        private System.Windows.Forms.ComboBox cmbCustomer;

        private System.Windows.Forms.Label labelItem;
        private System.Windows.Forms.ComboBox cmbItem;

        private System.Windows.Forms.Label labelQty;
        private System.Windows.Forms.TextBox txtQty;

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridView dgvItems;

        private System.Windows.Forms.Label labelTotal;
        private System.Windows.Forms.Label lblTotal;

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnDelete;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.labelInvoice = new System.Windows.Forms.Label();
            this.txtInvoiceNumber = new System.Windows.Forms.TextBox();
            this.dtDate = new System.Windows.Forms.DateTimePicker();
            this.labelCustomer = new System.Windows.Forms.Label();
            this.cmbCustomer = new System.Windows.Forms.ComboBox();
            this.labelItem = new System.Windows.Forms.Label();
            this.cmbItem = new System.Windows.Forms.ComboBox();
            this.labelQty = new System.Windows.Forms.Label();
            this.txtQty = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.dgvItems = new System.Windows.Forms.DataGridView();
            this.labelTotal = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            this.SuspendLayout();

            this.labelInvoice.Text = "Invoice Number:";
            this.labelInvoice.Location = new System.Drawing.Point(20, 20);
            this.labelInvoice.AutoSize = true;

            this.txtInvoiceNumber.Location = new System.Drawing.Point(140, 18);
            this.txtInvoiceNumber.Width = 120;
            this.txtInvoiceNumber.Text = "0";

            this.dtDate.Location = new System.Drawing.Point(300, 18);
            this.dtDate.Width = 250;

            this.labelCustomer.Text = "Customer:";
            this.labelCustomer.Location = new System.Drawing.Point(20, 60);
            this.labelCustomer.AutoSize = true;

            this.cmbCustomer.Location = new System.Drawing.Point(140, 58);
            this.cmbCustomer.Width = 200;
            this.cmbCustomer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.labelItem.Text = "Item:";
            this.labelItem.Location = new System.Drawing.Point(20, 100);
            this.labelItem.AutoSize = true;

            this.cmbItem.Location = new System.Drawing.Point(140, 98);
            this.cmbItem.Width = 200;
            this.cmbItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.labelQty.Text = "Qty:";
            this.labelQty.Location = new System.Drawing.Point(370, 100);
            this.labelQty.AutoSize = true;

            this.txtQty.Location = new System.Drawing.Point(410, 98);
            this.txtQty.Width = 60;

            this.btnAdd.Text = "Add";
            this.btnAdd.Location = new System.Drawing.Point(490, 96);
            this.btnAdd.Width = 90;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);

            this.dgvItems.Location = new System.Drawing.Point(20, 140);
            this.dgvItems.Size = new System.Drawing.Size(650, 220);
            this.dgvItems.AllowUserToAddRows = false;
            this.dgvItems.ReadOnly = true;
            this.dgvItems.RowHeadersVisible = false;

            this.dgvItems.Columns.Clear();
            this.dgvItems.Columns.Add("ItemId", "ItemId");
            this.dgvItems.Columns["ItemId"].Visible = false;

            this.dgvItems.Columns.Add("Item", "Item");
            this.dgvItems.Columns["Item"].Width = 220;

            this.dgvItems.Columns.Add("Qty", "Qty");
            this.dgvItems.Columns["Qty"].Width = 80;

            this.dgvItems.Columns.Add("Price", "Price");
            this.dgvItems.Columns["Price"].Width = 120;

            this.dgvItems.Columns.Add("Total", "Total");
            this.dgvItems.Columns["Total"].Width = 120;

            this.labelTotal.Text = "Total:";
            this.labelTotal.Location = new System.Drawing.Point(430, 375);
            this.labelTotal.AutoSize = true;

            this.lblTotal.Text = "0.00";
            this.lblTotal.Location = new System.Drawing.Point(480, 375);
            this.lblTotal.AutoSize = true;

            this.btnSave.Text = "Save";
            this.btnSave.Location = new System.Drawing.Point(20, 420);
            this.btnSave.Width = 100;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            this.btnNew.Text = "New";
            this.btnNew.Location = new System.Drawing.Point(140, 420);
            this.btnNew.Width = 100;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);

            this.btnLoad.Text = "Load";
            this.btnLoad.Location = new System.Drawing.Point(260, 420);
            this.btnLoad.Width = 100;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);

            this.btnDelete.Text = "Delete";
            this.btnDelete.Location = new System.Drawing.Point(380, 420);
            this.btnDelete.Width = 100;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            this.ClientSize = new System.Drawing.Size(700, 480);
            this.Controls.Add(this.labelInvoice);
            this.Controls.Add(this.txtInvoiceNumber);
            this.Controls.Add(this.dtDate);
            this.Controls.Add(this.labelCustomer);
            this.Controls.Add(this.cmbCustomer);
            this.Controls.Add(this.labelItem);
            this.Controls.Add(this.cmbItem);
            this.Controls.Add(this.labelQty);
            this.Controls.Add(this.txtQty);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dgvItems);
            this.Controls.Add(this.labelTotal);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.btnDelete);
            this.Text = "Sales Invoice";

            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
