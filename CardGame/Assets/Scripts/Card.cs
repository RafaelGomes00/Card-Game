using System;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] Image cardImage;
    [SerializeField] Animator cardAnimator;
    [SerializeField] AudioClip flipCardAudioClip;

    public CardData cardData { get; private set; }

    private Action<Card> onClickCard;
    private bool showing;

    public void Initialize(CardData cardData, Action<Card> onCardClicked, MemoryGameController memoryGame)
    {
        cardImage.sprite = cardData.sprite;
        this.onClickCard = onCardClicked;
        this.cardData = cardData;
    }

    public void OnClickCard()
    {
        if (!showing)
        {
            showing = true;
            onClickCard?.Invoke(this);
            cardAnimator.SetTrigger("ShowCard");
            SoundController.instance.PlayEffect(flipCardAudioClip);
        }
    }

    public void ToggleCardImage()
    {
        cardImage.enabled = !cardImage.enabled;
    }

    public bool CompareData(CardData comparedCard)
    {
        return cardData.sprite.GetHashCode() == comparedCard.sprite.GetHashCode();
    }

    public void HideCardDelayed()
    {
        Invoke("HideCard", 2f);
    }

    private void HideCard()
    {
        showing = false;
        cardAnimator.SetTrigger("HideCard");
    }

    public void DestroyCardDelayed()
    {
        Invoke("DestroyCard", 2f);
    }

    private void DestroyCard()
    {
        Destroy(this.gameObject);
    }
}
