using NUnit.Framework;

[TestFixture]
public class DeckSolverTests
{
    [Test]
    public void Solvable_WhenAllCardsFaceUp_ReturnsTrue()
    {
        var deck = new Deck(42);
        var gameState = new GameState(deck, DrawType.Single);

        foreach (var tableau in gameState.Tableaus)
        {
            foreach (var card in tableau.Cards)
            {
                card.IsFaceUp = true;
            }
        }
        var solver = new DeckSolver(gameState);

        bool result = solver.IsSolvable();

        Assert.IsTrue(result);
    }
    [Test]
    public void Unsolvable_WhenAllCardsFaceDown_ReturnsFalse()
    {
        var deck = new Deck(99);
        var gameState = new GameState(deck, DrawType.Single);

        foreach (var tableau in gameState.Tableaus)
        {
            foreach (var card in tableau.Cards)
            {
                card.IsFaceUp = false;
            }
        }

        var solver = new DeckSolver(gameState);

        bool result = solver.IsSolvable();

        Assert.IsFalse(result, "Can't solve all cards is face down.");
    }

    [Test]
    public void IsSolvable_ShouldVisitMultipleStates()
    {
        var deck = new Deck(123);
        var gameState = new GameState(deck, DrawType.Single);

        var solver = new DeckSolver(gameState);

        solver.IsSolvable();


        Assert.Greater(solver.CurrentGameState.GetUniqueHash(), 0UL);
    }

    [Test]
    public void Solver_WithSameSeed_ShouldSameResult()
    {
        var deck1 = new Deck(777);
        var deck2 = new Deck(777);

        var gameState1 = new GameState(deck1, DrawType.Single);
        var gameState2 = new GameState(deck2, DrawType.Single);

        var solver1 = new DeckSolver(gameState1);
        var solver2 = new DeckSolver(gameState2);

        var result1 = solver1.IsSolvable();
        var result2 = solver2.IsSolvable();

        Assert.AreEqual(result1, result2, "With the same seed, the results should be the same.");
    }
    [Test]
    public void Solvable_UsingParseDeckFromString_ReturnsTrue()
    {
        string solvableDeckString = "9S,9C,5D,KH,AD,2C,4H,8H,5H,10D,KC,6D,KS,4C,QH,5C,QC,7H,AS,8C,AC,6S,3D,6C,JD,4D,3C,10C,9D,5S,7S,3S,3H,QD,JS,KD,JH,8S,7D,4S,2H,7C,AH,10H,QS,JC,2D,6H,10S,9H,8D,2S";

        var allCards = CardExtension.ParseDeckFromString(solvableDeckString);

        var gameState = new GameState(new Deck(allCards), DrawType.Single);

        var solver = new DeckSolver(gameState);

        bool result = solver.IsSolvable();

        Assert.IsTrue(result);
    }
}
