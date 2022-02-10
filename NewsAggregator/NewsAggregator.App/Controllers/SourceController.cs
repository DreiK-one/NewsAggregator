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

        public SourceController(IMapper mapper, ISourceService sourceService)
        {
            _mapper = mapper;
            _sourceService = sourceService;
        }

        public async Task<IActionResult> Index()
        {
            var model = (await _sourceService.GetAllSourcesAsync())
                .Select(source => _mapper.Map<SourceModel>(source))
                .ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new SourceModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSource(SourceModel model)
        {
            return View();
            if (model != null)
            {
                await _sourceService.CreateAsync(_mapper.Map<SourceDto>(model));
            }
            return RedirectToAction("Index", "Source");
        }

        //TODO
        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            var model = new DeleteSourceViewModel() { Id = id };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSource(DeleteSourceViewModel model)
        {
            var delete = await _sourceService.DeleteAsync(model.Id);

            if (delete == null)
                return BadRequest();

            return RedirectToAction("Index", "Source");
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var model = new SourceModel() { Id = id };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSource(SourceModel model)
        {
            if (model != null)
            {
                await _sourceService.UpdateAsync(_mapper.Map<SourceDto>(model));
            }
            return RedirectToAction("Index", "Source");
        }
    }
}
