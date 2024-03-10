using RestApiChallenge.Models;

namespace RestApiChallenge.Services.Interfaces;

public interface IBookService
{
    Task<List<Book>> GetAllBooksAsync();
    Task<Book?> AddBookAsync(Book book);
    Task<Book?> AddBookToUserAsync(Book book, int userId);
    Task<Book?> UpdateBookAsync(int id, Book bookUpdate);
    Task<bool> DeleteBookAsync(int id);
    Task<Book?> GetByIdAsync(int id);
    Task<List<Book>> GetBooksByUserIdAsync(int userId);
}