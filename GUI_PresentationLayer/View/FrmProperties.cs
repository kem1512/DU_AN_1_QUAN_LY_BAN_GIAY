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
    public partial class FrmProperties : Form
    {
        private readonly iCategoryServices _iCategoryServices;
        private readonly iBrandServices _iBrandServices;
        private readonly iMaterialServices _iMaterialServices;
        private readonly iSizeServices _iSizeServices;
        private readonly iColorServices _iColorServices;
        public FrmProperties(Properties properties)
        {
            InitializeComponent();
            if (Properties.Category == properties)
            {
                _iCategoryServices = new CategoryServices();
                LoadCategories();
            }else if (Properties.Brand == properties)
            {
                _iBrandServices = new BrandServices();
                LoadBrands();
            }else if (Properties.Material == properties)
            {
                _iMaterialServices = new MaterialServices();
                LoadMaterials();
            }else if (Properties.Size == properties)
            {
                _iSizeServices = new SizeServices();
                LoadSizes();
            }else if (Properties.Color == properties)
            {
                _iColorServices = new ColorServices();
                LoadColors();
            }
        }

        private void LoadCategories()
        {
            dgridProperties.ColumnCount = 2;
            dgridProperties.Columns[0].Name = "Mã thể loại";
            dgridProperties.Columns[1].Name = "Tên thể loại";
            lblContent.Text = "Danh sách thể loại";
            lblId.Text = "Id thể loại";
            lblName.Text = "Tên thể loại";
            dgridProperties.Rows.Clear();
            foreach (var x in _iCategoryServices.GetCategories())
            {
                dgridProperties.Rows.Add(x.CategoryId, x.CategoryName);
            }
            txtId.Text = "";
            txtName.Text = "";
        }

        private void LoadBrands()
        {
            dgridProperties.ColumnCount = 2;
            dgridProperties.Columns[0].Name = "Mã thương hiệu";
            dgridProperties.Columns[1].Name = "Tên thương hiệu";
            lblContent.Text = "Danh sách thương hiệu";
            lblId.Text = "Mã thương hiệu";
            lblName.Text = "Tên thương hiệu";
            dgridProperties.Rows.Clear();
            foreach (var x in _iBrandServices.GetBrands())
            {
                dgridProperties.Rows.Add(x.BrandId, x.BrandName);
            }
            txtId.Text = "";
            txtName.Text = "";
        }

        private void LoadMaterials()
        {
            dgridProperties.ColumnCount = 2;
            dgridProperties.Columns[0].Name = "Mã chất liệu";
            dgridProperties.Columns[1].Name = "Tên chất liệu";
            lblContent.Text = "Danh sách chất liệu";
            lblId.Text = "Mã chất liệu";
            lblName.Text = "Tên chất liệu";
            dgridProperties.Rows.Clear();
            foreach (var x in _iMaterialServices.GetMaterials())
            {
                dgridProperties.Rows.Add(x.MaterialId, x.MaterialName);
            }
            txtId.Text = "";
            txtName.Text = "";
        }

        private void LoadSizes()
        {
            dgridProperties.ColumnCount = 2;
            dgridProperties.Columns[0].Name = "Mã kích cỡ";
            dgridProperties.Columns[0].Name = "Tên kích cỡ";
            lblContent.Text = "Danh sách kích cỡ";
            lblId.Text = "Mã kích cỡ";
            lblName.Text = "Tên kích cỡ";
            dgridProperties.Rows.Clear();
            foreach (var x in _iSizeServices.GetSizes())
            {
                dgridProperties.Rows.Add(x.SizeId, x.SizeName);
            }
            txtId.Text = "";
            txtName.Text = "";
        }

        private void LoadColors()
        {
            dgridProperties.ColumnCount = 2;
            dgridProperties.Columns[0].Name = "Mã màu";
            dgridProperties.Columns[1].Name = "Tên màu";
            lblContent.Text = "Danh sách màu";
            lblId.Text = "Mã màu";
            lblName.Text = "Tên màu";
            dgridProperties.Rows.Clear();
            foreach (var x in _iColorServices.GetColors())
            {
                dgridProperties.Rows.Add(x.ColorId, x.ColorName);
            }
            txtId.Text = "";
            txtName.Text = "";
        }

        public enum Properties
        {
            Category = 0, Brand = 1, Material = 2, Size = 3, Color = 4
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (_iCategoryServices != null)
            {
                if (txtName.Text == "")
                {
                    MessageBox.Show("Tên không được bỏ trống!");
                }
                else
                {
                    // tìm id lớn nhất và cộng thêm 1
                    int categoryId = !_iBrandServices.GetBrands().Any() ? 1 : _iCategoryServices.GetCategories().Max(c => int.Parse(c.CategoryId.Replace("CT", ""))) + 1;
                    if (MessageBox.Show("Bạn có chắc muốn thêm không?", "Thông báo", MessageBoxButtons.YesNo) ==
                        DialogResult.Yes)
                    {
                        MessageBox.Show(_iCategoryServices.AddCategory(new Category()
                        {
                            CategoryId = "CT" + categoryId,
                            CategoryName = txtName.Text
                        }));
                        LoadCategories();
                    }
                }
            }
            else if (_iBrandServices != null)
            {
                if (txtName.Text == "")
                {
                    MessageBox.Show("Tên không được bỏ trống!");
                }
                else
                {
                    // tìm id lớn nhất và cộng thêm 1
                    int brandId = !_iBrandServices.GetBrands().Any() ? 1 : _iBrandServices.GetBrands().Max(c => int.Parse(c.BrandId.Replace("BR", ""))) + 1;
                    if (MessageBox.Show("Bạn có chắc muốn thêm không?", "Thông báo", MessageBoxButtons.YesNo) ==
                        DialogResult.Yes)
                    {
                        MessageBox.Show(_iBrandServices.AddBrand(new Brand()
                        {
                            BrandId = "BR" + brandId,
                            BrandName = txtName.Text
                        }));
                        LoadBrands();
                    }
                }
            }
            else if (_iMaterialServices != null)
            {
                if (txtName.Text == "")
                {
                    MessageBox.Show("Tên không được bỏ trống!");
                }
                else
                {
                    // tìm id lớn nhất và cộng thêm 1
                    int materialId = !_iMaterialServices.GetMaterials().Any() ? 1 : _iMaterialServices.GetMaterials().Max(c => int.Parse(c.MaterialId.Replace("MT", ""))) + 1;
                    if (MessageBox.Show("Bạn có chắc muốn thêm không?", "Thông báo", MessageBoxButtons.YesNo) ==
                        DialogResult.Yes)
                    {
                        MessageBox.Show(_iMaterialServices.AddMaterial(new Material()
                        {
                            MaterialId = "MT" + materialId,
                            MaterialName = txtName.Text
                        }));
                        LoadCategories();
                    }
                }
            }
            else if (_iSizeServices != null)
            {
                if (txtName.Text == "")
                {
                    MessageBox.Show("Tên không được bỏ trống!");
                }
                else
                {
                    // tìm id lớn nhất và cộng thêm 1
                    int sizeId = !_iSizeServices.GetSizes().Any() ? 1 : _iSizeServices.GetSizes().Max(c => int.Parse(c.SizeId.Replace("S", ""))) + 1;
                    if (MessageBox.Show("Bạn có chắc muốn thêm không?", "Thông báo", MessageBoxButtons.YesNo) ==
                        DialogResult.Yes)
                    {
                        MessageBox.Show(_iSizeServices.AddSize(new DAL_DataAccessLayer.Entities.Size()
                        {
                            SizeId = "S" + sizeId,
                            SizeName = txtName.Text
                        }));
                        LoadSizes();
                    }
                }
            }
            else if (_iColorServices != null)
            {
                if (txtName.Text == "")
                {
                    MessageBox.Show("Tên không được bỏ trống!");
                }
                else
                {
                    // tìm id lớn nhất và cộng thêm 1
                    int colorId = !_iColorServices.GetColors().Any()
                        ? 1
                        : _iColorServices.GetColors().Max(c => int.Parse(c.ColorId.Replace("CL", ""))) + 1;
                    if (MessageBox.Show("Bạn có chắc muốn thêm không?", "Thông báo", MessageBoxButtons.YesNo) ==
                        DialogResult.Yes)
                    {
                        MessageBox.Show(_iColorServices.AddColor(new DAL_DataAccessLayer.Entities.Color()
                        {
                            ColorId = "CL" + colorId,
                            ColorName = txtName.Text
                        }));
                        LoadColors();
                    }
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgridProperties.CurrentRow != null)
            {
                if (_iCategoryServices != null)
                {
                    if (txtName.Text == "")
                    {
                        MessageBox.Show("Tên không được bỏ trống!");
                    }
                    else
                    {
                        var category = _iCategoryServices.GetCategoryById(txtId.Text);
                        if (MessageBox.Show("Bạn có chắc muốn sửa không?", "Thông báo", MessageBoxButtons.YesNo) ==
                            DialogResult.Yes)
                        {
                            MessageBox.Show(_iCategoryServices.UpdateCategory(new Category()
                            {
                                CategoryId = category.CategoryId,
                                CategoryName = txtName.Text
                            }));
                            LoadCategories();
                        }
                    }
                }
                else if (_iBrandServices != null)
                {
                    if (txtName.Text == "")
                    {
                        MessageBox.Show("Tên không được bỏ trống!");
                    }
                    else
                    {
                        var brand = _iBrandServices.GetBrandById(txtId.Text);
                        if (MessageBox.Show("Bạn có chắc muốn sửa không?", "Thông báo", MessageBoxButtons.YesNo) ==
                            DialogResult.Yes)
                        {
                            MessageBox.Show(_iBrandServices.UpdateBrand(new Brand()
                            {
                                BrandId = brand.BrandId,
                                BrandName = txtName.Text
                            }));
                            LoadBrands();
                        }
                    }
                }
                else if (_iMaterialServices != null)
                {
                    if (txtName.Text == "")
                    {
                        MessageBox.Show("Tên không được bỏ trống!");
                    }
                    else
                    {
                        var material = _iMaterialServices.GetMaterialById(txtId.Text);
                        if (MessageBox.Show("Bạn có chắc muốn sửa không?", "Thông báo", MessageBoxButtons.YesNo) ==
                            DialogResult.Yes)
                        {
                            MessageBox.Show(_iMaterialServices.UpdateMaterial(new Material()
                            {
                                MaterialId = material.MaterialId,
                                MaterialName = txtName.Text
                            }));
                            LoadMaterials();
                        }
                    }
                }
                else if (_iSizeServices != null)
                {
                    if (txtName.Text == "")
                    {
                        MessageBox.Show("Tên không được bỏ trống!");
                    }
                    else
                    {
                        var material = _iMaterialServices.GetMaterialById(txtId.Text);
                        if (MessageBox.Show("Bạn có chắc muốn sửa không?", "Thông báo", MessageBoxButtons.YesNo) ==
                            DialogResult.Yes)
                        {
                            MessageBox.Show(_iMaterialServices.UpdateMaterial(new Material()
                            {
                                MaterialId = material.MaterialId,
                                MaterialName = txtName.Text
                            }));
                            LoadMaterials();
                        }
                    }
                }
                else if (_iColorServices != null)
                {
                    if (txtName.Text == "")
                    {
                        MessageBox.Show("Tên không được bỏ trống!");
                    }
                    else
                    {
                        var color = _iColorServices.GetColorById(txtId.Text);
                        if (MessageBox.Show("Bạn có chắc muốn sửa không?", "Thông báo", MessageBoxButtons.YesNo) ==
                            DialogResult.Yes)
                        {
                            MessageBox.Show(_iColorServices.UpdateColor(new DAL_DataAccessLayer.Entities.Color()
                            {
                                ColorId = color.ColorId,
                                ColorName = txtName.Text
                            }));
                            LoadColors();
                        }
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgridProperties.CurrentRow != null)
            {
                if (_iCategoryServices != null)
                {
                    if (txtId.Text == "")
                    {
                        MessageBox.Show("Mã không được bỏ trống!");
                    }
                    else
                    {
                        if (MessageBox.Show("Bạn có chắc muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo) ==
                            DialogResult.Yes)
                        {
                            MessageBox.Show(_iCategoryServices.RemoveCategory(txtId.Text));
                            LoadCategories();
                        }
                    }
                }
                else if (_iBrandServices != null)
                {
                    if (txtId.Text == "")
                    {
                        MessageBox.Show("Mã không được bỏ trống!");
                    }
                    else
                    {
                        if (MessageBox.Show("Bạn có chắc muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo) ==
                            DialogResult.Yes)
                        {
                            MessageBox.Show(_iBrandServices.RemoveBrand(txtId.Text));
                            LoadBrands();
                        }
                    }
                }
                else if (_iMaterialServices != null)
                {
                    if (txtId.Text == "")
                    {
                        MessageBox.Show("Mã không được bỏ trống!");
                    }
                    else
                    {
                        if (MessageBox.Show("Bạn có chắc muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo) ==
                            DialogResult.Yes)
                        {
                            MessageBox.Show(_iMaterialServices.RemoveMaterial(txtId.Text));
                            LoadMaterials();
                        }
                    }
                }
                else if (_iSizeServices != null)
                {
                    if (txtId.Text == "")
                    {
                        MessageBox.Show("Mã không được bỏ trống!");
                    }
                    else
                    {
                        if (MessageBox.Show("Bạn có chắc muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo) ==
                            DialogResult.Yes)
                        {
                            MessageBox.Show(_iSizeServices.RemoveSize(txtId.Text));
                            LoadSizes();
                        }
                    }
                }
                else if (_iColorServices != null)
                {
                    if (txtId.Text == "")
                    {
                        MessageBox.Show("Mã không được bỏ trống!");
                    }
                    else
                    {
                        if (MessageBox.Show("Bạn có chắc muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo) ==
                            DialogResult.Yes)
                        {
                            MessageBox.Show(_iColorServices.RemoveColor(txtId.Text));
                            LoadColors();
                        }
                    }
                }
            }
        }

        private void dgridProperties_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                var row = dgridProperties.Rows[e.RowIndex];
                txtId.Text = row.Cells[0].Value.ToString();
                txtName.Text = row.Cells[1].Value.ToString();
            }
        }
    }
}