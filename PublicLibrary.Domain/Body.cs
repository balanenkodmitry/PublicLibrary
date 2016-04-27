using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicLibrary.Domain
{
    public class Body
    {
        public int Age { get; set; }
        public int ID { get; set; }
        public MaleFemale Sex { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BornDate { get; set; }
        
        public enum MaleFemale : int
        {
            Male = 1,
            Female = 2
        }

        public Body() { }

        public Body(string FirstName, string LastName, MaleFemale Sex, DateTime BornDate)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Sex = Sex;
            this.BornDate = BornDate;
            this.Age = DateTime.Now.AddYears(BornDate.Year * -1).Year;
        }
    }
}
