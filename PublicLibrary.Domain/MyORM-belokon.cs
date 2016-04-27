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
            const string ConnString = "Server=192.168.10.220;Database=ASP_NET_Test;Uid=sa;Pwd=Undergr0und;";

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

                DataTable Users = ds.Tables[0];

                var empList = ds.Tables[0].AsEnumerable()
                    .Select(dataRow => new User 
                    {
                        FirstName = dataRow.Field<string>("FirstName"),
                        LastName = dataRow.Field<string>("LastName"),
                        Sex = (Body.MaleFemale)dataRow.Field<int>("Sex"),
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

                //DataTableReader ResultData = ds.CreateDataReader();
                                            

                //if (ResultData.HasRows)
                //{                  

                //    return new User(
                //        (string)ResultData.GetValue((Users.Columns["FirstName"]).Ordinal),
                //        (string)ResultData.GetValue((Users.Columns["LastName"]).Ordinal),
                //        (User.MaleFemale)ResultData.GetValue((Users.Columns["Sex"]).Ordinal),
                //        (DateTime)ResultData.GetValue((Users.Columns["BornDate"]).Ordinal),
                //        (string)ResultData.GetValue((Users.Columns["EMail"]).Ordinal), 
                //        (int)ResultData.GetValue((Users.Columns["Age"]).Ordinal), 
                //        (int)ResultData.GetValue((Users.Columns["ID"]).Ordinal));
                //}

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

                Conn.Close();
            }

            foreach (var Author in Autors)
            {

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

                Adaptor.InsertCommand.Parameters["@Author_ID"].Value = Book.ID;
                Adaptor.InsertCommand.Parameters["@Book_ID"].Value = Author.ID;

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
    }
}
