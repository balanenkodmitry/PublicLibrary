using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace PublicLibrary.Domain
{
    class UserRepository
    {
        public static string GetConnectionString()
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

    }
}
