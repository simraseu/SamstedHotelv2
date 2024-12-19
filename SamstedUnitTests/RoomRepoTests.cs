using Microsoft.VisualStudio.TestTools.UnitTesting;
using SamstedHotel.Model;
using SamstedHotel.Repos;
using System.Linq;
using Microsoft.Data.SqlClient;

namespace SamstedUnitTests
{
    [TestClass]
    public class RoomRepoTests : TestBase
    {
        private RoomRepo _roomRepo;
        private RoomTypeRepo _roomTypeRepo;

        [TestInitialize]
        public void Setup()
        {
            _roomRepo = new RoomRepo(_connectionString);
            _roomTypeRepo = new RoomTypeRepo(_connectionString);
            CleanRoomTables(); // Specifik oprydning for room-relaterede tabeller
        }

        private void CleanRoomTables()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;

                    // Slet først data fra afhængige tabeller
                    command.CommandText = "DELETE FROM ReservationRoom";
                    command.ExecuteNonQuery();

                    command.CommandText = "DELETE FROM Rooms";
                    command.ExecuteNonQuery();

                    command.CommandText = "DELETE FROM RoomType";
                    command.ExecuteNonQuery();

                    // Nulstil identity counters hvis nødvendigt
                    command.CommandText = "DBCC CHECKIDENT ('Rooms', RESEED, 0)";
                    command.ExecuteNonQuery();

                    command.CommandText = "DBCC CHECKIDENT ('RoomType', RESEED, 0)";
                    command.ExecuteNonQuery();
                }
            }
        }

        [TestMethod]
        public void Add_ValidRoom_ShouldAddToDatabase()
        {
            // Arrange
            var roomType = new RoomType
            {
                Name = "TestType", // Brug unikke testværdier
                PricePerNight = 1000M,
                Capacity = 2
            };
            _roomTypeRepo.Add(roomType);
            var addedRoomType = _roomTypeRepo.GetAll().First();

            var room = new Room
            {
                RoomName = "Test101", // Brug unikke testværdier
                RoomTypeID = addedRoomType.RoomTypeID,
                Status = "Available"
            };

            // Act
            _roomRepo.Add(room);
            var result = _roomRepo.GetAll().FirstOrDefault();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Test101", result.RoomName);
            Assert.AreEqual(addedRoomType.RoomTypeID, result.RoomTypeID);
        }

        [TestMethod]
        public void GetRoomTypeName_ValidRoomTypeId_ShouldReturnCorrectName()
        {
            // Arrange
            var roomType = new RoomType
            {
                Name = "TestType2", // Brug unikke testværdier
                PricePerNight = 1500M,
                Capacity = 3
            };
            _roomTypeRepo.Add(roomType);
            var addedRoomType = _roomTypeRepo.GetAll().First();

            // Act
            var result = _roomRepo.GetRoomTypeName(addedRoomType.RoomTypeID);

            // Assert
            Assert.AreEqual("TestType2", result);
        }

        [TestMethod]
        public void Update_ExistingRoom_ShouldUpdateInDatabase()
        {
            // Arrange
            var roomType = new RoomType
            {
                Name = "TestType3", // Brug unikke testværdier
                PricePerNight = 1000M,
                Capacity = 2
            };
            _roomTypeRepo.Add(roomType);
            var addedRoomType = _roomTypeRepo.GetAll().First();

            var room = new Room
            {
                RoomName = "Test102", // Brug unikke testværdier
                RoomTypeID = addedRoomType.RoomTypeID,
                Status = "Available"
            };
            _roomRepo.Add(room);
            var addedRoom = _roomRepo.GetAll().First();

            // Act
            addedRoom.Status = "Occupied";
            _roomRepo.Update(addedRoom);
            var result = _roomRepo.GetById(addedRoom.RoomID);

            // Assert
            Assert.AreEqual("Occupied", result.Status);
        }
    }
}