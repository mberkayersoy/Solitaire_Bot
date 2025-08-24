using System.Collections.Generic;
public class TableauPile : BasePile
{
    public TableauPile(List<CardData> cards) : base(cards) { }
    public override bool CanAddCard(CardData card)
    {
        if (card == null)
        {
            return false;
        }

        if (!HasCard() && card.Rank == Rank.King)
        {
            return true;
        }

        var topCard = GetTopCard();
        if (topCard != null && (int)topCard.Rank == (int)card.Rank + 1 && topCard.Suit.IsOppositeColor(card.Suit))
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
        if (HasCard())
            for (int i = Cards.Count - 1; i > 0; i--)
            {
                if (Cards[i].IsFaceUp)
                    yield return Cards[i];
            }
    }
}