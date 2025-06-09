using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;

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
    int combo = 0;
    int totalPairs;
    int pairsCompleted;

    public void Start()
    {
        pointsTxt.text = $"Points: {GameData.points}";
        InitializeGame();
    }

    public void InitializeCards(List<CardData> cards)
    {
        List<CardData> _cards = new List<CardData>();
        totalPairs = cards.Count;

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

    public void OnClickKeepPlaying()
    {
        SceneManager.LoadScene("Game");
        // gameOver.SetActive(false);
        // InitializeGame();
    }

    public void OnClickMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    private void InitializeGame()
    {
        InitializeCards(cardsHolder.GetCards(4, 5));
        cardsGrid.constraintCount = 4;
        cardsGrid.enabled = true;
        pairsCompleted = 0;
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
            pairsCompleted++;
            UpdatePoints(100, combo);

            card1.DestroyCardDelayed();
            card2.DestroyCardDelayed();
            SoundController.instance.PlayEffect(correctMatchAudioClip);
            CheckGameCompletion();
        }
        else //Diferent cards
        {
            combo = 0;
            UpdatePoints(-100, 1);

            card1.HideCardDelayed();
            card2.HideCardDelayed();
            SoundController.instance.PlayEffect(incorrectMatchAudioClip);
        }
    }

    private void CheckGameCompletion()
    {
        if (pairsCompleted == totalPairs)
        {
            GameData.SaveText();
            SoundController.instance.PlayEffect(gameOverAudioClip);
            gameOver.SetActive(true);
        }
    }

    private void UpdatePoints(int receivedPoints, int combo)
    {
        int points = GameData.UpdatePoints(receivedPoints, combo);
        pointsTxt.text = $"Points: {points}";
    }
}
