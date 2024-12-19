using System.Windows;
using SamstedHotel.Model;
using SamstedHotel.Repos;
using SamstedHotel.ViewModel;

namespace SamstedHotel.View
{
    public partial class ReservationView : Window
    {
        public ReservationView(string connectionString)
        {
            InitializeComponent();
            // Initialize the ViewModel and set it as the DataContext of the View
            this.DataContext = new ReservationViewModel(connectionString);
        }

        // Button click event handler for booking a reservation (optional)
        // You don't need this if you bind the command directly in the XAML
        private void BookReservationButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as ReservationViewModel;
            viewModel?.BookReservation();  // Ensure it's not null before calling
        }
    }
}
