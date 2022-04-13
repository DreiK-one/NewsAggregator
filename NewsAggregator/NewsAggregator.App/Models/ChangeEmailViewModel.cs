using System.ComponentModel.DataAnnotations;

namespace NewsAggregator.App.Models
{
    public class ChangeEmailViewModel
    {
        public Guid UserId { get; set; } 

        [Display(Name = "Current email")]
        public string CurrentEmail { get; set; }

        [Display(Name = "New email")]
        public string NewEmail { get; set; }

        [Display(Name = "Confirm email")]
        public string ConfirmEmail { get; set; }
    }
}
