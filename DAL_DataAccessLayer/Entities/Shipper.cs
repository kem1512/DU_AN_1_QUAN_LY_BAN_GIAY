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
    
    public partial class Shipper
    {
        public string ShipperId { get; set; }
        public string ShipperName { get; set; }
        public string ShipperPhone { get; set; }
        public string RoleId { get; set; }
        public bool ShipperStatus { get; set; }
    
        public virtual Roles Roles { get; set; }
    }
}