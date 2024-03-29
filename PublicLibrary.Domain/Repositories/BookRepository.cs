﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace PublicLibrary.Domain
{
    public class BookRepository
    {
        public static string GetConnectionString()
        {
            const string ConnString = "Server=192.168.10.220;Database=_TEST_ASP_NET_Test;Uid=sa;Pwd=Undergr0und;";

            return ConnString;
        }

        public Book GetBook(int ID)
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

        public Book GetBook(string Name)
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

        public IEnumerable<Book> GetBooks()
        {
            using (SqlConnection Conn = new SqlConnection(GetConnectionString()))
            {
                SqlDataAdapter Adaptor = new SqlDataAdapter();

                DataSet ds = new DataSet();

                string CommandText = "select * from dbo.Books";

                Adaptor.SelectCommand = new SqlCommand(CommandText, Conn);        

                Adaptor.Fill(ds);

                var empList = ds.Tables[0].AsEnumerable()
                    .Select(dataRow => new Book
                    {
                        Name = dataRow.Field<string>("Name"),
                        BookAvailability = (Book.Availability)dataRow.Field<int>("Available"),
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

        public List<Author> GetBookAuthors(int Book_ID)
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

        public List<Book> GetBooksOfAthor(Author Author)
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

        public void AddBookAuthor(Book Book, Author Author)
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

        public bool IfExistBookAuthor(Book Book, Author Author)
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

        public void GiveBookToUser(Book BookForUser, User User)
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

        public void TakeBackBookFromUser(Book BookForUser, User User)
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

        public void AddBook(Book Book)
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
                                    
        }

        public void AddBook(Book Book, List<Author> Autors)
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

        public bool IfExistBook(Book Book)
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

        public void EditBook(Book book)
        {
            using (SqlConnection Conn = new SqlConnection(GetConnectionString()))
            {
                Conn.Open();

                SqlDataAdapter Adaptor = new SqlDataAdapter();

                string CommandText = " UPDATE dbo.Books SET Name = @Name, Available = @Available, WhosTaken_ID = @WhosTaken_ID WHERE ID = @ID";

                Adaptor.UpdateCommand = new SqlCommand(CommandText, Conn);
                Adaptor.UpdateCommand.CommandType = CommandType.Text;

                Adaptor.UpdateCommand.Parameters.Clear();

                Adaptor.UpdateCommand.Parameters.Add("@Name", SqlDbType.NVarChar);
                Adaptor.UpdateCommand.Parameters["@Name"].Value = book.Name;

                Adaptor.UpdateCommand.Parameters.Add("@Available", SqlDbType.Int);                
                Adaptor.UpdateCommand.Parameters["@Available"].Value = book.BookAvailability;

                Adaptor.UpdateCommand.Parameters.Add("@WhosTaken_ID", SqlDbType.Int);
                Adaptor.UpdateCommand.Parameters["@WhosTaken_ID"].Value = book.WhosTakenUser;

                Adaptor.UpdateCommand.Parameters.Add("@ID", SqlDbType.Int);
                Adaptor.UpdateCommand.Parameters["@ID"].Value = book.ID;

                Adaptor.UpdateCommand.ExecuteNonQuery();

                Conn.Close();
            }
        }

        public void DeleteBook(int ID)
        {
            using (SqlConnection Conn = new SqlConnection(GetConnectionString()))
            {
                Conn.Open();

                SqlDataAdapter Adaptor = new SqlDataAdapter();

                string CommandText = "DELETE dbo.Books WHERE ID = @ID";

                Adaptor.DeleteCommand = new SqlCommand(CommandText, Conn);
                Adaptor.DeleteCommand.CommandType = CommandType.Text;

                Adaptor.DeleteCommand.Parameters.Clear();

                Adaptor.DeleteCommand.Parameters.Add("@ID", SqlDbType.Int);
                Adaptor.DeleteCommand.Parameters["@ID"].Value = ID;

                Adaptor.DeleteCommand.ExecuteNonQuery();

                Conn.Close();
            }
        }

    }

}
