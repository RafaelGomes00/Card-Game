using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] int[] values;
    private void Start()
    {
        GameData.InitializeData();

        GameController.instance.SetRows(values[0]);
        GameController.instance.SetColumns(values[0]);
    }

    public void OnClickStart()
    {
        SceneManager.LoadScene("Game");
    }

    public void SetRows(int dropdownValue)
    {
        GameController.instance.SetRows(values[dropdownValue]);
    }

    public void SetColumns(int dropdownValue)
    {
        GameController.instance.SetColumns(values[dropdownValue]);
    }
}
