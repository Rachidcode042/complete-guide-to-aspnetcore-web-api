using my_books.Data.Models;
using my_books.Data.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Threading;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace my_books.Data.Services
{
    public class BooksService
    {

        private AppDbContext _context;
        public BooksService(AppDbContext context) 
        {
            _context = context;
        }

        public void AddBookWithAuthors(BookVM book)
        {
            var publisherExists = _context.Publishers.Any(p => p.Id == book.PublisherId);
            if (!publisherExists)
            {
                throw new Exception($"Publisher with ID {book.PublisherId} does not exist.");
            }

            // Vérification des auteurs
            foreach (var authorId in book.AuthorIds)
            {
                var authorExists = _context.Authors.Any(a => a.Id == authorId);
                if (!authorExists)
                {
                    throw new Exception($"Author with ID {authorId} does not exist.");
                }
            }

            var _book = new Book()
            {
                Title = book.Title,
                Description = book.Description,
                IsRead = book.IsRead,
                DateRead = book.DateRead,
                Rate = book.IsRead ? book.Rate.Value : null,
                Genre = book.Genre,
                CoverUrl = book.CoverUrl,
                DateAdded = DateTime.Now,
                PublisherId = book.PublisherId,


            };

            _context.Books.Add(_book);
            _context.SaveChanges();

            foreach (var id in book.AuthorIds) 
            {
                var _book_author = new Book_Author()
                {
                    BookId = _book.Id,
                    AuthorId = id,
                };

                _context.Book_Authors.Add(_book_author);
                _context.SaveChanges() ;
            }

            
        }

        public List<Book> GetAllBook()
        {
            var allBooks = _context.Books.ToList();
            return allBooks;
        }

        public BookWithAuthorsVM GetBookById(int bookId)
        {
            var _bookWithAuthors = _context.Books.Where(n => n.Id == bookId).Select(book => new BookWithAuthorsVM()
            {
                Title = book.Title,
                Description = book.Description,
                IsRead = book.IsRead,
                DateRead = book.DateRead,
                Rate = book.IsRead ? book.Rate.Value : null,
                Genre = book.Genre,
                CoverUrl = book.CoverUrl,
                PublisherName = book.Publisher.Name,
                AuthorNames = book.Book_Authors.Select(n => n.Author.FullName).ToList()

            }).FirstOrDefault();

            return _bookWithAuthors;
        }

        public Book UpdateBookById(int bookId, BookVM book)
        {
            var _book = _context.Books.FirstOrDefault(n => n.Id == bookId);
            if(_book != null)
            {
                _book.Title = book.Title;
                _book.Description = book.Description;
                _book.IsRead = book.IsRead;
                _book.DateRead = book.DateRead;
                _book.Rate = book.IsRead ? book.Rate.Value : null;
                _book.Genre = book.Genre;
                _book.CoverUrl = book.CoverUrl;

                _context.SaveChanges();
            }

            return _book;
        }

        public void DeleteBookById(int bookId)
        {
            var _book = _context.Books.FirstOrDefault(n => n.Id == bookId);
            if(_book != null)
            {
                _context.Books.Remove(_book);
                _context.SaveChanges();
            }
        }







    }
}
