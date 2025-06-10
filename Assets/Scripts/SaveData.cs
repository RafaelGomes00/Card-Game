using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int points;
    public int combo;
    public int handsPlayed;
    public int matchesDone;
    public int rows;
    public int columns;
    public List<string> instantiatedCardIndentifiers;
    public List<string> matchedCardsIdentifiers;

    public SaveData(int points, int combo, int hands, int matches, int rows, int columns, List<string> instantiatedCards, List<string> matchedCards)
    {
        this.points = points;
        this.handsPlayed = hands;
        this.matchesDone = matches;
        this.combo = combo;
        this.rows = rows;
        this.columns = columns;

        this.instantiatedCardIndentifiers = instantiatedCards;
        this.matchedCardsIdentifiers = matchedCards;
    }
}

