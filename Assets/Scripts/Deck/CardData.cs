public class CardData
{
    public Rank Rank;
    public Suit Suit;
    public bool IsFaceUp;
    public readonly string Name;
    public readonly CardColor Color;

    public CardData(Rank rank, Suit suit, bool isFaceUp = false)
    {
        Rank = rank;
        Suit = suit;
        Color = Suit.ColorOf();
        Name = $"{suit} {rank}";
        IsFaceUp = isFaceUp;
    }
}

