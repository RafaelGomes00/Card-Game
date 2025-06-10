using UnityEngine;

public class GameController : MonoBehaviour
{
    public int rows { get; private set; }
    public int columns { get; private set; }

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
}
