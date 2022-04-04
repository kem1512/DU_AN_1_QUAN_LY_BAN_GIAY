﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video.DirectShow;
using BUS_BussinessLayer.BUS_Services;
using BUS_BussinessLayer.iBUS_Services;
using BUS_BussinessLayer.Utilities;
using DAL_DataAccessLayer.Entities;
using ZXing;

namespace GUI_PresentationLayer.View
{
    public partial class FrmSales : Form
    {
        private FrmMain _frmMain;
        private iProductServices _iProductServices = new ProductSevices();
        private iShipperServices _iShipperServices = new ShipperServices();
        private iColorServices _iColorServices = new ColorServices();
        private iBrandServices _iBrandServices = new BrandServices();
        private iCustomerServices _iCustomerServices = new CustomerServices();
        private iInvoiceServices _iInvoiceServices = new InvoiceServices();
        private iEmployeeServices _iEmployeeServices = new EmployeeServices();
        private FilterInfoCollection _filterInfo;
        private VideoCaptureDevice _videoCaptureDevice;
        public string invoidId { get; set; }

        public FrmSales(FrmMain frmMain)
        {
            InitializeComponent();
            _frmMain = frmMain;
            _filterInfo = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo x in _filterInfo)
            {
                cmbCamera.Items.Add(x.Name);
                cmbCamera.SelectedIndex = 0;
            }
        }

        private string ValidateSale()
        {
            if (cmbPhone.SelectedIndex == -1)
            {
                return "Vui lòng nhập số điện thoại!";
            }

            if (txtAddress.Text == "")
            {
                return "Vui lòng nhập địa chỉ!";
            }

            if (txtName.Text == "")
            {
                return "Vui lòng nhập tên khách hàng!";
            }

            if (rbtnShip.Checked && txtShipCost.Text == "")
            {
                return "Vui lòng nhập giá ship!";
            }

            if (rbtnShip.Checked && cmbShipper.SelectedIndex != -1)
            {
                return "Vui lòng chọn shipper!";
            }

            if (txtCost.Text == "")
            {
                return "Vui lòng nhập tiền khách trả!";
            }

            if (dgridOrder.Rows.Count <= 0)
            {
                return "Chưa chọn sản phẩm!";
            }

            if (float.Parse(txtCost.Text) < float.Parse(lblTotalPrice.Text))
            {
                return "Khách chưa trả đủ tiền!";
            }

            return null;
        }

        private string TotalPrice()
        {
            float totalPrice = 0;
            foreach (DataGridViewRow x in dgridOrder.Rows)
            {
                totalPrice += float.Parse(x.Cells[3].Value.ToString()) * float.Parse(x.Cells[4].Value.ToString());
            }
            return ConvertMoney.ConvertToVND(totalPrice);
        }

        private void LoadData()
        {
            dgridProduct.Rows.Clear();
            var result = _iProductServices.GetViewProducts();
            foreach (var x in result.Where(c => c.product.Status))
            {
                using (FileStream fileStream = new FileStream(x.product.ProductImage, FileMode.Open))
                {
                    dgridProduct.Rows.Add(x.product.ProductId, new Bitmap(fileStream), x.product.ProductName,
                        x.inventory.Amount, ConvertMoney.ConvertToVND(x.productDetail.UnitPrice), x.product.Description,
                        x.productDetail.BrandId, x.productDetail.MaterialId, x.productDetail.ColorId,
                        x.productDetail.SizeId, x.productDetail.CategoryId, "Thêm");
                }
            }

            cmbColor.DataSource = _iColorServices.GetColors();
            cmbColor.DisplayMember = "ColorName";
            cmbColor.ValueMember = "ColorId";
            cmbColor.SelectedIndex = -1;

            cmbBrand.DataSource = _iBrandServices.GetBrands();
            cmbBrand.DisplayMember = "BrandName";
            cmbBrand.ValueMember = "BrandId";
            cmbBrand.SelectedIndex = -1;

            cmbShipper.DataSource = _iShipperServices.GetListShipper();
            cmbShipper.DisplayMember = "ShipperName";
            cmbShipper.ValueMember = "ShipperId";
            cmbShipper.SelectedIndex = -1;

            cmbPhone.DataSource = _iCustomerServices.GetCustomers();
            cmbPhone.DisplayMember = "Phone";
            cmbPhone.ValueMember = "CustomerId";
            cmbPhone.SelectedIndex = -1;

            btnCancel.Tag = "";
        }

        private void btnViewOrder_Click(object sender, EventArgs e)
        {
            _frmMain.VisibleInvoice();
        }

