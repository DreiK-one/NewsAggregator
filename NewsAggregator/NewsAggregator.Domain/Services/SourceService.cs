using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces;
using NewsAggregator.Core.Interfaces.Data;
using NewsAggregator.Data.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Domain.Services
{
    public class SourceService : ISourceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<SourceService> _logger;
        
        public SourceService(IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<SourceService> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<SourceDto>> GetAllSourcesAsync()
        {
            try
            {
                return await _unitOfWork.Sources.Get()
                .Select(source => _mapper.Map<SourceDto>(source))
                .ToListAsync();
            }
            catch (Exception ex )
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<int?> CreateAsync(SourceDto sourceDto)
        {
            try
            {
                if (sourceDto != null)
                {
                    var existSource = (await _unitOfWork.Sources.FindBy(s => s.Name.ToLower() == sourceDto.Name.ToLower()));

                    if (!existSource.Any())
                    {
                        await _unitOfWork.Sources.Add(_mapper.Map<Source>(sourceDto));
                        return await _unitOfWork.Save();
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
               throw;
            }
        }

        public async Task<int?> UpdateAsync(SourceDto sourceDto)
        {
            try
            {
                if (sourceDto != null)
                {
                    await _unitOfWork.Sources.Update(_mapper.Map<Source>(sourceDto));
                    return await _unitOfWork.Save();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<int?> DeleteAsync(Guid id)
        {
            try
            {
                if (await _unitOfWork.Sources.GetById(id) != null)
                {
                    await _unitOfWork.Sources.Remove(id);
                    return await _unitOfWork.Save();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<RssUrlsFromSourceDto>> GetRssUrlsAsync()
        {
            try
            {
                var result = await _unitOfWork.Sources.Get()
                    .Select(source => _mapper.Map<RssUrlsFromSourceDto>(source))
                    .ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<Guid> GetSourceByUrl(string url)
        {
            try
            {
                var domain = string.Join(".",
                new Uri(url).Host
                .Split('.')
                .TakeLast(2)
                .ToList());
                return (await _unitOfWork.Sources.Get()
                     .FirstOrDefaultAsync(source => source.BaseUrl.Equals(domain)))?.Id ?? Guid.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            } 
        }

        public async Task<SourceDto> GetSourceAsync(Guid Id)
        {
            try
            {
                var source = await _unitOfWork.Sources.GetById(Id);
                return _mapper.Map<SourceDto>(source);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
