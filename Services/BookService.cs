using Microsoft.EntityFrameworkCore;
using RestApiChallenge.Data;
using RestApiChallenge.Models;
using RestApiChallenge.Services.Interfaces;

namespace RestApiChallenge.Services;

public class BookService : IBookService
{
    private readonly ApplicationDbContext _context;

    public BookService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Book?> AddBookAsync(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }
    public async Task<List<Book>> GetAllBooksAsync()
    {
        return await _context.Books.ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _context.Books.FindAsync(id);
    }
        
    public async Task<List<Book>> GetBooksByUserIdAsync(int userId)
    {
        return await _context.Books
            .Where(b => b.UserBooks.Any(ub => ub.UserId == userId))
            .ToListAsync();
    }
        
    public async Task<Book?> AddBookToUserAsync(Book book, int userId)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        var userBook = new UserBook
        {
            UserId = userId,
            BookId = book.Id
        };

        _context.UserBooks.Add(userBook);
        await _context.SaveChangesAsync();

        return book;
    }

    public async Task<Book?> UpdateBookAsync(int id, Book bookUpdate)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null) return null;
            
        book.BookName = bookUpdate.BookName;
        book.AuthorName = bookUpdate.AuthorName;
            
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<bool> DeleteBookAsync(int id)
    {
        var bookToDelete = await _context.Books.FindAsync(id);
        if (bookToDelete == null) return false;

        _context.Books.Remove(bookToDelete);
        await _context.SaveChangesAsync();

        return true;
    }
    
}