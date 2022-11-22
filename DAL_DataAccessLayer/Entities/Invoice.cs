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
    
    public partial class Invoice
    {
        public Invoice()
        {
            this.InvoiceDetails = new HashSet<InvoiceDetail>();
        }
    
        public string InvoiceId { get; set; }
        public string CustomerId { get; set; }
        public string EmployeeId { get; set; }
        public string ShipperId { get; set; }
        public Nullable<double> ShipCost { get; set; }
        public System.DateTime DateCreate { get; set; }
        public Nullable<double> GuestPayments { get; set; }
        public bool InvoiceStatus { get; set; }
        public string Description { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Shipper Shipper { get; set; }
        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; }
    }
}
