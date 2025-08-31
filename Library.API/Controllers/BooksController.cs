using Library.Application.Abstractions.Services;
using Library.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Book>> GetById(int id, CancellationToken ct)
    {
        var book = await _bookService.GetAsync(id, ct);
        if (book == null)
            return NotFound();

        return Ok(book);
    }

    [HttpGet]
    public async Task<ActionResult<(IReadOnlyList<Book> Items, int TotalCount)>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null, [FromQuery] int? categoryId = null, CancellationToken ct = default)
    {
        var result = await _bookService.SearchAsync(page, pageSize, search, categoryId, ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Book>> Create([FromBody] Book book, CancellationToken ct)
    {
        var createdBook = await _bookService.CreateAsync(book, ct);
        return CreatedAtAction(nameof(GetById), new { id = createdBook.Id }, createdBook);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Book updatedBook, CancellationToken ct)
    {
        await _bookService.UpdateAsync(id, updatedBook, ct);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _bookService.DeleteAsync(id, ct);
        return NoContent();
    }
}

