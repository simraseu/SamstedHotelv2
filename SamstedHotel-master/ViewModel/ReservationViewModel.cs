using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using SamstedHotel.Model;
using SamstedHotel.Repos;
using SamstedHotel.View;

namespace SamstedHotel.ViewModel
{
    public class ReservationViewModel : BaseViewModel
    {
        private readonly string _connectionString;
        private readonly ReservationRepo _reservationRepo;
        private readonly CourseRoomRepo _courseRoomRepo;
        private readonly RoomRepo _roomRepo;
        private readonly RoomTypeRepo _roomTypeRepo;
        private readonly CustomerRepo _customerRepo;

        private ObservableCollection<Reservation> _reservations;
        public ObservableCollection<Reservation> Reservations
        {
            get => _reservations;
            set
            {
                _reservations = value;
                OnPropertyChanged(nameof(Reservations));
            }
        }

        private Reservation _selectedReservation;
        public Reservation SelectedReservation
        {
            get => _selectedReservation;
            set { _selectedReservation = value; OnPropertyChanged(); }
        }

        // Ny property til at holde valgt kunde
        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set { _selectedCustomer = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Customer> _customers;
        public ObservableCollection <Customer> Customers
        {
            get => _customers;
            set
            {
                _customers = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<SelectableItem<Room>> _availableRooms;
        public ObservableCollection<SelectableItem<Room>> AvailableRooms
        {
            get => _availableRooms;
            set { _availableRooms = value; OnPropertyChanged(); }
        }

        private ObservableCollection<SelectableItem<CourseRoom>> _availableCourseRooms;
        public ObservableCollection<SelectableItem<CourseRoom>> AvailableCourseRooms
        {
            get => _availableCourseRooms;
            set { _availableCourseRooms = value; OnPropertyChanged(); }
        }

        private DateTime _startDate;
        private DateTime _endDate;

        // Constructor
        public ReservationViewModel(string connectionString)
        {
            _connectionString = connectionString;
            _courseRoomRepo = new CourseRoomRepo(_connectionString);
            _roomRepo = new RoomRepo(_connectionString);
            _roomTypeRepo = new RoomTypeRepo(_connectionString);
            _reservationRepo = new ReservationRepo(_connectionString);
            _customerRepo = new CustomerRepo(_connectionString);
           
            _selectedCustomer = null; 
            _startDate = DateTime.Now;
            _endDate = DateTime.Now.AddDays(1);
            _reservations = new ObservableCollection<Reservation>();

            LoadCustomers();
            LoadReservations();
            LoadRoomsAndCourseRooms();

            // Initialize commands
            AddCustomerCommand = new RelayCommand(AddCustomer);
            BookReservationCommand = new RelayCommand(BookReservation);
            DeleteReservationCommand = new RelayCommand(DeleteReservation);
            SaveToCsvCommand = new RelayCommand(SaveReservationsToCsv);
        }

        // Properties for Start and End Date
        public DateTime StartDate
        {
            get => _startDate;
            set { _startDate = value; OnPropertyChanged(); }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set { _endDate = value; OnPropertyChanged(); }
        }

        // Commands for different reservation actions
        public ICommand BookReservationCommand { get; }
        public ICommand DeleteReservationCommand { get; }
        public ICommand SaveToCsvCommand { get; }
        public ICommand AddCustomerCommand { get; }

        // Method to load reservations from the repository
        private void LoadReservations()
        {
            var reservations = _reservationRepo.GetAllReservations();
            Reservations = new ObservableCollection<Reservation>(reservations);
        }

        private void LoadCustomers()
        {
            var customers = _customerRepo.GetAll(); // Assuming GetAllCustomers method fetches all customers
            Customers = new ObservableCollection<Customer>(customers);
        }

        // Load available rooms and course rooms based on the selected dates
        private void LoadRoomsAndCourseRooms()
        {
            // Fetch rooms and their associated room type name
            var rooms = _roomRepo.GetAll()
                .Where(room => _roomRepo.IsRoomAvailable(room.RoomID, StartDate, EndDate))
                .Select(room => new SelectableItem<Room>(new Room
                {
                    RoomID = room.RoomID,
                    RoomName = room.RoomName,
                    RoomTypeID = room.RoomTypeID,
                    Status = room.Status,
                    RoomTypeName = _roomRepo.GetRoomTypeName(room.RoomTypeID) // Fetch room type name using RoomTypeID
                }))
                .ToList();

            // Fetch course rooms (no modification needed, as it's already set up)
            var courseRooms = _courseRoomRepo.GetAll()
                .Where(courseRoom => _courseRoomRepo.IsCourseRoomAvailable(courseRoom.CourseRoomID, StartDate, EndDate))
                .Select(courseRoom => new SelectableItem<CourseRoom>(courseRoom))
                .ToList();

            AvailableRooms = new ObservableCollection<SelectableItem<Room>>(rooms);
            AvailableCourseRooms = new ObservableCollection<SelectableItem<CourseRoom>>(courseRooms);
        }

        // New method to select a customer from the list
        public void SelectCustomer(Customer customer)
        {
            SelectedCustomer = customer;
        }

        // Method to open the AddCustomerDialog and add a customer to the database
        private void AddCustomer()
        {
            try
            {
                var addCustomerDialog = new AddCustomerDialog(_connectionString);

                if (addCustomerDialog.ShowDialog() == true)
                {
                    LoadCustomers();  // Genindlæs kundelisten efter vellykket tilføjelse
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Der opstod en fejl under oprettelsen af kunde: {ex.Message}",
                               "Fejl",
                               MessageBoxButton.OK,
                               MessageBoxImage.Error);
            }
        }


        // Method to handle booking a reservation
        public void BookReservation()
        {
            if (SelectedCustomer == null)
            {
                MessageBox.Show("Vælg en kunde først.");
                return;
            }

            var selectedRooms = AvailableRooms.Where(r => r.IsSelected).Select(r => r.Item).ToList();
            var selectedCourseRooms = AvailableCourseRooms.Where(cr => cr.IsSelected).Select(cr => cr.Item).ToList();

            if (!selectedRooms.Any() && !selectedCourseRooms.Any())
            {
                MessageBox.Show("Vælg mindst et værelse eller kursuslokale.");
                return;
            }

            decimal totalAmount = 0;
            foreach (var room in selectedRooms)
            {
                // Now use _roomTypeRepo to get the room type price
                decimal roomTypePrice = _roomTypeRepo.GetRoomTypePriceByName(room.RoomTypeName); // Correct repository usage

                totalAmount += roomTypePrice * (EndDate - StartDate).Days;
            }

            foreach (var courseRoom in selectedCourseRooms)
            {
                totalAmount += courseRoom.TotalPrice * (EndDate - StartDate).Days;
            }

            if (totalAmount <= 0)
            {
                MessageBox.Show("Fejl i beregning af beløbet.");
                return;
            }

            var reservation = new Reservation
            {
                CustomerID = SelectedCustomer.CustomerID,
                StartDate = StartDate,
                EndDate = EndDate,
                TotalAmount = totalAmount,
                Status = "Booked",
                // Modify BookingType to use RoomTypeName (instead of RoomType.Name)
                BookingType = string.Join(", ", selectedRooms.Select(r => r.RoomTypeName).Concat(selectedCourseRooms.Select(cr => cr.CourseRoomName)))
            };

            _reservationRepo.AddReservation(reservation);
            LoadReservations();
            MessageBox.Show("Reservation oprettet.");
        }





        // Method to handle canceling a reservation
        private void DeleteReservation()
        {
            if (SelectedReservation == null)
            {
                MessageBox.Show("Vælg en reservation at slette.");
                return;
            }

            _reservationRepo.DeleteReservation(SelectedReservation.ReservationID);
            LoadReservations();
            MessageBox.Show("Reservation slettet.");
        }

        // Method to save reservations to a CSV file
        private void SaveReservationsToCsv()
        {
            try
            {
                var reservations = Reservations.Select(r => new
                {
                    r.ReservationID,
                    r.CustomerID,
                    r.StartDate,
                    r.EndDate,
                    r.TotalAmount,
                    r.Status
                });

                // Placering af filen (du kan evt. vise en filvælger dialog)
                string filePath = "reservations.csv";

                // Opret filens indhold
                var sb = new StringBuilder();
                sb.AppendLine("ReservationID,CustomerID,StartDate,EndDate,TotalAmount,Status");

                foreach (var reservation in reservations)
                {
                    sb.AppendLine($"{reservation.ReservationID},{reservation.CustomerID},{reservation.StartDate:yyyy-MM-dd},{reservation.EndDate:yyyy-MM-dd},{reservation.TotalAmount},{reservation.Status}");
                }

                // Skriv filen
                System.IO.File.WriteAllText(filePath, sb.ToString());

                // Vis en besked om succes
                MessageBox.Show($"Reservationer blev gemt til filen:\n{filePath}", "Gem Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                // Vis en besked om fejl
                MessageBox.Show($"Der opstod en fejl ved gemning af reservationer:\n{ex.Message}", "Gem Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Method to refresh available course rooms based on the selected dates
        private void RefreshAvailableCourseRooms()
        {
            if (StartDate == default || EndDate == default) return;

            var courseRooms = _courseRoomRepo.GetAll()
                .Where(courseRoom => _courseRoomRepo.IsCourseRoomAvailable(courseRoom.CourseRoomID, StartDate, EndDate))
                .Select(courseRoom => new SelectableItem<CourseRoom>(courseRoom))
                .ToList();

            AvailableCourseRooms = new ObservableCollection<SelectableItem<CourseRoom>>(courseRooms);
        }
    }
}
