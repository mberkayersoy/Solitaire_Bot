using System.Collections.Generic;
using System;
public class Deck
{
    private List<CardData> _deckCards;
    public List<CardData> DeckCards { get => _deckCards; private set => _deckCards = value; }

    public Deck(int? seed = null)
    {
        CreateDeck();
        Shuffle(_deckCards, seed);
    }
    public Deck(string input)
    {
        _deckCards = CardExtension.ParseDeckFromString(input);
    }
    public Deck(List<CardData> cards)
    {
        _deckCards = cards;
    }
    private void CreateDeck()
    {
        _deckCards = new List<CardData>();

        foreach (Suit suit in Enum.GetValues(typeof(Suit)))
        {
            foreach (Rank rank in Enum.GetValues(typeof(Rank)))
            {
                _deckCards.Add(new CardData(rank, suit));
            }
        }
    }
    public void Shuffle(IList<CardData> list, int? seed = null)
    {
        Random rng = seed.HasValue ? new Random(seed.Value) : new Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            CardData value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
