using System;
using System.Collections.Generic;
public class MoveCardCommand : ICommand
{
    private readonly CardData _card;
    private readonly BasePile _sourcePile;
    private readonly BasePile _targetPile;
    private bool _wasTopCardFlipped;
    public MoveCardCommand(CardData card, BasePile sourcePile, BasePile targetPile)
    {
        _card = card;
        _sourcePile = sourcePile;
        _targetPile = targetPile;
    }
    public MoveType MoveType
    {
        get
        {
            if (_sourcePile is TableauPile && _targetPile is TableauPile)
                return MoveType.TableauToTableau;
            if (_sourcePile is TableauPile && _targetPile is FoundationPile)
                return MoveType.TableauToFoundation;
            if (_sourcePile is WastePile && _targetPile is TableauPile)
                return MoveType.WasteToTableau;
            if (_sourcePile is WastePile && _targetPile is FoundationPile)
                return MoveType.WasteToFoundation;
            if (_sourcePile is FoundationPile && _targetPile is TableauPile)
                return MoveType.FoundationToTableau;

            throw new InvalidOperationException("Invalid Move!: " + _sourcePile.GetType() + " " + _targetPile.GetType());
        }
    }

    public void Apply()
    {
        IList<CardData> cards;

        if (_sourcePile is WastePile)
        {
            cards = new List<CardData> { _card };
        }
        else
        {
            cards = _sourcePile.SplitAt(_card);
            if (cards == null || cards.Count == 0) return;
        }
        _sourcePile.RemoveCards(cards);
        _targetPile.AddCards(cards);

        var cardBelow = _sourcePile.GetTopCard();
        if (_sourcePile is TableauPile && cardBelow != null && !cardBelow.IsFaceUp)
        {
            cardBelow.IsFaceUp = true;
            _wasTopCardFlipped = true;
        }
    }
    public void Undo()
    {
        var topCard = _sourcePile.GetTopCard();
        if (_sourcePile is TableauPile && _wasTopCardFlipped && topCard != null && topCard.IsFaceUp)
        {
            topCard.IsFaceUp = false;
        }

        IList<CardData> cards;

        if (_sourcePile is WastePile)
        {
            cards = new List<CardData> { _card };
        }
        else
        {
            cards = _targetPile.SplitAt(_card);
            if (cards == null || cards.Count == 0) return;
        }

        _targetPile.RemoveCards(cards);
        _sourcePile.AddCards(cards);
    }
}
