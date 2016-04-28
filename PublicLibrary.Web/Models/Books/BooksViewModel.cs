using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PublicLibrary.Domain;

namespace PublicLibrary.Web.Models.Books
{
    public class BooksViewModel
    {
        public IEnumerable<Book> Books { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}