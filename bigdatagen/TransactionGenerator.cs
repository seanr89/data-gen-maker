
public class TransactionGenerator
{
    private readonly string[] _transactionTypes = ["Credit", "Debit", "Transfer"];
    private readonly string[] _cardTypes = ["Visa", "Mastercard", "Amex", "Discover"];
    List<string> _reversalIndicators = ["Y", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N"];
    
    public TransactionGenerator(string[] transactionTypes, string[] cardTypes)
    {
        _transactionTypes = transactionTypes;
        _cardTypes = cardTypes;
    }

    public void setRefundIndicator(int percentage)
    {
        // add 100 - percentage of N to a list of strings
        // add percentage of Y to a list of strings
        // add the list of strings to _reversalIndicators

        var newList = new List<string>();
        for (int i = 0; i < 100 - percentage; i++)
        {
            newList.Add("N");
        }
        for (int i = 0; i < percentage; i++)
        {
            newList.Add("Y");
        }
        _reversalIndicators = newList;
    }

    public IEnumerable<Transaction> GenerateBatch(int batchCount = 0)
    {
        var result = new List<Transaction>();

        return result;
    }


}