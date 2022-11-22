//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL_DataAccessLayer.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductDetail
    {
        public ProductDetail()
        {
            this.InvoiceDetails = new HashSet<InvoiceDetail>();
        }
    
        public string ProductId { get; set; }
        public string ColorId { get; set; }
        public string SizeId { get; set; }
        public string BrandId { get; set; }
        public string MaterialId { get; set; }
        public string CategoryId { get; set; }
        public double UnitPrice { get; set; }
    
        public virtual Brand Brand { get; set; }
        public virtual Category Category { get; set; }
        public virtual Color Color { get; set; }
        public virtual Inventory Inventory { get; set; }
        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; }
        public virtual Material Material { get; set; }
        public virtual Product Product { get; set; }
        public virtual Size Size { get; set; }
    }
}
