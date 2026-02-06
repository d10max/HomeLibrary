using homeLibrary.Helpers;
using homeLibrary.Models;
using homeLibrary.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace homeLibrary.UI
{
    public class ConsoleService
    {
        private LibraryManager _libraryManager;
        private bool IsProgramRunning = true;
        public ConsoleService(LibraryManager libraryManager)
        {
            _libraryManager = libraryManager;
        }
        private void PrintMainMenu()
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
        private int GetUserChoice()
        {
            int choice = -1;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 0 || choice > 8)
            {
                Console.WriteLine();
                Console.WriteLine("[ERROR] Invalid input. Please enter a number between 0 and 8.");
            }

            return choice;
        }
        private int GetYear()
        {
            int year = -1;
            var currentYear = DateTime.Now.Year;
            while (!int.TryParse(Console.ReadLine(), out year) || year < 0 || year > currentYear)
            {
                Console.WriteLine();
                Console.WriteLine($"[ERROR] Invalid input. Please enter a number between 0 and {currentYear}.");
            }

            return year;
        }
        private int GetYearForUpdate(int prevYear)
        {
            if (string.IsNullOrWhiteSpace(Console.ReadLine())) return prevYear;

            int year = -1;
            int currentYear = DateTime.Now.Year;
            while (!int.TryParse(Console.ReadLine(), out year) || year < 0 || year > currentYear)
            {
                Console.WriteLine();
                Console.WriteLine($"[ERROR] Invalid input. Please enter a number between 0 and {currentYear}.");
            }

            return year;
        }
        private int GetIdFromUser()
        {
            int id = -1;
            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine();
                Console.WriteLine("[ERROR] Invalid input. Please enter a ID(number) of book you want to delete (or 0 to cancel)");
            }

            return id;
        }
        private void ExitUI()
        {
            Console.Clear();
            Console.WriteLine("Thank you for using the Virtual Library.\r\nHave a great day!");
            IsProgramRunning = false;
        }
        private void AddNewBookUI()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("==========================\r\n        ADD NEW BOOK\r\n==========================");

            Console.WriteLine();
            Console.WriteLine("> Enter Book Title: ");
            string name = Console.ReadLine() ?? "None";

            Console.WriteLine();
            Console.WriteLine("> Enter Publication Year: ");
            int year = GetYear();

            Console.WriteLine();
            Console.WriteLine("> Enter Author Name: ");
            string authorName = Console.ReadLine() ?? "None";

            _libraryManager.AddNewBook(name, year, authorName);

            Console.Clear();
            Console.WriteLine("Your book was successfully added to library!");
            Console.WriteLine("Press enter to return to menu...");
            Console.ReadLine();
        }
        private void AddNewAuthorUI()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("==========================\r\n        ADD NEW AUTHOR\r\n==========================");

            Console.WriteLine();
            Console.WriteLine("> Enter Author Name: ");
            string authorFullName = Console.ReadLine() ?? "";

            _libraryManager.AddNewAuthor(authorFullName);

            Console.Clear();
            Console.WriteLine("Your author was successfully added to library!");
            Console.WriteLine("Press any key to return to menu...");
            Console.ReadLine();

        }
        private void PrintAllBooksUI()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("==========================\r\n   LIST OF ALL BOOKS\r\n==========================");

            var books = _libraryManager.GetBooks();

            foreach (var book in books)
            {
                Console.WriteLine();
                Console.WriteLine($"Title: {book.Name}");
                Console.WriteLine($"Publication year: {book.Year}");
                Console.WriteLine($"Author: {book.Author.FullName}");
            }

            Console.WriteLine("");
            Console.WriteLine("Press enter to return to menu...");
            Console.ReadLine();
        }
        private void FindBookUI()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("==========================\r\n        FIND BOOK\r\n==========================");

            Console.WriteLine();
            Console.WriteLine("> Enter Book Title: ");

            string rawInput = Console.ReadLine() ?? "";

            string query = rawInput.NormalizeTitle();

            var results = _libraryManager.GetResultsOfSearch(query);

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
                    Console.WriteLine($"Author: {book.Author.FullName}");
                }
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("There is no book with this name in library :(");
            }

            Console.WriteLine("");
            Console.WriteLine("Press enter to return to menu...");
            Console.ReadLine();
        }
        private void PrintAllAuthorsUI()
        {
            Console.Clear();
            Console.WriteLine("=============================================\r\n          " +
                    "List of all authors" + "\n=============================================");

            var authors = _libraryManager.GetAuthors();

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
        private void DeleteBookUI()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("==========================\r\n        DELETE BOOK\r\n==========================");

            Console.WriteLine();
            Console.WriteLine("> Enter Title of book you want to delete: ");

            string rawInput = Console.ReadLine() ?? "";
            string input = rawInput.NormalizeTitle();

            var results = _libraryManager.GetResultsOfSearch(input);

            if (results.Count > 0)
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine($"============================================\r\n   Found multiple books matching {rawInput}:\r\n============================================");

                foreach (var book in results)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Title: {book.Name}");
                    Console.WriteLine($"Publication year: {book.Year}");
                    Console.WriteLine($"Author: {book.Author.FullName}");
                    Console.WriteLine($"ID: {book.Id}");
                }

                Console.WriteLine();
                Console.WriteLine("> Enter Id of book you want to delete (or 0 to cancel): ");
                int idToDelete = GetIdFromUser();

                if (idToDelete == 0)
                {
                    return;
                }
                else
                {
                    bool isDeleted = _libraryManager.DeleteBookById(idToDelete);

                    if (isDeleted)
                    {
                        Console.Clear();
                        Console.WriteLine();
                        Console.WriteLine("Your book was successfully deleted!");
                        Console.WriteLine("Press enter to return to menu...");
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine();
                        Console.WriteLine("Your book wasn`t deleted!");
                        Console.WriteLine("Press enter to return to menu...");
                        Console.ReadLine();
                    }
                }
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("There is no book with this name in library :(");
                Console.WriteLine("Press enter to return to menu...");
                Console.ReadLine();
            }
        }
        private void EditBookInfoUI()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("==========================\r\n        EDIT BOOK INFO\r\n==========================");

            Console.WriteLine();
            Console.WriteLine("> Enter Title of book you want to edit: ");

            string rawInput = Console.ReadLine() ?? "";
            string input = rawInput.NormalizeTitle();

            var results = _libraryManager.GetResultsOfSearch(input);

            if (results.Count > 0)
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine($"============================================\r\n   Found multiple books matching {rawInput}:\r\n============================================");

                foreach (var book in results)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Title: {book.Name}");
                    Console.WriteLine($"Publication year: {book.Year}");
                    Console.WriteLine($"Author: {book.Author.FullName}");
                    Console.WriteLine($"ID: {book.Id}");
                }

                Console.WriteLine();
                Console.WriteLine("> Enter Id of book you want to edit (or 0 to cancel): ");
                int idToEdit = GetIdFromUser();

                if (idToEdit == 0)
                {
                    return;
                }

                var bookToEdit = _libraryManager.FindBookById(idToEdit);

                if (bookToEdit == null)
                {
                    Console.WriteLine();
                    Console.WriteLine("ERROR: there is no book with this id!");
                    return;
                }

                Console.Clear();
                Console.WriteLine("CURRENT BOOK INFO:");
                Console.WriteLine();
                Console.WriteLine($"Title: {bookToEdit.Name}");
                Console.WriteLine($"Publication year: {bookToEdit.Year}");
                Console.WriteLine($"Author: {bookToEdit.Author.FullName}");
                Console.WriteLine($"ID: {bookToEdit.Id}");

                Console.WriteLine();
                Console.WriteLine("> Enter new Book Title (or press ENTER to leave it without changes): ");
                string name = Console.ReadLine() ?? "";
                if (!string.IsNullOrWhiteSpace(name)) bookToEdit.Name = name;

                Console.WriteLine();
                Console.WriteLine("> Enter new Publication Year (or press ENTER to leave it without changes): ");
                bookToEdit.Year = GetYearForUpdate(bookToEdit.Year);

                Console.WriteLine();
                Console.WriteLine("> Enter new Author Name (or press ENTER to leave it without changes): ");
                string authorName = Console.ReadLine() ?? "None";
                if (!string.IsNullOrWhiteSpace(authorName)) bookToEdit.AuthorId = _libraryManager.GetOrCreateAuthorId(authorName);

                _libraryManager.SaveChanges();

                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Your book was successfully edited!");
                Console.WriteLine("\nEDITED BOOK INFO:");
                Console.WriteLine();
                Console.WriteLine($"Title: {bookToEdit.Name}");
                Console.WriteLine($"Publication year: {bookToEdit.Year}");
                Console.WriteLine($"Author: {bookToEdit.Author.FullName}");
                Console.WriteLine($"ID: {bookToEdit.Id}");
                Console.WriteLine("\nPress enter to return to menu...");
                Console.ReadLine();

            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("There is no book with this name in library :(");
                Console.WriteLine("Press enter to return to menu...");
                Console.ReadLine();
            }
        }
        private void PrintAllBooksOfSomeAuthorUI()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("==========================\r\n   FIND BOOKS BY AUTHOR\r\n==========================");

            Console.WriteLine("\n>Enter name of author: ");
            string query = (Console.ReadLine() ?? "None").NormalizeTitle();

            var results = _libraryManager.GetResultsOfAuthorSearch(query);

            if (results.Count <= 0)
            {
                Console.WriteLine();
                Console.WriteLine("There is no author with this name in library :(");
                Console.WriteLine("Press enter to return to menu...");
                Console.ReadLine();
                return;
            }

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine($"==============================================\r\n   Found multiple Authors matching {query}:\r\n==============================================");

            foreach (var author in results)
            {
                Console.WriteLine();
                Console.WriteLine($"Author`s name: {author.FullName}");
                Console.WriteLine($"ID: {author.Id}");
            }

            Console.WriteLine();
            Console.WriteLine("> Enter Id of author (or 0 to cancel): ");
            int authorId = GetIdFromUser();

            if (authorId == 0)
            {
                Console.WriteLine("Press enter to return to menu...");
                Console.ReadLine();
                return;
            }

            var findedAuthor = _libraryManager.GetAuthorById(authorId);

            if (findedAuthor == null)
            {
                Console.WriteLine();
                Console.WriteLine("There is no author with this ID in library :(");
                Console.WriteLine("Press enter to return to menu...");
                Console.ReadLine();
                return;
            }

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine($"=================================\r\n   BOOKS BY AUTHOR - {findedAuthor.FullName} \r\n=================================");

            var books = findedAuthor.Books;
            foreach (var book in books)
            {
                Console.WriteLine();
                Console.WriteLine($"Title: {book.Name}");
                Console.WriteLine($"Publication year: {book.Year}");
                Console.WriteLine($"Id: {book.Id}");
            }

            Console.WriteLine();
            Console.WriteLine("Press enter to return to menu...");
            Console.ReadLine();
        }
        public void HandleInput(int choice)
        {
            switch (choice)
            {
                case 0:
                    ExitUI();
                    return;
                case 1:
                    AddNewBookUI();
                    break;
                case 2:
                    AddNewAuthorUI();
                    break;
                case 3:
                    PrintAllBooksUI();
                    break;
                case 4:
                    FindBookUI();
                    break;
                case 5:
                    EditBookInfoUI();
                    break;
                case 6:
                    DeleteBookUI();
                    break;
                case 7:
                    PrintAllAuthorsUI();
                    break;
                case 8:
                    PrintAllBooksOfSomeAuthorUI();
                    break;
            }
        }
        public void MainMenuLogic()
        {
            while (IsProgramRunning)
            {
                PrintMainMenu();

                int choice = GetUserChoice();

                HandleInput(choice);
            }

        }
    }
}
