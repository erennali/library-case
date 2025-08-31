using Library.Application.Abstractions.Services;
using Library.Domain.Entities;
using Library.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Category>> GetById(int id, CancellationToken ct)
    {
        var category = await _categoryService.GetAsync(id, ct);
        if (category == null)
            return NotFound();

        return Ok(category);
    }

    [HttpGet]
    public async Task<ActionResult<(IReadOnlyList<Category> Items, int TotalCount)>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null, [FromQuery] int? parentCategoryId = null, CancellationToken ct = default)
    {
        var result = await _categoryService.SearchAsync(page, pageSize, search, parentCategoryId, ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Category>> Create([FromBody] Category category, CancellationToken ct)
    {
        var createdCategory = await _categoryService.CreateAsync(category, ct);
        return CreatedAtAction(nameof(GetById), new { id = createdCategory.Id }, createdCategory);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Category updatedCategory, CancellationToken ct)
    {
        await _categoryService.UpdateAsync(id, updatedCategory, ct);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _categoryService.DeleteAsync(id, ct);
        return NoContent();
    }
}


