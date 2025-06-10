using UnityEngine;

[System.Serializable]
public struct CardData
{
    public Sprite sprite { get; private set; }
    public string identifier { get; private set; }

    public CardData(Sprite sprite, string identifier)
    {
        this.sprite = sprite;
        this.identifier = identifier;
    }
}
