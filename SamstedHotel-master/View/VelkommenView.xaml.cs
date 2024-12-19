    using System.Windows;
    using SamstedHotel.ViewModel;

    namespace SamstedHotel.View
    {
        public partial class VelkommenView : Window
        {
            // Parameterless constructor required for XAML instantiation
            public VelkommenView()
            {
                InitializeComponent();

                // Set a default ViewModel (if needed)
                DataContext = new ReservationViewModel(App.ConnectionString);
            }

            // Parameterized constructor for manual instantiation with connection string
            public VelkommenView(string connectionString) : this()
            {
                // Additional logic can go here if necessary
            }

            private void BookReservation_Click(object sender, RoutedEventArgs e)
            {
                // Open ReservationView using the centralized connection string from App
                var reservationView = new ReservationView(App.ConnectionString);
                reservationView.Show();
            }
        }
    }
