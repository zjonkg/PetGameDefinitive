using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardGridUI : MonoBehaviour
{
    [System.Serializable]
    public class Card
    {
        public string cardName;
        public Sprite cardImage;
    }

    [SerializeField] private List<Card> cardList = new List<Card>();
    [SerializeField] private List<Card> cardListToSort = new List<Card>();
    [SerializeField] private Transform cardContainer;
    [SerializeField] private Transform cardPrefab;
    public static DifficultyEnum CurrentDifficulty { get; private set; }

    private void Start()
    {
        cardPrefab.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        FillGrid();
    }

    private void FillGrid()
    {
        CurrentDifficulty = (DifficultyEnum)Random.Range(0, 3);

        int cardsToShow = 0;

        switch (CurrentDifficulty)
        {
            case DifficultyEnum.Easy:
                cardsToShow = 4;
                break;
            case DifficultyEnum.Normal:
                cardsToShow = 6;
                break;
            case DifficultyEnum.Hard:
                cardsToShow = 8;
                break;
        }

        Debug.Log("Dificultad aleatoria: " + CurrentDifficulty + " con " + cardsToShow + " cartas");

        cardListToSort.Clear();

        for (int i = 0; i < cardsToShow; i++)
        {
            cardListToSort.Add(cardList[i]);
            cardListToSort.Add(cardList[i]);
        }

        var randomized = cardListToSort.OrderBy(i => Random.value); 

        foreach (Card card in randomized)
        {
            Transform cardTransform = Instantiate(cardPrefab, cardContainer);
            cardTransform.gameObject.SetActive(true);
            cardTransform.name = card.cardName;
            cardTransform.GetComponent<CardSingleUI>().SetCardImage(card.cardImage);
        }
    }
}

