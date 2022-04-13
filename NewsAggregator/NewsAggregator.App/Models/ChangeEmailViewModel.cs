namespace NewsAggregator.App.Models
{
    public class ChangeEmailViewModel
    {
        public Guid UserId { get; set; }
        public string OldEmail { get; set; }
        public string NewEmail { get; set; }
        public string ConfirmEmail { get; set; }
    }
}
