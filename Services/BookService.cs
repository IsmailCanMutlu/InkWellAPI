using Microsoft.EntityFrameworkCore;
using RestApiChallenge.Data;
using RestApiChallenge.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestApiChallenge.Services
{
    public class BookService
    {
        private readonly ApplicationDbContext _context;

        public BookService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            return await _context.Books
                .Include(b => b.User) // kullanıcı bilgileriyle birlikte getir
                .ToListAsync();
        }

        public async Task<Book?> AddBookAsync(Book book)
        {
            _context.Books.Add(book);
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

        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _context.Books
                .Include(b => b.User) 
                .FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}
