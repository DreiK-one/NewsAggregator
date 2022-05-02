using NewsAggregator.Core.DTOs;


namespace NewsAggregator.Core.Interfaces.InterfacesCQS
{
    public interface ICommentServiceCQS
    {
        Task<CreateOrEditCommentDto> GetByIdAsync(Guid id);
        Task<bool> CreateAsync(CreateOrEditCommentDto dto);
        Task<bool> EditAsync(CreateOrEditCommentDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
