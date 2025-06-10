using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI pointsTxt;
    [SerializeField] TextMeshProUGUI handsTxt;
    [SerializeField] TextMeshProUGUI matchesTxt;
    [SerializeField] TextMeshProUGUI warningTxt;
    [SerializeField] TMP_Dropdown rowsDropdown;
    [SerializeField] TMP_Dropdown columnsDropdown;
    [SerializeField] Button continueButton;

    [SerializeField] int[] cardValues;
    private void Start()
    {
        GameData.InitializeData();
        continueButton.interactable = GameData.loadedInfo; // If there is no data saved, the continue button will disable itself.

        rowsDropdown.value = PlayerPrefs.GetInt("Rows", 0);
        columnsDropdown.value = PlayerPrefs.GetInt("Columns", 0);

        pointsTxt.text = $"Points: {GameData.points}";
        handsTxt.text = $"Hands: {GameData.hands}";
        matchesTxt.text = $"Matches: {GameData.matches}";
    }

    public void OnClickStartNewGame()
    {
        GameData.DeleteData();
        GameController.instance.SetRows(cardValues[rowsDropdown.value]);
        GameController.instance.SetColumns(cardValues[columnsDropdown.value]);
        GameController.instance.StartNewGame(cardValues[rowsDropdown.value], cardValues[columnsDropdown.value]);
    }

    public void OnClickStartLoadedGame()
    {
        GameController.instance.StartLoadedGame();
    }

    public void SetRows(int dropdownValue)
    {
        PlayerPrefs.SetInt("Rows", dropdownValue);
    }

    public void SetColumns(int dropdownValue)
    {
        PlayerPrefs.SetInt("Columns", dropdownValue);
    }

    public void CheckOddNumbers()
    {
        if (cardValues[rowsDropdown.value] % 2 != 0 && cardValues[columnsDropdown.value] % 2 != 0) warningTxt.gameObject.SetActive(true);
        else warningTxt.gameObject.SetActive(false);
    }
}
