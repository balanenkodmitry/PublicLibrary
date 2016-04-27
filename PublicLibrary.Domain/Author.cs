using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicLibrary.Domain
{
    public class Author : Body
    {
        public DateTime? DeathDate { get; set; }

        public Author() : base() { }

        public Author(string FirstName, string LastName, MaleFemale Sex, DateTime BornDate, DateTime? DeathDate = null) : base(FirstName, LastName, Sex, BornDate)
        {
            this.DeathDate = DeathDate;
        }
    }
}
