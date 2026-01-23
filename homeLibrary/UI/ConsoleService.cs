using homeLibrary.Helpers;
using homeLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private static void PrintMainMenu()
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
        private static int GetUserChoice()
        {
            int choice = -1;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 0 || choice > 8)
            {
                Console.WriteLine();
                Console.WriteLine("[ERROR] Invalid input. Please enter a number between 0 and 8.");
            }

            return choice;
        }
        private static int GetYear()
        {
            int year = -1;
            while (!int.TryParse(Console.ReadLine(), out year) || year < 0 || year > 2026)
            {
                Console.WriteLine();
                Console.WriteLine("[ERROR] Invalid input. Please enter a number between 0 and 2026.");
            }

            return year;
        }
        private static int GetIdToDelete()
        {
            int id = -1;
            while(!int.TryParse(Console.ReadLine(), out id))
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
            string name = Console.ReadLine() ?? "";

            Console.WriteLine();
            Console.WriteLine("> Enter Publication Year: ");
            int year = GetYear();

            Console.WriteLine();
            Console.WriteLine("> Enter Author Name: ");
            int authorId = _libraryManager.GetAuthorId();

            _libraryManager.AddNewBook(name, year, authorId);

            Console.Clear();
            Console.WriteLine("Your book was succesfully added to library!");
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
                Console.WriteLine($"Author: {_libraryManager.GetBookAuthor(book.AuthorId)}");
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
                    Console.WriteLine($"Author: {_libraryManager.GetBookAuthor(book.AuthorId)}");
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

            if(results.Count > 0)
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine($"============================================\r\n   Found multiple books matching {rawInput}:\r\n============================================");

                foreach (var book in results)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Title: {book.Name}");
                    Console.WriteLine($"Publication year: {book.Year}");
                    Console.WriteLine($"Author: {_libraryManager.GetBookAuthor(book.AuthorId)}");
                    Console.WriteLine($"ID: {book.Id}");
                }

                Console.WriteLine();
                Console.WriteLine("> Enter Id of book you want to delete (or 0 to cancel): ");
                int idToDelete = GetIdToDelete();

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
                    //EditBookTitleUI();
                    break;
                case 6:
                    DeleteBookUI();
                    break;
                case 7:
                    PrintAllAuthorsUI();
                    break;
                case 8:
                    //PrintAllBooksOfSomeAuthorUI();
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
