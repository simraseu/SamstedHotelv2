using Microsoft.VisualStudio.TestTools.UnitTesting;
using SamstedHotel.Model;
using SamstedHotel.Repos;
using Microsoft.Data.SqlClient;
using System;
using System.Linq;

namespace SamstedUnitTests
{
    [TestClass]
    public class CourseRoomRepoTests : TestBase
    {
        private CourseRoomRepo _courseRoomRepo;

        [TestInitialize]
        public void Setup()
        {
            _courseRoomRepo = new CourseRoomRepo(_connectionString);
            CleanCourseRoomTables();
        }

        private void CleanCourseRoomTables()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;

                    // Slet først data fra afhængige tabeller
                    command.CommandText = "DELETE FROM ReservationCourseRoom";
                    command.ExecuteNonQuery();

                    command.CommandText = "DELETE FROM CourseRooms";
                    command.ExecuteNonQuery();

                    // Nulstil identity counter
                    command.CommandText = "DBCC CHECKIDENT ('CourseRooms', RESEED, 0)";
                    command.ExecuteNonQuery();
                }
            }
        }

        [TestMethod]
        public void Add_ValidCourseRoom_ShouldAddToDatabase()
        {
            // Arrange
            var courseRoom = new CourseRoom
            {
                CourseRoomName = "TestConference", // Brug unikke testværdier
                EventPackage = "Test Full Day Package",
                TotalPrice = 5000M
            };

            // Act
            _courseRoomRepo.Add(courseRoom);
            var result = _courseRoomRepo.GetAll().FirstOrDefault();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("TestConference", result.CourseRoomName);
            Assert.AreEqual("Test Full Day Package", result.EventPackage);
            Assert.AreEqual(5000M, result.TotalPrice);
        }

        [TestMethod]
        public void GetById_ExistingCourseRoom_ShouldReturnCourseRoom()
        {
            // Arrange
            var courseRoom = new CourseRoom
            {
                CourseRoomName = "TestConference2", // Brug unikke testværdier
                EventPackage = "Test Half Day Package",
                TotalPrice = 3000M
            };
            _courseRoomRepo.Add(courseRoom);
            var addedCourseRoom = _courseRoomRepo.GetAll().First();

            // Act
            var result = _courseRoomRepo.GetById(addedCourseRoom.CourseRoomID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("TestConference2", result.CourseRoomName);
            Assert.AreEqual("Test Half Day Package", result.EventPackage);
            Assert.AreEqual(3000M, result.TotalPrice);
        }

        [TestMethod]
        public void IsCourseRoomAvailable_NonOverlappingDates_ShouldReturnTrue()
        {
            // Arrange
            var courseRoom = new CourseRoom
            {
                CourseRoomName = "TestConference3", // Brug unikke testværdier
                EventPackage = "Test Full Day Package",
                TotalPrice = 5000M
            };
            _courseRoomRepo.Add(courseRoom);
            var addedCourseRoom = _courseRoomRepo.GetAll().First();

            var startDate = DateTime.Now.AddDays(1).Date;
            var endDate = startDate.AddDays(1);

            // Act
            var result = _courseRoomRepo.IsCourseRoomAvailable(addedCourseRoom.CourseRoomID, startDate, endDate);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GetAll_ShouldReturnAllCourseRooms()
        {
            // Arrange
            var courseRoom1 = new CourseRoom
            {
                CourseRoomName = "TestConference4",
                EventPackage = "Test Package 1",
                TotalPrice = 4000M
            };
            var courseRoom2 = new CourseRoom
            {
                CourseRoomName = "TestConference5",
                EventPackage = "Test Package 2",
                TotalPrice = 6000M
            };

            _courseRoomRepo.Add(courseRoom1);
            _courseRoomRepo.Add(courseRoom2);

            // Act
            var result = _courseRoomRepo.GetAll().ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(r => r.CourseRoomName == "TestConference4"));
            Assert.IsTrue(result.Any(r => r.CourseRoomName == "TestConference5"));
        }
    }
}