        public void GetInfoFromInvoice(string id)
        {
            var invoice = _iInvoiceServices.GetInvoices().FirstOrDefault(c => c.InvoiceId == id);
            var invoiceDetail = _iInvoiceServices.GetInvoicesDetail().Where(c => invoice != null && c.InvoiceId == invoice.InvoiceId).ToList();
            if (invoice != null)
            {
                var customer = _iCustomerServices.GetCustomerById(invoice.CustomerId);
                cmbPhone.SelectedValue = invoice.CustomerId;
                txtAddress.Text = customer.Address;
                txtName.Text = customer.CustomerName;
                if (invoice.ShipperId != null)
                {
                    rbtnShip.Checked = true;
                    txtShipCost.Text = ConvertMoney.ConvertToVND((double)invoice.ShipCost);
                    cmbShipper.SelectedValue = invoice.ShipperId;
                }
                else
                {
                    rbtnShop.Checked = true;
                }

                dgridOrder.Rows.Clear();
                var totalPrice = 0;
                foreach (var x in invoiceDetail)
                {
                    using (FileStream fileStream = new FileStream(_iProductServices.GetProductById(x.ProductId).ProductImage, FileMode.Open))
                    {
                        dgridOrder.Rows.Add(x.ProductId, new Bitmap(fileStream), _iProductServices.GetProductById(x.ProductId).ProductName, x.Quantity, ConvertMoney.ConvertToVND(x.Price), ConvertMoney.ConvertToVND(x.TotalPrice), "Thêm");
                    }
                    totalPrice += int.Parse(x.TotalPrice.ToString());
                }
                lblTotalPrice.Text = ConvertMoney.ConvertToVND(totalPrice);
                txtCost.Text = ConvertMoney.ConvertToVND((double)invoice.GuestPayments);
                btnCancel.Tag = invoice.InvoiceId;
            }
        }

