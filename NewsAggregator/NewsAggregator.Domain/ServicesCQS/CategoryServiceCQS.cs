using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Helpers;
using NewsAggregator.Core.Interfaces.InterfacesCQS;
using NewsAggregetor.CQS.Models.Commands.CategoryCommands;
using NewsAggregetor.CQS.Models.Queries.CategoryQueries;


namespace NewsAggregator.Domain.ServicesCQS
{
    public class CategoryServiceCQS : ICategoryServiceCQS
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryServiceCQS> _logger;
        private readonly IMediator _mediator;

        public CategoryServiceCQS(IMapper mapper, IMediator mediator, 
            ILogger<CategoryServiceCQS> logger)
        {
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<IEnumerable<CategoryWithArticlesDto>> GetAllCategories()
        {
            try
            {
                return await _mediator.Send(new GetAllCategoriesQuery(),
                new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<CategoryDto> GetCategoryAsync(Guid id)
        {
            try
            {
                return await _mediator.Send(new GetCategoryAsyncQuery(id), 
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            } 
        }

        public async Task<Guid> GetCategoryByUrlAsync(string url)
        {
            try
            {
                return await _mediator.Send(new GetCategoryByUrlAsyncQuery(url), 
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<CategoryWithArticlesDto> GetCategoryByIdWithArticlesAsync(Guid id)
        {
            try
            {
                return await _mediator.Send(new GetCategoryByIdWithArticlesAsyncQuery(id),
                new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<CategoryWithArticlesDto> GetCategoryByNameWithArticlesAsync(string name)
        {
            try
            {
                return await _mediator.Send(new GetCategoryByNameWithArticlesAsyncQuery(name),
                new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }          
        }

        public async Task<CategoryWithArticlesDto> GetCategoryByNameWithArticlesForAdminAsync(string name)
        {
            try
            {
                return await _mediator.Send(new GetCategoryByNameWithArticlesForAdminAsyncQuery(name), 
                    new CancellationToken());
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<int?> CreateAsync(CategoryDto categoryDto)
        {
            try
            {
                if (categoryDto != null)
                {
                    var existCategory = GetAllCategories().Result
                        .Select(c => c.Name.ToLower() == categoryDto.Name.ToLower());

                    if (!existCategory.Any())
                    {
                        var command = _mapper.Map<CreateCategoryCommand>(categoryDto);

                        return await _mediator.Send(command, 
                            new CancellationToken());
                    }
                    else
                    {
                        throw new NullReferenceException();
                    }
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<int?> UpdateAsync(CategoryDto categoryDto)
        {
            try
            {
                if (categoryDto != null)
                {
                    var command = _mapper.Map<UpdateCategoryCommand>(categoryDto);

                    return await _mediator.Send(command, 
                        new CancellationToken());
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }

        public async Task<int?> DeleteAsync(Guid id)
        {
            try
            {
                if (GetCategoryAsync(id) != null)
                {
                    return await _mediator.Send(new DeleteCategoryCommand(id), 
                        new CancellationToken());
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                throw;
            }
        }
    }
}
