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
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            return await _unitOfWork.Categories.Get()
                .Select(category => _mapper.Map<CategoryDto>(category))
                .ToListAsync();
        }

        public async Task<int?> CreateAsync(CategoryDto categoryDto)
        {
            try
            {
                if (categoryDto != null)
                {
                    var existCategory = (await _unitOfWork.Categories.FindBy(c => c.Name.ToLower() == categoryDto.Name.ToLower()));

                    if (!existCategory.Any())
                    {
                        await _unitOfWork.Categories.Add(_mapper.Map<Category>(categoryDto));
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

        public async Task<int?> UpdateAsync(CategoryDto categoryDto)
        {
            try
            {
                if (categoryDto != null)
                {
                    await _unitOfWork.Categories.Update(_mapper.Map<Category>(categoryDto));
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
                if (await _unitOfWork.Categories.GetById(id) != null)
                {
                    await _unitOfWork.Categories.Remove(id);
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
