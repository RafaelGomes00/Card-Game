using UnityEngine;

[System.Serializable]
public struct CardData
{
    public Sprite sprite { get; private set; }

    public CardData(Sprite sprite)
    {
        this.sprite = sprite;
    }
}
