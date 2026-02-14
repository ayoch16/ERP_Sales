using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ERP_Sales
{
    public partial class Form1 : Form
    {
        private readonly string cs = ConfigurationManager.ConnectionStrings["Db"].ConnectionString;

        private DataTable itemsTable = new DataTable();

        public Form1()
        {
            InitializeComponent();
            LoadCustomers();
            LoadItems();
            NewInvoice();
        }

        private void LoadCustomers()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT Id, Name FROM Customers ORDER BY Name", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbCustomer.DataSource = dt;
                cmbCustomer.DisplayMember = "Name";
                cmbCustomer.ValueMember = "Id";
                cmbCustomer.SelectedIndex = -1;
            }
        }

        private void LoadItems()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT Id, Name, Price FROM Items ORDER BY Name", con);
                itemsTable = new DataTable();
                da.Fill(itemsTable);

                cmbItem.DataSource = itemsTable;
                cmbItem.DisplayMember = "Name";
                cmbItem.ValueMember = "Id";
                cmbItem.SelectedIndex = -1;
            }
        }

        private void NewInvoice()
        {
            txtInvoiceNumber.Text = "0";
            dtDate.Value = DateTime.Now;
            cmbCustomer.SelectedIndex = -1;
            cmbItem.SelectedIndex = -1;
            txtQty.Text = "";
            dgvItems.Rows.Clear();
            lblTotal.Text = "0.00";
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            NewInvoice();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cmbItem.SelectedValue == null)
            {
                MessageBox.Show("Select Item");
                return;
            }

            int qty;
            if (!int.TryParse(txtQty.Text, out qty) || qty <= 0)
            {
                MessageBox.Show("Enter valid Quantity");
                return;
            }

            DataRowView row = (DataRowView)cmbItem.SelectedItem;

            int itemId = Convert.ToInt32(row["Id"]);
            string itemName = row["Name"].ToString();
            decimal price = Convert.ToDecimal(row["Price"]);
            decimal lineTotal = qty * price;

            int idx = dgvItems.Rows.Add();
            dgvItems.Rows[idx].Cells["ItemId"].Value = itemId;
            dgvItems.Rows[idx].Cells["Item"].Value = itemName;
            dgvItems.Rows[idx].Cells["Qty"].Value = qty;
            dgvItems.Rows[idx].Cells["Price"].Value = price;
            dgvItems.Rows[idx].Cells["Total"].Value = lineTotal;

            CalculateTotal();

            txtQty.Text = "";
            cmbItem.SelectedIndex = -1;
        }

        private void CalculateTotal()
        {
            decimal sum = 0;

            foreach (DataGridViewRow r in dgvItems.Rows)
            {
                if (r.IsNewRow) continue;
                object v = r.Cells["Total"].Value;
                if (v != null) sum += Convert.ToDecimal(v);
            }

            lblTotal.Text = sum.ToString("0.00");
        }

        private bool ValidateBeforeSave()
        {
            if (cmbCustomer.SelectedValue == null)
            {
                MessageBox.Show("Customer is required");
                return false;
            }

            if (dgvItems.Rows.Count == 0)
            {
                MessageBox.Show("Add at least one item first");
                return false;
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateBeforeSave()) return;

            int invoiceId;
            int.TryParse(txtInvoiceNumber.Text, out invoiceId); // 0 => new

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlTransaction tx = con.BeginTransaction();

                try
                {
                    if (invoiceId == 0)
                    {
                        // INSERT header
                        using (SqlCommand cmd = new SqlCommand(
                            "INSERT INTO Invoices (InvoiceNumber, CustomerId, Date, Total) " +
                            "OUTPUT INSERTED.ID VALUES (NULL, @cust, @date, @total)", con, tx))
                        {
                            cmd.Parameters.AddWithValue("@cust", (int)cmbCustomer.SelectedValue);
                            cmd.Parameters.AddWithValue("@date", dtDate.Value);
                            cmd.Parameters.AddWithValue("@total", decimal.Parse(lblTotal.Text));
                            invoiceId = (int)cmd.ExecuteScalar();
                        }
                    }
                    else
                    {
                        // UPDATE header
                        using (SqlCommand cmd = new SqlCommand(
                            "UPDATE Invoices SET CustomerId=@cust, Date=@date, Total=@total WHERE Id=@id", con, tx))
                        {
                            cmd.Parameters.AddWithValue("@cust", (int)cmbCustomer.SelectedValue);
                            cmd.Parameters.AddWithValue("@date", dtDate.Value);
                            cmd.Parameters.AddWithValue("@total", decimal.Parse(lblTotal.Text));
                            cmd.Parameters.AddWithValue("@id", invoiceId);

                            int rows = cmd.ExecuteNonQuery();
                            if (rows == 0) throw new Exception("Invoice not found.");
                        }

                        // delete old details
                        using (SqlCommand del = new SqlCommand("DELETE FROM InvoiceDetails WHERE InvoiceId=@id", con, tx))
                        {
                            del.Parameters.AddWithValue("@id", invoiceId);
                            del.ExecuteNonQuery();
                        }
                    }

                    // INSERT details
                    foreach (DataGridViewRow r in dgvItems.Rows)
                    {
                        if (r.IsNewRow) continue;
                        if (r.Cells["ItemId"].Value == null) continue;

                        int itemId = Convert.ToInt32(r.Cells["ItemId"].Value);
                        int qty = Convert.ToInt32(r.Cells["Qty"].Value);
                        decimal price = Convert.ToDecimal(r.Cells["Price"].Value);
                        decimal totalLine = Convert.ToDecimal(r.Cells["Total"].Value);

                        using (SqlCommand cmd2 = new SqlCommand(
                            "INSERT INTO InvoiceDetails (InvoiceId, ItemId, Quantity, Price, Total) " +
                            "VALUES (@inv, @item, @qty, @price, @total)", con, tx))
                        {
                            cmd2.Parameters.AddWithValue("@inv", invoiceId);
                            cmd2.Parameters.AddWithValue("@item", itemId);
                            cmd2.Parameters.AddWithValue("@qty", qty);
                            cmd2.Parameters.AddWithValue("@price", price);
                            cmd2.Parameters.AddWithValue("@total", totalLine);
                            cmd2.ExecuteNonQuery();
                        }
                    }

                    tx.Commit();

                    txtInvoiceNumber.Text = invoiceId.ToString();
                    MessageBox.Show("Saved Successfully");
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    MessageBox.Show("Save Failed: " + ex.Message);
                }
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            int invoiceId;
            if (!int.TryParse(txtInvoiceNumber.Text, out invoiceId) || invoiceId <= 0)
            {
                MessageBox.Show("Enter valid Invoice Number");
                return;
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                // header
                using (SqlCommand cmd = new SqlCommand("SELECT Id, CustomerId, Date, Total FROM Invoices WHERE Id=@id", con))
                {
                    cmd.Parameters.AddWithValue("@id", invoiceId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            MessageBox.Show("Invoice not found");
                            return;
                        }

                        int customerId = Convert.ToInt32(reader["CustomerId"]);
                        DateTime date = Convert.ToDateTime(reader["Date"]);
                        decimal total = Convert.ToDecimal(reader["Total"]);

                        dtDate.Value = date;
                        cmbCustomer.SelectedValue = customerId;
                        lblTotal.Text = total.ToString("0.00");
                    }
                }

                // details
                using (SqlCommand cmd2 = new SqlCommand(
                    @"SELECT d.ItemId, i.Name, d.Quantity, d.Price, d.Total
                      FROM InvoiceDetails d
                      JOIN Items i ON i.Id = d.ItemId
                      WHERE d.InvoiceId = @id", con))
                {
                    cmd2.Parameters.AddWithValue("@id", invoiceId);

                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd2);
                    da.Fill(dt);

                    dgvItems.Rows.Clear();
                    foreach (DataRow r in dt.Rows)
                    {
                        int idx = dgvItems.Rows.Add();
                        dgvItems.Rows[idx].Cells["ItemId"].Value = r["ItemId"];
                        dgvItems.Rows[idx].Cells["Item"].Value = r["Name"];
                        dgvItems.Rows[idx].Cells["Qty"].Value = r["Quantity"];
                        dgvItems.Rows[idx].Cells["Price"].Value = r["Price"];
                        dgvItems.Rows[idx].Cells["Total"].Value = r["Total"];
                    }
                }

                MessageBox.Show("Loaded");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int invoiceId;
            if (!int.TryParse(txtInvoiceNumber.Text, out invoiceId) || invoiceId <= 0)
            {
                MessageBox.Show("Enter valid Invoice Number");
                return;
            }

            if (MessageBox.Show("Delete this invoice?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Invoices WHERE Id=@id", con))
                {
                    cmd.Parameters.AddWithValue("@id", invoiceId);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows == 0)
                    {
                        MessageBox.Show("Invoice not found");
                        return;
                    }
                }
            }

            MessageBox.Show("Deleted");
            NewInvoice();
        }
    }
}
