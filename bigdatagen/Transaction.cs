

/// <summary>
/// First pass at a simple transaction model
/// </summary>
/// <value></value>
public record Transaction
{
    public Guid TransactionId { get; init; }
    public Guid AccountId { get; init; }
    public required Guid CustomerId { get; init; }
    public DateTime TransactionDate { get; init; }
    public string TransactionType { get; init; } = default!;
    public string TransactionCurrency { get; init; } = default!;
    public double TransactionAmount { get; init; }
    public string TransactionNarrative { get; init; }
    public string ReversalIndicator { get; init; }
    public string MID { get; init; }
    public string CardType { get; init; }
}