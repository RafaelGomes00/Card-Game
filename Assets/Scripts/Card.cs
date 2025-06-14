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
            ShowCard();
            onClickCard?.Invoke(this);
            SoundController.instance.PlayEffect(flipCardAudioClip);
        }
    }

    //Triggered through animation events
    public void ToggleCardImage()
    {
        cardImage.enabled = !cardImage.enabled;
    }

    public bool CompareData(CardData comparedCard)
    {
        return cardData.identifier == comparedCard.identifier;
    }

    public void ShowCard()
    {
        showing = true;
        cardAnimator?.SetTrigger("ShowCard");
    }

    public void HideCardDelayed()
    {
        Invoke("HideCard", 2f);
    }

    public void HideCard()
    {
        showing = false;
        cardAnimator?.SetTrigger("HideCard");
    }

    public void AnimateCardPair()
    {
        cardAnimator?.SetTrigger("MatchCard");
        DestroyCardDelayed();
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
