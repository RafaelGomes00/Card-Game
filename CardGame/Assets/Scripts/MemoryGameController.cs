using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using TMPro;

public class MemoryGameController : MonoBehaviour
{
    [SerializeField] private Card cardGO;
    [SerializeField] private GridLayoutGroup cardsGrid;
    [SerializeField] private TextMeshProUGUI pointsTxt;
    [SerializeField] private AudioClip incorrectMatchAudioClip;
    [SerializeField] private AudioClip correctMatchAudioClip;
    [SerializeField] private AudioClip gameOverAudioClip;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private CardsHolder cardsHolder;

    Card selectedCard;
    int points;
    int combo = 0;

    public void Start()
    {
        pointsTxt.text = $"Points: {points}";
        InitializeCards(cardsHolder.GetCards(4, 5));
        cardsGrid.constraintCount = 4;
    }

    public void InitializeCards(List<CardData> cards)
    {
        List<CardData> _cards = new List<CardData>();

        _cards.AddRange(cards);
        _cards.AddRange(cards);

        var cardsShuffled = _cards.Shuffle();

        foreach (CardData cardData in cardsShuffled)
        {
            Card newCard = Instantiate(cardGO, cardsGrid.transform);
            newCard.Initialize(cardData, OnCardClicked, this);
        }

        StartCoroutine(DisableLayoutGroupRoutine());
    }

    private IEnumerator DisableLayoutGroupRoutine()
    {
        yield return new WaitForEndOfFrame();
        cardsGrid.enabled = false;
    }

    private void OnCardClicked(Card card)
    {
        if (selectedCard != null) CompareCards(selectedCard, card);
        else selectedCard = card;
    }

    private void CompareCards(Card card1, Card card2)
    {
        selectedCard = null;

        if (card1.CompareData(card2.cardData)) //Equal cards
        {
            combo++;
            UpdatePoints();
            card1.DestroyCardDelayed();
            card2.DestroyCardDelayed();
            SoundController.instance.PlayEffect(correctMatchAudioClip);
            CheckGameCompletion();
        }
        else //Diferent cards
        {
            combo = 0;
            card1.HideCardDelayed();
            card2.HideCardDelayed();
            SoundController.instance.PlayEffect(incorrectMatchAudioClip);
        }
    }

    private void CheckGameCompletion()
    {
        if (cardsGrid.transform.childCount == 0)
        {
            SoundController.instance.PlayEffect(gameOverAudioClip);
            gameOver.SetActive(true);
        }
    }

    private void UpdatePoints()
    {
        points += 100 * combo;
        pointsTxt.text = $"Points: {points}";
    }

    private void OnCardFlipped()
    {
        // if (flippedCards.Count < MAX_FLIP_COUNT) return;
        // StartCoroutine(CheckAndRemoveCards());
    }
}
