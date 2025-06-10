using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Create new card", fileName = "New card")]
public class CardsInfoHolder : ScriptableObject
{
    [SerializeField] private Sprite card;
    public string identifier;

    public CardData GetCardData()
    {
        return new CardData(card, identifier);
    }
}
