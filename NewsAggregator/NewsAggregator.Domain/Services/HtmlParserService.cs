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
                        var articleRia = await ParseRiaArticle(url);
                        await _unitOfWork.Articles.Add(_mapper.Map<Article>(articleRia));
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

                var titleInText = bodyNode.SelectSingleNode("//div[@class='news-text']/div[contains(@class, 'news-header__title')]");
                if (titleInText != null)
                {
                    bodyNode.RemoveChild(titleInText);
                }

                var titleNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='news-header__title']/h1");
                var dateNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='article:published_time']");
                var descriptionNode = htmlDoc.DocumentNode.SelectSingleNode("//meta[@property='og:description']");
                var imageNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='news-header__image']");

                var text = bodyNode.InnerHtml.Trim();
                var title = titleNode.InnerHtml.Trim();
                var date = dateNode.Attributes["content"].Value.Trim();
                var description = descriptionNode.Attributes["content"].Value.Trim();
                var image = imageNode.Attributes["style"].Value.Trim();

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

        public async Task<NewArticleDto> ParseRiaArticle(string url)
        {
            try
            {
                var web = new HtmlWeb();
                var htmlDoc = web.Load(url);

                var bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'article__body')]");

                var scriptNode = bodyNode.SelectNodes("//div[contains(@class, 'article__body')]/script");
                if (scriptNode != null)
                {
                    bodyNode.RemoveChildren(scriptNode);
                }

                var bannerNode = bodyNode.SelectSingleNode("//div[contains(@class, 'article__body')]/div[contains(@data-type, 'banner')]");
                if (bannerNode != null)
                {
                    bodyNode.RemoveChild(bannerNode);
                }

                var fotoNode = bodyNode.SelectSingleNode("//div[contains(@class, 'article__body')]/div[contains(@data-article, 'main-foto')]");
                if (fotoNode != null)
                {
                    bodyNode.RemoveChild(fotoNode);
                }

                var imagesNodes = bodyNode.SelectNodes("//div[contains(@class, 'article__article-image')]");
                if (imagesNodes != null)
                {
                    bodyNode.RemoveChildren(imagesNodes);
                }

                var editorNode = bodyNode.SelectSingleNode("//div[contains(@class, 'article__body')]/div[contains(@class, 'article__editor')]");
                if (editorNode != null)
                {
                    bodyNode.RemoveChild(editorNode);
                }

                var titleNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='article__title']");
                var dateNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='article__info-date']/a");
                var descriptionNode = htmlDoc.DocumentNode.SelectSingleNode("//h1[@class='article__second-title']");
                var imageNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='photoview__open']");

                var text = bodyNode.InnerHtml.Trim();
                var title = titleNode.InnerHtml.Trim();
                var date = dateNode.InnerHtml.Trim();
                var description = descriptionNode.InnerHtml.Trim();

                var imageSource = imageNode.Attributes["data-photoview-src"].Value.Trim();
                var image = $"background-image: url('{imageSource}');";

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

                var bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//section[contains(@class, 'Entry__content')]");

                var spanNode = bodyNode.SelectNodes("//section[contains(@class, 'Entry__content')]/span[contains(@class, 'Entry__embed')]");
                if (spanNode != null)
                {
                    bodyNode.RemoveChildren(spanNode);
                }

                var titleNode = htmlDoc.DocumentNode.SelectSingleNode("//h1[contains(@class, 'sm:max-w-4xl')]");
                var dateNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='flex flex-col text-xs']/time");
                var descriptionNode = htmlDoc.DocumentNode.SelectSingleNode("//section[contains(@class, 'Entry__content')]/p");
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

                var text = bodyNode.InnerHtml.Trim();
                var title = titleNode.InnerHtml.Trim();
                var date = dateNode.Attributes["datetime"].Value.Trim();
                var description = descriptionNode.FirstChild.InnerText.Trim();
                
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
