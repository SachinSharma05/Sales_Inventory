using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sales_Inventory.Models
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "New Password is required")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm New Password is required")]
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }

        public string PasswordResetToken { get; set; }
    }
}