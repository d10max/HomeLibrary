using homeLibrary.Data;
using homeLibrary.Services;
using homeLibrary.UI;
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
        static void Main(string[] args)
        {
            using (var db = new AppDbContext())
            {
                db.Database.EnsureCreated();

                var libraryManager = new LibraryManager(db);
                var consoleService = new ConsoleService(libraryManager);

                consoleService.MainMenuLogic();
            }
        }
    }
}