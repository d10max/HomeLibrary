using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homeLibrary.Models
{
    public class Author
    {
        public string FullName { get; set; } = "";
        public int Id { get; set; }

        public Author()
        {
        }
        public Author(string fullName)
        {
            FullName = fullName;
        }
        public virtual List<Book> Books { get; set; } = new();
    }
}
