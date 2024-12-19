using Microsoft.Data.SqlClient;
using SamstedHotel.Model;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SamstedHotel.Repos
{
    public class RoomRepo : IRepository<Room>
    {
        private readonly string _connectionString;

        public RoomRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Henter alle værelser
        public IEnumerable<Room> GetAll()
        {
            var rooms = new List<Room>();
            string query = "SELECT * FROM Rooms";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        rooms.Add(new Room
                        {
                            RoomID = (int)reader["RoomID"],
                            RoomName = reader["RoomName"].ToString(),
                            RoomTypeID = (int)reader["RoomTypeID"],
                            Status = reader["Status"].ToString()
                        });
                    }
                }
            }

            return rooms;
        }

        // New method to get room type name
        public string GetRoomTypeName(int roomTypeID)
        {
            string roomTypeName = string.Empty;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT Name FROM RoomType WHERE RoomTypeID = @RoomTypeID";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoomTypeID", roomTypeID);

                    roomTypeName = (string)command.ExecuteScalar();
                }
            }

            return roomTypeName;
        }

        // Henter et værelse baseret på ID
        public Room GetById(int roomId)
        {
            Room room = null;
            string query = "SELECT * FROM Rooms WHERE RoomID = @RoomID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RoomID", roomId);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        room = new Room
                        {
                            RoomID = (int)reader["RoomID"],
                            RoomName = reader["RoomName"].ToString(),
                            RoomTypeID = (int)reader["RoomTypeID"],
                            Status = reader["Status"].ToString()
                        };
                    }
                }
            }

            return room;
        }

        // Tilføjer et nyt værelse
        public void Add(Room entity)
        {
            string query = "INSERT INTO Rooms (RoomName, RoomTypeID, Status) VALUES (@RoomName, @RoomTypeID, @Status)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RoomName", entity.RoomName);
                command.Parameters.AddWithValue("@RoomTypeID", entity.RoomTypeID);
                command.Parameters.AddWithValue("@Status", entity.Status);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // Opdaterer et værelse
        public void Update(Room entity)
        {
            string query = "UPDATE Rooms SET RoomName = @RoomName, RoomTypeID = @RoomTypeID, Status = @Status WHERE RoomID = @RoomID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RoomID", entity.RoomID);
                command.Parameters.AddWithValue("@RoomName", entity.RoomName);
                command.Parameters.AddWithValue("@RoomTypeID", entity.RoomTypeID);
                command.Parameters.AddWithValue("@Status", entity.Status);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // Sletter et værelse
        public void Delete(Room entity)
        {
            string query = "DELETE FROM Rooms WHERE RoomID = @RoomID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RoomID", entity.RoomID);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // Tjek om et værelse er ledigt i den ønskede periode
        public bool IsRoomAvailable(int roomID, DateTime startDate, DateTime endDate)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Find alle reservationer for det valgte værelse
                var command = new SqlCommand(@"
            SELECT r.StartDate, r.EndDate, r.Status
            FROM Reservations r
            INNER JOIN ReservationRoom rr ON rr.ReservationID = r.ReservationID
            WHERE rr.RoomID = @RoomID AND r.Status != 'Cancelled'", connection);

                command.Parameters.AddWithValue("@RoomID", roomID);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var reservationStartDate = (DateTime)reader["StartDate"];
                        var reservationEndDate = (DateTime)reader["EndDate"];
                        var status = (string)reader["Status"];

                        // Tjek for overlap med eksisterende reservation og om status er "Booked"
                        if (status == "Booked" && startDate < reservationEndDate && endDate > reservationStartDate)
                        {
                            return false; // Værelset er allerede booket i den valgte periode
                        }
                    }
                }
            }

            return true; // Værelset er ledigt
        }
    }
}
