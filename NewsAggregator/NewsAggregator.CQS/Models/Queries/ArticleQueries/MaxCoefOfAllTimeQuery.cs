using MediatR;


namespace NewsAggregetor.CQS.Models.Queries.ArticleQueries
{
    public class MaxCoefOfAllTimeQuery : IRequest<float?>
    {
    }
}
