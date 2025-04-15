using BettyWallet.Domain.Enums;
using BettyWallet.Application.Services;
using BettyWallet.Domain.Entities;

namespace BettyWallet.Tests
{
    public class TransactionHistoryServiceTests : IDisposable
    {
        private readonly string _testFilePath = "test_transactions.json";
        private readonly TransactionHistoryService _transactionHistoryService;

        public TransactionHistoryServiceTests()
        {
            _transactionHistoryService = new TransactionHistoryService(_testFilePath);
        }
        public void Dispose()
        {
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }

        [Fact]
        public void TransactionHistoryService_ShouldCreateFile_WhenNotExists()
        {
            var transactions = _transactionHistoryService.LoadTransactions();

            Assert.True(File.Exists(_testFilePath));
            Assert.Empty(transactions);
        }

        [Fact]
        public void TransactionHistoryService_ShouldSaveTransaction()
        {
            var transaction = new Transaction(TransactionType.Deposit, 100);

            _transactionHistoryService.SaveTransaction(transaction);
            var transactions = _transactionHistoryService.LoadTransactions();

            Assert.Single(transactions);
            Assert.Equal(TransactionType.Deposit, transactions[0].Type);
            Assert.Equal(100, transactions[0].Amount);
        }

        [Fact]
        public void TransactionHistoryService_ShouldSaveMultipleTransactions()
        {
            var transaction1 = new Transaction(TransactionType.Deposit, 50);
            var transaction2 = new Transaction(TransactionType.Withdrawal, 30);

            _transactionHistoryService.SaveTransaction(transaction1);
            _transactionHistoryService.SaveTransaction(transaction2);
            var transactions = _transactionHistoryService.LoadTransactions();

            Assert.Equal(2, transactions.Count);
            Assert.Contains(transactions, t => t.Type == TransactionType.Deposit && t.Amount == 50);
            Assert.Contains(transactions, t => t.Type == TransactionType.Withdrawal && t.Amount == 30);
        }

        [Fact]
        public void TransactionHistoryService_ShouldHandleCorruptFile()
        {
            File.WriteAllText(_testFilePath, "{ invalid json }");

            var transactions = _transactionHistoryService.LoadTransactions();

            Assert.Empty(transactions);
        }

        [Fact]
        public void TransactionHistoryService_ShouldHandleEmptyFile()
        {
            File.WriteAllText(_testFilePath, ""); // Empty file

            var transactions = _transactionHistoryService.LoadTransactions();

            Assert.Empty(transactions);
        }

        [Fact]
        public void TransactionHistoryService_ShouldClearHistory()
        {
            var transaction = new Transaction(TransactionType.Deposit, 100);
            _transactionHistoryService.SaveTransaction(transaction);

            _transactionHistoryService.ClearHistory();
            var transactions = _transactionHistoryService.LoadTransactions();

            Assert.Empty(transactions);
        }

        [Fact]
        public void TransactionHistoryService_ShouldAppendTransactions()
        {
            var transaction1 = new Transaction(TransactionType.Deposit, 100);
            var transaction2 = new Transaction(TransactionType.Withdrawal, 50);

            _transactionHistoryService.SaveTransaction(transaction1);
            _transactionHistoryService.SaveTransaction(transaction2);
            var transactions = _transactionHistoryService.LoadTransactions();

            Assert.Equal(2, transactions.Count);
        }

        [Fact]
        public void TransactionHistoryService_ShouldThrowException_WhenSavingNullTransaction()
        {
            Assert.Throws<ArgumentNullException>(() => _transactionHistoryService.SaveTransaction(null));
        }

        [Fact]
        public void TransactionHistoryService_ShouldThrowException_WhenTransactionHasInvalidAmount()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Transaction(TransactionType.Deposit, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Transaction(TransactionType.Withdrawal, -5));
        }

        [Fact]
        public void TransactionHistoryService_ShouldReturnEmptyList_WhenFileDoesNotExist()
        {
            if (File.Exists(_testFilePath))
                File.Delete(_testFilePath);

            var transactions = _transactionHistoryService.LoadTransactions();

            Assert.Empty(transactions);
        }
    }
}