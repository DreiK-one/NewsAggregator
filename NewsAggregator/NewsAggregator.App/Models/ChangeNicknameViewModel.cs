using System.ComponentModel.DataAnnotations;

namespace NewsAggregator.App.Models
{
    public class ChangeNicknameViewModel
    {
        public Guid UserId { get; set; }

        [Display(Name = "Current nickname")]
        public string CurrentNickname { get; set; }

        [Display(Name = "New nickname")]
        public string NewNickname { get; set; }

        [Display(Name = "Confirm new nickname")]
        public string ConfirmNickname { get; set; }
    }
}
