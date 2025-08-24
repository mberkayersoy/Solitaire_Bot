
using System.Collections.Generic;
public class StockPile : BasePile
{
    public readonly DrawType DrawType;
    public StockPile(List<CardData> cards, DrawType drawType = DrawType.Single) : base(cards)
    {
        DrawType = drawType;
    }
    public override void AddCard(CardData card)
    {
        base.AddCard(card);
        card.IsFaceUp = false;
    }
    public override void RemoveCard(CardData card)
    {
        card.IsFaceUp = true;
        base.RemoveCard(card);
    }
    public override bool CanAddCard(CardData card)
    {
        return false;
    }
    public override IEnumerable<CardData> GetAccessibleCards()
    {
        if (!HasCard())
            yield break;

        switch (DrawType)
        {
            case DrawType.Single:
                foreach (var card in Cards)
                    yield return card;
                break;

            case DrawType.Three:
                for (int i = Cards.Count - 1; i >= 0; i -= 3)
                {
                    yield return Cards[i];
                }
                break;
        }
    }
}