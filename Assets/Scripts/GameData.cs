using System.IO;
using UnityEngine;

public static class GameData
{
    private const string ASSET_PATH = "MemoryGame";

    public static int points { get; private set; }
    public static int hands { get; private set; }
    public static int matches { get; private set; }

    public static void InitializeData()
    {
        string path = $"{Application.persistentDataPath}/{ASSET_PATH}/Save.txt";

        if (!File.Exists(path))
        {
            points = 0;
            matches = 0;
            hands = 0;
            return;
        }

        StreamReader sr = new StreamReader(path);
        points = int.Parse(sr.ReadLine());
        matches = int.Parse(sr.ReadLine());
        hands = int.Parse(sr.ReadLine());

        sr.Close();
    }

    public static void SaveText()
    {
        string path = $"{Application.persistentDataPath}/{ASSET_PATH}";

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        StreamWriter sw = new StreamWriter(path + "/Save.txt");
        sw.WriteLine(points);
        sw.WriteLine(matches);
        sw.WriteLine(hands);
        sw.Close();

        Debug.Log($"Saved {points} points, {matches} matches, {hands} hands");
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
