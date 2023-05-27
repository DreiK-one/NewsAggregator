using MediatR;


namespace NewsAggregetor.CQS.Models.Commands.AccountCommands
{
    public class UpdateNicknameCommand : IRequest<int>
    {
        public UpdateNicknameCommand(Guid userId, string nickname)
        {
            UserId = userId;
            Nickname = nickname;
            NormalizedNickname = nickname.ToUpperInvariant();
        }

        public Guid UserId { get; set; }
        public string Nickname { get; set; }
        public string NormalizedNickname { get; set; }
    }
}
