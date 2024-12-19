using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using SamstedHotel.Model;

namespace SamstedHotel.Repos
{
    public class ReservationRepo
    {
        private readonly string _connectionString;

        public ReservationRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Opret en reservation
        public void AddReservation(Reservation reservation)
        {
            string query = "INSERT INTO Reservations (CustomerID, StartDate, EndDate, TotalAmount, Status, BookingType) VALUES (@CustomerID, @StartDate, @EndDate, @TotalAmount, @Status, @BookingType)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CustomerID", reservation.CustomerID);
                command.Parameters.AddWithValue("@StartDate", reservation.StartDate);
                command.Parameters.AddWithValue("@EndDate", reservation.EndDate);
                command.Parameters.AddWithValue("@TotalAmount", reservation.TotalAmount);
                command.Parameters.AddWithValue("@Status", reservation.Status);
                command.Parameters.AddWithValue("@BookingType", reservation.BookingType);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // Hent alle reservationer
        public List<Reservation> GetAllReservations()
        {
            List<Reservation> reservations = new List<Reservation>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Reservations", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reservations.Add(new Reservation
                        {
                            ReservationID = (int)reader["ReservationID"],
                            CustomerID = (int)reader["CustomerID"],
                            StartDate = (DateTime)reader["StartDate"],
                            EndDate = (DateTime)reader["EndDate"],
                            BookingType = (string)reader["BookingType"],
                            TotalAmount = (decimal)reader["TotalAmount"],
                            Status = (string)reader["Status"]
                        });
                    }
                }
            }

            return reservations;
        }

        // Hent alle kursusrum
        public List<CourseRoom> GetAllCourseRooms()
        {
            var courseRooms = new List<CourseRoom>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT CourseRoomID, CourseRoomName FROM CourseRooms", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        courseRooms.Add(new CourseRoom
                        {
                            CourseRoomID = (int)reader["CourseRoomID"],
                            CourseRoomName = (string)reader["CourseRoomName"]
                        });
                    }
                }
            }

            return courseRooms;
        }

        // Hent alle værelser
        public List<Room> GetAllRooms()
        {
            var rooms = new List<Room>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT RoomID, RoomName FROM Rooms", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        rooms.Add(new Room
                        {
                            RoomID = (int)reader["RoomID"],
                            RoomName = (string)reader["RoomName"]
                        });
                    }
                }
            }

            return rooms;
        }

        // Opdater en reservation
        public void UpdateReservation(Reservation reservation)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("UPDATE Reservations SET StartDate = @StartDate, EndDate = @EndDate, TotalAmount = @TotalAmount, Status = @Status WHERE ReservationID = @ReservationID", connection);
                command.Parameters.AddWithValue("@StartDate", reservation.StartDate);
                command.Parameters.AddWithValue("@EndDate", reservation.EndDate);
                command.Parameters.AddWithValue("@TotalAmount", reservation.TotalAmount);
                command.Parameters.AddWithValue("@Status", reservation.Status);
                command.Parameters.AddWithValue("@ReservationID", reservation.ReservationID);

                command.ExecuteNonQuery();
            }
        }

        // Slet en reservation
        public void DeleteReservation(int reservationID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM Reservations WHERE ReservationID = @ReservationID", connection);
                command.Parameters.AddWithValue("@ReservationID", reservationID);

                command.ExecuteNonQuery();
            }
        }

        // Hent en reservation ved ID
        public Reservation GetReservationById(int reservationID)
        {
            Reservation reservation = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Reservations WHERE ReservationID = @ReservationID", connection);
                command.Parameters.AddWithValue("@ReservationID", reservationID);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        reservation = new Reservation
                        {
                            ReservationID = (int)reader["ReservationID"],
                            CustomerID = (int)reader["CustomerID"],
                            StartDate = (DateTime)reader["StartDate"],
                            EndDate = (DateTime)reader["EndDate"],
                            BookingType = (string)reader["BookingType"],
                            TotalAmount = (decimal)reader["TotalAmount"],
                            Status = (string)reader["Status"]
                        };
                    }
                }
            }

            return reservation;
        }



        
    }
}
