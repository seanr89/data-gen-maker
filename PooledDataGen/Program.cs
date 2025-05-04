using Bogus;

await Main();

/// <summary>
/// Main App Runner function
/// </summary>
/// <returns></returns>
static async Task Main()
{
    // Get the number of available worker threads in the thread pool.
    ThreadPool.GetMaxThreads(out int workerThreads, out int completionPortThreads);
    Console.WriteLine($"Maximum worker threads: {workerThreads}, completion port threads: {completionPortThreads}");

    // Get the current number of active threads in the thread pool.
    ThreadPool.GetAvailableThreads(out int availableWorkerThreads, out int availableCompletionPortThreads);
    Console.WriteLine($"Available worker threads: {availableWorkerThreads}, completion port threads: {availableCompletionPortThreads}");

    // Create a list to store the tasks.
    var tasks = new System.Collections.Generic.List<Task>();

    // Queue some work items to the thread pool.
    Console.WriteLine("Queuing tasks to the thread pool...");
    for (int i = 0; i < 5; i++)
    {
        int taskNumber = i; // Capture the loop variable.
        // Start the task using Task.Run and add it to the list.
        Task task = Task.Run(() => WorkerThreadAsync(taskNumber));
        tasks.Add(task);
        Console.WriteLine($"Task {taskNumber} queued.");
    }

    // Get the current number of active threads in the thread pool after adding tasks.
    ThreadPool.GetAvailableThreads(out availableWorkerThreads, out availableCompletionPortThreads);
    //Console.WriteLine($"Available worker threads after queueing: {availableWorkerThreads}, completion port threads: {availableCompletionPortThreads}");

    // Wait for all tasks to complete using Task.WhenAll.
    Console.WriteLine("Waiting for tasks to complete...");
    await Task.WhenAll(tasks);

    Console.WriteLine("All tasks completed. Exiting.");
    Console.ReadKey(); // Keep the console window open.
}

/// <summary>
/// 
/// </summary>
/// <param name="state"></param>
/// <returns></returns>
static async Task WorkerThreadAsync(object state)
{
    // Convert the state object to the expected type.  In this case, an integer.
    int taskNumber = (int)state;

    // Simulate some work using Task.Delay, which is async-friendly.
    Console.WriteLine($"Worker thread processing task: {taskNumber}, Thread ID: {Thread.CurrentThread.ManagedThreadId}");
    try
    {
        //await Task.Delay(2000); // Wait for 2 seconds asynchronously.
        await Task.Run(() =>
        {
            int totalRecordCount = 5000000;
            int batchSize = 20000;
            int batchCount = totalRecordCount / batchSize;
            for (int i = 0; i < batchCount; i++)
            {
                // Simulate processing each batch
                //Console.WriteLine($"Processing batch {i + 1} of {batchCount} in task {taskNumber}");
                var transactions = CreateTransactions(batchSize);
                FileWriter.TryWriteOrAppendToFile(transactions, $"transactions_{taskNumber}.csv");
            }
        });
    }
    catch (ThreadInterruptedException)
    {
        Console.WriteLine($"Thread was interrupted while processing task: {taskNumber}");
        return; // Important: Exit the method if interrupted.
    }
    Console.WriteLine($"Worker thread finished processing task: {taskNumber}, Thread ID: {Thread.CurrentThread.ManagedThreadId}");
}

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

