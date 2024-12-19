using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Data.SqlClient;

namespace SamstedUnitTests
{
    public class TestBase
    {
        protected readonly string _connectionString;

        public TestBase()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _connectionString = config.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found in appsettings.json");
            }
        }

        protected void CleanDatabase()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        DELETE FROM ReservationRoom;
                        DELETE FROM ReservationCourseRoom;
                        DELETE FROM Reservations;
                        DELETE FROM Customers;
                        DBCC CHECKIDENT ('Customers', RESEED, 0);
                        DBCC CHECKIDENT ('Reservations', RESEED, 0);";
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}