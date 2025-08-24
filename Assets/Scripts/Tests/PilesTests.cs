using NUnit.Framework;
using System.Collections.Generic;
[TestFixture]
public class PileTests
{
    [Test]
    public void FoundationPile_CanAddCard()
    {
        var pile = new FoundationPile(new List<CardData>(), Suit.Hearts);

        var ace = new CardData(Rank.Ace, Suit.Hearts);
        var two = new CardData(Rank.Two, Suit.Hearts);
        var wrongSuit = new CardData(Rank.Ace, Suit.Spades);

        Assert.IsTrue(pile.CanAddCard(ace));
        pile.AddCard(ace);
        Assert.IsTrue(pile.CanAddCard(two));
        Assert.IsFalse(pile.CanAddCard(wrongSuit));
    }
    [Test]
    public void StockPile_CanAddCard()
    {
        var pile = new StockPile(new List<CardData>());
        var card = new CardData(Rank.Ace, Suit.Clubs);
        Assert.IsFalse(pile.CanAddCard(card));
        pile.AddCard(card);
        Assert.IsFalse(card.IsFaceUp);
    }
    [Test]
    public void TableauPile_CanAddSingleCard()
    {
        var pile = new TableauPile(new List<CardData>());
        var king = new CardData(Rank.King, Suit.Spades);
        var queenRed = new CardData(Rank.Queen, Suit.Hearts);

        Assert.IsTrue(pile.CanAddCard(king));
        pile.AddCard(king);
        Assert.IsTrue(pile.CanAddCard(queenRed));
    }
    [Test]
    public void TableauPile_AddMultipleCards_AndSplitAt()
    {
        var pile = new TableauPile(new List<CardData>());

        var king = new CardData(Rank.King, Suit.Spades);
        var queenRed = new CardData(Rank.Queen, Suit.Hearts);
        var jackBlack = new CardData(Rank.Jack, Suit.Clubs);


        Assert.IsTrue(pile.CanAddCard(king));
        pile.AddCard(king);
        Assert.AreEqual(1, pile.Cards.Count);

        Assert.IsTrue(pile.CanAddCard(queenRed));
        pile.AddCard(queenRed);
        Assert.AreEqual(2, pile.Cards.Count);

        Assert.IsTrue(pile.CanAddCard(jackBlack));
        pile.AddCard(jackBlack);
        Assert.AreEqual(3, pile.Cards.Count);

        var splitCards = pile.SplitAt(queenRed);
        Assert.IsNotNull(splitCards);
        Assert.AreEqual(2, splitCards.Count);
        Assert.AreEqual(queenRed, splitCards[0]);
        Assert.AreEqual(jackBlack, splitCards[1]);
    }
    [Test]
    public void WastePile_AddCard()
    {
        var pile = new WastePile(new List<CardData>(), DrawType.Single);
        var card = new CardData(Rank.Ace, Suit.Clubs);

        pile.AddCard(card);
        Assert.IsTrue(card.IsFaceUp);
    }
    [Test]
    public void StockPile_GetAccessibleCards_SingleAndThreeDraw()
    {
        var ace = new CardData(Rank.Ace, Suit.Hearts);
        var two = new CardData(Rank.Two, Suit.Spades);
        var three = new CardData(Rank.Three, Suit.Diamonds);
        var four = new CardData(Rank.Four, Suit.Diamonds);
        var five = new CardData(Rank.Five, Suit.Clubs);


        var stockSingle = new StockPile(new List<CardData> { ace, two, three, four, five }, DrawType.Single);
        var singleCards = new List<CardData>(stockSingle.GetAccessibleCards());
        CollectionAssert.AreEqual(new List<CardData> { ace, two, three, four, five }, singleCards);


        var stockThree = new StockPile(new List<CardData> { ace, two, three, four, five }, DrawType.Three);
        var threeCards = new List<CardData>(stockThree.GetAccessibleCards());
        CollectionAssert.AreEqual(new List<CardData> { five, two }, threeCards);

        var wasteSingle = new WastePile(new List<CardData> { ace, two, three, four, five }, DrawType.Single);
        var singleCardsWaste = new List<CardData>(wasteSingle.GetAccessibleCards());
        CollectionAssert.AreEqual(new List<CardData> { five, four, three, two, ace }, singleCardsWaste);


        var wasteThree = new WastePile(new List<CardData> { ace, two, three, four, five }, DrawType.Three);
        var threeCardsWaste = new List<CardData>(wasteThree.GetAccessibleCards());
        CollectionAssert.AreEqual(new List<CardData> { five, two }, threeCardsWaste);
    }
}
