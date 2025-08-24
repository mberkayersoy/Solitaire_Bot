using System.Collections.Generic;
public class WastePile : BasePile
{
    public readonly DrawType DrawType;
    public WastePile(List<CardData> cards, DrawType drawType) : base(cards)
    {
        DrawType = drawType;
    }
    public override void AddCard(CardData card)
    {
        base.AddCard(card);
        card.IsFaceUp = true;
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
                for (int i = Cards.Count - 1; i >= 0; i--)
                    yield return Cards[i];
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