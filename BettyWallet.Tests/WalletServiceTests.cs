using BettyWallet.Application.Interfaces;
using BettyWallet.Application.Services;
using BettyWallet.Domain.Entities;
using BettyWallet.Domain.Enums;
using Moq;

namespace BettyWallet.Tests
{
    public class WalletServiceTests
    {
        private readonly Mock<ILoggerService> _mockLoggerService;
        private readonly Mock<ITransactionHistoryService> _mockTransactionHistoryService;
        private readonly WalletService _walletService;
        private readonly Wallet _wallet;

        public WalletServiceTests()
        {
            _mockLoggerService = new Mock<ILoggerService>();
            _mockTransactionHistoryService = new Mock<ITransactionHistoryService>();

            _walletService = new WalletService(_mockLoggerService.Object, _mockTransactionHistoryService.Object);
            _wallet = new Wallet();
        }

        [Fact]
        public void Deposit_ShouldIncreaseBalance()
        {
            var wallet = new Wallet();
            _walletService.Deposit(wallet, 50);

            Assert.Equal(50, wallet.Balance);
        }

        [Fact]
        public void Deposit_ShouldLogTransaction()
        {
            var wallet = new Wallet();
            _walletService.Deposit(wallet, 100);

            _mockLoggerService.Verify(
                x => x.LogTransaction(It.Is<Transaction>(t => t.Type == TransactionType.Deposit && t.Amount == 100)),
                Times.Once
            );
        }

        [Fact]
        public void Deposit_ShouldSaveTransactionToHistory()
        {
            var wallet = new Wallet();
            var mockLoggerService = new Mock<ILoggerService>();
            var mockTransactionHistoryService = new Mock<ITransactionHistoryService>(); // ✅ Mock history service
            var walletService = new WalletService(mockLoggerService.Object, mockTransactionHistoryService.Object);

            walletService.Deposit(wallet, 75);

            mockTransactionHistoryService.Verify(
                x => x.SaveTransaction(It.Is<Transaction>(t => t.Type == TransactionType.Deposit && t.Amount == 75)),
                Times.Once
            );
        }

        [Fact]
        public void Deposit_ZeroAmount_ShouldThrowException()
        {
            var wallet = new Wallet();
            Assert.Throws<ArgumentException>(() => _walletService.Deposit(wallet, 0));
        }

        [Fact]
        public void Deposit_NegativeAmount_ShouldThrowException()
        {
            var wallet = new Wallet();
            Assert.Throws<ArgumentException>(() => _walletService.Deposit(wallet, -100));
        }

        [Fact]
        public void Withdraw_WithSufficientBalance_ShouldDecreaseBalance()
        {
            var wallet = new Wallet();
            _walletService.Deposit(wallet, 100);
            bool success = _walletService.Withdraw(wallet, 40);

            Assert.True(success);
            Assert.Equal(60, wallet.Balance);
        }

        [Fact]
        public void Withdraw_ShouldLogTransaction()
        {
            var wallet = new Wallet();
            _walletService.Deposit(wallet, 100);
            _walletService.Withdraw(wallet, 50);

            _mockLoggerService.Verify(
                x => x.LogTransaction(It.Is<Transaction>(t => t.Type == TransactionType.Withdrawal && t.Amount == 50)),
                Times.Once
            );
        }

        [Fact]
        public void Withdraw_ShouldSaveTransactionToHistory()
        {
            var wallet = new Wallet();
            wallet.Deposit(200);

            var mockLoggerService = new Mock<ILoggerService>();
            var mockTransactionHistoryService = new Mock<ITransactionHistoryService>(); // ✅ Mock history service
            var walletService = new WalletService(mockLoggerService.Object, mockTransactionHistoryService.Object);

            walletService.Withdraw(wallet, 80);

            mockTransactionHistoryService.Verify(
                x => x.SaveTransaction(It.Is<Transaction>(t => t.Type == TransactionType.Withdrawal && t.Amount == 80)),
                Times.Once
            );
        }

        [Fact]
        public void Withdraw_ExactBalance_ShouldSetBalanceToZero()
        {
            var wallet = new Wallet();
            _walletService.Deposit(wallet, 100);
            bool success = _walletService.Withdraw(wallet, 100);

            Assert.True(success);
            Assert.Equal(0, wallet.Balance);
        }

        [Fact]
        public void Withdraw_InsufficientBalance_ShouldFail()
        {
            var wallet = new Wallet();
            _walletService.Deposit(wallet, 50);
            bool success = _walletService.Withdraw(wallet, 100);

            Assert.False(success);
            Assert.Equal(50, wallet.Balance);
        }

        [Fact]
        public void Withdraw_ZeroAmount_ShouldThrowException()
        {
            var wallet = new Wallet();
            Assert.Throws<ArgumentException>(() => _walletService.Withdraw(wallet, 0));
        }

        [Fact]
        public void Withdraw_NegativeAmount_ShouldThrowException()
        {
            var wallet = new Wallet();
            Assert.Throws<ArgumentException>(() => _walletService.Withdraw(wallet, -10));
        }

        [Fact]
        public void GetBalance_ShouldReturnCorrectAmount()
        {
            var wallet = new Wallet();
            _walletService.Deposit(wallet, 200);

            Assert.Equal(200, _walletService.GetBalance(wallet));
        }

        [Fact]
        public void MultipleDeposits_ShouldIncreaseBalance()
        {
            var wallet = new Wallet();
            _walletService.Deposit(wallet, 50);
            _walletService.Deposit(wallet, 30);
            Assert.Equal(80, wallet.Balance);
        }

        [Fact]
        public void MultipleWithdrawals_ShouldDecreaseBalance()
        {
            var wallet = new Wallet();
            _walletService.Deposit(wallet, 100);
            _walletService.Withdraw(wallet, 30);
            _walletService.Withdraw(wallet, 20);
            Assert.Equal(50, wallet.Balance);
        }

        [Fact]
        public void AlternatingDepositsAndWithdrawals_ShouldCalculateCorrectBalance()
        {
            var wallet = new Wallet();
            _walletService.Deposit(wallet, 200);
            _walletService.Withdraw(wallet, 50);
            _walletService.Deposit(wallet, 100);
            _walletService.Withdraw(wallet, 80);
            Assert.Equal(170, wallet.Balance);
        }

        [Fact]
        public void LargeDeposit_ShouldUpdateBalanceCorrectly()
        {
            var wallet = new Wallet();
            _walletService.Deposit(wallet, 1_000_000);
            Assert.Equal(1_000_000, wallet.Balance);
        }

        [Fact]
        public void LargeWithdrawal_ShouldFailIfBalanceIsLow()
        {
            var wallet = new Wallet();
            _walletService.Deposit(wallet, 500);
            bool result = _walletService.Withdraw(wallet, 100_000);
            Assert.False(result);
        }

        [Fact]
        public void Wallet_ShouldNotAllowNegativeBalance()
        {
            var wallet = new Wallet();
            bool result = _walletService.Withdraw(wallet, 10);
            Assert.False(result);
            Assert.Equal(0, wallet.Balance);
        }

        [Fact]
        public void Wallet_ShouldHandleDecimalValuesCorrectly()
        {
            var wallet = new Wallet();
            _walletService.Deposit(wallet, 100.75m);
            _walletService.Withdraw(wallet, 50.25m);
            Assert.Equal(50.50m, wallet.Balance);
        }
    }
}