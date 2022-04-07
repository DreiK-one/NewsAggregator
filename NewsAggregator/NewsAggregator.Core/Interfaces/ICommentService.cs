﻿using NewsAggregator.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Core.Interfaces
{
    public interface ICommentService
    {
        Task<CreateOrEditCommentDto> GetCommentAsync(Guid Id);
        Task<int?> CreateAsync(CreateOrEditCommentDto commentDto);
        Task<int?> UpdateAsync(CreateOrEditCommentDto commentDto);
        Task<int?> DeleteAsync(Guid modelId);
    }
}
