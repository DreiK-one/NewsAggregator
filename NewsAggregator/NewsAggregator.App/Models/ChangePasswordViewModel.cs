using System.ComponentModel.DataAnnotations;

namespace NewsAggregator.App.Models
{
    public class ChangePasswordViewModel
    {
        public Guid UserId { get; set; }

        [Display(Name = "Current password")]
        public string CurrentPassword { get; set; }

        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm new password")]
        public string ConfirmPassword { get; set; }
    }
}
