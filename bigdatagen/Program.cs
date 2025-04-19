// See https://aka.ms/new-console-template for more information
using Bogus;

int totalTransactionLoops = 20;
int createdCount = 0;
for (int i = 0; i < totalTransactionLoops; i++)
{
    Console.WriteLine($"Transaction Loop {i + 1}");
    var transactions = CreateTransactions(7500);
    FileWriter.TryWriteOrAppendToFile(transactions);
    createdCount += transactions.Count();
}
Console.WriteLine($"Total Transactions Created: {createdCount}");

/// <summary>
/// Static step to build a set of transactions.
/// This is a simple transaction model with a few fields.
/// </summary>
/// <param name="count"></param>
/// <returns></returns>
static IEnumerable<Transaction> CreateTransactions(int count = 7500)
{
    string[] _transactionTypes = ["Credit", "Debit", "Transfer"];
    string[] _cardTypes = ["Visa", "Mastercard", "Amex", "Discover"];
    string[] _reversalIndicators = ["Y", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N"];
    IEnumerable<Transaction> transactions = new List<Transaction>();

    DateTime startDate = DateTime.Now.AddDays(-100);
    DateTime endDate = DateTime.Now;
    Random random = new Random();

    var transFaker = new Faker<Transaction>()
        .RuleFor(t => t.TransactionId, f => f.Random.Guid())
        .RuleFor(t => t.AccountId, f => f.Random.Guid())
        .RuleFor(t => t.TransactionAmount, f => Math.Round(f.Random.Double(1, 375), 2))
        .RuleFor(t => t.TransactionDate, f => f.Date.Between(startDate, endDate))
        .RuleFor(t => t.TransactionType, f => f.PickRandom(_transactionTypes))
        .RuleFor(t => t.TransactionNarrative, f => f.Lorem.Word())
        .RuleFor(t => t.ReversalIndicator, f => f.PickRandom(_reversalIndicators))
        .RuleFor(t => t.MID, f => f.Random.AlphaNumeric(15))
        .RuleFor(t => t.CardType, f => f.PickRandom(_cardTypes))
        .RuleFor(t => t.TransactionCurrency, f => f.Finance.Currency().Code);
    
    transactions = transFaker.Generate(count);

    return transactions;
}

var fileData = File.ReadAllLines("./files/transactions.csv");
int totalLines = fileData.Count();
Console.WriteLine($"Total Lines in File: {totalLines:N0} with 1 header line.");