using AutoMapper;
using HtmlAgilityPack;
using MediatR;
using Microsoft.Extensions.Logging;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Helpers;
using NewsAggregator.Core.Interfaces.InterfacesCQS;
using NewsAggregetor.CQS.Models.Commands.ArticleCommands;
using NewsAggregetor.CQS.Models.Queries.ArticleQueries;



namespace NewsAggregator.Domain.ServicesCQS
{
    public class HtmlParserServiceCQS : IHtmlParserServiceCQS
    {
        private readonly IMapper _mapper;
        private readonly ILogger<HtmlParserServiceCQS> _logger;
        private readonly IMediator _mediator;
        private readonly ISourceServiceCQS _sourceServiceCQS;

        public HtmlParserServiceCQS(IMapper mapper, 
            ILogger<HtmlParserServiceCQS> logger,
            IMediator mediator,
            ISourceServiceCQS sourceServiceCQS)
        {
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
            _sourceServiceCQS = sourceServiceCQS;
        }


        public async Task<int?> GetArticleContentFromUrlAsync()
        {
            var article = await _mediator.Send(new GetArticleWithEmptyBodyAndTitleQuery(), 
                new CancellationToken());

            if (article == null)
            {
                return null;
            }

            var sourceId = await _sourceServiceCQS.GetSourceByUrl(article.SourceUrl);

            switch (sourceId.ToString("D").ToUpperInvariant())
            {
                case Variables.IdOfNewsSources.Onliner:
                    var articleOnliner = await ParseOnlinerArticle(article.SourceUrl);

                    return await _mediator.Send(_mapper.Map<EditArticleCommand>(articleOnliner), 
                        new CancellationToken());

                case Variables.IdOfNewsSources.Goha:
                    var articleGoha = await ParseGohaArticle(article.SourceUrl);

                    return await _mediator.Send(_mapper.Map<EditArticleCommand>(articleGoha),
                        new CancellationToken());

                case Variables.IdOfNewsSources.Shazoo:
                    var article4pda = await ParseShazooArticle(article.SourceUrl);

                    return await _mediator.Send(_mapper.Map<EditArticleCommand>(article4pda),
                        new CancellationToken());

                default:
                    break;
            }

            return null;
        }

        public async Task<NewArticleDto> ParseOnlinerArticle(string url)
        {
            try
            {
                var web = new HtmlWeb();
                var htmlDoc = web.Load(url);

                var titleNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='news-header__title']/h1");
                var title = titleNode.InnerHtml.Trim();

                var descriptionNode = htmlDoc.DocumentNode.SelectSingleNode("//meta[@property='og:description']");
                var description = descriptionNode.Attributes["content"].Value.Trim();

                var dateNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='article:published_time']");
                string date;
                if (dateNode == null)
                {
                    dateNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='news-header__time']");
                    date = dateNode.InnerHtml.Trim();
                }
                else
                {
                    date = dateNode.Attributes["content"].Value.Trim();
                }

