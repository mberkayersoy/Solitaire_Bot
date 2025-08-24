using System;
using System.Collections.Generic;

public class GameState
{
    private readonly Deck _deck;
    public DrawType DrawType;
    public List<TableauPile> Tableaus { get; private set; }
    public Dictionary<Suit, FoundationPile> Foundations { get; private set; }
    public WastePile Waste { get; private set; }
    public StockPile Stock { get; private set; }
    public Deck Deck => _deck;

    public GameState(Deck deck, DrawType drawType)
    {
        DrawType = drawType;
        _deck = deck;
        DealCards();
    }
    // Instead of directly checking all foundation piles,
    // check if all cards in the tableau are face-up to avoid creating extra states.
    // If all tableau cards are face-up, the game is effectively in a winning state.
    public bool IsWinningState()
    {
        foreach (var tableau in Tableaus)
        {
            foreach (var card in tableau.Cards)
            {
                if (!card.IsFaceUp)
                {
                    return false;
                }
            }
        }
        return true;
    }
    private void DealCards()
    {
        Tableaus = new List<TableauPile>();
        Foundations = new Dictionary<Suit, FoundationPile>();
        Waste = new WastePile(new List<CardData>(), DrawType);
        Stock = new StockPile(new List<CardData>(), DrawType);

        foreach (Suit suit in Enum.GetValues(typeof(Suit)))
        {
            Foundations.Add(suit, new FoundationPile(new List<CardData>(), suit));
        }

        int cardIndex = 51;
        for (int i = 0; i < 7; i++)
        {
            var pileCards = new List<CardData>();
            for (int j = 0; j <= i; j++)
            {
                var card = _deck.DeckCards[cardIndex--];
                card.IsFaceUp = j == i;
                pileCards.Add(card);
            }
            Tableaus.Add(new TableauPile(pileCards));
        }

        while (cardIndex >= 0)
        {
            Stock.AddCard(_deck.DeckCards[cardIndex--]);
        }
    }
    public ulong GetUniqueHash()
    {
        ulong hash = 0;
        int shift = 0;

        void AddCardToHash(CardData c)
        {
            ulong val = ((ulong)c.Rank & 0xF) | (((ulong)c.Suit & 0x3) << 4) | ((c.IsFaceUp ? 1UL : 0UL) << 6);
            hash ^= val << shift;

            shift += 7;
            if (shift >= 64) shift = 0;
        }

        foreach (var tableau in Tableaus)
            foreach (var c in tableau.Cards)
                AddCardToHash(c);

        foreach (var pile in Foundations.Values)
            foreach (var c in pile.Cards)
                AddCardToHash(c);

        foreach (var c in Waste.Cards)
            AddCardToHash(c);

        foreach (var c in Stock.Cards)
            AddCardToHash(c);

        return hash;
    }
}
