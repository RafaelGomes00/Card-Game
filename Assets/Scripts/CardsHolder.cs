using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

[System.Serializable]
public class CardsHolder
{
    [SerializeField] List<CardsInfoHolder> cards;

    public List<CardData> GetCards(int rows, int columns)
    {
        int numberOfPairs = rows * columns / 2;

        List<CardData> ops = new List<CardData>();
        for (int i = 0; i < numberOfPairs; i++)
        {
            CardData sorted;
            do
            {
                sorted = cards[UnityEngine.Random.Range(0, cards.Count)].GetCardData();
            } while (ops.Contains(sorted));
            ops.Add(sorted);
        }

        return ops;
    }

    public CardData GetCardByIdentifier(string identifier)
    {
        CardsInfoHolder data = cards.Find(x => x.identifier.Contains(identifier));

        if (data != null)
            return data.GetCardData();

        Debug.LogError($"Card of identifier {identifier} was not found, returning empty CardData.");
        return new CardData();
    }

    public List<CardData> GetCards(List<string> loadedCardIdentifiers)
    {
        List<CardData> loadedCards = new List<CardData>();

        foreach (string id in loadedCardIdentifiers)
        {
            loadedCards.Add(GetCardByIdentifier(id));
        }

        return loadedCards;
    }
}
