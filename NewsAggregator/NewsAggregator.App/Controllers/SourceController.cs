﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsAggregator.App.Models;
using NewsAggregator.Core.DTOs;
using NewsAggregator.Core.Helpers;
using NewsAggregator.Core.Interfaces;


namespace NewsAggregator.App.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SourceController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<SourceController> _logger;
        private readonly ISourceService _sourceService;

        public SourceController(IMapper mapper, 
            ILogger<SourceController> logger, 
            ISourceService sourceService)
        {
            _mapper = mapper;
            _logger = logger;
            _sourceService = sourceService;
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
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                return StatusCode(500, new { ex.Message });
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
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSource(SourceModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model != null)
                    {
                        await _sourceService.CreateAsync(_mapper.Map<SourceDto>(model));
                    }
                    return RedirectToAction("Index", "Source");
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var source = await _sourceService.GetSourceAsync(id);
                var model = _mapper.Map<DeleteSourceViewModel>(source);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSource(DeleteSourceViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var delete = await _sourceService.DeleteAsync(model.Id);
                    if (delete == null)
                    {
                        _logger.LogWarning($"{DateTime.Now}: Model is null in DeleteSource method");
                        return BadRequest();
                    }
                    return RedirectToAction("Index", "Source");
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var source = await _sourceService.GetSourceAsync(id);
                var model = _mapper.Map<SourceModel>(source);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                return StatusCode(500, new { ex.Message });
            } 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSource(SourceModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model != null)
                    {
                        await _sourceService.UpdateAsync(_mapper.Map<SourceDto>(model));
                    }
                    return RedirectToAction("Index", "Source");
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ExceptionMessageHelper.GetExceptionMessage(ex));
                return BadRequest();
            }
        }
    }
}
