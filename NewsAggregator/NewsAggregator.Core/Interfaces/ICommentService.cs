using NewsAggregator.Core.DTOs;


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
