using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sales_Inventory.Models
{
    public class EmployeeViewModel
    {
        public int EmployeeId { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public System.DateTime DOB { get; set; }
        public string BloodGroup { get; set; }
        public string PermanentAddress { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNo { get; set; }
        public string AlternateNumber { get; set; }
        public string AadharNumber { get; set; }
        public Nullable<System.DateTime> DateofJoining { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    }
}