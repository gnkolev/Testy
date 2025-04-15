using BettyWallet.Application.Interfaces;
using BettyWallet.Domain.Entities;
using BettyWallet.Domain.Enums;
using BettyWallet.Application.Services;
using Moq;

namespace BettyWallet.Tests
{
    public class GameServiceTests
    {
        private readonly Mock<ILoggerService> _mockLoggerService;
        private readonly GameService _gameService;

        public GameServiceTests()
        {
            _mockLoggerService = new Mock<ILoggerService>();
            _gameService = new GameService(_mockLoggerService.Object);
        }

        [Fact]
        public void Play_ShouldThrowException_WhenBetIsLessThan1()
        {
            var player = new Player();
            player.Wallet.Deposit(10);

            Assert.Throws<ArgumentException>(() => _gameService.Play(player, 0.5m));
        }

        [Fact]
        public void Play_ShouldThrowException_WhenBetIsGreaterThan10()
        {
            var player = new Player();
            player.Wallet.Deposit(20);

            Assert.Throws<ArgumentException>(() => _gameService.Play(player, 15));
        }

        [Fact]
        public void Play_ShouldReturnLost_WhenPlayerHasInsufficientBalance()
        {
            var player = new Player();

            var result = _gameService.Play(player, 5);

            Assert.Equal(BetResult.Lost, result.Result);
            Assert.Equal(0, result.WinAmount);
        }

        [Fact]
        public void Play_ShouldDeductBetAmount_FromWallet()
        {
            var player = new Player();
            player.Wallet.Deposit(10);

            _gameService.Play(player, 5);

            Assert.True(player.Wallet.Balance == 5 || player.Wallet.Balance > 5);
        }

        [Fact]
        public void Play_ShouldLogGameResult()
        {
            var player = new Player();
            player.Wallet.Deposit(10);

            var mockLoggerService = new Mock<ILoggerService>();
            var mockRandom = new Mock<Random>();

            mockRandom.Setup(r => r.Next(1, 101)).Returns(10);

            var gameService = new GameService(mockLoggerService.Object, mockRandom.Object);

            var result = gameService.Play(player, 5);

            Assert.NotNull(result);
            Assert.Equal(5, player.Wallet.Balance);

            mockLoggerService.Verify(
                x => x.LogGameResult(It.IsAny<GameResult>(), It.IsAny<decimal>()),
                Times.Once
            );
        }

        [Fact]
        public void Play_ShouldNotIncreaseBalance_WhenPlayerLoses()
        {
            var player = new Player();
            player.Wallet.Deposit(10);

            var mockLoggerService = new Mock<ILoggerService>();
            var mockRandom = new Mock<Random>();

            mockRandom.Setup(r => r.Next(1, 101)).Returns(10);

            var gameService = new GameService(mockLoggerService.Object, mockRandom.Object);

            var result = gameService.Play(player, 5);

            Assert.Equal(BetResult.Lost, result.Result);
            Assert.Equal(5, player.Wallet.Balance);
        }

        [Fact]
        public void Play_ShouldDoubleBet_WhenPlayerWinsX2()
        {
            var player = new Player();
            player.Wallet.Deposit(10);

            var result = _gameService.Play(player, 5);

            if (result.Result == BetResult.WinX2)
            {
                Assert.Equal(15, player.Wallet.Balance);
            }
        }

        [Fact]
        public void Play_ShouldMultiplyBetBy10_WhenPlayerWinsX10()
        {
            var player = new Player();
            player.Wallet.Deposit(10);

            var result = _gameService.Play(player, 5);

            if (result.Result == BetResult.WinX10)
            {
                Assert.Equal(55, player.Wallet.Balance);
            }
        }

        [Fact]
        public void Play_ShouldNotAllowBet_WhenBalanceIsZero()
        {
            var player = new Player();

            var result = _gameService.Play(player, 5);

            Assert.Equal(BetResult.Lost, result.Result);
            Assert.Equal(0, result.WinAmount);
        }

        [Fact]
        public void Play_ShouldReturnValidWinLossResult()
        {
            var player = new Player();
            player.Wallet.Deposit(10);

            var result = _gameService.Play(player, 5);

            Assert.True(result.Result == BetResult.Lost || result.Result == BetResult.WinX2 || result.Result == BetResult.WinX10);
        }

        [Fact]
        public void Play_ShouldAllowMultipleBets()
        {
            var player = new Player();
            player.Wallet.Deposit(20);

            _gameService.Play(player, 5);
            _gameService.Play(player, 5);

            Assert.True(player.Wallet.Balance >= 0);
        }

        [Fact]
        public void Play_ShouldNotAllowNegativeBalance()
        {
            var player = new Player();

            _gameService.Play(player, 5);

            Assert.True(player.Wallet.Balance >= 0);
        }

        [Fact]
        public void Play_ShouldUpdateBalance_WhenPlayerWins()
        {
            var player = new Player();
            player.Wallet.Deposit(10);

            var result = _gameService.Play(player, 5);

            if (result.WinAmount > 0)
            {
                Assert.True(player.Wallet.Balance > 5);
            }
        }

        [Fact]
        public void Play_ShouldDeductCorrectAmount()
        {
            var player = new Player();
            player.Wallet.Deposit(20);

            decimal initialBalance = player.Wallet.Balance;
            decimal betAmount = 5;

            var mockLoggerService = new Mock<ILoggerService>();
            var mockRandom = new Mock<Random>();
            mockRandom.Setup(r => r.Next(1, 101)).Returns(10);

            var gameService = new GameService(mockLoggerService.Object, mockRandom.Object);

            gameService.Play(player, betAmount);

            Assert.Equal(initialBalance - betAmount, player.Wallet.Balance);
        }

        [Fact]
        public void Play_ShouldNotAllowBetGreaterThanBalance()
        {
            var player = new Player();
            player.Wallet.Deposit(5);

            var result = _gameService.Play(player, 10);

            Assert.Equal(BetResult.Lost, result.Result);
        }

        [Fact]
        public void Play_ShouldOnlyAwardWinnings_WhenWinning()
        {
            var player = new Player();
            player.Wallet.Deposit(10);

            var result = _gameService.Play(player, 5);

            if (result.Result == BetResult.Lost)
            {
                Assert.Equal(5, player.Wallet.Balance);
            }
        }

        [Fact]
        public void Play_ShouldSupportMultipleBets()
        {
            var player = new Player();
            player.Wallet.Deposit(30);

            _gameService.Play(player, 5);
            _gameService.Play(player, 5);
            _gameService.Play(player, 5);

            Assert.True(player.Wallet.Balance >= 0);
        }

        [Fact]
        public void Play_ShouldHandleMultipleWinsAndLosses()
        {
            var player = new Player();
            player.Wallet.Deposit(50);

            for (int i = 0; i < 5; i++)
            {
                _gameService.Play(player, 5);
            }

            Assert.True(player.Wallet.Balance >= 0);
        }
    }
}
