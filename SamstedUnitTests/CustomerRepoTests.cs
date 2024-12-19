using Microsoft.VisualStudio.TestTools.UnitTesting;
using SamstedHotel.Model;
using SamstedHotel.Repos;
using System.Linq;

namespace SamstedUnitTests
{
    [TestClass]
    public class CustomerRepoTests : TestBase
    {
        private CustomerRepo _customerRepo;

        [TestInitialize]
        public void Setup()
        {
            _customerRepo = new CustomerRepo(_connectionString);
            CleanDatabase(); // Ryd test-data før hver test
        }

        [TestMethod]
        public void Add_ValidCustomer_ShouldAddToDatabase()
        {
            // Arrange
            var customer = new Customer
            {
                FirstName = "Test",
                LastName = "Testesen",
                Email = "test@test.dk",
                TLF = "12345678"
            };

            // Act
            _customerRepo.Add(customer);
            var result = _customerRepo.GetAll().FirstOrDefault(c =>
                c.FirstName == customer.FirstName &&
                c.LastName == customer.LastName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(customer.Email, result.Email);
            Assert.AreEqual(customer.TLF, result.TLF);
        }

        [TestMethod]
        public void GetById_ExistingCustomer_ShouldReturnCustomer()
        {
            // Arrange
            var customer = new Customer
            {
                FirstName = "Test",
                LastName = "Testesen",
                Email = "test@test.dk",
                TLF = "12345678"
            };
            _customerRepo.Add(customer);
            var addedCustomer = _customerRepo.GetAll().First(c =>
                c.FirstName == customer.FirstName &&
                c.LastName == customer.LastName);

            // Act
            var result = _customerRepo.GetById(addedCustomer.CustomerID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(customer.FirstName, result.FirstName);
            Assert.AreEqual(customer.LastName, result.LastName);
        }

        [TestMethod]
        public void Delete_ExistingCustomer_ShouldRemoveFromDatabase()
        {
            // Arrange
            var customer = new Customer
            {
                FirstName = "Test",
                LastName = "Testesen",
                Email = "test@test.dk",
                TLF = "12345678"
            };
            _customerRepo.Add(customer);
            var addedCustomer = _customerRepo.GetAll().First(c =>
                c.FirstName == customer.FirstName);

            // Act
            _customerRepo.Delete(addedCustomer);
            var result = _customerRepo.GetById(addedCustomer.CustomerID);

            // Assert
            Assert.IsNull(result);
        }
    }
}