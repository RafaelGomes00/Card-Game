using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public static class GameData
{
    private const string ASSET_PATH = "MemoryGame";

    public static int points { get; private set; }
    public static int hands { get; private set; }
    public static int matches { get; private set; }

    public static bool loadedInfo { get; private set; }

    public static void InitializeData()
    {
        string path = $"{Application.persistentDataPath}/{ASSET_PATH}/Save.txt";

        if (!File.Exists(path))
        {
            loadedInfo = false;
            return;
        }

        try
        {
            SaveData savedData = JsonUtility.FromJson<SaveData>(File.ReadAllText(path));

            points = savedData.points;
            hands = savedData.handsPlayed;
            matches = savedData.matchesDone;

            GameController.instance.LoadSavedData(savedData.combo, savedData.rows, savedData.columns, savedData.instantiatedCardIndentifiers, savedData.matchedCardsIdentifiers);
            loadedInfo = true;
        }
        catch
        {
            Debug.LogError("Error reading data from save file.");
            loadedInfo = false;
        }
    }

    public static void DeleteData()
    {
        points = 0;
        hands = 0;
        matches = 0;
        File.Delete($"{Application.persistentDataPath}/{ASSET_PATH}/Save.txt");
    }

    public static void Save(List<string> instantiatedCards, List<string> matchedCards, int combo, int rows, int columns)
    {
        SaveData saveData = new SaveData(points, combo, hands, matches, rows, columns, instantiatedCards, matchedCards);
        WriteSaveText(saveData);
    }

    public static void Save(int combo, int rows, int columns) //The game was completed, therefore there is no board to be saved.
    {
        SaveData saveData = new SaveData(points, combo, hands, matches, rows, columns, null, null);
        WriteSaveText(saveData);
    }

    private static void WriteSaveText(SaveData saveData)
    {
        string path = $"{Application.persistentDataPath}/{ASSET_PATH}";

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        File.WriteAllText(path + "/Save.txt", JsonUtility.ToJson(saveData));
    }

    public static int UpdatePoints(int receivedPoints, int combo)
    {
        points += receivedPoints * combo;
        return points;
    }
    public static void UpdateHands()
    {
        hands++;
    }
    public static void UpdateMatches()
    {
        matches++;
    }
}
