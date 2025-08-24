using System.Collections.Generic;
public abstract class BasePile
{
    private readonly List<CardData> _cards = new();
    public List<CardData> Cards => _cards;

    public BasePile(List<CardData> cards)
    {
        _cards = cards;
    }
    public abstract bool CanAddCard(CardData card);

    public virtual void AddCard(CardData card)
    {
        if (!ContainsCard(card) && card != null)
        {
            _cards.Add(card);
        }
        else
        {
            return;
        }
    }
    public bool HasCard()
    {
        return _cards.Count > 0;
    }
    private bool ContainsCard(CardData card)
    {
        return _cards.Contains(card);
    }
    public CardData GetTopCard()
    {
        return Cards.Count > 0 ? Cards[^1] : null;
    }
    public virtual void RemoveCard(CardData card)
    {
        if (ContainsCard(card) && card != null)
            _cards.Remove(card);
    }
    public void AddCards(IList<CardData> cards)
    {
        if (cards == null)
            return;

        for (var i = 0; i < cards.Count; i++)
            AddCard(cards[i]);
    }
    public void RemoveCards(IList<CardData> cards)
    {
        if (cards == null)
            return;

        for (var i = 0; i < cards.Count; i++)
            RemoveCard(cards[i]);
    }
    public virtual IEnumerable<CardData> GetAccessibleCards()
    {
        if (!HasCard())
            yield break;
    }
    public IList<CardData> SplitAt(CardData card)
    {
        IList<CardData> splitCards = new List<CardData>();
        var index = _cards.IndexOf(card);

        if (index < 0 || index >= _cards.Count) return null;

        splitCards.Clear();

        for (var i = index; i < _cards.Count; i++)
        {
            splitCards.Add(_cards[i]);
        }

        return splitCards;
    }
}
