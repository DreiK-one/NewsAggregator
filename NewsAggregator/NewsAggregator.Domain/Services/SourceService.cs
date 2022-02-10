using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class SourceService : ISourceService
    {
        private readonly IMapper _mapper;

        private readonly IUnitOfWork _unitOfWork;

        public SourceService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<SourceDto>> GetAllSourcesAsync()
        {
            return await _unitOfWork.Sources.Get()
                .Select(source => _mapper.Map<SourceDto>(source))
                .ToListAsync();
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
            catch (Exception)
            {
                //add log
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
            catch (Exception)
            {
                //add log
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
            catch (Exception)
            {
                //add log
                throw;
            }
        }
    }
}
