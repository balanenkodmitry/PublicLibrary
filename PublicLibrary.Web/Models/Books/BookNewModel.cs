using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PublicLibrary.Domain;


namespace PublicLibrary.Web.Models.Books
{
    public class BookNewModel
    {
        public Book Book { get; set; }
        public List<User> WhosTakenBook { get; set; }
    }
}
