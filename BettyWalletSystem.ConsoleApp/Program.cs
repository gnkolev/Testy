using BettyWallet.Application.Interfaces;
using BettyWallet.Application.Models;
using BettyWallet.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static void Main()
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        var appSettings = config.Get<AppSettings>() ?? throw new InvalidOperationException("Failed to load app settings.");

        var serviceProvider = new ServiceCollection()
            .AddSingleton(appSettings)
            .AddSingleton<IWalletService, WalletService>()
            .AddSingleton<IGameService, GameService>()
            .AddSingleton<ILoggerService, LoggerService>()
            .AddSingleton<ITransactionHistoryService, TransactionHistoryService>()
            .BuildServiceProvider();

        var walletService = serviceProvider.GetRequiredService<IWalletService>();
        var gameService = serviceProvider.GetRequiredService<IGameService>();
        var loggerService = serviceProvider.GetRequiredService<ILoggerService>();
        var transactionHistoryService = serviceProvider.GetRequiredService<ITransactionHistoryService>();

        var uiService = new UIService(walletService, gameService, transactionHistoryService, appSettings);
        uiService.Run();
    }
}