using Microsoft.Data.SqlClient;
using SamstedHotel.Model;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SamstedHotel.Repos;

namespace SamstedHotel.Repos
{
    public class RoomTypeRepo : IRepository<RoomType>
    {
        private readonly string _connectionString;

        public RoomTypeRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<RoomType> GetAll()
        {
            var roomTypes = new List<RoomType>();
            string query = "SELECT * FROM RoomType";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        roomTypes.Add(new RoomType
                        {
                            RoomTypeID = (int)reader["RoomTypeID"],
                            Name = reader["Name"].ToString(),
                            PricePerNight = (decimal)reader["PricePerNight"],
                            Capacity = (int)reader["Capacity"]
                        });
                    }
                }
            }

            return roomTypes;
        }

        public RoomType GetById(int id)
        {
            RoomType roomType = null;
            string query = "SELECT * FROM RoomType WHERE RoomTypeID = @RoomTypeID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RoomTypeID", id);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        roomType = new RoomType
                        {
                            RoomTypeID = (int)reader["RoomTypeID"],
                            Name = reader["Name"].ToString(),
                            PricePerNight = (decimal)reader["PricePerNight"],
                            Capacity = (int)reader["Capacity"]
                        };
                    }
                }
            }

            return roomType;
        }

        public void Add(RoomType entity)
        {
            string query = "INSERT INTO RoomType (Name, PricePerNight, Capacity) VALUES (@Name, @PricePerNight, @Capacity)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", entity.Name);
                command.Parameters.AddWithValue("@PricePerNight", entity.PricePerNight);
                command.Parameters.AddWithValue("@Capacity", entity.Capacity);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Update(RoomType entity)
        {
            string query = "UPDATE RoomType SET Name = @Name, PricePerNight = @PricePerNight, Capacity = @Capacity WHERE RoomTypeID = @RoomTypeID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RoomTypeID", entity.RoomTypeID);
                command.Parameters.AddWithValue("@Name", entity.Name);
                command.Parameters.AddWithValue("@PricePerNight", entity.PricePerNight);
                command.Parameters.AddWithValue("@Capacity", entity.Capacity);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(RoomType entity)
        {
            string query = "DELETE FROM RoomType WHERE RoomTypeID = @RoomTypeID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RoomTypeID", entity.RoomTypeID);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public decimal GetRoomTypePriceByName(string roomTypeName)
        {
            decimal price = 0m;
            string query = "SELECT PricePerNight FROM RoomType WHERE Name = @Name";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", roomTypeName);
                connection.Open();

                object result = command.ExecuteScalar(); // Use ExecuteScalar to get a single value (PricePerNight)

                if (result != null && result != DBNull.Value)
                {
                    price = Convert.ToDecimal(result);
                }
            }

            return price;
        }

    }
}
