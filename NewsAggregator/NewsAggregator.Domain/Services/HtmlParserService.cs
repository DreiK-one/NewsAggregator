using HtmlAgilityPack;
using NewsAggregator.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Domain.Services
{
    public class HtmlParserService : IHtmlParserService
    {
        private readonly ISourceService _sourceService;

        public HtmlParserService(ISourceService sourceService)
        {
            _sourceService = sourceService;
        }

        public async Task<string> GetArticleContentFromUrlAsync(string url)
        {
            var sourceId = await _sourceService.GetSourceByUrl(url);

            switch (sourceId.ToString("D").ToUpperInvariant())
            {
                case "F2FB2A60-C1DE-4DA5-B047-0871D2D677B5":
                    var articleBody = await ParseOnlinerArticle(url);
                    break;

                default:
                    break;
            }
            return null;
        }

        public async Task<string> ParseOnlinerArticle(string url)
        {
            var web = new HtmlWeb();
            var htmlDoc = web.Load(url);
            var node = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='news-text']");

            var scriptNode = node.SelectSingleNode("//div[@class='news-text']/script");
            if (scriptNode != null)
            {
                node.RemoveChild(scriptNode);
            }
                
            var refNode = node.SelectSingleNode("//div[@class='news-text']/div[@class='news-reference']");
            if (refNode != null)
            {
                node.RemoveChild(refNode);
            }

            var widget = node.SelectSingleNode("//div[@class='news-text']/div[contains(@class, 'news-widget')]");
            if (widget != null)
            {
                node.RemoveChild(widget);
            }

            var telegramLinks = node.SelectNodes("//div[@class='news-text']/p[@style='text-align: right;']");
            if (telegramLinks != null)
            {
                node.RemoveChildren(telegramLinks);
            }

            var articleText = node.InnerHtml.Trim();

            return null;
        }

        public async Task<string> ParseLentaArticle(string url)
        {
            //todo algorythm of web scrapping
            return null;
        }
    }
}
