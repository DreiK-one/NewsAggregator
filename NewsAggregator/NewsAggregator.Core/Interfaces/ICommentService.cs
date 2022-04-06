using NewsAggregator.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Core.Interfaces
{
    public interface ICommentService
    {
        Task<int?> CreateAsync(CreateOrEditCommentDto commentDto);
    }
}
