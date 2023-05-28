using MediatR;


namespace NewsAggregetor.CQS.Models.Commands.SourceCommands
{
    public class UpdateSourceCommand : IRequest<int?>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string BaseUrl { get; set; }
        public string RssUrl { get; set; }
    }
}
