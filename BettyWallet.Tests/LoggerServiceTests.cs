using BettyWallet.Application.Services;
using BettyWallet.Domain.Entities;
using BettyWallet.Domain.Enums;

namespace BettyWallet.Tests
{
    public class LoggerServiceTests
    {
        private readonly string _testLogFilePath = "test_wallet_log.txt";
        private readonly LoggerService _loggerService;

        public LoggerServiceTests()
        {
            _loggerService = new LoggerService(_testLogFilePath);
        }

        [Fact]
        public void Logger_ShouldCreateLogFile_IfNotExists()
        {
            _loggerService.LogTransaction(new Transaction(TransactionType.Deposit, 100));

            Assert.True(File.Exists(_testLogFilePath));
        }

        [Fact]
        public void Logger_ShouldLogDepositTransaction()
        {
            var transaction = new Transaction(TransactionType.Deposit, 50);

            _loggerService.LogTransaction(transaction);
            string logContent = File.ReadAllText(_testLogFilePath);

            Assert.Contains("TRANSACTION | Deposit | Amount: $50.00", logContent);
        }

        [Fact]
        public void Logger_ShouldLogWithdrawalTransaction()
        {
            var transaction = new Transaction(TransactionType.Withdrawal, 30);

            _loggerService.LogTransaction(transaction);
            string logContent = File.ReadAllText(_testLogFilePath);

            Assert.Contains("TRANSACTION | Withdrawal | Amount: $30.00", logContent);
        }

        [Fact]
        public void Logger_ShouldLogGameResult_WhenPlayerLoses()
        {
            var gameResult = new GameResult(BetResult.Lost, 0);
            decimal betAmount = 5;

            _loggerService.LogGameResult(gameResult, betAmount);
            string logContent = File.ReadAllText(_testLogFilePath);

            Assert.Contains("GAME RESULT | Bet: $5.00 | Outcome: Lost | Won: $0.00", logContent);
        }

        [Fact]
        public void Logger_ShouldLogGameResult_WhenPlayerWinsX2()
        {
            var gameResult = new GameResult(BetResult.WinX2, 20);
            decimal betAmount = 10;

            _loggerService.LogGameResult(gameResult, betAmount);
            string logContent = File.ReadAllText(_testLogFilePath);

            Assert.Contains("GAME RESULT | Bet: $10.00 | Outcome: WinX2 | Won: $20.00", logContent);
        }

        [Fact]
        public void Logger_ShouldLogGameResult_WhenPlayerWinsX10()
        {
            var gameResult = new GameResult(BetResult.WinX10, 100);
            decimal betAmount = 10;

            _loggerService.LogGameResult(gameResult, betAmount);
            string logContent = File.ReadAllText(_testLogFilePath);

            Assert.Contains("GAME RESULT | Bet: $10.00 | Outcome: WinX10 | Won: $100.00", logContent);
        }

        [Fact]
        public void Logger_ShouldAppendNewEntries_ToExistingLogFile()
        {
            var transaction1 = new Transaction(TransactionType.Deposit, 50);
            var transaction2 = new Transaction(TransactionType.Withdrawal, 20);

            _loggerService.LogTransaction(transaction1);
            _loggerService.LogTransaction(transaction2);

            int retries = 5;
            string logContent = "";
            while (retries-- > 0)
            {
                Thread.Sleep(50);
                logContent = File.Exists(_testLogFilePath) ? File.ReadAllText(_testLogFilePath) : "";
                if (!string.IsNullOrEmpty(logContent)) break;
            }

            Assert.Contains("TRANSACTION | Deposit | Amount: $50.00", logContent);
            Assert.Contains("TRANSACTION | Withdrawal | Amount: $20.00", logContent);
        }

        [Fact]
        public void Logger_ShouldNotCrash_IfLogFileIsLocked()
        {
            var transaction = new Transaction(TransactionType.Deposit, 100);

            using (var fileStream = new FileStream(_testLogFilePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None))
            {
                Exception? exception = Record.Exception(() => _loggerService.LogTransaction(transaction));

                Assert.Null(exception);
            }
        }

        [Fact]
        public void Logger_ShouldNotWriteLog_IfTransactionIsNull()
        {
            Exception? exception = Record.Exception(() => _loggerService.LogTransaction(null));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void Logger_ShouldNotWriteLog_IfGameResultIsNull()
        {
            Exception? exception = Record.Exception(() => _loggerService.LogGameResult(null, 5));

            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
    }
}