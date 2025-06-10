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
    [SerializeField] private PointsFeedbackAnimator pointsFeedbackGO;
    [SerializeField] private Transform pointsFeedbackParent;
    [SerializeField] private GridLayoutGroup cardsGrid;
    [SerializeField] private RectTransform cardsGridRect;
    [SerializeField] private TextMeshProUGUI pointsTxt;
    [SerializeField] private TextMeshProUGUI handsTxt;
    [SerializeField] private TextMeshProUGUI matchesTxt;
    [SerializeField] private AudioClip incorrectMatchAudioClip;
    [SerializeField] private AudioClip correctMatchAudioClip;
    [SerializeField] private AudioClip gameOverAudioClip;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private float showCardsDelay;
    [SerializeField] private CardsHolder cardsHolder;

    Card selectedCard;
    int combo = 0;
    int totalPairs;
    int pairsCompleted;

    public void Start()
    {
        pointsTxt.text = $"Points: {GameData.points}";
        matchesTxt.text = $"Matches: {GameData.matches}";
        handsTxt.text = $"Hands: {GameData.hands}";
        InitializeGame();
    }

    public void InitializeCards(List<CardData> cards)
    {
        List<CardData> _cards = new List<CardData>();
        totalPairs = cards.Count;

        _cards.AddRange(cards);
        _cards.AddRange(cards);

        var cardsShuffled = _cards.Shuffle();

        List<Card> instantiatedCards = new List<Card>();

        foreach (CardData cardData in cardsShuffled)
        {
            Card newCard = Instantiate(cardGO, cardsGrid.transform);
            newCard.Initialize(cardData, OnCardClicked, this);
            instantiatedCards.Add(newCard);
        }

        StartCoroutine(ShowCards(instantiatedCards));
        StartCoroutine(DisableLayoutGroupRoutine());
    }

    public void OnClickKeepPlaying()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnClickMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    private IEnumerator ShowCards(List<Card> cards)
    {
        yield return new WaitForEndOfFrame();
        foreach (Card card in cards) card.ShowCard();
        yield return new WaitForSeconds(showCardsDelay);
        foreach (Card card in cards) card.HideCard();
    }

    private void InitializeGame()
    {
        float cellSize = ResolveGridSize(GameController.instance.rows);
        cardsGrid.cellSize = new Vector2(cellSize, cellSize);

        InitializeCards(cardsHolder.GetCards(GameController.instance.rows, GameController.instance.columns));
        cardsGrid.constraintCount = GameController.instance.rows;
        cardsGrid.enabled = true;
        pairsCompleted = 0;
    }

    private float ResolveGridSize(int numberOfElements)
    {
        float xSize = Mathf.Clamp((cardsGridRect.rect.width - ((numberOfElements - 1) * ((cardsGrid.spacing.x / 2) + cardsGrid.padding.left + cardsGrid.padding.right))) / numberOfElements, 20, 200);
        float ySize = Mathf.Clamp((cardsGridRect.rect.height - ((numberOfElements - 1) * ((cardsGrid.spacing.y / 2) + cardsGrid.padding.top + cardsGrid.padding.bottom))) / numberOfElements, 20, 200);

        Debug.Log($"X {xSize} / Y {ySize}");

        return xSize < ySize ? xSize : ySize;
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
        GameData.UpdateHands();

        if (card1.CompareData(card2.cardData)) //Equal cards
        {
            combo++;
            pairsCompleted++;
            GameData.UpdateMatches();
            UpdateData(100, combo);

            card1.AnimateCardPair();
            card2.AnimateCardPair();
            SoundController.instance.PlayEffect(correctMatchAudioClip);
            CheckGameCompletion();
        }
        else //Diferent cards
        {
            combo = 0;
            UpdateData(-100, 1);

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

    private void UpdateData(int receivedPoints, int combo)
    {
        Instantiate(pointsFeedbackGO, pointsFeedbackParent).SetText(receivedPoints, combo);
        int points = GameData.UpdatePoints(receivedPoints, combo);
        pointsTxt.text = $"Points: {points}";
        handsTxt.text = $"Hands: {GameData.hands}";
        matchesTxt.text = $"Matches: {GameData.matches}";
    }
}
