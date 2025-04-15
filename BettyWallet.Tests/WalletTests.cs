using BettyWallet.Domain.Entities;

namespace BettyWallet.Tests
{
    public class WalletTests
    {
        [Fact]
        public void Wallet_ShouldInitialize_WithZeroBalance()
        {
            var wallet = new Wallet();
            Assert.Equal(0, wallet.Balance);
        }

        [Fact]
        public void Deposit_ShouldIncreaseBalance()
        {
            var wallet = new Wallet();
            wallet.Deposit(100);
            Assert.Equal(100, wallet.Balance);
        }

        [Fact]
        public void Deposit_Zero_ShouldThrowException()
        {
            var wallet = new Wallet();
            Assert.Throws<ArgumentException>(() => wallet.Deposit(0));
        }

        [Fact]
        public void Deposit_Negative_ShouldThrowException()
        {
            var wallet = new Wallet();
            Assert.Throws<ArgumentException>(() => wallet.Deposit(-50));
        }

        [Fact]
        public void Withdraw_WithSufficientBalance_ShouldDecreaseBalance()
        {
            var wallet = new Wallet();
            wallet.Deposit(100);
            bool result = wallet.Withdraw(50);
            Assert.True(result);
            Assert.Equal(50, wallet.Balance);
        }

        [Fact]
        public void Withdraw_ExactBalance_ShouldSetBalanceToZero()
        {
            var wallet = new Wallet();
            wallet.Deposit(100);
            bool result = wallet.Withdraw(100);
            Assert.True(result);
            Assert.Equal(0, wallet.Balance);
        }

        [Fact]
        public void Withdraw_InsufficientBalance_ShouldFail()
        {
            var wallet = new Wallet();
            wallet.Deposit(50);
            bool result = wallet.Withdraw(100);
            Assert.False(result);
            Assert.Equal(50, wallet.Balance);
        }

        [Fact]
        public void Withdraw_Zero_ShouldThrowException()
        {
            var wallet = new Wallet();
            Assert.Throws<ArgumentException>(() => wallet.Withdraw(0));
        }

        [Fact]
        public void Withdraw_Negative_ShouldThrowException()
        {
            var wallet = new Wallet();
            Assert.Throws<ArgumentException>(() => wallet.Withdraw(-10));
        }

        [Fact]
        public void MultipleDeposits_ShouldIncreaseBalance()
        {
            var wallet = new Wallet();
            wallet.Deposit(50);
            wallet.Deposit(30);
            Assert.Equal(80, wallet.Balance);
        }

        [Fact]
        public void MultipleWithdrawals_ShouldDecreaseBalance()
        {
            var wallet = new Wallet();
            wallet.Deposit(100);
            wallet.Withdraw(30);
            wallet.Withdraw(20);
            Assert.Equal(50, wallet.Balance);
        }

        [Fact]
        public void WithdrawAfterDeposit_ShouldUpdateBalanceCorrectly()
        {
            var wallet = new Wallet();
            wallet.Deposit(200);
            wallet.Withdraw(50);
            Assert.Equal(150, wallet.Balance);
        }

        [Fact]
        public void AlternatingDepositsAndWithdrawals_ShouldCalculateCorrectBalance()
        {
            var wallet = new Wallet();
            wallet.Deposit(200);
            wallet.Withdraw(50);
            wallet.Deposit(100);
            wallet.Withdraw(80);
            Assert.Equal(170, wallet.Balance);
        }

        [Fact]
        public void LargeDeposit_ShouldUpdateBalanceCorrectly()
        {
            var wallet = new Wallet();
            wallet.Deposit(1_000_000);
            Assert.Equal(1_000_000, wallet.Balance);
        }

        [Fact]
        public void LargeWithdrawal_ShouldFailIfBalanceIsLow()
        {
            var wallet = new Wallet();
            wallet.Deposit(500);
            bool result = wallet.Withdraw(1_000_000);
            Assert.False(result);
        }

        [Fact]
        public void ConsecutiveZeroDeposits_ShouldThrowException()
        {
            var wallet = new Wallet();
            Assert.Throws<ArgumentException>(() => wallet.Deposit(0));
            Assert.Throws<ArgumentException>(() => wallet.Deposit(0));
        }

        [Fact]
        public void ConsecutiveZeroWithdrawals_ShouldThrowException()
        {
            var wallet = new Wallet();
            wallet.Deposit(100);
            Assert.Throws<ArgumentException>(() => wallet.Withdraw(0));
            Assert.Throws<ArgumentException>(() => wallet.Withdraw(0));
        }

        [Fact]
        public void MultipleTransactions_ShouldResultInCorrectBalance()
        {
            var wallet = new Wallet();
            wallet.Deposit(100);
            wallet.Withdraw(40);
            wallet.Deposit(60);
            wallet.Withdraw(50);
            Assert.Equal(70, wallet.Balance);
        }

        [Fact]
        public void Wallet_ShouldNotAllowNegativeBalance()
        {
            var wallet = new Wallet();
            bool result = wallet.Withdraw(10);
            Assert.False(result);
            Assert.Equal(0, wallet.Balance);
        }

        [Fact]
        public void Wallet_ShouldHandleDecimalValuesCorrectly()
        {
            var wallet = new Wallet();
            wallet.Deposit(100.75m);
            wallet.Withdraw(50.25m);
            Assert.Equal(50.50m, wallet.Balance);
        }
    }
}