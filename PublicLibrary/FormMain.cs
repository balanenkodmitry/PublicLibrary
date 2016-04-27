using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PublicLibrary.Domain;

namespace PublicLibrary
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();

            User us = new User("Vasya", "Pupkin", User.MaleFemale.Male, new DateTime(1980, 01, 01), "qwerty@mail.com", "qwerty");

            MyORM.AddUser(us);

            User us1 = new User("Pavlic", "Marozov", User.MaleFemale.Male, new DateTime(1928, 05, 15), "asdfg@mail.ru", "qwerty");

            MyORM.AddUser(us1);

            User us2 = new User("Anka", "Mashinganner", User.MaleFemale.Female, new DateTime(1885, 06, 28), "asdfg@mail.su", "qwerty");

            MyORM.AddUser(us2);

            Author au = new Author("Jack", "London", Body.MaleFemale.Male, new DateTime(1875, 07, 14), new DateTime(1910, 05, 10));

            MyORM.AddAuthor(au);

            Author au1 = new Author("Boris", "Akunin", Body.MaleFemale.Male, new DateTime(1962, 08, 14));

            MyORM.AddAuthor(au1);

            Author au2 = new Author("Anna", "Ahmatova", Body.MaleFemale.Female, new DateTime(1910, 10, 20), new DateTime(1968, 05, 20));

            MyORM.AddAuthor(au2);

            List<Author> Authors = new List<Author>();
            Authors.Add(au);
            Authors.Add(au1);
            Authors.Add(au2);

            Book Book = new Book("Robinzon Cruzo", Book.Availability.Available, Authors);

            MyORM.AddBook(Book, Authors);

            User foundUs = MyORM.GetUser(19);

            User foundUs1 = MyORM.GetUser(30);

            User foundUs2 = MyORM.GetUser("Anka", "Mashinganner");

            List<Author> aut = MyORM.GetBookAuthors(9);

            List<Book> bk = MyORM.GetBooksOfAthor(MyORM.GetAuthor(14));

            Book Book1 = MyORM.GetBook("Robinzon Cruzo");

        }
    }
}
