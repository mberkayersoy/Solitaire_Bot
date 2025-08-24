public class RefillStockCommand : ICommand
{
    private readonly StockPile _stockPile;
    private readonly WastePile _wastePile;
    public RefillStockCommand(StockPile stockPile, WastePile wastePile)
    {
        _stockPile = stockPile;
        _wastePile = wastePile;
    }
    public MoveType MoveType
    {
        get
        {
            return MoveType.Refill;
        }
    }
    public bool IsApplied { get; set; }
    public void Apply()
    {
        IsApplied = true;
        while (_wastePile.HasCard())
        {
            var card = _wastePile.GetTopCard();
            _wastePile.RemoveCard(card);
            _stockPile.AddCard(card);
        }
    }
    public void Undo()
    {
        while (_stockPile.HasCard())
        {
            var card = _stockPile.GetTopCard();
            _stockPile.RemoveCard(card);
            _wastePile.AddCard(card);
        }
        IsApplied = false;
    }
}