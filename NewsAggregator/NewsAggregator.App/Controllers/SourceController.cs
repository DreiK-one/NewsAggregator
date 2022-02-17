using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewsAggregator.App.Models;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Interfaces;

namespace NewsAggregator.App.Controllers
{
    public class SourceController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ISourceService _sourceService;
        private readonly ILogger<SourceController> _logger;

        public SourceController(IMapper mapper, ISourceService sourceService, ILogger<SourceController> logger)
        {
            _mapper = mapper;
            _sourceService = sourceService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var model = (await _sourceService.GetAllSourcesAsync())
                .Select(source => _mapper.Map<SourceModel>(source))
                .ToList();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            try
            {
                var model = new SourceModel();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSource(SourceModel model)
        {
            try
            {
                if (model != null)
                {
                    await _sourceService.CreateAsync(_mapper.Map<SourceDto>(model));
                }
                return RedirectToAction("Index", "Source");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var model = new DeleteSourceViewModel() { Id = id };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}");
                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSource(DeleteSourceViewModel model)
        {
            try
            {
                var delete = await _sourceService.DeleteAsync(model.Id);

                if (delete == null)
                {
                    _logger.LogWarning($"{DateTime.Now}: Model is null in DeleteSource method");
                    return BadRequest();
                }

                return RedirectToAction("Index", "Source");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, StackTrace: {ex.StackTrace}");
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            try
            {
                var model = new SourceModel() { Id = id };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, StackTrace: {ex.StackTrace}");
                return BadRequest();
            } 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSource(SourceModel model)
        {
            try
            {
                if (model != null)
                {
                    await _sourceService.UpdateAsync(_mapper.Map<SourceDto>(model));
                }
                return RedirectToAction("Index", "Source");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, StackTrace: {ex.StackTrace}");
                return BadRequest();
            }
        }
    }
}
