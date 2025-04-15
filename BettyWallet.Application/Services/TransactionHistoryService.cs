using BettyWallet.Application.Interfaces;
using BettyWallet.Domain.Entities;
using Newtonsoft.Json;

namespace BettyWallet.Application.Services
{
    /// <summary>
    /// Service for handling persistent transaction storage.
    /// </summary>
    public class TransactionHistoryService : ITransactionHistoryService
    {
        private readonly string _filePath;
        private readonly object _lock = new object();

        public TransactionHistoryService(string filePath = "transactions.json")
        {
            _filePath = filePath;
            EnsureTransactionFileExists();
        }

        private void EnsureTransactionFileExists()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    File.WriteAllText(_filePath, "[]");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error initializing transaction file: {ex.Message}");
            }
        }

        public void SaveTransaction(Transaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction), "Transaction cannot be null.");

            try
            {
                lock (_lock)
                {
                    var transactions = LoadTransactions();
                    transactions.Add(transaction);

                    string json = JsonConvert.SerializeObject(transactions, Formatting.Indented);
                    File.WriteAllText(_filePath, json);
                }
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"❌ File I/O error while saving transaction: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Unexpected error while saving transaction: {ex.Message}");
            }
        }

        public List<Transaction> LoadTransactions()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    return new List<Transaction>();
                }

                string json = File.ReadAllText(_filePath);
                return JsonConvert.DeserializeObject<List<Transaction>>(json) ?? new List<Transaction>();
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"❌ File I/O error while loading transactions: {ioEx.Message}");
                return new List<Transaction>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Unexpected error while loading transactions: {ex.Message}");
                return new List<Transaction>();
            }
        }

        /// <summary>
        /// Clears the entire transaction history.
        /// </summary>
        public void ClearHistory()
        {
            try
            {
                lock (_lock)
                {
                    File.WriteAllText(_filePath, "[]");
                }
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"File I/O error while clearing transactions: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while clearing transactions: {ex.Message}");
            }
        }
    }
}