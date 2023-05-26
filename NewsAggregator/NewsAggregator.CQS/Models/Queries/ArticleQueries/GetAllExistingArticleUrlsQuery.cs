using MediatR;


namespace NewsAggregetor.CQS.Models.Queries.ArticleQueries
{
    public class GetAllExistingArticleUrlsQuery : IRequest<List<string>>
    {
    }
}
