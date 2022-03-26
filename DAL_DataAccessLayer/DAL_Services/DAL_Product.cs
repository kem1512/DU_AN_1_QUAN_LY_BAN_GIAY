﻿using DAL_DataAccessLayer.Entities;
using DAL_DataAccessLayer.iDAL_Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_DataAccessLayer.DAL_Services
{
    public class DAL_Product : iDAL_Product
    {
        private QuanLyBanGiayEntities _db;
        public string AddProduct(Product product, ProductDetail productDetail, Inventory inventory)
        {
            try
            {
                using (_db = new QuanLyBanGiayEntities())
                {
                    _db.Product.Add(product);
                    _db.ProductDetail.Add(productDetail);
                    _db.Inventory.Add(inventory);
                    return _db.SaveChanges() > 0 ? "Thêm thành công" : "Thêm thất bại";
                }
            }
            catch(Exception e)
            {
                return e.ToString();
            }
        }

        public string UpdateProduct(Product product, ProductDetail productDetail, Inventory inventory)
        {
            try
            {
                using (_db = new QuanLyBanGiayEntities())
                {
                    var pr = _db.Product.First(c => c.ProductId == product.ProductId);
                    pr.Description = product.Description;
                    pr.ProductImage = product.ProductImage;
                    pr.Status = product.Status;
                    pr.ProductName = product.ProductName;
                    var prd = _db.ProductDetail.First(c => c.ProductId == product.ProductId);
                    prd.UnitPrice = productDetail.UnitPrice;
                    prd.CategoryId = productDetail.CategoryId;
                    prd.ColorId = productDetail.ColorId;
                    prd.MaterialId = productDetail.MaterialId;
                    prd.SizeId = productDetail.SizeId;
                    prd.BrandId = productDetail.BrandId;
                    var iv = _db.Inventory.First(c => c.ProductId == product.ProductId);
                    iv.Amount = inventory.Amount;
                    _db.SaveChanges();
                    return "Sửa thành công!";
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public string DeleteProduct(string id)
        {
            try
            {
                using (_db = new QuanLyBanGiayEntities())
                {
                    _db.Inventory.Remove(_db.Inventory.First(c => c.ProductId == id));
                    _db.ProductDetail.Remove(_db.ProductDetail.First(c => c.ProductId == id));
                    _db.Product.Remove(_db.Product.First(c => c.ProductId == id));
                    return _db.SaveChanges() > 0 ? "Xóa thành công" : "Xóa thất bại";
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public List<Product> GetProducts()
        {
            using(_db = new QuanLyBanGiayEntities())
            {
                return _db.Product.ToList();
            }
        }

        public Product GetProductByName(string name)
        {
            using (_db = new QuanLyBanGiayEntities())
            {
                return _db.Product.First(c => c.ProductName == name);
            }
        }

        public string ChangeStatus(string id)
        {
            try
            {
                using (_db = new QuanLyBanGiayEntities())
                {
                    var product = _db.Product.First(c => c.ProductId == id);
                    product.Status = false;
                    return _db.SaveChanges() > 0 ? "Sản phẩm đã được chuyển vào thùng rác" : "Thất bại";
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public List<ProductDetail> GetProductDetail()
        {
            using (_db = new QuanLyBanGiayEntities())
            {
                return _db.ProductDetail.ToList();
            }
        }

        public List<Inventory> GetInventory()
        {
            using (_db = new QuanLyBanGiayEntities())
            {
                return _db.Inventory.ToList();
            }
        }

        public Product GetProductById(string id)
        {
            using (_db = new QuanLyBanGiayEntities())
            {
                return _db.Product.First(c => c.ProductId == id);
            }
        }
    }
}
