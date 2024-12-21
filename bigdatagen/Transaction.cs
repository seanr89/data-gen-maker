

public record Transaction
{
    public int TransactionId { get; init; }
    public Guid AccountId { get; init; }
    public DateTime TransactionDate { get; init; }
    public string TransactionType { get; init; }
    public string TransactionCurrency { get; init; }
    public double TransactionAmount { get; init; }
    public string TransactionNarrative { get; init; }
}