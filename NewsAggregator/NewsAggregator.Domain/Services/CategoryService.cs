﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces.Data;
using NewsAggregator.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Domain.Services
{
    public class CategoryService
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

                throw;
            }
        }

    }
}
