using Banking;
using Moq;
using System;
using Xunit;
using FluentAssertions;
using System.Diagnostics.CodeAnalysis;
using Communicator;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using Logging;

namespace BankingTests
{
    [ExcludeFromCodeCoverage]
    public class BankTests
    {
        private Bank bank;
        private Mock<IVault> vaultMock;
        private IApiCaller apiCaller;
        private Mock<IApiCaller> apiCallerMock;
        private Mock<ILogger> loggerMock;

        public BankTests()
        {
            #region LoggerMock

            loggerMock = new Mock<ILogger>();
            loggerMock.Setup(x => x.Log(It.IsAny<string>())).Verifiable();

            #endregion

            vaultMock = new Mock<IVault>();
            vaultMock.Setup(x => x.Add(It.IsAny<Account>()))
                .Verifiable();

            vaultMock.Setup(x => x.Deposit(It.IsAny<Guid>(), It.IsAny<decimal>()))
                .Verifiable();

            vaultMock.Setup(x => x.Withdraw(It.IsAny<Guid>(), It.IsAny<decimal>()))
                .Verifiable();

            apiCaller = new ApiCaller();

            #region ApiCaller Mock

            apiCallerMock = new Mock<IApiCaller>();

            apiCallerMock.Setup(x => x.Call(It.IsAny<Uri>(), It.IsAny<HttpMethod>(), It.Is<Customer>(x => x.Id == 1)))
                .Returns(Task.Factory.StartNew(() =>
                {
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent("Customer 1 created successfully")
                    };
                }));

            apiCallerMock.Setup(x => x.Call(It.IsAny<Uri>(), It.IsAny<HttpMethod>(), It.Is<Customer>(x => x.Id == 2)))
                .Returns(Task.Factory.StartNew(() =>
                {
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent("Customer 2 already exists")
                    };
                }));

            #endregion

            //Without Mock
            //bank = new Bank(vaultMock.Object, apiCaller);

            //With Mock
            //bank = new Bank(vaultMock.Object, apiCallerMock.Object);

            //With Mock and Logger
            bank = new Bank(vaultMock.Object, apiCallerMock.Object, loggerMock.Object);
        }

        [Fact]
        public void AddAccount_ShallRequestAddNewAccount()
        {
            //Arrange
            var account = new Account
            {
                Balance = 100.00m,
                Customer = new Customer
                {
                    Id = 1,
                    Name = "Customer A"
                },
                Id = Guid.NewGuid()
            };

            //Act
            bank.AddAccount(account);

            //Assert
            vaultMock.Verify(x => x.Add(It.IsAny<Account>()), Times.Once);
        }

        [Fact]
        public void Deposit_ShallRequestIncreaseBalance()
        {
            //Arrange
            var value = 10.00m;
            var id = Guid.NewGuid();

            //Act
            bank.Deposit(id, value);

            //Assert
            vaultMock.Verify(x => x.Deposit(It.IsAny<Guid>(), It.IsAny<decimal>()), Times.Once);
        }

        [Fact]
        public void Withdraw_ShallRequestDecreaseBalance()
        {
            //Arrange
            var value = 10.00m;
            var id = Guid.NewGuid();

            //Act
            bank.Withdraw(id, value);

            //Assert
            vaultMock.Verify(x => x.Withdraw(It.IsAny<Guid>(), It.IsAny<decimal>()), Times.Once);
        }

        [Fact]
        public async void RegisterCustomer_WhenNotExists_ShallRegister()
        {
            #region With Mock

            //Arrange
            var customer = new Customer
            {
                Id = 1,
                Name = "John Doe"
            };
            var expected = $"Customer {customer.Id} created successfully";

            //Act
            var actual = await bank.RegisterCustomer(customer);

            //Assert
            actual.Should().NotBeNullOrEmpty();
            actual.Should().Contain(expected);
            apiCallerMock.Verify(x => x.Call(It.IsAny<Uri>(), It.IsAny<HttpMethod>(), It.Is<Customer>(x => x.Id == 1)), Times.Once);

            #endregion

            #region Without Mock

            ////Arrange
            //var customer = new Customer
            //{
            //    Id = await GetAvailableId(),
            //    Name = "John Doe"
            //};
            //var expected = $"Customer {customer.Id} created successfully";

            ////Act
            //var actual = await bank.RegisterCustomer(customer);

            ////Assert
            //actual.Should().NotBeNullOrEmpty();
            //actual.Should().Contain(expected);

            #endregion
        }

        [Fact]
        public async void RegisterCustomer_WhenExists_ShallNotRegister()
        {
            #region With Mock

            //Arrange
            var customer = new Customer
            {
                Id = 2,
                Name = "Jane Doe"
            };
            var expected = $"Customer {customer.Id} already exists";

            //Act
            var actual = await bank.RegisterCustomer(customer);

            //Assert
            actual.Should().NotBeNullOrEmpty();
            actual.Should().Contain(expected);
            apiCallerMock.Verify(x => x.Call(It.IsAny<Uri>(), It.IsAny<HttpMethod>(), It.Is<Customer>(x => x.Id == 2)), Times.Once);

            #endregion

            #region Without Mock

            ////Arrange
            //var customer = new Customer
            //{
            //    Id = await GetAvailableId(),
            //    Name = "Jane Doe"
            //};
            //var expected = $"Customer {customer.Id} already exists";
            //await bank.RegisterCustomer(customer);

            ////Act
            //var actual = await bank.RegisterCustomer(customer);

            ////Assert
            //actual.Should().NotBeNullOrEmpty();
            //actual.Should().Contain(expected);

            #endregion
        }

        //public async Task<int> GetAvailableId()
        //{
        //    int id = 1;

        //    while (true)
        //    {
        //        var response = await apiCaller.Call(new Uri($"http://localhost:5000/api/customer/get/{id}"), HttpMethod.Get);
        //        var content = await response.Content.ReadAsStringAsync();
        //        if (content == "")
        //        {
        //            return id;
        //        }

        //        id++;
        //    }
        //}
    }
}