                var imageNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='news-header__image']");
                var image = imageNode.Attributes["style"].Value.Trim();

                var bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='news-text']");
                var rightBannerNode = bodyNode.SelectNodes("//div[@class='news-text']/div[contains(@class, 'news-incut')]");
                if (rightBannerNode != null)
                {
                    bodyNode.RemoveChildren(rightBannerNode);
                }
                var scriptNode = bodyNode.SelectSingleNode("//div[@class='news-text']/script");
                if (scriptNode != null)
                {
                    bodyNode.RemoveChild(scriptNode);
                }
                var refNode = bodyNode.SelectSingleNode("//div[@class='news-text']/div[@class='news-reference']");
                if (refNode != null)
                {
                    bodyNode.RemoveChild(refNode);
                }
                var widget = bodyNode.SelectSingleNode("//div[@class='news-text']/div[contains(@class, 'news-widget')]");
                if (widget != null)
                {
                    bodyNode.RemoveChild(widget);
                }
                var telegramLinks = bodyNode.SelectNodes("//div[@class='news-text']/p[@style='text-align: right;']");
                if (telegramLinks != null)
                {
                    bodyNode.RemoveChildren(telegramLinks);
                }
                var videoNode = bodyNode.SelectNodes("//div[@class='news-text']/p[iframe]");
                if (videoNode != null)
                {
                    bodyNode.RemoveChildren(videoNode);
                }
                var imgNode = bodyNode.SelectNodes("//div[@class='news-text']/div[contains(@class, 'news-media')]");
                if (imgNode != null)
                {
                    bodyNode.RemoveChildren(imgNode);
                }
                var image2Node = bodyNode.SelectNodes("//div[@class='news-text']/a[img]");
                if (image2Node != null)
                {
                    bodyNode.RemoveChildren(image2Node);
                }
                var hrefNode = bodyNode.SelectNodes("//div[@class='news-text']/p[a]");
                if (hrefNode != null)
                {
                    bodyNode.RemoveChildren(hrefNode);
                }
                var href2Node = bodyNode.SelectSingleNode("//div[@class='news-text']/a");
                if (href2Node != null)
                {
                    href2Node.SetAttributeValue("href", "");
                }
                var imgInBodyNode = bodyNode.SelectSingleNode("//div[@class='news-media__preview']/img");
                if (imgInBodyNode != null)
                {
                    imgInBodyNode.SetAttributeValue("src", "");
                }
                var titleInText = bodyNode.SelectSingleNode("//div[@class='news-text']/div[contains(@class, 'news-header')]");
                if (titleInText != null)
                {
                    bodyNode.RemoveChild(titleInText);
                }
                var iframe = bodyNode.SelectNodes("//div[@class='news-text']/iframe");
                if (iframe != null)
                {
                    bodyNode.RemoveChildren(iframe);
                }
                var div = bodyNode.SelectNodes("//div[@class='news-text']/div");
                if (div != null)
                {
                    bodyNode.RemoveChildren(div);
                }
                var text = bodyNode.InnerHtml.Trim();

                var model = new NewArticleDto()
                {
                    Title = title,
                    Description = description,
                    Body = text,
                    Image = image
                };

                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<NewArticleDto> ParseGohaArticle(string url)
        {
            try
            {
                var web = new HtmlWeb();
                var htmlDoc = web.Load(url);

                var titleNode = htmlDoc.DocumentNode.SelectSingleNode("//head/title");
                var title = titleNode.InnerHtml.Trim();

                var descriptionNode = htmlDoc.DocumentNode.SelectSingleNode("//meta[@name='description']");
                var description = descriptionNode.InnerHtml.Trim();

                var dateNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='entry-article__article-date']");
                string? date = null;
                if (dateNode == null)
                {
                    dateNode = htmlDoc.DocumentNode.SelectSingleNode("//meta[@property='article:published_time']");
                    date = dateNode.Attributes["content"].Value.Trim();
                }
                else
                {
                    date = dateNode.InnerHtml.Trim();
                }

                var imageNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='wrapper']/div[@class='image-wrapper']/img");
                var image = "";
                if (imageNode != null)
                {
                    var imageSource = imageNode.Attributes["src"].Value.Trim();
                    image = $"background-image: url('{imageSource}');";
                }

                var bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'editor-body entry-article__article-body')]");
                var videoNode = bodyNode.SelectNodes("//div[@class='editor-body-youtube']");
                if (videoNode != null)
                {
                    bodyNode.RemoveChildren(videoNode);
                }
                var text = bodyNode.InnerHtml.Trim();

                var model = new NewArticleDto()
                {
                    Title = title,
                    Description = description,
                    Body = text,
                    Image = image
                };

                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<NewArticleDto> ParseShazooArticle(string url)
        {
            try
            {
                var web = new HtmlWeb();
                var htmlDoc = web.Load(url);

                var titleNode = htmlDoc.DocumentNode.SelectSingleNode("//h1[contains(@class, 'sm:max-w-4xl')]");
                if (titleNode == null)
                {
                    titleNode = htmlDoc.DocumentNode.SelectSingleNode("//article[contains(@class, 'Entry__feature')]/section/h1");
                }
                var title = titleNode.InnerHtml.Trim();

                var descriptionNode = htmlDoc.DocumentNode.SelectSingleNode("//section[contains(@class, 'Entry__content')]/p");
                var description = descriptionNode.FirstChild.InnerText.Trim();

                var dateNode = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'flex')]/time");
                if (dateNode == null)
                {
                    dateNode = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'flex text-center')]/time");
                }
                var date = dateNode.Attributes["datetime"].Value.Trim();

                var imageNode = htmlDoc.DocumentNode.SelectSingleNode("//img[@class='w-full rounded-md']");
                var imageNodeAlt = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'flex-shrink-0')]");
                string image;
                string imageSource;
                if (imageNode != null)
                {
                    imageSource = imageNode.Attributes["src"].Value.Trim();
                    image = $"background-image: url('{imageSource}');";
                }
                else
                {
                    image = imageNodeAlt.Attributes["style"].Value.Trim();
                }

                var bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//section[contains(@class, 'Entry__content')]");
                var spanNode = bodyNode.SelectNodes("//section[contains(@class, 'Entry__content')]/span[contains(@class, 'Entry__embed')]");
                if (spanNode != null)
                {
                    bodyNode.RemoveChildren(spanNode);
                }
                var figureNode = bodyNode.SelectNodes("//section[contains(@class, 'Entry__content')]/figure");
                if (figureNode != null)
                {
                    bodyNode.RemoveChildren(figureNode);
                }
                var imgNode = bodyNode.SelectNodes("//section[contains(@class, 'Entry__content')]/div[contains(@class, 'Gallery')]");
                if (imgNode != null)
                {
                    bodyNode.RemoveChildren(imgNode);
                }
                var text = bodyNode.InnerHtml.Trim();

                var model = new NewArticleDto()
                {
                    Title = title,
                    Description = description,
                    Body = text,
                    Image = image
                };

                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }
    }
}

