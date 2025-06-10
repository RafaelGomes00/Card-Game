using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

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

    private List<string> instantiatedCardsIdentifiers = new List<string>();
    private List<string> matchedCardsIdentifiers = new List<string>();

    Card selectedCard;
    int combo;
    int totalPairs;
    int pairsCompleted;

    public void Start()
    {
        pointsTxt.text = $"Points: {GameData.points}";
        matchesTxt.text = $"Matches: {GameData.matches}";
        handsTxt.text = $"Hands: {GameData.hands}";

        if (GameController.instance.loadedGame)
        {
            combo = GameController.instance.combo;
            RestoreBoard(GameController.instance.loadedCardIdentifiers, GameController.instance.loadedMatchedCardIdentifiers);
        }
        else
        {
            InitializeNewGame();
        }
    }

    public void InitializeCards(List<CardData> cards) //Initialize cards from new game
    {
        List<CardData> _cards = new List<CardData>();
        totalPairs = cards.Count;

        _cards.AddRange(cards);
        _cards.AddRange(cards);

        var cardsArray = _cards.Shuffle();

        List<Card> instantiatedCards = InstantiateCards(cardsArray);

        StartCoroutine(ShowCards(instantiatedCards));
        StartCoroutine(DisableLayoutGroupRoutine());
    }

    public void InitializeCards(List<CardData> cards, List<string> loadedMatchedCards) //Initialize cards from loaded game
    {
        List<Card> instantiatedCards = InstantiateCards(cards);
        totalPairs = cards.Count / 2;

        StartCoroutine(ShowCards(instantiatedCards));
        StartCoroutine(DisableLayoutGroupRoutine(instantiatedCards, loadedMatchedCards, DestroyLoadedMatchedCards));
    }

    public void OnClickKeepPlaying()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnClickMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    private List<Card> InstantiateCards(IEnumerable<CardData> cards)
    {
        List<Card> instantiatedCards = new List<Card>();

        foreach (CardData cardData in cards)
        {
            Card newCard = Instantiate(cardGO, cardsGrid.transform);
            newCard.Initialize(cardData, OnCardClicked, this);
            instantiatedCards.Add(newCard);
            instantiatedCardsIdentifiers.Add(cardData.identifier);
        }

        return instantiatedCards;
    }

    //Destroy previous found matches when loading the game
    private void DestroyLoadedMatchedCards(List<Card> instantiatedCards, List<string> loadedMatchedCards)
    {
        foreach (Card card in instantiatedCards)
        {
            foreach (string id in loadedMatchedCards)
            {
                if (card.cardData.identifier == id)
                {
                    matchedCardsIdentifiers.Add(id);
                    Destroy(card.gameObject);
                }
            }
        }
    }

    //Restore saved board
    private void RestoreBoard(List<string> loadedCardIdentifiers, List<string> loadedMatchedCardsIdentifiers)
    {
        if (loadedCardIdentifiers.Count > 0)
            InitializeCards(cardsHolder.GetCards(loadedCardIdentifiers), loadedMatchedCardsIdentifiers);
        else
            InitializeCards(cardsHolder.GetCards(GameController.instance.rows, GameController.instance.columns));

        float cellSize = ResolveGridSize();
        cardsGrid.cellSize = new Vector2(cellSize, cellSize);

        cardsGrid.constraintCount = GameController.instance.rows;
        pairsCompleted = loadedMatchedCardsIdentifiers.Count;
        cardsGrid.enabled = true;
    }

    //Show the cards at the start of the minigame
    private IEnumerator ShowCards(List<Card> cards)
    {
        yield return new WaitForEndOfFrame();
        foreach (Card card in cards) if (card != null) card.ShowCard();
        yield return new WaitForSeconds(showCardsDelay);
        foreach (Card card in cards) if (card != null) card.HideCard();
    }

    private void InitializeNewGame()
    {
        float cellSize = ResolveGridSize();
        cardsGrid.cellSize = new Vector2(cellSize, cellSize);

        InitializeCards(cardsHolder.GetCards(GameController.instance.rows, GameController.instance.columns));
        cardsGrid.constraintCount = GameController.instance.rows;
        cardsGrid.enabled = true;
        pairsCompleted = 0;
    }

    //Increase or decrease size of the cards based on number of elements present on the board
    private float ResolveGridSize()
    {
        float xSize = Mathf.Clamp((cardsGridRect.rect.width - ((GameController.instance.columns - 1) * cardsGrid.spacing.x) - cardsGrid.padding.left - cardsGrid.padding.right) / GameController.instance.columns, 20, 200);
        float ySize = Mathf.Clamp((cardsGridRect.rect.height - ((GameController.instance.rows - 1) * cardsGrid.spacing.y) - cardsGrid.padding.top - cardsGrid.padding.bottom) / GameController.instance.rows, 20, 200);

        return xSize < ySize ? xSize : ySize;
    }

    //Disable grid layout group component to make the cards static
    private IEnumerator DisableLayoutGroupRoutine(List<Card> instantiatedCards = null, List<string> loadedMatchedCards = null, Action<List<Card>, List<string>> onDisabledCallback = null)
    {
        yield return new WaitForEndOfFrame();
        cardsGrid.enabled = false;
        onDisabledCallback?.Invoke(instantiatedCards, loadedMatchedCards);
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

        if (card1.CompareData(card2.cardData)) //Cards drawn are equal
        {
            combo++;
            pairsCompleted++;

            GameData.UpdateMatches();
            UpdateData(100, combo);

            card1.AnimateCardPair();
            card2.AnimateCardPair();

            SoundController.instance.PlayEffect(correctMatchAudioClip);

            matchedCardsIdentifiers.Add(card1.cardData.identifier);
            SaveBoard();
            CheckGameCompletion();
        }
        else //Cards drawn are different
        {
            combo = 0;
            UpdateData(-100, 1);

            card1.HideCardDelayed();
            card2.HideCardDelayed();
            SoundController.instance.PlayEffect(incorrectMatchAudioClip);
        }
    }

    private void SaveBoard()
    {
        GameData.Save(instantiatedCardsIdentifiers, matchedCardsIdentifiers, combo, GameController.instance.rows, GameController.instance.columns);
    }

    private void CheckGameCompletion()
    {
        if (pairsCompleted == totalPairs)
        {
            GameData.Save(combo, GameController.instance.rows, GameController.instance.columns);
            SoundController.instance.PlayEffect(gameOverAudioClip);
            gameOver.SetActive(true);
        }
    }

    private void UpdateData(int receivedPoints, int combo) //Update GameData and instantiate a points given or lost feedback
    {
        Instantiate(pointsFeedbackGO, pointsFeedbackParent).SetText(receivedPoints, combo);
        int points = GameData.UpdatePoints(receivedPoints, combo);
        pointsTxt.text = $"Points: {points}";
        handsTxt.text = $"Hands: {GameData.hands}";
        matchesTxt.text = $"Matches: {GameData.matches}";
    }
}
