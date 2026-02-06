using homeLibrary.Data;
using homeLibrary.Helpers;
using homeLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace homeLibrary.Services
{
    public class LibraryManager
    {
        private readonly AppDbContext _db;

        public LibraryManager(AppDbContext db) {
            _db = db;
        }
        public int GetAuthorId(string authorName)
        {
            var author = _db.Authors.FirstOrDefault(a => a.FullName == authorName);

            if (author == null)
            {
                author = new Author(authorName);

                _db.Authors.Add(author);
                _db.SaveChanges();
            }

            return author.Id;
        }
        public void AddNewBook(string name, int year, int authorID)
        {
            var newBook = new Book();
            newBook.Name = name;
            newBook.Year = year;
            newBook.AuthorId = authorID;

            _db.Books.Add(newBook);
            _db.SaveChanges();
        }
        public void AddNewAuthor(string fullName)
        {
            var newAuthor = new Author(fullName);

            _db.Authors.Add(newAuthor);
            _db.SaveChanges();
        }
        public List<Book> GetBooks()
        {
            var books = _db.Books.ToList();
            return books;
        }
        public List<Author> GetAuthors()
        {
            var authors = _db.Authors.ToList();
            return authors;
        }
        public string GetBookAuthor(int id)
        {
            var author = _db.Authors.FirstOrDefault(a => a.Id == id);

            return (author != null) ? author.FullName : "";
        }
        public List<Book> GetResultsOfSearch(string query)
        {
            var allBooks = _db.Books.ToList();
            int maxMistakes = 3;

            var results = allBooks.Select(book => new
            {
                OriginalBook = book,
                NormalizedName = book.Name.NormalizeTitle(),
                levenstDist = TextHelpers.GetLevenshteinDistance(book.Name.NormalizeTitle(), query)
            })
            .Where(b => b.levenstDist <= maxMistakes || b.NormalizedName.Contains(query))
            .OrderBy(b => b.levenstDist)
            .Select(b => b.OriginalBook)
            .ToList();

            return results;
        }
        public List<Author> GetResultsOfAuthorSearch(string query)
        {
            var allAuthors = _db.Authors.ToList();
            int maxMistakes = 3;

            var results = allAuthors.Select(author => new
            {
                OriginalAuthor = author,
                NormalizedName = author.FullName.NormalizeTitle(),
                levenstDist = TextHelpers.GetLevenshteinDistance(author.FullName.NormalizeTitle(), query)
            })
            .Where(b => b.levenstDist <= maxMistakes || b.NormalizedName.Contains(query))
            .OrderBy(b => b.levenstDist)
            .Select(b => b.OriginalAuthor)
            .ToList();

            return results;
        }
        public bool DeleteBookById(int id)
        {
            var book = _db.Books.Find(id);

            if (book == null) return false;

            _db.Books.Remove(book);
            _db.SaveChanges();

            return true;
        }
        public Book? FindBookById(int id)
        {
            var book = _db.Books.Find(id);
            return book;
        }
        public void SaveChanges()
        {
            _db.SaveChanges();
        }
    }
}
