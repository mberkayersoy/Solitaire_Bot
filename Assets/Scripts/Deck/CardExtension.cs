using System;
using System.Collections.Generic;
public static class CardExtension
{
    public static CardColor ColorOf(this Suit suit)
    {
        switch (suit)
        {
            default:
            case Suit.Hearts:
                return CardColor.Red;
            case Suit.Diamonds:
                return CardColor.Red;
            case Suit.Clubs:
                return CardColor.Black;
            case Suit.Spades:
                return CardColor.Black;
        }
    }
    public static bool IsOppositeColor(this Suit suit, Suit other)
    {
        return suit.ColorOf() != other.ColorOf();
    }
    public static List<CardData> ParseDeckFromString(string input)
    {
        var deck = new List<CardData>();
        var cards = input.Split(',', StringSplitOptions.RemoveEmptyEntries);

        foreach (var cardStr in cards)
        {
            var trimmed = cardStr.Trim();

            string rankPart = trimmed.Length == 3 ? trimmed.Substring(0, 2) : trimmed.Substring(0, 1);
            string suitPart = trimmed.Substring(trimmed.Length - 1, 1);

            Rank rank = rankPart switch
            {
                "A" => Rank.Ace,
                "2" => Rank.Two,
                "3" => Rank.Three,
                "4" => Rank.Four,
                "5" => Rank.Five,
                "6" => Rank.Six,
                "7" => Rank.Seven,
                "8" => Rank.Eight,
                "9" => Rank.Nine,
                "10" => Rank.Ten,
                "J" => Rank.Jack,
                "Q" => Rank.Queen,
                "K" => Rank.King,
                _ => throw new Exception("Unknown rank: " + rankPart)
            };

            Suit suit = suitPart switch
            {
                "C" => Suit.Clubs,
                "D" => Suit.Diamonds,
                "H" => Suit.Hearts,
                "S" => Suit.Spades,
                _ => throw new Exception("Unknown suit: " + suitPart)
            };

            deck.Add(new CardData(rank, suit));
        }

        return deck;
    }
    public static string DeckToString(List<CardData> deck)
    {
        if (deck == null || deck.Count == 0) return string.Empty;

        var sb = new System.Text.StringBuilder();

        for (int i = 0; i < deck.Count; i++)
        {
            var card = deck[i];

            string rankStr = card.Rank switch
            {
                Rank.Ace => "A",
                Rank.Two => "2",
                Rank.Three => "3",
                Rank.Four => "4",
                Rank.Five => "5",
                Rank.Six => "6",
                Rank.Seven => "7",
                Rank.Eight => "8",
                Rank.Nine => "9",
                Rank.Ten => "10",
                Rank.Jack => "J",
                Rank.Queen => "Q",
                Rank.King => "K",
                _ => throw new Exception("Unknown rank: " + card.Rank)
            };

            string suitStr = card.Suit switch
            {
                Suit.Clubs => "C",
                Suit.Diamonds => "D",
                Suit.Hearts => "H",
                Suit.Spades => "S",
                _ => throw new Exception("Unknown suit: " + card.Suit)
            };

            sb.Append(rankStr + suitStr);

            if (i < deck.Count - 1)
                sb.Append(",");
        }

        return sb.ToString();
    }

}