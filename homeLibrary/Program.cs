using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Xml.Serialization;
using static System.Reflection.Metadata.BlobBuilder;

namespace homeLibrary
{
    internal class Program
    {
        static void PrintMainMenu()
        {
            Console.Clear();
            Console.Write("=============================================\r\n          " +
                    "VIRTUAL LIBRARY SYSTEM\r\n=============================================\r\n  " +
                    "[1] Add New Book\r\n  " +
                    "[2] Add New Author\r\n  ---------------------------------------------\r\n  " +
                    "[3] Show All Books\r\n  " +
                    "[4] Search for a Book\r\n  ---------------------------------------------\r\n  " +
                    "[5] Edit Book Title\r\n  " +
                    "[6] Delete Book\r\n  ---------------------------------------------\r\n  " +
                    "[7] Show All Authors\r\n  " + 
                    "[8] Show All Books Of Some Author\r\n  ---------------------------------------------\r\n  " +
                    "[0] Exit\r\n=============================================\r\n  " +
                    "Select an option (0-8): ");
        }
        static int GetUserChoise()
        {
            int choice = -1;
            while(!int.TryParse(Console.ReadLine(), out choice) || (choice < 0 || choice > 8))
            {
                Console.WriteLine();
                Console.WriteLine("[ERROR] Invalid input. Please enter a number between 0 and 8.");
            }

            return choice;
        }
        static int GetYear()
        {
            int year = -1;
            while (!int.TryParse(Console.ReadLine(), out year) || (year < 0 || year > 2026))
            {
                Console.WriteLine();
                Console.WriteLine("[ERROR] Invalid input. Please enter a number between 0 and 2026.");
            }

            return year;
        }
        static int GetAuthorId(AppDbContext db)
        {
            string authorName = Console.ReadLine() ?? "";

            var author = db.Authors.FirstOrDefault(a => a.FullName == authorName);

            if(author == null)
            {
                author = new Author(authorName);

                db.Authors.Add(author);
                db.SaveChanges();
            }

            return author.Id;
        }

        //1. +
        static void AddNewBook(AppDbContext db) 
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("==========================\r\n        ADD NEW BOOK\r\n==========================");

            Book newBook = new Book();

            Console.WriteLine();
            Console.WriteLine("> Enter Book Title: ");
            newBook.Name = Console.ReadLine() ?? "";

            Console.WriteLine();
            Console.WriteLine("> Enter Publication Year: ");
            newBook.Year = GetYear();

            Console.WriteLine();
            Console.WriteLine("> Enter Author Name: ");
            newBook.AuthorId = GetAuthorId(db);

            db.Books.Add(newBook);
            db.SaveChanges();

            Console.Clear();
            Console.WriteLine("Your book was succesfully added to library!");
            Console.WriteLine("Press any key to return to menu...");
            Console.ReadLine();
        }
        //2. +
        static void AddNewAuthor(AppDbContext db)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("==========================\r\n        ADD NEW AUTHOR\r\n==========================");

            Author newAuthor = new Author();

            Console.WriteLine();
            Console.WriteLine("> Enter Author Name: ");
            newAuthor.FullName = Console.ReadLine() ?? "";

            db.Authors.Add(newAuthor);
            db.SaveChanges();

