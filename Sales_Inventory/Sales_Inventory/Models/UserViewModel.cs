using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sales_Inventory.Models
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public int? RoleId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Fullname { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public int State { get; set; }
        public string City { get; set; }
        public int PostalCode { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string PasswordResetToken { get; set; }
    }
}