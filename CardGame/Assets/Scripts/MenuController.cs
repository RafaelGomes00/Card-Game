using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI pointsTxt;
    [SerializeField] TextMeshProUGUI handsTxt;
    [SerializeField] TextMeshProUGUI matchesTxt;

    [SerializeField] int[] cardValues;
    private void Start()
    {
        GameData.InitializeData();

        GameController.instance.SetRows(cardValues[0]);
        GameController.instance.SetColumns(cardValues[0]);

        pointsTxt.text = $"Points: {GameData.points}";
        handsTxt.text = $"Hands: {GameData.hands}";
        matchesTxt.text = $"Matches: {GameData.matches}";
    }

    public void OnClickStart()
    {
        SceneManager.LoadScene("Game");
    }

    public void SetRows(int dropdownValue)
    {
        GameController.instance.SetRows(cardValues[dropdownValue]);
    }

    public void SetColumns(int dropdownValue)
    {
        GameController.instance.SetColumns(cardValues[dropdownValue]);
    }
}
