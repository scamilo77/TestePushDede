using Banking;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Xunit;

namespace BankingTests
{
    [ExcludeFromCodeCoverage]
    public class VaultTests
    {
        private Vault vault;
        private Guid Id = Guid.Parse("f4486bf9-d2e5-4e01-bde1-3af02a9ece4d");
        public VaultTests()
        {
            vault = new Vault();
            vault.Accounts.Add(new Account
            {
                Balance = 100.00m,
                Customer = new Customer
                {
                    Id = 1,
                    Name = "Customer A"
                },
                Id = Id
            });
        }

        [Fact]
        public void Add_WithNonExistenAccountId_ShallAddNewAccount()
        {
            //Arrange
            var id = Guid.NewGuid();
            var account = new Account
            {
                Balance = 100.00m,
                Customer = new Customer
                {
                    Id = 2,
                    Name = "Customer B"
                },
                Id = id
            };

            //Act
            vault.Add(account);

            //Assert
            vault.Accounts.Where(x => x.Id == id).Should().HaveCount(1);
        }

        [Fact]
        public void Add_WithExistenAccountId_ShallIgnore()
        {
            //Arrange
            var account = new Account
            {
                Balance = 50.00m,
                Customer = new Customer
                {
                    Id = 3,
                    Name = "Customer C"
                },
                Id = Id
            };

            //Act
            vault.Add(account);

            //Assert
            vault.Accounts.Where(x => x.Id == Id).Should().HaveCount(1);
            vault.Accounts.FirstOrDefault(x => x.Id == Id).Balance.Should().Be(100.00m);
            vault.Accounts.FirstOrDefault(x => x.Id == Id).Customer.Name.Should().Be("Customer A");
        }

        [Fact]
        public void Deposit_WithExistentAccount_ShallDeposit()
        {
            //Arrange
            var account = vault.Accounts.FirstOrDefault(x => x.Id == Id);
            var oldBalance = account.Balance;
            var depositValue = 50.00m;

            //Act
            vault.Deposit(Id, depositValue);

            //Assert
            account = vault.Accounts.FirstOrDefault(x => x.Id == Id);
            account.Balance.Should().BeGreaterThan(0);
            account.Balance.Should().Be(oldBalance + depositValue);
        }

        [Fact]
        public void Deposit_WithNonExistentAccount_ShallThrowException()
        {
            //Arrange
            var depositValue = 50.00m;
            var message = "Inexistent Account";

            //Act
            var exception = Assert.Throws<Exception>(() => vault.Deposit(Guid.NewGuid(), depositValue));

            //Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<Exception>();
            exception.Message.Should().Contain(message);
        }

        [Fact]
        public void Withdraw_WithExistentAccount_AndPositiveBalance_ShallWithdraw()
        {
            //Arrange
            var account = vault.Accounts.FirstOrDefault(x => x.Id == Id);
            var oldBalance = account.Balance;
            var withdrawValue = 50.00m;

            //Act
            vault.Withdraw(Id, withdrawValue);

            //Assert
            account = vault.Accounts.FirstOrDefault(x => x.Id == Id);
            account.Balance.Should().BeLessThan(oldBalance);
            account.Balance.Should().Be(oldBalance - withdrawValue);
        }

        [Fact]
        public void Withdraw_WithNonExistentAccount_ShallThrowException()
        {
            //Arrange
            var withdrawValue = 50.00m;
            var message = "Inexistent Account";

            //Act
            var exception = Assert.Throws<Exception>(() => vault.Withdraw(Guid.NewGuid(), withdrawValue));

            //Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<Exception>();
            exception.Message.Should().Contain(message);
        }

        [Fact]
        public void Withdraw_WithExistentAccount_AndNoBalance_ShallThrowException()
        {
            //Arrange
            var account = vault.Accounts.FirstOrDefault(x => x.Id == Id);
            var oldBalance = account.Balance;
            vault.Withdraw(account.Id, account.Balance);
            var withdrawValue = 10.00m;
            var message = "Insufficient Balance";

            //Act
            var exception = Assert.Throws<Exception>(() => vault.Withdraw(Id, withdrawValue));

            //Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<Exception>();
            exception.Message.Should().Contain(message);
        }
    }
}
