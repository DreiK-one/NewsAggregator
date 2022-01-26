using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces;
using NewsAggregator.Core.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Domain.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IMapper _mapper;

        private readonly IUnitOfWork _unitOfWork;

        public ArticleService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }


        public async Task<IEnumerable<ArticleDto>> GetAllNewsAsync()
        {
            return await _unitOfWork.Articles.Get()
                .Select(article => _mapper.Map<ArticleDto>(article))
                .ToArrayAsync();
        }

        public async Task<int?> DeleteAsync(Guid modelId)
        {
            try
            {
                if (await _unitOfWork.Articles.GetById(modelId) != null)
                {
                    await _unitOfWork.Articles.Remove(modelId);
                    return await _unitOfWork.Save();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                //add log
                throw;
            }
        }

        public async Task<ArticleDto> GetArticleWithAllNavigationProperties(Guid id)
        {
            var article = await _unitOfWork.Articles
                .GetByIdWithIncludes(id, article => article.Source, article => article.Comments);

            return _mapper.Map<ArticleDto>(article);
        }
    }
}
