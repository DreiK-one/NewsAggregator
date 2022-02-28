using AutoMapper;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces;
using NewsAggregator.Core.Interfaces.Data;
using NewsAggregator.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Domain.Services
{
    public class HtmlParserService : IHtmlParserService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<HtmlParserService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISourceService _sourceService;
        private readonly ICategoryService _categoryService;

        public HtmlParserService(IMapper mapper, 
            ILogger<HtmlParserService> logger,
            IUnitOfWork unitOfWork,
            ISourceService sourceService,
            ICategoryService categoryService)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _sourceService = sourceService;
            _categoryService = categoryService;
        }

        public async Task<int?> GetArticleContentFromUrlAsync(string url)
        {
            try
            {
                var sourceId = await _sourceService.GetSourceByUrl(url);

                switch (sourceId.ToString("D").ToUpperInvariant())
                {
                    case "F2FB2A60-C1DE-4DA5-B047-0871D2D677B5":
                        var articleOnliner = await ParseOnlinerArticle(url);
                        await _unitOfWork.Articles.Add(_mapper.Map<Article>(articleOnliner));
                        return await _unitOfWork.Save();

                    case "F2FB2A60-C1DE-4DA5-B047-0871D2D677B4":
                        var articleGoha = await ParseGohaArticle(url);
                        await _unitOfWork.Articles.Add(_mapper.Map<Article>(articleGoha));
                        return await _unitOfWork.Save();

                    case "C13088A4-9467-4FCE-9EF7-3903425F1F81":
                        var article4pda = await ParseShazooArticle(url);
                        await _unitOfWork.Articles.Add(_mapper.Map<Article>(article4pda));
                        return await _unitOfWork.Save();

                    default:
                        break;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }  
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
                var imgNode = bodyNode.SelectNodes("//div[@class='news-text']/div[contains(@class, 'news-media')]");
                if (imgNode != null)
                {
                    bodyNode.RemoveChildren(imgNode);
                }
                var hrefNode = bodyNode.SelectSingleNode("//div[@class='news-text']/p/a");
                if (hrefNode != null)
                {
                    hrefNode.SetAttributeValue("href", "");
                }
                var imgInBodyNode = bodyNode.SelectSingleNode("//div[@class='news-media__preview']/img");
                if (imgInBodyNode != null)
                {
                    imgInBodyNode.SetAttributeValue("src", "");
                }
                var titleInText = bodyNode.SelectSingleNode("//div[@class='news-text']/div[contains(@class, 'news-header__title')]");
                if (titleInText != null)
                {
                    bodyNode.RemoveChild(titleInText);
                }
                var text = bodyNode.InnerHtml.Trim();

                var model = new NewArticleDto()
                {
                    Id = Guid.NewGuid(),
                    Title = title,
                    Description = description,
                    Body = text,
                    CreationDate = DateTime.Now,   //todo date from source article
                    //Coefficient = ??,             
                    SourceUrl = url,
                    CategoryId = await _categoryService.GetCategoryByUrl(url),
                    SourceId = await _sourceService.GetSourceByUrl(url),
                    Image = image
                };

                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            } 
        }

        public async Task<NewArticleDto> ParseGohaArticle(string url)
        {
            try
            {
                var web = new HtmlWeb();
                var htmlDoc = web.Load(url);

                var titleNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='head']/h1");
                var title = titleNode.InnerHtml.Trim();

                var descriptionNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='head']/h2");
                if(descriptionNode == null)
                {
                    descriptionNode = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'body goharumd')]/p");
                }
                var description = descriptionNode.InnerHtml.Trim();

                var dateNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='data']/div[@class='date']");
                var date = dateNode.InnerHtml.Trim();

                var imageNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='wrapper']/div[@class='image-wrapper']/img");
                var imageSource = imageNode.Attributes["src"].Value.Trim();
                var image = $"background-image: url('{imageSource}');";

                var bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'body goharumd')]");
                var quoteNode = bodyNode.SelectNodes("//div[contains(@class, 'body goharumd')]/div[contains(@class, 'quote')]");
                if (quoteNode != null)
                {
                    bodyNode.RemoveChildren(quoteNode);
                }
                var videoNode = bodyNode.SelectNodes("//div[contains(@class, 'body goharumd')]/div[@class='youtube-container']");
                if (videoNode != null)
                {
                    bodyNode.RemoveChildren(videoNode);
                }
                var text = bodyNode.InnerHtml.Trim();
                
                var model = new NewArticleDto()
                {
                    Id = Guid.NewGuid(),
                    Title = title,
                    Description = description,
                    Body = text,
                    CreationDate = DateTime.Now,   //todo date from source article
                    //Coefficient = ??,            
                    SourceUrl = url,
                    CategoryId = await _categoryService.GetCategoryByUrl(url),
                    SourceId = await _sourceService.GetSourceByUrl(url),
                    Image = image
                };

                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
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

                var dateNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='flex flex-col text-xs']/time");
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
                var text = bodyNode.InnerHtml.Trim();
                
                var model = new NewArticleDto()
                {
                    Id = Guid.NewGuid(),
                    Title = title,
                    Description = description,
                    Body = text,
                    CreationDate = DateTime.Now,   //todo date from source article
                    //Coefficient = ??,            
                    SourceUrl = url,
                    CategoryId = await _categoryService.GetCategoryByUrl(url),
                    SourceId = await _sourceService.GetSourceByUrl(url),
                    Image = image
                };

                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
