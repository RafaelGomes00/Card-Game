using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private void Start()
    {
        GameData.InitializeData();
    }

    public void OnClickStart()
    {
        SceneManager.LoadScene("Game");
    }
}
