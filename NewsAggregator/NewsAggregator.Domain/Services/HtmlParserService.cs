using AutoMapper;
using HtmlAgilityPack;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISourceService _sourceService;
        private readonly ICategoryService _categoryService;

        public HtmlParserService(ISourceService sourceService, 
            IUnitOfWork unitOfWork, 
            ICategoryService categoryService, 
            IMapper mapper)
        {
            _sourceService = sourceService;
            _unitOfWork = unitOfWork;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public async Task<int?> GetArticleContentFromUrlAsync(string url)
        {
            var sourceId = await _sourceService.GetSourceByUrl(url);

            switch (sourceId.ToString("D").ToUpperInvariant())
            {
                case "F2FB2A60-C1DE-4DA5-B047-0871D2D677B5":
                    var article = await ParseOnlinerArticle(url);
                    await _unitOfWork.Articles.Add(_mapper.Map<Article>(article));
                    return await _unitOfWork.Save();
                //case "C13088A4-9467-4FCE-9EF7-3903425F1F81":
                //    articleBody = await ParseShazooArticle(url);
                //    break;
                //case "C13088A4-9467-4FCE-9EF7-3903425F1F82":
                //    articleBody = await Parse4pdaArticle(url);
                //    break;

                default:
                    break;
            }
            return null;
        }

        public async Task<NewArticleDto> ParseOnlinerArticle(string url)
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

            var imgNode = bodyNode.SelectNodes("//div[@class='news-text']/div[contains(@class, 'news-media news-media_condensed')]");
            if (imgNode != null)
            {
                bodyNode.RemoveChildren(imgNode);
            }

            var titleNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='news-header__title']/h1");

            var dateNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='news-header__time']");

            var descriptionNode = htmlDoc.DocumentNode.SelectSingleNode("//meta[@property='og:description']");

            var sourceUrlNode = htmlDoc.DocumentNode.SelectSingleNode("//meta[@property='og:url']");


            var articleText = bodyNode.InnerHtml.Trim();
            var articleTitle = titleNode.InnerHtml.Trim();
            var articleDate = dateNode.InnerHtml.Trim();
            var articleDesc = descriptionNode.Attributes["content"].Value.Trim();
            var articleSourceUrl = sourceUrlNode.Attributes["content"].Value.Trim();

            var model = new NewArticleDto()
            {
                Id = Guid.NewGuid(),
                Title = articleTitle,
                Description = articleDesc,
                Body = articleText,
                //CreationDate = articleDate,   todo!!!!!
                //Coefficient = ??,             todo!!!!!
                SourceUrl = articleSourceUrl,
                CategoryId = new Guid("8453B0BD-9860-45FA-A2AE-A77953D842A4"), //change to CategoryId = await _categoryService.GetCategoryByUrl(url)
                SourceId = await _sourceService.GetSourceByUrl(url),
            };

            return model;
        }


        //public async Task<string> ParseShazooArticle(string url)
        //{
        //    //todo algorythm of web scrapping
        //    return null;
        //}

        //public async Task<string> Parse4pdaArticle(string url)
        //{
        //    //todo algorythm of web scrapping
        //    return null;
        //}
    }
}
