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
    
    public partial class Employee
    {
        public Employee()
        {
            this.Invoice = new HashSet<Invoice>();
        }
    
        public string EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Pass { get; set; }
        public string Phone { get; set; }
        public bool Gender { get; set; }
        public string Address { get; set; }
        public string EmployeeImage { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public string RoleId { get; set; }
    
        public virtual Roles Roles { get; set; }
        public virtual ICollection<Invoice> Invoice { get; set; }
    }
}
