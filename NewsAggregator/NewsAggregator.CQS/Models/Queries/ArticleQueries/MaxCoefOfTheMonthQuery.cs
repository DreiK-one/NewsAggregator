using MediatR;


namespace NewsAggregetor.CQS.Models.Queries.ArticleQueries
{
    public class MaxCoefOfTheMonthQuery : IRequest<float?>
    {
    }
}