            Console.Clear();
            Console.WriteLine("Your author was succesfully added to library!");
            Console.WriteLine("Press any key to return to menu...");
            Console.ReadLine();

        }
        static string GetBookAuthor(AppDbContext db, int id)
        {
            var author = db.Authors.FirstOrDefault(a => a.Id == id);

            if (author != null)
            {
                return author.FullName;
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("This book hasn't author!");

                return "";
            }
        }

        //3. +
        static void PrintAllBooks(AppDbContext db)
        {
            var books = db.Books.ToList();

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("==========================\r\n   LIST OF ALL BOOKS\r\n==========================");

            foreach (var book in books) {
                Console.WriteLine();
                Console.WriteLine($"Title: {book.Name}");
                Console.WriteLine($"Publication year: {book.Year}");
                Console.WriteLine($"Author: {GetBookAuthor(db, book.AuthorId)}");
            }

            Console.WriteLine("");
            Console.WriteLine("Press any key to return to menu...");
            Console.ReadLine();
        }

        //видаляє зайві пробіли, та перетворює всі символи рядка в нижній регістер
        static string NormalizeTitle(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";

            input = input.Trim().ToLower();

            var words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return string.Join(" ", words);
        }
        //4.
        static void FindBook(AppDbContext db)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("==========================\r\n        FIND BOOK\r\n==========================");

            Console.WriteLine();
            Console.WriteLine("> Enter Book Title: ");

            string rawInput = Console.ReadLine() ?? "";

            string query = NormalizeTitle(rawInput);

            //розібратись з select щоб вибирати потрібні книги зі всіх
            var allBooks = db.Books.ToList();
            int maxMistakes = 3;

            var results = allBooks.Select(book => new
            {
                OriginalBook = book,
                NormalizedName = NormalizeTitle(book.Name),
                levenstDist = LevenstainDist(NormalizeTitle(book.Name), query)
            })
            .Where(b => b.levenstDist <= maxMistakes || b.NormalizedName.Contains(query))
            .OrderBy(b => b.levenstDist)
            .Select(b => b.OriginalBook)
            .ToList();

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("==========================\r\n   RESULT OF YOUR SEARCH\r\n==========================");

            if (results.Count > 0)
            {
                foreach (var book in results)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Title: {book.Name}");
                    Console.WriteLine($"Publication year: {book.Year}");
                    Console.WriteLine($"Author: {GetBookAuthor(db, book.AuthorId)}");
                }
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("There is no book with this name in library :(");
            }

            Console.WriteLine("");
            Console.WriteLine("Press any key to return to menu...");
            Console.ReadLine();
        }
        //5.
        static void EditBookTitle(AppDbContext db)
        {

        }
        //6.
        static void DeleteBook(AppDbContext db)
        {

        }
        //7. +
        static void PrintAllAuthors(AppDbContext db) 
        {

            var authors = db.Authors.ToList();

            Console.Clear();
            Console.WriteLine("=============================================\r\n          " +
                    "List of all authors" + "\n=============================================");

            foreach (var author in authors)
            {
                Console.WriteLine($"Name: {author.FullName}");
                Console.WriteLine($"Id: {author.Id}");
                Console.WriteLine("---------------------------------------------");
            }

            Console.WriteLine("");
            Console.WriteLine("Press any key to return to menu...");
            Console.ReadLine();
        }

        static void PrintAllBooksOfSomeAuthor(AppDbContext db)
        {
            

        }

        static int LevenstainDist(string s1, string s2)
        {
            int n = s1.Length, m = s2.Length;
            int[,] d = new int[n + 1, m + 1];

            for (int i = 0; i <= n; i++) d[i, 0] = i;
            for (int j = 0; j <= m; j++) d[0, j] = j;

            for(int i = 1; i <= n; i++)
            {
                for(int j = 1; j <= m; j++)
                {
                    int cost = (s1[i - 1] == s2[j - 1]) ? 0 : 1;
                    int insertion = d[i, j - 1] + 1;
                    int deletion = d[i - 1, j] + 1;
                    int substitution = d[i - 1, j - 1] + cost;

                    d[i, j] = Math.Min(insertion, Math.Min(deletion, substitution) );
                }
            }

            /*
            for(int i = 0; i <= n; i++)
            {
                for(int j = 0; j <= m; j++)
                {
                    Console.Write($"{d[i, j]} ");
                }
                Console.WriteLine();
            }*/

            return d[n, m];
        }

        static void MainMenu()
        {
            while (true)
            {
                PrintMainMenu();

                int choise = GetUserChoise();

                using (var db = new AppDbContext()) {
                    switch (choise)
                    {
                        case 0:
                            Console.Clear();
                            Console.WriteLine("Thank you for using the Virtual Library.\r\nHave a great day!");
                            return;
                        case 1:
                            AddNewBook(db);
                            break;
                        case 2:
                            AddNewAuthor(db);
                            break;
                        case 3:
                            PrintAllBooks(db);
                            break;
                        case 4:
                            FindBook(db);
                            break;
                        case 5:
                            EditBookTitle(db);
                            break;
                        case 6:
                            DeleteBook(db);
                            break;
                        case 7:
                            PrintAllAuthors(db);
                            break;
                        case 8:
                            PrintAllBooksOfSomeAuthor(db);
                            break;
                    }
                }  
            }

        }
        
        static void Main(string[] args)
        {
            //FindBook();
            //LevenstainAlgo(Console.ReadLine(), Console.ReadLine());
            MainMenu();
        }
    }
} 