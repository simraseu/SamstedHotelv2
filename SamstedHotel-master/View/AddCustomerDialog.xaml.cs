using System.Windows;
using SamstedHotel.Model;
using SamstedHotel.Repos;
using Microsoft.Data.SqlClient;

namespace SamstedHotel.View
{
    public partial class AddCustomerDialog : Window
    {
        private readonly CustomerRepo _customerRepo;

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string TLF { get; private set; }

        public AddCustomerDialog(string connectionString)
        {
            InitializeComponent();
            _customerRepo = new CustomerRepo(connectionString);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            FirstName = FirstNameTextBox.Text;
            LastName = LastNameTextBox.Text;
            Email = EmailTextBox.Text;
            TLF = TLFTextBox.Text;

            if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName) ||
                string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(TLF))
            {
                MessageBox.Show("Alle felter skal være udfyldt.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var newCustomer = new Customer
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Email = Email,
                    TLF = TLF
                };

                _customerRepo.Add(newCustomer);
                DialogResult = true;
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Der opstod en database fejl: {ex.Message}", "Database Fejl",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                DialogResult = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Der opstod en fejl: {ex.Message}", "Fejl",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                DialogResult = false;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}