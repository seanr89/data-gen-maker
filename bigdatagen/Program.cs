// See https://aka.ms/new-console-template for more information
using Bogus;

int totalTransactionLoops = 750;
for (int i = 0; i < totalTransactionLoops; i++)
{
    Console.WriteLine($"Transaction Loop {i + 1}");
    var transactions = CreateTransactions(2500);
    FileWriter.TryWriteOrAppendToFile(transactions);
}


static IEnumerable<Transaction> CreateTransactions(int count = 1500)
{
    string[] _transactionTypes = ["Credit", "Debit", "Transfer"];
    IEnumerable<Transaction> transactions = new List<Transaction>();

    Random random = new Random();

    var transFaker = new Faker<Transaction>()
        .RuleFor(t => t.TransactionId, f => f.IndexGlobal)
        .RuleFor(t => t.AccountId, f => f.Random.Guid())
        .RuleFor(t => t.TransactionAmount, f => Math.Round(f.Random.Double(1, 275), 2))
        .RuleFor(t => t.TransactionDate, f => f.Date.Past())
        .RuleFor(t => t.TransactionType, f => f.PickRandom(_transactionTypes))
        .RuleFor(t => t.TransactionNarrative, f => f.Lorem.Sentence())
        .RuleFor(t => t.TransactionCurrency, f => f.Finance.Currency().Code);
    
    transactions = transFaker.Generate(count);

    return transactions;
}