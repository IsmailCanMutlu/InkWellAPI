using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiChallenge.Models;
using RestApiChallenge.Models.Requests;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using RestApiChallenge.Services.Interfaces;

namespace RestApiChallenge.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
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
        
    [HttpGet("v2/user")]
    public async Task<IActionResult> GetBooksByUserIdV2()
    {
        var userClaimId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var userId = int.Parse(userClaimId);
        
        var books = await _bookService.GetBooksByUserIdAsync(userId);
        if (books == null || books.Count == 0)
        {
            return NotFound(new { message = $"No books found for user ID {userId}." });
        }
        return Ok(books);
    }
    
    [Obsolete ("this endpoint used for test you can use v2/user for implementation")]
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetBooksByUserId(int userId)
    {
        // add log 
        var books = await _bookService.GetBooksByUserIdAsync(userId);
        if (books == null || books.Count == 0)
        {
            return NotFound(new { message = $"No books found for user ID {userId}." });
        }
        return Ok(books);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddBook([FromBody] BookRequest bookRequest)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized(new { message = "User ID not found in token." });
        }

        var userId = int.Parse(userIdClaim.Value);

        var book = new Book
        {
            BookName = bookRequest.BookName,
            AuthorName = bookRequest.AuthorName,
            UserId = userId
        };

        var addedBook = await _bookService.AddBookToUserAsync(book, userId);
        if (addedBook == null)
        {
            return BadRequest(new { message = "An error occurred while adding the book." });
        }

        return CreatedAtAction(nameof(GetById), new { id = addedBook.Id }, addedBook);
    }



    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBook(int id, [FromBody] BookRequest bookRequest)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized(new { message = "User ID not found in token." });
        }

        var userId = int.Parse(userIdClaim.Value);

        var bookToUpdate = await _bookService.GetByIdAsync(id);
        if (bookToUpdate == null)
        {
            return NotFound(new { message = $"Book with ID {id} not found." });
        }

        if (bookToUpdate.UserId != userId)
        {
            return Unauthorized(new { message = "You are not authorized to update this book." });
        }

        bookToUpdate.BookName = bookRequest.BookName;
        bookToUpdate.AuthorName = bookRequest.AuthorName;

        var updatedBook = await _bookService.UpdateBookAsync(id, bookToUpdate);
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