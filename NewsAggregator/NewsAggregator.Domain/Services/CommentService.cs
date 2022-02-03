using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Domain.Services
{
    public class CommentService
    {
        private readonly IMapper _mapper;

        private readonly IUnitOfWork _unitOfWork;

        public CommentService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }


        //THINK ABOUT IT

        //public async Task<IEnumerable<CommentDto>> GetAllCommentsByArticle(Guid id)
        //{
        //    var comments = await _unitOfWork.Comments.Get().Select(article => article.ArticleId == id).ToListAsync();

        //    var model = new List<CommentDto>();

        //    return model;
        //}
    }
}
