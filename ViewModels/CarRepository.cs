using Første_SQL.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Første_SQL.ViewModels
{
    public class CarRepository
    {
        private readonly string ConnectionString;
        private List<Car> cars;
        public CarRepository()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            cars = new List<Car>();

            ConnectionString = config.GetConnectionString("MyDBConnection");
        }

        public void Search(string carName)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                if (string.IsNullOrWhiteSpace(carName))
                {
                    cars = RetrieveAll();
                }
                else
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Car WHERE Make LIKE @Make", con))
                    {
                        cmd.Parameters.AddWithValue("@Make", "%" + carName + "%");

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            cars.Clear();
                            while (reader.Read())
                            {
                                cars.Add(new Car
                                {
                                    Id = reader.GetInt32(0),
                                    Make = reader.GetString(1),
                                    Model = reader.GetString(2),
                                    Year = reader.GetInt32(3),
                                    Description = reader.GetString(4)
                                });
                            }
                        }
                    }
                }
            }
        }
        public void Create(Car carToBeCreated)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Car (Make, Model, Year, Description) " + "VALUES(@Make,@Model,@Year,@Description)" + "SELECT @@IDENTITY", con))
                {
                    cmd.Parameters.Add("@Make", SqlDbType.NVarChar).Value = carToBeCreated.Make;
                    cmd.Parameters.Add("@Model", SqlDbType.NVarChar).Value = carToBeCreated.Model;
                    cmd.Parameters.Add("@Year", SqlDbType.Int).Value = carToBeCreated.Year;
                    cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = carToBeCreated.Description;
                    carToBeCreated.Id = Convert.ToInt32(cmd.ExecuteScalar());
                    cars.Add(carToBeCreated);
                }
            }
        }
        public Car? GetById(int id)
        {
            Car? car = null;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT Id, Make, Model, Year, Description FROM Car WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        car = new Car
                        {
                            Id = dr.GetInt32(0),
                            Make = dr.GetString(1),
                            Model = dr.GetString(2),
                            Year = dr.GetInt32(3),
                            Description = dr.GetString(4)
                        };
                    }
                }
            }
            return car;
        }

        public List<Car> RetrieveAll()
        {
            List<Car> cars = new List<Car>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open(); SqlCommand cmd = new SqlCommand("SELECT Id, Make, Model, Year, Description FROM Car", con);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Car car = new Car
                        {
                            Id = dr.GetInt32(0),
                            Make = dr.GetString(1),
                            Model = dr.GetString(2),
                            Year = dr.GetInt32(3),
                            Description = dr.GetString(4)
                        };
                        cars.Add(car);
                    }
                }
            }
            return cars;
        }

        public List<Car> GetCars() //Gør det muligt for os at modtage listen af biler som er filtreret. Vores Retrieveall() får all biler fra SQL så derfor kan vi ikke bruge den.
        {
            return cars;
        }
        public void Update(Car carToBeUpdated)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Car SET Make = @Make, Model = @Model, Year = @Year, Description = @Description WHERE Id = @Id", con);
                cmd.Parameters.Add("@Make", SqlDbType.NVarChar).Value = carToBeUpdated.Make;
                cmd.Parameters.Add("@Model", SqlDbType.NVarChar).Value = carToBeUpdated.Model;
                cmd.Parameters.Add("@Year", SqlDbType.Int).Value = carToBeUpdated.Year;
                cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = carToBeUpdated.Description;
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = carToBeUpdated.Id;
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int? id)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Car WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}