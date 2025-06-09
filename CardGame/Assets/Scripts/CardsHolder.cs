using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

[System.Serializable]
public class CardsHolder
{
    [SerializeField] List<Sprite> cardImages;

    public List<CardData> GetCards(int rows, int columns)
    {
        int numberOfPairs = rows * columns / 2;

        List<CardData> ops = new List<CardData>();
        for (int i = 0; i < numberOfPairs; i++)
        {
            CardData sorted;
            do
            {
                sorted = new CardData(cardImages[UnityEngine.Random.Range(0, cardImages.Count)]);
            } while (ops.Contains(sorted));
            ops.Add(sorted);
        }

        return ops;
    }
}
