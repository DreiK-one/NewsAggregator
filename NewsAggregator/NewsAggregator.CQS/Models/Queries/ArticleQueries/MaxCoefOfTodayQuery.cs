using MediatR;


namespace NewsAggregetor.CQS.Models.Queries.ArticleQueries
{
    public class MaxCoefOfTodayQuery : IRequest<float?>
    {
    }
}
