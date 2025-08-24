using System;
using System.Collections.Generic;
using System.Linq;
[Serializable]
public class DeckSolver
{
    public GameState CurrentGameState;
    private HashSet<ulong> _visitedStates = new HashSet<ulong>();
    private readonly CommandHandler _commandHandler;
    private const int MAX_CAPACITY = 500000;
    public HashSet<ulong> VisitedStates { get => _visitedStates; private set => _visitedStates = value; }
    public DeckSolver(GameState gameState)
    {
        CurrentGameState = gameState;
        _commandHandler = new CommandHandler();
    }
    public bool IsSolvable()
    {
        _visitedStates.Clear();
        var result = TrySolveDFS();
        return result;
    }
    private bool TrySolveDFS()
    {
        if (_visitedStates.Count > MAX_CAPACITY)
        {
            return false;
        }

        ulong hash = CurrentGameState.GetUniqueHash();

        if (_visitedStates.Contains(hash))
        {
            return false;
        }
        _visitedStates.Add(hash);

        if (CurrentGameState.IsWinningState())
        {
            return true;
        }

        var moves = FindAllCardsPossibleMovesForState(CurrentGameState).ToList();

        foreach (var move in moves)
        {
            move.Apply();
            _commandHandler.Add(move);

            if (TrySolveDFS())
                return true;

            _commandHandler.Undo();
        }

        return false;
    }
    private IEnumerable<ICommand> FindAllCardsPossibleMovesForState(GameState gameState)
    {
        foreach (var tableau in gameState.Tableaus)
        {
            for (int i = 0; i < tableau.Cards.Count; i++)
            {
                var card = tableau.Cards[i];
                if (!card.IsFaceUp) continue;
                if (IsValidMovableSequence(tableau, i))
                {
                    foreach (var move in FindAllPossibleMovesForCard(card, tableau, gameState)) yield return move;
                }
            }
        }
        if (gameState.Stock.HasCard())
        {
            foreach (var card in gameState.Stock.GetAccessibleCards())
            {
                if (IsMovePossibleForCard(card, gameState.Stock, out BasePile targetPile))
                {
                    yield return new DrawCardCommand(card, gameState.Stock, gameState.Waste, targetPile);
                }
            }
        }
        if (gameState.Waste.HasCard() && !gameState.Stock.HasCard())
        {
            var accessibleCards = gameState.Waste.GetAccessibleCards();

            foreach (var card in accessibleCards)
            {
                if (IsMovePossibleForCard(card, gameState.Waste))
                {
                    yield return new RefillStockCommand(gameState.Stock, gameState.Waste);
                    break;
                }
            }
        }
        foreach (var kv in gameState.Foundations)
        {
            foreach (var move in FindAllPossibleMovesForCard(kv.Value.GetTopCard(), kv.Value, gameState)) yield return move;
        }
    }
    private IEnumerable<ICommand> FindAllPossibleMovesForCard(CardData card, BasePile cardPile, GameState gameState)
    {
        if (card == null) yield break;
        if (cardPile is not FoundationPile)
        {
            foreach (var pile in gameState.Foundations)
            {
                if (pile.Value.CanAddCard(card) && cardPile.GetTopCard() == card)
                {
                    yield return new MoveCardCommand(card, cardPile, pile.Value);
                }
            }
        }
        foreach (var pile in gameState.Tableaus)
        {
            if (cardPile == pile) continue;

            if (pile.CanAddCard(card))
            {
                yield return new MoveCardCommand(card, cardPile, pile);
            }
        }
    }
    private bool IsMovePossibleForCard(CardData card, BasePile cardPile)
    {
        var Tableaus = CurrentGameState.Tableaus;
        var Foundations = CurrentGameState.Foundations;
        if (card == null) return false;
        if (cardPile is not FoundationPile)
        {
            foreach (var pile in Foundations)
            {
                if (pile.Value.CanAddCard(card))
                {
                    return true;
                }
            }
        }
        foreach (var pile in Tableaus)
        {
            if (pile.CanAddCard(card))
            {
                return true;
            }
        }
        return false;
    }
    private bool IsMovePossibleForCard(CardData card, BasePile cardPile, out BasePile targetPile)
    {
        targetPile = null;

        var Tableaus = CurrentGameState.Tableaus;
        var Foundations = CurrentGameState.Foundations;

        if (card == null) return false;

        if (cardPile is not FoundationPile)
        {
            foreach (var pile in Foundations)
            {
                if (pile.Value.CanAddCard(card))
                {
                    targetPile = pile.Value;
                    return true;
                }
            }
        }
        foreach (var pile in Tableaus)
        {
            if (pile.CanAddCard(card))
            {
                targetPile = pile;
                return true;
            }
        }
        return false;
    }
    private bool IsValidMovableSequence(TableauPile pile, int startIndex)
    {
        for (int j = startIndex; j < pile.Cards.Count - 1; j++)
        {
            var current = pile.Cards[j];
            var next = pile.Cards[j + 1];
            if (!next.IsFaceUp || next.Rank != current.Rank - 1 || !current.Suit.IsOppositeColor(next.Suit))
            {
                return false;
            }
        }
        return true;
    }
}