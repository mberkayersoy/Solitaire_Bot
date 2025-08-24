using NUnit.Framework;
using System.Collections.Generic;

[TestFixture]
public class CommandTests
{
    private TableauPile _tableau;
    private FoundationPile _foundation;
    private StockPile _stock;
    private WastePile _waste;
    private CardData _kingSpades, _queenHearts, _aceClubs;

    [SetUp]
    public void Setup()
    {
        _kingSpades = new CardData(Rank.King, Suit.Spades);
        _queenHearts = new CardData(Rank.Queen, Suit.Hearts);
        _aceClubs = new CardData(Rank.Ace, Suit.Clubs);
        _tableau = new TableauPile(new List<CardData>());
        _foundation = new FoundationPile(new List<CardData>(), Suit.Clubs);
        _stock = new StockPile(new List<CardData> { _aceClubs, _queenHearts, _kingSpades }, DrawType.Single);
        _waste = new WastePile(new List<CardData>(), DrawType.Single);
    }

    [Test]
    public void MoveCardCommand_TableauToFoundation_ApplyUndo()
    {
        _tableau.AddCard(_aceClubs);
        var cmd = new MoveCardCommand(_aceClubs, _tableau, _foundation);
        cmd.Apply();
        Assert.IsFalse(_tableau.HasCard(), "Tableau should be empty after moving Ace to Foundation.");
        Assert.IsTrue(_foundation.HasCard(), "Foundation should have the Ace.");
        Assert.AreEqual(_aceClubs, _foundation.GetTopCard(), "Foundation should have the Ace as top card.");
        cmd.Undo();
        Assert.IsTrue(_tableau.HasCard(), "Tableau should have the Ace back after undo.");
        Assert.IsFalse(_foundation.HasCard(), "Foundation should be empty after undo.");
    }

    [Test]
    public void MoveCardCommand_WasteToFoundation_ApplyUndo()
    {
        _waste.AddCard(_aceClubs);
        var cmd = new MoveCardCommand(_aceClubs, _waste, _foundation);
        cmd.Apply();
        Assert.IsFalse(_waste.HasCard(), "Waste should be empty after moving Ace to Foundation.");
        Assert.IsTrue(_foundation.HasCard(), "Foundation should have the Ace.");
        Assert.AreEqual(_aceClubs, _foundation.GetTopCard(), "Foundation should have the Ace as top card.");
        cmd.Undo();
        Assert.IsTrue(_waste.HasCard(), "Waste should have the Ace back after undo.");
        Assert.IsFalse(_foundation.HasCard(), "Foundation should be empty after undo.");
    }

    [Test]
    public void MoveCardCommand_TableauToTableau_MultiCard_ApplyUndo()
    {
        var pile2 = new TableauPile(new List<CardData>());
        var jack = new CardData(Rank.Jack, Suit.Diamonds);
        _tableau.AddCard(_kingSpades);
        _tableau.AddCard(_queenHearts);
        _tableau.AddCard(jack);
        var cmd = new MoveCardCommand(_queenHearts, _tableau, pile2);
        cmd.Apply();
        Assert.AreEqual(1, _tableau.Cards.Count, "Tableau should keep King after moving Queen+Jack.");
        Assert.AreEqual(2, pile2.Cards.Count, "Destination tableau should have Queen+Jack.");
        Assert.AreEqual(jack, pile2.GetTopCard(), "Destination tableau should have Jack as top card.");
        cmd.Undo();
        Assert.AreEqual(3, _tableau.Cards.Count, "Tableau should have all 3 cards back after undo.");
        Assert.AreEqual(jack, _tableau.GetTopCard(), "Tableau should have Jack as top card after undo.");
        Assert.AreEqual(0, pile2.Cards.Count, "Destination tableau should be empty after undo.");
    }

    [Test]
    public void RefillStockCommand_ApplyUndo()
    {
        _waste.AddCard(_aceClubs);
        var cmd = new RefillStockCommand(_stock, _waste);
        cmd.Apply();
        Assert.IsFalse(_waste.HasCard(), "Waste should be empty after refill.");
        Assert.IsTrue(_stock.HasCard(), "Stock should have the card after refill.");
        Assert.AreEqual(_aceClubs, _stock.Cards[0], "Stock should have Ace a card.");
        cmd.Undo();
        Assert.IsTrue(_waste.HasCard(), "Waste should have the card back after undo.");
        Assert.IsFalse(_stock.HasCard(), "Stock should be empty after undo.");
    }

