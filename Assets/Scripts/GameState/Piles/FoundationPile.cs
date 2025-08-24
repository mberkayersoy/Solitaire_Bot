using System.Collections.Generic;
public class FoundationPile : BasePile
{
    public readonly Suit FoundationSuit;
    public FoundationPile(List<CardData> cards, Suit foundationSuit) : base(cards)
    {
        FoundationSuit = foundationSuit;
    }
    public override bool CanAddCard(CardData card)
    {
        if (card == null)
        {
            return false;
        }

        if (!HasCard() && card.Suit == FoundationSuit && card.Rank == Rank.Ace)
        {
            return true;
        }
        else if (HasCard() && card.Suit == FoundationSuit && (int)GetTopCard().Rank == (int)card.Rank - 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public override IEnumerable<CardData> GetAccessibleCards()
    {
        if (!HasCard())
            yield break;
        yield return GetTopCard();
    }
}