//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sales_Inventory.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Employee
    {
        public int EmployeeId { get; set; }
        public int UserId { get; set; }
        public string Gender { get; set; }
        public System.DateTime DOB { get; set; }
        public int BloodGroupId { get; set; }
        public string PermanentAddress { get; set; }
        public string PhoneNo { get; set; }
        public string AlternatePhoneNo { get; set; }
        public string GuardianNumber { get; set; }
        public string AadharNumber { get; set; }
        public Nullable<System.DateTime> DateofJoining { get; set; }
        public string LastCompanyWorkedin { get; set; }
        public Nullable<System.DateTime> DateLeft { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    }
}
