using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create new card", fileName = "New card")]
public class CardsInfoHolder : ScriptableObject
{
    [SerializeField] private Sprite card;
    public string identifier { get; private set; }

    public CardData GetCardData()
    {
        return new CardData(card, identifier);
    }

    private void OnValidate()
    {
        identifier = card.GetHashCode().ToString();
    }
}
