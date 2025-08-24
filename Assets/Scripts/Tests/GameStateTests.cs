using NUnit.Framework;
[TestFixture]
public class GameStateTests
{
    private Deck _fullDeck;
    private Deck _customDeck;
    [SetUp]
    public void Setup()
    {
        _fullDeck = new Deck();

        string input = "AC,2C,3C,4C,5C,6C,7C,8C,9C,10C,JC,QC,KC," +
                       "AD,2D,3D,4D,5D,6D,7D,8D,9D,10D,JD,QD,KD," +
                       "AH,2H,3H,4H,5H,6H,7H,8H,9H,10H,JH,QH,KH," +
                       "AS,2S,3S,4S,5S,6S,7S,8S,9S,10S,JS,QS,KS";
        _customDeck = new Deck(input);
    }
    [Test]
    public void GameState_CreatesTableausCorrectly()
    {
        GameState state = new GameState(_fullDeck, DrawType.Single);
        Assert.AreEqual(7, state.Tableaus.Count);

        int expectedCount = 1;
        foreach (var tableau in state.Tableaus)
        {
            Assert.AreEqual(expectedCount, tableau.Cards.Count);
            expectedCount++;
        }
    }
    [Test]
    public void GameState_CreatesFoundationsCorrectly()
    {
        GameState state = new GameState(_fullDeck, DrawType.Single);
        Assert.AreEqual(4, state.Foundations.Count);

        foreach (var kv in state.Foundations)
        {
            Assert.IsEmpty(kv.Value.Cards);
        }
    }
    [Test]
    public void GameState_CreatesWasteAndStockCorrectly()
    {
        GameState state = new GameState(_fullDeck, DrawType.Single);
        int totalTableauCards = 0;
        foreach (var tableau in state.Tableaus)
            totalTableauCards += tableau.Cards.Count;

        int expectedStockCards = 52 - totalTableauCards;
        Assert.AreEqual(expectedStockCards, state.Stock.Cards.Count);
        Assert.IsEmpty(state.Waste.Cards);
    }
    [Test]
    public void GetUniqueHash_DifferentDecks_HaveDifferentHashes()
    {
        GameState state1 = new GameState(_fullDeck, DrawType.Single);
        GameState state2 = new GameState(_customDeck, DrawType.Single);

        ulong hash1 = state1.GetUniqueHash();
        ulong hash2 = state2.GetUniqueHash();

        Assert.AreNotEqual(hash1, hash2);
    }
    [Test]
    public void GetUniqueHash_SameDeck_HashIsConsistent()
    {
        GameState state = new GameState(_fullDeck, DrawType.Single);
        ulong hash1 = state.GetUniqueHash();
        ulong hash2 = state.GetUniqueHash();

        Assert.AreEqual(hash1, hash2);
    }
}