        private void FrmSales_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dgridProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 11)
            {
                var result = _iProductServices.GetViewProducts().FirstOrDefault(c =>
                    c.product.ProductId == dgridProduct.Rows[e.RowIndex].Cells[0].Value.ToString());
                if (result != null)
                {
                    foreach (DataGridViewRow x in dgridOrder.Rows)
                    {
                        if (x.Cells[2].Value.ToString() == result.product.ProductName)
                        {
                            x.Cells[3].Value = int.Parse(x.Cells[3].Value.ToString()) + 1;
                            x.Cells[4].Value = ConvertMoney.ConvertToVND(result.productDetail.UnitPrice);
                            x.Cells[5].Value = ConvertMoney.ConvertToVND(double.Parse(x.Cells[3].Value.ToString()) * double.Parse(x.Cells[4].Value.ToString()));
                            lblTotalPrice.Text = TotalPrice();
                            return;
                        }
                    }

                    using (FileStream fileStream = new FileStream(result.product.ProductImage, FileMode.Open))
                    {
                        dgridOrder.Rows.Add(result.product.ProductId, new Bitmap(fileStream),
                            result.product.ProductName,
                            "1", ConvertMoney.ConvertToVND(result.productDetail.UnitPrice), ConvertMoney.ConvertToVND(result.productDetail.UnitPrice), "+", "-", "Xóa");
                        lblTotalPrice.Text = TotalPrice();
                    }
                }
            }
        }

        private void dgridOrder_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 6)
            {
                var row = dgridOrder.Rows[e.RowIndex];
                var quantity = int.Parse(row.Cells[3].Value.ToString()) + 1;
                row.Cells[3].Value = quantity;
                row.Cells[5].Value = ConvertMoney.ConvertToVND(double.Parse(row.Cells[3].Value.ToString()) * double.Parse(row.Cells[4].Value.ToString()));
                lblTotalPrice.Text = TotalPrice();
            }
            if (e.ColumnIndex == 7)
            {
                var row = dgridOrder.Rows[e.RowIndex];
                if (int.Parse(row.Cells[3].Value.ToString()) != 0)
                {
                    var quantity = int.Parse(row.Cells[3].Value.ToString()) - 1;
                    row.Cells[3].Value = quantity;
                    row.Cells[5].Value = ConvertMoney.ConvertToVND(double.Parse(row.Cells[3].Value.ToString()) * double.Parse(row.Cells[4].Value.ToString()));
                    lblTotalPrice.Text = TotalPrice();
                }
                else
                {
                    dgridOrder.Rows.Remove(dgridOrder.Rows[e.RowIndex]);
                }
            }
            if (e.ColumnIndex == 8)
            {
                if (MessageBox.Show("Bạn có chắc muốn xóa?", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    dgridOrder.Rows.RemoveAt(e.RowIndex);
                }
            }
        }

        private void txtSearch_OnValueChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow x in dgridOrder.Rows)
            {
                x.Visible = x.Cells[2].Value.ToString().Contains(txtSearch.Text);
            }
        }

        private void cmbBrand_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmbBrand.SelectedIndex != -1)
            {
                foreach (DataGridViewRow x in dgridProduct.Rows)
                {
                    if (cmbColor.SelectedIndex != -1)
                    {
                        x.Visible = x.Cells[6].Value.ToString().Equals(cmbBrand.SelectedValue.ToString()) && x.Cells[8].Value.ToString().Equals(cmbColor.SelectedValue.ToString());
                    }
                    else
                    {
                        x.Visible = x.Cells[6].Value.ToString().Equals(cmbBrand.SelectedValue.ToString());
                    }
                }
            }
        }

        private void cmbColor_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmbColor.SelectedIndex != -1)
            {
                foreach (DataGridViewRow x in dgridProduct.Rows)
                {
                    if (cmbBrand.SelectedIndex != -1)
                    {
                        x.Visible = x.Cells[8].Value.ToString().Equals(cmbColor.SelectedValue.ToString()) && x.Cells[6].Value.ToString().Equals(cmbBrand.SelectedValue.ToString());
                    }
                    else
                    {
                        x.Visible = x.Cells[8].Value.ToString().Equals(cmbColor.SelectedValue.ToString());
                    }
                }
            }
        }

        private void cmbInvoice_SelectionChangeCommitted(object sender, EventArgs e)
        {
            _frmMain.FilterInvoice(cmbInvoice.SelectedItem.ToString());
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (ValidateSale() is null)
            {
                if (MessageBox.Show("Bạn có chắc muốn thêm vào hóa đơn chờ?", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                        var invoiceId = !_iInvoiceServices.GetInvoices().Any()
                            ? "IV1"
                            : "IV" + _iInvoiceServices.GetInvoices().Max(c => int.Parse(c.InvoiceId.Replace("IV", "")) + 1);
                        var invoice = new Invoice()
                        {
                            InvoiceId = invoiceId,
                            DateCreate = DateTime.Now,
                            CustomerId = cmbPhone.SelectedValue.ToString(),
                            EmployeeId = _iEmployeeServices.GetEmployees().First().EmployeeId,
                            InvoiceStatus = false,
                            ShipperId = cmbShipper.Enabled ? cmbShipper.SelectedValue.ToString() : null,
                            ShipCost = txtShipCost.Text != "" ? float.Parse(txtShipCost.Text) : 0,
                            GuestPayments = float.Parse(txtCost.Text)
                        };
                        List<InvoiceDetail> invoiceDetails = new List<InvoiceDetail>();
                        foreach (DataGridViewRow x in dgridOrder.Rows)
                        {
                            var result = new InvoiceDetail()
                            {
                                InvoiceId = invoiceId,
                                ProductId = x.Cells[0].Value.ToString(),
                                Quantity = int.Parse(x.Cells[3].Value.ToString()),
                                Price = float.Parse(x.Cells[4].Value.ToString()),
                                TotalPrice = float.Parse(x.Cells[5].Value.ToString())
                            };
                            invoiceDetails.Add(result);
                        }
                        MessageBox.Show(_iInvoiceServices.AddInvoice(invoice, invoiceDetails));
                }
                _frmMain.LoadData();
                _iCustomerServices.IncreasePurchase(cmbPhone.SelectedValue.ToString());
                
            }
            else
            {
                MessageBox.Show(ValidateSale());
            }
        }

        private void cmbPhone_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmbPhone.SelectedIndex != -1)
            {
                var customer = _iCustomerServices.GetCustomerById(cmbPhone.SelectedValue.ToString());
                txtName.Text = customer.CustomerName;
                txtAddress.Text = customer.Address;
                txtName.Enabled = false;
                txtAddress.Enabled = false;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn hoàn thành!", "Thông báo", MessageBoxButtons.YesNo) ==
                DialogResult.Yes)
            {
                MessageBox.Show(_iInvoiceServices.CompleteInvoice(invoidId));
            }
        }

        private void btnQr_Click(object sender, EventArgs e)
        {
            if (btnQr.ButtonText == "Quét mã")
            {
                btnQr.ButtonText = "Thêm thủ công";
                dgridProduct.Visible = false;
                pnlQr.Visible = true;
            }
            else
            {
                btnQr.ButtonText = "Quét mã";
                dgridProduct.Visible = true;
                pnlQr.Visible = false;
            }
        }

        private void btnQr2_Click(object sender, EventArgs e)
        {
            if (btnQr2.ButtonText == "Quét mã")
            {
                btnQr2.ButtonText = "Dừng quét";
                _videoCaptureDevice = new VideoCaptureDevice(_filterInfo[cmbCamera.SelectedIndex].MonikerString);
                _videoCaptureDevice.NewFrame += (o, args) => pbxQr.Image = (Bitmap)args.Frame.Clone();
                _videoCaptureDevice.Start();
                timer1.Start();
            }
            else
            {
                if (_videoCaptureDevice != null && _videoCaptureDevice.IsRunning)
                    _videoCaptureDevice.Stop();
                pbxQr.Image = null;
                btnQr2.ButtonText = "Quét mã";
            }
        }

        private void FrmSales_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_videoCaptureDevice != null && _videoCaptureDevice.IsRunning)
                _videoCaptureDevice.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (pbxQr.Image != null)
            {
                BarcodeReader barcodeReader = new BarcodeReader();
                Result result = barcodeReader.Decode((Bitmap)pbxQr.Image);
                if (result != null)
                {
                    var product = _iProductServices.GetViewProducts()
                        .FirstOrDefault(c => c.product.Barcode == result.ToString());
                    if (product != null)
                    {
                        foreach (DataGridViewRow x in dgridOrder.Rows)
                        {
                            if (x.Cells[2].Value.ToString() == product.product.ProductName)
                            {
                                x.Cells[3].Value = int.Parse(x.Cells[3].Value.ToString()) + 1;
                                x.Cells[4].Value = int.Parse(x.Cells[4].Value.ToString()) +
                                                   product.productDetail.UnitPrice;
                                x.Cells[5].Value = int.Parse(x.Cells[3].Value.ToString()) *
                                                   int.Parse(x.Cells[4].Value.ToString());
                                lblTotalPrice.Text = TotalPrice();
                                return;
                            }
                        }

                        using (FileStream fileStream = new FileStream(product.product.ProductImage, FileMode.Open))
                        {
                            dgridOrder.Rows.Add(product.product.ProductId, new Bitmap(fileStream),
                                product.product.ProductName,
                                "1", product.productDetail.UnitPrice, product.productDetail.UnitPrice, "Xóa");
                        }
                        lblTotalPrice.Text = TotalPrice();
                        btnQr2.ButtonText = "Quét mã";
                        pbxQr.Image = null;
                    }
                    else
                    {
                        MessageBox.Show("Không tồn tại sản phẩm!");
                    }
                }
            }
        }

        private void rbtnShip_CheckedChanged(object sender, EventArgs e)
        {
            if (cmbShipper.Enabled == false)
            {
                txtShipCost.Enabled = true;
                cmbShipper.Enabled = true;
            }
            else
            {
                txtShipCost.Enabled = false;
                cmbShipper.Enabled = false;
            }
        }

        private void txtCost_OnValueChanged(object sender, EventArgs e)
        {
            if (txtCost.Text != "" && lblTotalPrice.Text != "0 VNĐ")
            {
                if (float.Parse(txtCost.Text) > float.Parse(lblTotalPrice.Text))
                {
                    lblMoneyLeft.Text = ConvertMoney.ConvertToVND(float.Parse(txtCost.Text) - float.Parse(lblTotalPrice.Text));
                }
            }
        }

        private void lblCancel_Click(object sender, EventArgs e)
        {
            Form form = new Form();

            Label label = new Label();
            label.Dock = DockStyle.Top;
            label.AutoSize = false;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Text = "Lý do hủy";
            label.Font = new Font("Arial", 10);

            TextBox textBox = new TextBox();
            textBox.Dock = DockStyle.Top;
            textBox.Font = new Font("Arial", 18);
            textBox.Multiline = true;
            textBox.Size =  new System.Drawing.Size(0, 150);

            Button button = new Button();
            button.Dock = DockStyle.Top;
            button.Size = new System.Drawing.Size(0, 80);
            button.Text = "Xác nhận";
            button.Click += (o, args) =>
            {
                if (MessageBox.Show("Bạn có chắc muốn xóa?", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    MessageBox.Show(_iInvoiceServices.CancelInvoice(invoidId, textBox.Text));
                    _frmMain.LoadData();
                }
                else
                {
                    return;
                }
                form.Close();
            };

            form.StartPosition = FormStartPosition.CenterParent;
            form.Controls.Add(button);
            form.Controls.Add(textBox);
            form.Controls.Add(label);
            form.ShowDialog();
        }

        private void cmbPhone_TextChanged(object sender, EventArgs e)
        {
            // if (cmbPhone.SelectedValue != null)
            // {
            //     txtAddress.Enabled = false;
            //     txtName.Enabled = false;
            // }
            // else
            // {
            //     txtAddress.Enabled = true;
            //     txtName.Enabled = true;
            // }
        }

        private void cmbBrand_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        private void cmbBrand_MouseDown(object sender, MouseEventArgs e)
        {
            var combobox = sender as ComboBox;
            if (e.Button == MouseButtons.Right)
            {
                if (combobox != null) combobox.SelectedIndex = -1;
                foreach (DataGridViewRow x in dgridProduct.Rows)
                {
                    if (x.Visible == false)
                    {
                        x.Visible = true;
                    }
                }
            }
        }
    }
}
