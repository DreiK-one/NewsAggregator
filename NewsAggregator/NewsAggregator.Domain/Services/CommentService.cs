using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class CommentService : ICommentService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CommentService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CommentService(IMapper mapper, 
            ILogger<CommentService> logger, 
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateOrEditCommentDto> GetCommentAsync(Guid Id)
        {
            try
            {
                var comment = await _unitOfWork.Comments.GetById(Id);
                return _mapper.Map<CreateOrEditCommentDto>(comment);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<int?> CreateAsync(CreateOrEditCommentDto commentDto)
        {
            try
            {
                if (commentDto != null)
                {
                    await _unitOfWork.Comments.Add(_mapper.Map<Comment>(commentDto));
                    return await _unitOfWork.Save();
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<int?> UpdateAsync(CreateOrEditCommentDto commentDto)
        {
            try
            {
                if (commentDto != null)
                {
                    await _unitOfWork.Comments.Update(_mapper.Map<Comment>(commentDto));
                    return await _unitOfWork.Save();
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<int?> DeleteAsync(Guid modelId)
        {
            try
            {
                if (await _unitOfWork.Comments.GetById(modelId) != null)
                {
                    await _unitOfWork.Comments.Remove(modelId);
                    return await _unitOfWork.Save();
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
