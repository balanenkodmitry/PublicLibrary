using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicLibrary.Domain
{
    public class User : Body
    {
        public String Password { get; set; }
        public string EMail { get; set; }

        public User() : base() { }

        public User(string FirstName, string LastName, MaleFemale Sex, DateTime BornDate, string EMail, string Password) : base(FirstName, LastName, Sex, BornDate)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Sex = Sex;
            this.BornDate = BornDate;
            this.EMail = EMail;
            this.Age = DateTime.Now.AddYears(BornDate.Year * -1).Year;
            this.Password = Password;
        }

        public User(string FirstName, string LastName, MaleFemale Sex, DateTime BornDate, string EMail, int Age, int ID) : base(FirstName, LastName, Sex, BornDate)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Sex = Sex;
            this.BornDate = BornDate;
            this.EMail = EMail;
            this.Age = Age;
            this.ID = ID;
        }

    }
}
