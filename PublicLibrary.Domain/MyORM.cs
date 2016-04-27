using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace PublicLibrary.Domain
{
    public static class MyORM
    {
        private static string GetConnectionString()
        {
            const string ConnString = "Server=192.168.10.220;Database=_TEST_ASP_NET_Test;Uid=sa;Pwd=Undergr0und;";

            return ConnString;
        }

        public static void AddUser(User User)
        {
            bool Exist = false;

            try
            {
                Exist = IfExistUser(User);
            }
            catch (DataBaseAlreadyHasElementException<User> e)
            {
                Exist = true;
            }


            if (Exist)
            {
                return;
            }

            using (SqlConnection Conn = new SqlConnection(GetConnectionString()))
            {
                Conn.Open();

                SqlDataAdapter Adaptor = new SqlDataAdapter();

                string CommandText = "insert into dbo.Users (FirstName, LastName, Sex, EMail, Password, BornDate, Age) values(@FirstName, @LastName, @Sex, @Email, @Password, @BornDate, @Age)";

                Adaptor.InsertCommand = new SqlCommand(CommandText, Conn);
                Adaptor.InsertCommand.CommandType = CommandType.Text;

                Adaptor.InsertCommand.Parameters.Clear();

                Adaptor.InsertCommand.Parameters.Add("@FirstName", SqlDbType.NVarChar);
                Adaptor.InsertCommand.Parameters.Add("@LastName", SqlDbType.NVarChar);
                Adaptor.InsertCommand.Parameters.Add("@Sex", SqlDbType.SmallInt);
                Adaptor.InsertCommand.Parameters.Add("@Email", SqlDbType.NVarChar);
                Adaptor.InsertCommand.Parameters.Add("@Password", SqlDbType.NVarChar);
                Adaptor.InsertCommand.Parameters.Add("@BornDate", SqlDbType.Date);
                Adaptor.InsertCommand.Parameters.Add("@Age", SqlDbType.Int);

                Adaptor.InsertCommand.Parameters["@FirstName"].Value = User.FirstName;
                Adaptor.InsertCommand.Parameters["@LastName"].Value = User.LastName;
                Adaptor.InsertCommand.Parameters["@Sex"].Value = (int)User.Sex;
                Adaptor.InsertCommand.Parameters["@Email"].Value = User.EMail;
                Adaptor.InsertCommand.Parameters["@Password"].Value = User.Password;
                Adaptor.InsertCommand.Parameters["@BornDate"].Value = User.BornDate;
                Adaptor.InsertCommand.Parameters["@Age"].Value = User.Age;

                Adaptor.InsertCommand.ExecuteNonQuery();

                User.ID = GetUser(User.FirstName, User.LastName).ID;

                Conn.Close();
            }
        }

        public static bool IfExistUser(User User)
        {
            using (SqlConnection Conn = new SqlConnection(GetConnectionString()))
            {
                SqlDataAdapter Adaptor = new SqlDataAdapter();

                DataSet ds = new DataSet();

                string CommandText = "select * from Users where FirstName = @FirstName AND LastName = @LastName";

                Adaptor.SelectCommand = new SqlCommand(CommandText, Conn);

                Adaptor.SelectCommand.Parameters.Clear();

                Adaptor.SelectCommand.Parameters.Add("@FirstName", SqlDbType.NVarChar);
                Adaptor.SelectCommand.Parameters["@FirstName"].Value = User.FirstName;
                Adaptor.SelectCommand.Parameters.Add("@LastName", SqlDbType.NVarChar);
                Adaptor.SelectCommand.Parameters["@LastName"].Value = User.LastName;

                Adaptor.Fill(ds);

                DataTable Users = ds.Tables[0];

                var query = Users.AsEnumerable().Select(user => new
                {
                    UserName = user.Field<string>("FirstName"),
                    UserLastName = user.Field<string>("LastName"),
                    UserID = user.Field<int>("ID")
                });

                query.Where(cond => cond.UserName == User.FirstName && cond.UserLastName == User.LastName);

                Conn.Close();

                if (query.Count() > 0)
                {
                    throw new DataBaseAlreadyHasElementException<User>(User);                    
                }
            }

            return false;
        }

        public static User GetUser(int ID)
        {
            using (SqlConnection Conn = new SqlConnection(GetConnectionString()))
            {
                SqlDataAdapter Adaptor = new SqlDataAdapter();

                DataSet ds = new DataSet();

                string CommandText = "select * from Users where ID = @ID";

                Adaptor.SelectCommand = new SqlCommand(CommandText, Conn);

                Adaptor.SelectCommand.Parameters.Clear();

                Adaptor.SelectCommand.Parameters.Add("@ID", SqlDbType.Int);
                Adaptor.SelectCommand.Parameters["@ID"].Value = ID;
                
                Adaptor.Fill(ds);

                var empList = ds.Tables[0].AsEnumerable()
                    .Select(dataRow => new User 
                    {
                        FirstName = dataRow.Field<string>("FirstName"),
                        LastName = dataRow.Field<string>("LastName"),
                        Sex = dataRow.Field<User.MaleFemale>("Sex"),
                        BornDate = dataRow.Field<DateTime>("BornDate"),
                        EMail = dataRow.Field<string>("EMail"),
                        Age = dataRow.Field<int>("Age"),
                        ID = dataRow.Field<int>("ID")
                    })
                    .ToList();

                if (empList.Count > 0) 
                {
                    return empList[0];
                }                            

                Conn.Close();                       
            }

            return null;
        }

        public static User GetUser(string FirstName, string LastName)
        {
            using (SqlConnection Conn = new SqlConnection(GetConnectionString()))
            {
                SqlDataAdapter Adaptor = new SqlDataAdapter();

                DataSet ds = new DataSet();

                string CommandText = "select * from Users where FirstName = @FirstName AND LastName = @LastName";

                Adaptor.SelectCommand = new SqlCommand(CommandText, Conn);

                Adaptor.SelectCommand.Parameters.Clear();

                Adaptor.SelectCommand.Parameters.Add("@FirstName", SqlDbType.NVarChar);
                Adaptor.SelectCommand.Parameters["@FirstName"].Value = FirstName;

                Adaptor.SelectCommand.Parameters.Add("@LastName", SqlDbType.NVarChar);
                Adaptor.SelectCommand.Parameters["@LastName"].Value = LastName;

                Adaptor.Fill(ds);

                var empList = ds.Tables[0].AsEnumerable()
                    .Select(dataRow => new User
                    {
                        FirstName = dataRow.Field<string>("FirstName"),
                        LastName = dataRow.Field<string>("LastName"),
                        Sex = dataRow.Field<User.MaleFemale>("Sex"),
                        BornDate = dataRow.Field<DateTime>("BornDate"),
                        EMail = dataRow.Field<string>("EMail"),
                        Age = dataRow.Field<int>("Age"),
                        ID = dataRow.Field<int>("ID")
                    })
                    .ToList();

                if (empList.Count > 0)
                {
                    return empList[0];
                }

                Conn.Close();
            }

            return null;
        }

        public static void AddAuthor(Author Author)
        {
            bool Exist = false;

            try
            {
                Exist = IfExistAuthor(Author);
            }
            catch (DataBaseAlreadyHasElementException<Author> e)
            {
                Exist = true;
            }


            if (Exist)
            {
                return;
            }

            using (SqlConnection Conn = new SqlConnection(GetConnectionString()))
            {
                Conn.Open();

                string CommandText;

                SqlDataAdapter Adaptor = new SqlDataAdapter();
                if (Author.DeathDate != null)
                {
                    CommandText = "insert into dbo.Authors (FirstName, LastName, Sex, BornDate, Age, DeathDate) values(@FirstName, @LastName, @Sex, @BornDate, @Age, @DeathDate)";
                }
                else
                {
                    CommandText = "insert into dbo.Authors (FirstName, LastName, Sex, BornDate, Age) values(@FirstName, @LastName, @Sex, @BornDate, @Age)";
                }

                Adaptor.InsertCommand = new SqlCommand(CommandText, Conn);
                Adaptor.InsertCommand.CommandType = CommandType.Text;

                Adaptor.InsertCommand.Parameters.Clear();

                Adaptor.InsertCommand.Parameters.Add("@FirstName", SqlDbType.NVarChar);
                Adaptor.InsertCommand.Parameters.Add("@LastName", SqlDbType.NVarChar);
                Adaptor.InsertCommand.Parameters.Add("@Sex", SqlDbType.SmallInt);
                Adaptor.InsertCommand.Parameters.Add("@BornDate", SqlDbType.Date);
                Adaptor.InsertCommand.Parameters.Add("@Age", SqlDbType.Int);
                if (Author.DeathDate != null)
                {
                    Adaptor.InsertCommand.Parameters.Add("@DeathDate", SqlDbType.Date);
                }


                Adaptor.InsertCommand.Parameters["@FirstName"].Value = Author.FirstName;
                Adaptor.InsertCommand.Parameters["@LastName"].Value = Author.LastName;
                Adaptor.InsertCommand.Parameters["@Sex"].Value = Author.Sex;
                Adaptor.InsertCommand.Parameters["@BornDate"].Value = Author.BornDate;
                Adaptor.InsertCommand.Parameters["@Age"].Value = Author.Age;
                if (Author.DeathDate != null)
                {
                    Adaptor.InsertCommand.Parameters["@DeathDate"].Value = Author.DeathDate;
                }

                Adaptor.InsertCommand.ExecuteNonQuery();

                Author.ID = GetAuthor(Author.FirstName, Author.LastName).ID;

                Conn.Close();
            }
        }

        public static bool IfExistAuthor(Author Author)
        {
            using (SqlConnection Conn = new SqlConnection(GetConnectionString()))
            {
                SqlDataAdapter Adaptor = new SqlDataAdapter();

                DataSet ds = new DataSet();

                string CommandText = "select * from Authors where FirstName = @FirstName AND LastName = @LastName";

                Adaptor.SelectCommand = new SqlCommand(CommandText, Conn);

                Adaptor.SelectCommand.Parameters.Clear();

                Adaptor.SelectCommand.Parameters.Add("@FirstName", SqlDbType.NVarChar);
                Adaptor.SelectCommand.Parameters["@FirstName"].Value = Author.FirstName;
                Adaptor.SelectCommand.Parameters.Add("@LastName", SqlDbType.NVarChar);
                Adaptor.SelectCommand.Parameters["@LastName"].Value = Author.LastName;

                Adaptor.Fill(ds);

                DataTable Authors = ds.Tables[0];

                var query = Authors.AsEnumerable().Select(author => new
                {
                    FirstName = author.Field<string>("FirstName"),
                    LastName = author.Field<string>("LastName"),
                    ID = author.Field<int>("ID")
                });

                query.Where(cond => cond.FirstName == Author.FirstName && cond.LastName == Author.LastName);

                if (query.Count() > 0)
                {
                    throw new DataBaseAlreadyHasElementException<Author>(Author);
                }

            }

            return false;
        }

        public static Author GetAuthor(int ID)
        {
            using (SqlConnection Conn = new SqlConnection(GetConnectionString()))
            {
                SqlDataAdapter Adaptor = new SqlDataAdapter();

                DataSet ds = new DataSet();

                string CommandText = "select * from dbo.Authors where ID = @ID";

                Adaptor.SelectCommand = new SqlCommand(CommandText, Conn);

                Adaptor.SelectCommand.Parameters.Clear();

                Adaptor.SelectCommand.Parameters.Add("@ID", SqlDbType.Int);
                Adaptor.SelectCommand.Parameters["@ID"].Value = ID;

                Adaptor.Fill(ds);

                var empList = ds.Tables[0].AsEnumerable()
                    .Select(dataRow => new Author
                    {
                        FirstName = dataRow.Field<string>("FirstName"),
                        LastName = dataRow.Field<string>("LastName"),
                        Sex = dataRow.Field<User.MaleFemale>("Sex"),
                        BornDate = dataRow.Field<DateTime>("BornDate"),
                        DeathDate = dataRow.Field<DateTime>("DeathDate"),
                        Age = dataRow.Field<int>("Age"),
                        ID = dataRow.Field<int>("ID")
                    })
                    .ToList();

                if (empList.Count > 0)
                {
                    return empList[0];
                }

                Conn.Close();
            }

            return null;
        }

        public static Author GetAuthor(string FirstName, string LastName)
        {
            using (SqlConnection Conn = new SqlConnection(GetConnectionString()))
            {
                SqlDataAdapter Adaptor = new SqlDataAdapter();

                DataSet ds = new DataSet();

                string CommandText = "select * from dbo.Authors where FirstName = @FirstName AND LastName = @LastName";

                Adaptor.SelectCommand = new SqlCommand(CommandText, Conn);

                Adaptor.SelectCommand.Parameters.Clear();

                Adaptor.SelectCommand.Parameters.Add("@FirstName", SqlDbType.NChar);
                Adaptor.SelectCommand.Parameters["@FirstName"].Value = FirstName;

                Adaptor.SelectCommand.Parameters.Add("@LastName", SqlDbType.NChar);
                Adaptor.SelectCommand.Parameters["@LastName"].Value = LastName;

                Adaptor.Fill(ds);

                var empList = ds.Tables[0].AsEnumerable()
                    .Select(dataRow => new Author
                    {
                        FirstName = dataRow.Field<string>("FirstName"),
                        LastName = dataRow.Field<string>("LastName"),
                        Sex = dataRow.Field<User.MaleFemale>("Sex"),
                        BornDate = dataRow.Field<DateTime>("BornDate"),
                        DeathDate = dataRow.Field<DateTime?>("DeathDate"),
                        Age = dataRow.Field<int>("Age"),
                        ID = dataRow.Field<int>("ID")
                    })
                    .ToList();

                if (empList.Count > 0)
                {
                    return empList[0];
                }

                Conn.Close();
            }

            return null;
        }

        public static void AddBook(Book Book, List<Author> Autors)
        {
            bool Exist = false;

            try
            {
                Exist = IfExistBook(Book);
            }
            catch (DataBaseAlreadyHasElementException<Book> e)
            {
                Exist = true;
            }

            if (Exist)
            {
                return;
            }

            using (SqlConnection Conn = new SqlConnection(GetConnectionString()))
            {
                Conn.Open();

                SqlDataAdapter Adaptor = new SqlDataAdapter();

                string CommandText = "INSERT INTO dbo.Books (Name, Available) VALUES (@Name, @Available)";

                Adaptor.InsertCommand = new SqlCommand(CommandText, Conn);
                Adaptor.InsertCommand.CommandType = CommandType.Text;

                Adaptor.InsertCommand.Parameters.Clear();

                Adaptor.InsertCommand.Parameters.Add("@Name", SqlDbType.NVarChar);
                Adaptor.InsertCommand.Parameters.Add("@Available", SqlDbType.Int);

                Adaptor.InsertCommand.Parameters["@Name"].Value = Book.Name;
                Adaptor.InsertCommand.Parameters["@Available"].Value = Book.BookAvailability;

                Adaptor.InsertCommand.ExecuteNonQuery();

                Book.ID = GetBook(Book.Name).ID;

                Conn.Close();
            }            

            foreach (var Author in Autors)
            {
                //Author must have ID != NULL
                AddBookAuthor(Book, Author);
            }
        }

        public static bool IfExistBook(Book Book)
        {
            using (SqlConnection Conn = new SqlConnection(GetConnectionString()))
            {
                SqlDataAdapter Adaptor = new SqlDataAdapter();

                DataSet ds = new DataSet();

                string CommandText = "select * from dbo.Books where Name = @Name";

                Adaptor.SelectCommand = new SqlCommand(CommandText, Conn);

                Adaptor.SelectCommand.Parameters.Clear();

                Adaptor.SelectCommand.Parameters.Add("@Name", SqlDbType.NVarChar);
                Adaptor.SelectCommand.Parameters["@Name"].Value = Book.Name;


                Adaptor.Fill(ds);

                DataTable Books = ds.Tables[0];

                var query = Books.AsEnumerable().Select(book => new { Name = book.Field<string>("Name") });

                query.Where(cond => cond.Name == Book.Name);

                if (query.Count() > 0)
                {
                    throw new DataBaseAlreadyHasElementException<Book>(Book);
                }

            }

            return false;
        }

        public static Book GetBook(int ID)
        {
            using (SqlConnection Conn = new SqlConnection(GetConnectionString()))
            {
                SqlDataAdapter Adaptor = new SqlDataAdapter();

                DataSet ds = new DataSet();

                string CommandText = "select * from dbo.Books where ID = @ID";

                Adaptor.SelectCommand = new SqlCommand(CommandText, Conn);

                Adaptor.SelectCommand.Parameters.Clear();

                Adaptor.SelectCommand.Parameters.Add("@ID", SqlDbType.Int);
                Adaptor.SelectCommand.Parameters["@ID"].Value = ID;

                Adaptor.Fill(ds);

                var empList = ds.Tables[0].AsEnumerable()
                    .Select(dataRow => new Book
                    {
                        Name = dataRow.Field<string>("Name"),
                        BookAvailability = (Book.Availability)dataRow.Field<int>("Available"),
                        ID = dataRow.Field<int>("ID"),
                        Autors = GetBookAuthors(ID)                        
                    })
                    .ToList();

                if (empList.Count > 0)
                {
                    return empList[0];
                }

                Conn.Close();
            }

            return null;
        }

        public static Book GetBook(string Name)
        {
            using (SqlConnection Conn = new SqlConnection(GetConnectionString()))
            {
                SqlDataAdapter Adaptor = new SqlDataAdapter();

                DataSet ds = new DataSet();

                string CommandText = "select * from dbo.Books where Name = @Name";

                Adaptor.SelectCommand = new SqlCommand(CommandText, Conn);

                Adaptor.SelectCommand.Parameters.Clear();

                Adaptor.SelectCommand.Parameters.Add("@Name", SqlDbType.NVarChar);
                Adaptor.SelectCommand.Parameters["@Name"].Value = Name;

                Adaptor.Fill(ds);

                var empList = ds.Tables[0].AsEnumerable()
                    .Select(dataRow => new Book
                    {
                        Name = dataRow.Field<string>("Name"),
                        BookAvailability = (Book.Availability)dataRow.Field<int>("Available"),
                        ID = dataRow.Field<int>("ID"),
                        Autors = GetBookAuthors(dataRow.Field<int>("ID"))
                    })
                    .ToList();

                if (empList.Count > 0)
                {
                    return empList[0];
                }

                Conn.Close();
            }

            return null;
        }

        public static List<Author> GetBookAuthors(int Book_ID)
        {
            using (SqlConnection Conn = new SqlConnection(GetConnectionString()))
            {
                SqlDataAdapter Adaptor = new SqlDataAdapter();

                DataSet ds = new DataSet();

                string CommandText = "select * from Authors as au where au.id in (select Author_ID from Book_Authors where Book_ID = @Book_ID)";

                Adaptor.SelectCommand = new SqlCommand(CommandText, Conn);

                Adaptor.SelectCommand.Parameters.Clear();

                Adaptor.SelectCommand.Parameters.Add("@Book_ID", SqlDbType.Int);
                Adaptor.SelectCommand.Parameters["@Book_ID"].Value = Book_ID;

                Adaptor.Fill(ds);

                var empList = ds.Tables[0].AsEnumerable()
                    .Select(dataRow => new Author
                    {
                        FirstName = dataRow.Field<string>("FirstName"),
                        LastName = dataRow.Field<string>("LastName"),
                        Sex = dataRow.Field<User.MaleFemale>("Sex"),
                        BornDate = dataRow.Field<DateTime>("BornDate"),
                        DeathDate = dataRow.Field<DateTime?>("DeathDate"),
                        Age = dataRow.Field<int>("Age"),
                        ID = dataRow.Field<int>("ID")
                    })
                    .ToList();

                if (empList.Count > 0)
                {
                    return empList;
                }

                Conn.Close();
            }

            return null;
        }

        public static List<Book> GetBooksOfAthor(Author Author)
        {
            using (SqlConnection Conn = new SqlConnection(GetConnectionString()))
            {
                SqlDataAdapter Adaptor = new SqlDataAdapter();

                DataSet ds = new DataSet();

                string CommandText = "select distinct * from dbo.Books where ID in (select distinct Book_ID from Book_Authors Where Author_ID = @Author_ID)";

                Adaptor.SelectCommand = new SqlCommand(CommandText, Conn);

                Adaptor.SelectCommand.Parameters.Clear();

                Adaptor.SelectCommand.Parameters.Add("@Author_ID", SqlDbType.Int);
                Adaptor.SelectCommand.Parameters["@Author_ID"].Value = Author.ID;

                Adaptor.Fill(ds);

                var empList = ds.Tables[0].AsEnumerable()
                    .Select(dataRow => new Book
                    {
                        Name = dataRow.Field<string>("Name"),
                        BookAvailability = (Book.Availability)dataRow.Field<int>("Available"),
                        ID = dataRow.Field<int>("ID"),
                        Autors = GetBookAuthors(dataRow.Field<int>("ID"))
                    })
                    .ToList();

                if (empList.Count > 0)
                {
                    return empList;
                }

                Conn.Close();
            }

            return null;
        }

        public static void AddBookAuthor(Book Book, Author Author)
        {
            bool Exist = false;

            try
            {
                Exist = IfExistBookAuthor(Book, Author);
            }
            catch (DataBaseAlreadyHasElementException<Book> e)
            {
                Exist = true;
            }

            if (Exist)
            {
                return;
            }

            using (SqlConnection Conn = new SqlConnection(GetConnectionString()))
            {
                Conn.Open();

                SqlDataAdapter Adaptor = new SqlDataAdapter();

                string CommandText = "INSERT INTO dbo.Book_Authors (Author_ID, Book_ID) VALUES (@Author_ID, @Book_ID)";

                Adaptor.InsertCommand = new SqlCommand(CommandText, Conn);
                Adaptor.InsertCommand.CommandType = CommandType.Text;

                Adaptor.InsertCommand.Parameters.Clear();

                Adaptor.InsertCommand.Parameters.Add("@Author_ID", SqlDbType.Int);
                Adaptor.InsertCommand.Parameters.Add("@Book_ID", SqlDbType.Int);

                Adaptor.InsertCommand.Parameters["@Author_ID"].Value = Author.ID;
                Adaptor.InsertCommand.Parameters["@Book_ID"].Value = Book.ID;

                Adaptor.InsertCommand.ExecuteNonQuery();

                Conn.Close();
            }
        }

        public static bool IfExistBookAuthor(Book Book, Author Author)
        {
            using (SqlConnection Conn = new SqlConnection(GetConnectionString()))
            {
                SqlDataAdapter Adaptor = new SqlDataAdapter();

                DataSet ds = new DataSet();

                string CommandText = "select * from dbo.Book_Authors where Author_ID = @Author_ID and Book_ID = @Book_ID";

                Adaptor.SelectCommand = new SqlCommand(CommandText, Conn);

                Adaptor.SelectCommand.Parameters.Clear();

                Adaptor.SelectCommand.Parameters.Add("@Author_ID", SqlDbType.Int);
                Adaptor.SelectCommand.Parameters["@Author_ID"].Value = Author.ID;

                Adaptor.SelectCommand.Parameters.Add("@Book_ID", SqlDbType.Int);
                Adaptor.SelectCommand.Parameters["@Book_ID"].Value = Book.ID;
                
                Adaptor.Fill(ds);

                DataTable Books = ds.Tables[0];

                var query = Books.AsEnumerable().Select(book => new
                {
                    Author_ID = book.Field<int>("Author_ID"),
                    Book_ID = book.Field<int>("Book_ID")
                });

                query.Where(cond => cond.Author_ID == Author.ID && cond.Book_ID == Book.ID);

                if (query.Count() > 0)
                {
                    throw new DataBaseAlreadyHasElementException<Book>(Book);
                }

            }

            return false;
        }

        public static void GiveBookToUser(Book BookForUser, User User)
        {
            if (BookForUser.BookAvailability == Book.Availability.Missing || BookForUser.BookAvailability == Book.Availability.Unavailable)
            {
                return;
            }

            using (SqlConnection Conn = new SqlConnection(GetConnectionString()))
            {
                Conn.Open();

                SqlDataAdapter Adaptor = new SqlDataAdapter();

                string CommandText = "update Books set Available = @Available, WhosTaken_ID = @WhosTaken_ID where ID = @Book_ID";

                Adaptor.UpdateCommand = new SqlCommand(CommandText, Conn);
                Adaptor.UpdateCommand.CommandType = CommandType.Text;

                Adaptor.UpdateCommand.Parameters.Clear();

                Adaptor.UpdateCommand.Parameters.Add("@Book_ID", SqlDbType.Int);
                Adaptor.UpdateCommand.Parameters["@Book_ID"].Value = BookForUser.ID;

                Adaptor.UpdateCommand.Parameters.Add("@Available", SqlDbType.Int);
                Adaptor.UpdateCommand.Parameters["@Available"].Value = 0;

                Adaptor.UpdateCommand.Parameters.Add("@WhosTaken_ID", SqlDbType.Int);
                Adaptor.UpdateCommand.Parameters["@WhosTaken_ID"].Value = User.ID;

                Adaptor.UpdateCommand.ExecuteNonQuery();                       

                Conn.Close();
            }
        }

        public static void takeBackBookFromUser(Book BookForUser, User User)
        {
            if (BookForUser.BookAvailability == Book.Availability.Missing || BookForUser.BookAvailability == Book.Availability.Available)
            {
                return;
            }

            using (SqlConnection Conn = new SqlConnection(GetConnectionString()))
            {
                Conn.Open();

                SqlDataAdapter Adaptor = new SqlDataAdapter();

                string CommandText = "update Books set Available = @Available, WhosTaken_ID = @WhosTaken_ID where ID = @Book_ID";

                Adaptor.UpdateCommand = new SqlCommand(CommandText, Conn);
                Adaptor.UpdateCommand.CommandType = CommandType.Text;

                Adaptor.UpdateCommand.Parameters.Clear();

                Adaptor.UpdateCommand.Parameters.Add("@Book_ID", SqlDbType.Int);
                Adaptor.UpdateCommand.Parameters["@Book_ID"].Value = BookForUser.ID;

                Adaptor.UpdateCommand.Parameters.Add("@Available", SqlDbType.Int);
                Adaptor.UpdateCommand.Parameters["@Available"].Value = 1;

                Adaptor.UpdateCommand.Parameters.Add("@WhosTaken_ID", SqlDbType.Int);
                Adaptor.UpdateCommand.Parameters["@WhosTaken_ID"].Value = null;

                Adaptor.UpdateCommand.ExecuteNonQuery();

                Conn.Close();
            }
        }
    }
}
