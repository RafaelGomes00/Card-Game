using System.IO;
using UnityEngine;

public static class GameData
{
    private const string ASSET_PATH = "MemoryGame";

    public static int points { get; private set; }

    public static void InitializeData()
    {
        string path = $"{Application.persistentDataPath}/{ASSET_PATH}/Save.txt";

        if (!File.Exists(path))
        {
            points = 0;
            return;
        }

        string data = File.ReadAllText(path);
        points = int.Parse(data);
    }

    public static void SaveText()
    {
        string path = $"{Application.persistentDataPath}/{ASSET_PATH}";

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        File.WriteAllText(path + "/Save.txt", points.ToString());

        Debug.Log($"Saved {points} points");
    }

    public static int UpdatePoints(int receivedPoints, int combo)
    {
        points += receivedPoints * combo;
        return points;
    }
}
