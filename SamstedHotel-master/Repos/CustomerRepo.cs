using Microsoft.Data.SqlClient;
using SamstedHotel.Model;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Windows;

namespace SamstedHotel.Repos
{
    public class CustomerRepo : IRepository<Customer>
    {
        private readonly string _connectionString;

        public CustomerRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Henter alle kunder
        public IEnumerable<Customer> GetAll()
        {
            var customers = new List<Customer>();
            string query = "SELECT * FROM Customers";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customers.Add(new Customer
                        {
                            CustomerID = (int)reader["CustomerID"],
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Email = reader["Email"].ToString(),
                            TLF = reader["TLF"].ToString()
                        });
                    }
                }
            }

            return customers;
        }

        // Henter en kunde baseret på ID
        public Customer GetById(int customerId)
        {
            Customer customer = null;
            string query = "SELECT * FROM Customers WHERE CustomerID = @CustomerID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CustomerID", customerId);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        customer = new Customer
                        {
                            CustomerID = (int)reader["CustomerID"],
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Email = reader["Email"].ToString(),
                            TLF = reader["TLF"].ToString()
                        };
                    }
                }
                return customer;
            }
      
        }

        // Tilføjer en ny kunde
        public void Add(Customer entity)
        {
            string query = @"INSERT INTO Customers (FirstName, LastName, Email, TLF) 
                    OUTPUT INSERTED.CustomerID 
                    VALUES (@FirstName, @LastName, @Email, @TLF)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FirstName", entity.FirstName);
                command.Parameters.AddWithValue("@LastName", entity.LastName);
                command.Parameters.AddWithValue("@Email", entity.Email);
                command.Parameters.AddWithValue("@TLF", entity.TLF);

                try
                {
                    connection.Open();
                    // Get the newly created CustomerID and assign it back to the entity
                    entity.CustomerID = (int)command.ExecuteScalar();
                }
                catch (SqlException ex)
                {
                    // Log the error or handle it appropriately
                    if (ex.Number == 2627) // Primary key violation error number
                    {
                        MessageBox.Show("En kunde med disse oplysninger findes allerede.", "Fejl ved oprettelse", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        MessageBox.Show("Der opstod en fejl ved oprettelse af kunden.", "Database Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    throw; // Re-throw the exception to be handled by the calling method
                }
            }
        }

        // Opdaterer en kunde
        public void Update(Customer entity)
        {
            string query = "UPDATE Customers SET FirstName = @FirstName, LastName = @LastName, Email = @Email, TLF = @TLF WHERE CustomerID = @CustomerID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CustomerID", entity.CustomerID);
                command.Parameters.AddWithValue("@FirstName", entity.FirstName);    
                command.Parameters.AddWithValue("@LastName", entity.LastName);
                command.Parameters.AddWithValue("@Email", entity.Email);
                command.Parameters.AddWithValue("@TLF", entity.TLF);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // Sletter en kunde
        public void Delete(Customer entity)
        {
            string query = "DELETE FROM Customers WHERE CustomerID = @CustomerID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CustomerID", entity.CustomerID);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

    }
}