    [Test]
    public void CommandHandler_AddAndUndo()
    {
        var pile2 = new TableauPile(new List<CardData>());
        _tableau.AddCard(_kingSpades);
        var cmd = new MoveCardCommand(_kingSpades, _tableau, pile2);
        var handler = new CommandHandler();
        handler.Add(cmd);
        Assert.AreEqual(1, handler.Count, "Handler should have 1 command.");
        handler.Undo();
        Assert.AreEqual(0, handler.Count, "Handler should be empty after undo.");
        Assert.IsTrue(_tableau.HasCard(), "Tableau should have King back after undo.");
    }

    [Test]
    public void DrawCardCommand_SingleDraw_ApplyUndo()
    {
        _stock = new StockPile(new List<CardData> { _kingSpades, _queenHearts, _aceClubs }, DrawType.Single);
        _waste = new WastePile(new List<CardData>(), DrawType.Single);
        _foundation = new FoundationPile(new List<CardData>(), Suit.Clubs);

        var cmd = new DrawCardCommand(_aceClubs, _stock, _waste, _foundation);
        cmd.Apply();

        Assert.AreEqual(2, _stock.Cards.Count, "Stock should have 2 cards left.");
        Assert.IsTrue(_foundation.HasCard() && _foundation.GetTopCard() == _aceClubs, "Ace should be on foundation.");
        Assert.IsFalse(_waste.HasCard(), "Waste should be empty because last drawn went to foundation.");

        cmd.Undo();

        Assert.AreEqual(3, _stock.Cards.Count, "Stock should be fully restored after undo.");
        Assert.AreEqual(_aceClubs, _stock.GetTopCard(), "Top of stock should be Ace after undo (original top).");
        Assert.IsFalse(_waste.HasCard(), "Waste should be empty after undo.");
        Assert.IsFalse(_foundation.HasCard(), "Foundation should be empty after undo.");
    }

    [Test]
    public void DrawCardCommand_ThreeDraw_ApplyUndo()
    {
        _stock = new StockPile(new List<CardData> { _aceClubs, _queenHearts, _kingSpades }, DrawType.Three);
        _waste = new WastePile(new List<CardData>(), DrawType.Three);
        _foundation = new FoundationPile(new List<CardData>(), Suit.Clubs);

        var cmd = new DrawCardCommand(_aceClubs, _stock, _waste, _foundation);
        cmd.Apply();

        Assert.AreEqual(0, _stock.Cards.Count, "Stock should be empty after drawing 3 from 3.");
        Assert.AreEqual(2, _waste.Cards.Count, "Waste should have first two drawn cards.");
        Assert.AreEqual(_queenHearts, _waste.GetTopCard(), "Top of waste should be Queen (drawn second).");
        Assert.IsTrue(_foundation.HasCard() && _foundation.GetTopCard() == _aceClubs, "Ace should be on foundation.");

        cmd.Undo();

        Assert.AreEqual(3, _stock.Cards.Count, "Stock should be fully restored after undo.");
        Assert.AreEqual(_kingSpades, _stock.GetTopCard(), "Top of stock should be King after undo (original top).");
        Assert.IsFalse(_waste.HasCard(), "Waste should be empty after undo.");
        Assert.IsFalse(_foundation.HasCard(), "Foundation should be empty after undo.");
    }

    [Test]
    public void DrawCardCommand_CardNotInStock_ApplyUndo()
    {
        var notInStock = new CardData(Rank.Ten, Suit.Hearts);
        var cmd = new DrawCardCommand(notInStock, _stock, _waste, _foundation);
        cmd.Apply();

        Assert.AreEqual(3, _stock.Cards.Count, "Stock should remain unchanged.");
        Assert.IsFalse(_waste.HasCard(), "Waste should remain empty.");
        Assert.IsFalse(_foundation.HasCard(), "Foundation should remain empty.");

        cmd.Undo();
        Assert.AreEqual(3, _stock.Cards.Count, "Stock should remain unchanged after undo.");
        Assert.IsFalse(_waste.HasCard(), "Waste should remain empty after undo.");
        Assert.IsFalse(_foundation.HasCard(), "Foundation should remain empty after undo.");
    }

    [Test]
    public void DrawCardCommand_EmptyStock_ApplyUndo()
    {
        DrawType drawType = DrawType.Single;
        _stock = new StockPile(new List<CardData>(), drawType);
        var cmd = new DrawCardCommand(_aceClubs, _stock, _waste, _foundation);
        cmd.Apply();
        Assert.IsFalse(_stock.HasCard(), "Stock should remain empty.");
        Assert.IsFalse(_waste.HasCard(), "Waste should remain empty.");
        Assert.IsFalse(_foundation.HasCard(), "Foundation should remain empty.");
        cmd.Undo();
        Assert.IsFalse(_stock.HasCard(), "Stock should remain empty after undo.");
        Assert.IsFalse(_waste.HasCard(), "Waste should remain empty after undo.");
        Assert.IsFalse(_foundation.HasCard(), "Foundation should remain empty after undo.");
    }
}