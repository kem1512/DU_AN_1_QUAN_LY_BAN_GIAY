﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BUS_BussinessLayer.BUS_Services;
using BUS_BussinessLayer.iBUS_Services;
using DAL_DataAccessLayer.Entities;

namespace GUI_PresentationLayer.View
{
    public partial class FrmCustomer : Form
    {
        private iCustomerServices _iCustomerServices = new CustomerServices();
        public FrmCustomer()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            dgridCustomer.Rows.Clear();
            foreach (var x in _iCustomerServices.GetCustomers().Where(c => c.Status))
            {
                dgridCustomer.Rows.Add(x.CustomerId, x.CustomerName, x.Phone, x.Address, x.ShoppingCount, "Xóa");
            }

            dgridDisable.Rows.Clear();
            foreach (var x in _iCustomerServices.GetCustomers().Where(c => c.Status == false))
            {
                dgridDisable.Rows.Add(x.CustomerId, x.CustomerName, x.Phone, x.Address, x.ShoppingCount, "Phục hồi");
            }

            txtName.Text = "";
            txtAddress.Text = "";
            txtPhone.Text = "";
            txtSearch.Text = "";
        }

        private string ValidateCustomer()
        {
            if (txtName.Text == "")
            {
                return "Tên không được bỏ trống!";
            }

            if (txtPhone.Text == "")
            {
                return "Số điện thoại không được bỏ trống!";
            }

            if (txtAddress.Text == "")
            {
                return "Địa chỉ không được bỏ trống!";
            }
            return null;
        }

        private void FrmCustomer_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dgridCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                var row = dgridCustomer.Rows[e.RowIndex];
                txtName.Text = row.Cells[1].Value.ToString();
                txtPhone.Text = row.Cells[2].Value.ToString();
                txtAddress.Text = row.Cells[3].Value.ToString();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateCustomer() is null)
            {
                var customerId = !_iCustomerServices.GetCustomers().Any() ? "CT1" : "CT" + _iCustomerServices.GetCustomers().Max(c => int.Parse(c.CustomerId.Replace("CT", "")) + 1);
                if (MessageBox.Show("Bạn có chắc muốn thêm?", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    MessageBox.Show(_iCustomerServices.AddCustomer(new Customer()
                    {
                        CustomerId = customerId,
                        CustomerName = txtName.Text,
                        Address = txtAddress.Text,
                        Phone = txtPhone.Text,
                        ShoppingCount = 0,
                        Status = true
                    }));
                    LoadData();
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (ValidateCustomer() is null)
            {
                if (dgridCustomer.CurrentRow != null)
                {
                    var row = dgridCustomer.Rows[dgridCustomer.CurrentRow.Index];
                    if (MessageBox.Show("Bạn có chắc muốn sửa?", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        MessageBox.Show(_iCustomerServices.UpdateCustomer(new Customer()
                        {
                            CustomerId = row.Cells[0].Value.ToString(),
                            CustomerName = txtName.Text,
                            Address = txtAddress.Text,
                            Phone = txtPhone.Text,
                            ShoppingCount = int.Parse(row.Cells[4].Value.ToString()),
                            Status = true
                        }));
                        LoadData();
                    }
                }
            }
        }

        private void dgridCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5)
            {
                if (MessageBox.Show("Bạn có chắc muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo) ==
                    DialogResult.Yes)
                {
                    MessageBox.Show(
                        _iCustomerServices.DisableCustomer(dgridCustomer.Rows[e.RowIndex].Cells[0].Value.ToString()));
                    LoadData();
                }
            }
        }

        private void dgridDisable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 5)
            {
                if (MessageBox.Show("Bạn có chắc muốn phục hồi không?", "Thông báo", MessageBoxButtons.YesNo) ==
                    DialogResult.Yes)
                {
                    MessageBox.Show(
                        _iCustomerServices.RecoveryCustomer(dgridDisable.Rows[e.RowIndex].Cells[0].Value.ToString()));
                    LoadData();
                }
            }
        }
    }
}