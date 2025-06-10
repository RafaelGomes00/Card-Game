using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI pointsTxt;
    [SerializeField] TextMeshProUGUI handsTxt;
    [SerializeField] TextMeshProUGUI matchesTxt;
    [SerializeField] TextMeshProUGUI warningTxt;
    [SerializeField] TMP_Dropdown rowsDropdown;
    [SerializeField] TMP_Dropdown columnsDropdown;

    [SerializeField] int[] cardValues;
    private void Start()
    {
        GameData.InitializeData();

        GameController.instance.SetRows(cardValues[0]);
        GameController.instance.SetColumns(cardValues[0]);

        rowsDropdown.value = PlayerPrefs.GetInt("Rows", 0);
        columnsDropdown.value = PlayerPrefs.GetInt("Columns", 0);

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
        PlayerPrefs.SetInt("Rows", dropdownValue);
        GameController.instance.SetRows(cardValues[dropdownValue]);

        CheckOddNumbers();
    }

    public void SetColumns(int dropdownValue)
    {
        PlayerPrefs.SetInt("Columns", dropdownValue);
        GameController.instance.SetColumns(cardValues[dropdownValue]);

        CheckOddNumbers();
    }

    private void CheckOddNumbers()
    {
        if (cardValues[rowsDropdown.value] % 2 != 0 && cardValues[columnsDropdown.value] % 2 != 0) warningTxt.gameObject.SetActive(true);
        else warningTxt.gameObject.SetActive(false);
    }
}
