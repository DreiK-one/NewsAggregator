namespace NewsAggregator.App.Models
{
    public class ChangeNicknameViewModel
    {
        public Guid UserId { get; set; }
        public string OldNickname { get; set; }
        public string NewNickname { get; set; }
        public string ConfirmNickname { get; set; }
    }
}
