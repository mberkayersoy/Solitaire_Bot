using NUnit.Framework;
[TestFixture]
public class DeckTests
{
    [Test]
    public void DefaultDeck_Has52Cards()
    {
        Deck deck = new Deck();
        Assert.AreEqual(52, deck.DeckCards.Count);
    }
    [Test]
    public void ParseDeckFromString_CreatesCorrectDeck()
    {
        string input = "AC,2C,3C,4C,5C,6C,7C,8C,9C,10C,JC,QC,KC," +
                       "AD,2D,3D,4D,5D,6D,7D,8D,9D,10D,JD,QD,KD," +
                       "AH,2H,3H,4H,5H,6H,7H,8H,9H,10H,JH,QH,KH," +
                       "AS,2S,3S,4S,5S,6S,7S,8S,9S,10S,JS,QS,KS";

        Deck deck = new Deck(input);

        Assert.AreEqual(52, deck.DeckCards.Count);
        Assert.AreEqual(Rank.Ace, deck.DeckCards[0].Rank);
        Assert.AreEqual(Suit.Diamonds, deck.DeckCards[15].Suit);
        Assert.AreEqual(Rank.King, deck.DeckCards[12].Rank);
    }
    [Test]
    public void Shuffle_WithSameSeed_IsDeterministic()
    {
        Deck deck1 = new Deck(42);
        Deck deck2 = new Deck(42);

        for (int i = 0; i < deck1.DeckCards.Count; i++)
        {
            Assert.AreEqual(deck1.DeckCards[i].Rank, deck2.DeckCards[i].Rank);
            Assert.AreEqual(deck1.DeckCards[i].Suit, deck2.DeckCards[i].Suit);
        }
    }
    [Test]
    public void ParseDeckWithFullDeckString_Has52Cards()
    {
        string fullDeck = "AC,2C,3C,4C,5C,6C,7C,8C,9C,10C,JC,QC,KC," +
                          "AD,2D,3D,4D,5D,6D,7D,8D,9D,10D,JD,QD,KD," +
                          "AH,2H,3H,4H,5H,6H,7H,8H,9H,10H,JH,QH,KH," +
                          "AS,2S,3S,4S,5S,6S,7S,8S,9S,10S,JS,QS,KS";

        Deck deck = new Deck(fullDeck);
        Assert.AreEqual(52, deck.DeckCards.Count);
    }
}
