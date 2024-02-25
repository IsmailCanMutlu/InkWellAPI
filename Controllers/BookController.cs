using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiChallenge.Models;
using RestApiChallenge.Services;
using System.Threading.Tasks;

namespace RestApiChallenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // JWT token doğrulaması için
    public class BookController : ControllerBase
    {
        private readonly BookService _bookService;

        public BookController(BookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _bookService.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound(new { message = $"Book with ID {id} not found." });
            }
            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] Book book)
        {
            var addedBook = await _bookService.AddBookAsync(book);
            if (addedBook == null)
            {
                return Problem("An unexpected error occurred while adding the book.");
            }
            return CreatedAtAction(nameof(GetById), new { id = addedBook.Id }, addedBook);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] Book book)
        {
            var updatedBook = await _bookService.UpdateBookAsync(id, book);
            if (updatedBook == null)
            {
                return NotFound(new { message = $"Book with ID {id} not found." });
            }
            return Ok(updatedBook);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var deleted = await _bookService.DeleteBookAsync(id);
            if (!deleted)
            {
                return NotFound(new { message = $"Book with ID {id} not found." });
            }
            return NoContent();
        }
    }
}
