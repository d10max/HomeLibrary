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
            using(var db = new AppDbContext())
            {
                var LibraryManager = new LibraryManager(db);
                var ConsoleService = new ConsoleService(LibraryManager);

                ConsoleService.MainMenuLogic();
            }
        }
    }
} 