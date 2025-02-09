using Første_SQL.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Første_SQL.ViewModels
{
    public class UserRepository
    {
        private readonly string ConnectionString;
        public List<User> Users { get; set; }

        public UserRepository()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            Users = new List<User>();

            ConnectionString = config.GetConnectionString("MyDBConnection");

        }

        public User UserLogin(string username, string password)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT Id, Username, Password FROM [User] WHERE Username = @Username AND Password = @Password", con))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                Id = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                Password = reader.GetString(2)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public void Add(User newUser)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("INSERT INTO [User] (Username, Password, IsAdmin) " + "VALUES(@Username,@Password,@IsAdmin)" + "SELECT @@IDENTITY", con))
                {
                    cmd.Parameters.Add("@Username", SqlDbType.NVarChar).Value = newUser.Username;
                    cmd.Parameters.Add("@Password", SqlDbType.NVarChar).Value = newUser.Password;
                    cmd.Parameters.Add("@IsAdmin", SqlDbType.Bit).Value = newUser.IsAdmin;
                    newUser.Id = Convert.ToInt32(cmd.ExecuteScalar());
                    Users.Add(newUser);
                }
            }
        }
        
        public void Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM [User] WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(User userToBeUpdated)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE [User] SET Username = @Username, Password = @Password, IsAdmin = @IsAdmin WHERE Id = @Id", con);
                cmd.Parameters.Add("@Username", SqlDbType.NVarChar).Value = userToBeUpdated.Username;
                cmd.Parameters.Add("@Password", SqlDbType.NVarChar).Value = userToBeUpdated.Password;
                cmd.Parameters.Add("@IsAdmin", SqlDbType.Bit).Value = userToBeUpdated.IsAdmin;
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = userToBeUpdated.Id;
                cmd.ExecuteNonQuery();
            }
        }

        public List<User> RetrieveAll()
        {
            List<User> users = new List<User>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT Id, Username, Password, IsAdmin FROM [User]", con);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        User user = new User()
                        {
                            Id = reader.GetInt32(0),
                            Username = reader.GetString(1),
                            Password = reader.GetString(2),
                            IsAdmin = reader.GetBoolean(3)
                        };
                        users.Add(user);
                    }
                }
                return users;
            }
        }
    }
}
