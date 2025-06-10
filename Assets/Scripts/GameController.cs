using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public int rows { get; private set; }
    public int columns { get; private set; }
    public int combo { get; private set; }
    public List<string> loadedCardIdentifiers { get; private set; }
    public List<string> loadedMatchedCardIdentifiers { get; private set; }
    public bool loadedGame { get; private set; }

    public static GameController instance;

    void Awake()
    {
        DontDestroyOnLoad(this);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetRows(int rows)
    {
        this.rows = rows;
    }

    public void SetColumns(int columns)
    {
        this.columns = columns;
    }

    public void LoadSavedData(int combo, int rows, int columns, List<string> instantiatedCardIndentifiers, List<string> matchedCardsIdentifiers)
    {
        this.combo = combo;
        this.rows = rows;
        this.columns = columns;
        this.loadedCardIdentifiers = instantiatedCardIndentifiers;
        this.loadedMatchedCardIdentifiers = matchedCardsIdentifiers;
    }

    public void StartLoadedGame()
    {
        loadedGame = true;
        SceneManager.LoadScene("Game");
    }

    public void StartNewGame(int rows, int columns)
    {
        loadedGame = false;
        this.combo = 0;
        this.rows = rows;
        this.columns = columns;
        this.loadedCardIdentifiers = null;
        SceneManager.LoadScene("Game");
    }
}
