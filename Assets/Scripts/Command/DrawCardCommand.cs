using System.Collections.Generic;
using UnityEngine;
public class DrawCardCommand : ICommand
{
    private readonly StockPile _stockPile;
    private readonly WastePile _wastePile;
    private readonly BasePile _targetPile;
    private readonly CardData _card;
    private readonly List<CardData> _movedCards;
    private bool _playedToTarget;
    public DrawCardCommand(CardData card, StockPile stockPile, WastePile wastePile, BasePile targetPile)
    {
        _stockPile = stockPile;
        _wastePile = wastePile;
        _targetPile = targetPile;
        _card = card;
        _movedCards = new List<CardData>();
        _playedToTarget = false;
    }
    public MoveType MoveType => MoveType.StockToWaste;
    public void Apply()
    {
        _movedCards.Clear();
        _playedToTarget = false;

        if (!_stockPile.Cards.Contains(_card)) return;

        int drawCount = _stockPile.DrawType == DrawType.Three ? 3 : 1;
        int available = Mathf.Min(drawCount, _stockPile.Cards.Count);

        if (available <= 0) return;

        var drawnCards = new List<CardData>(available);

        for (int k = 0; k < available; k++)
        {
            var top = _stockPile.Cards[_stockPile.Cards.Count - 1];
            _stockPile.RemoveCard(top);
            drawnCards.Add(top);
            _movedCards.Add(top);
        }

        var lastDrawn = drawnCards[drawnCards.Count - 1];


        for (int i = 0; i < drawnCards.Count - 1; i++)
        {
            _wastePile.AddCard(drawnCards[i]);
        }

        if (lastDrawn == _card && _targetPile != null && _targetPile.CanAddCard(lastDrawn))
        {
            _targetPile.AddCard(lastDrawn);
            _playedToTarget = true;
        }
        else
        {
            _wastePile.AddCard(lastDrawn);
            _playedToTarget = false;
        }
    }
    public void Undo()
    {
        if (_playedToTarget && _targetPile.HasCard() && _targetPile.GetTopCard() == _card)
        {
            _targetPile.RemoveCard(_card);
            _wastePile.AddCard(_card);
        }

        for (int i = _movedCards.Count - 1; i >= 0; i--)
        {
            var card = _movedCards[i];
            if (_wastePile.Cards.Contains(card))
            {
                _wastePile.RemoveCard(card);
                _stockPile.AddCard(card);
            }
        }
        _movedCards.Clear();
        _playedToTarget = false;
    }
}
