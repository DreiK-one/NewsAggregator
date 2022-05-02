using NewsAggregator.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Core.Interfaces.InterfacesCQS
{
    public interface ICommentServiceCQS
    {
        Task<bool> CreateAsync(CreateOrEditCommentDto dto);
    }
}
