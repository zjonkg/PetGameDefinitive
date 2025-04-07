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
        // Elige dificultad al azar
        DifficultyEnum randomDifficulty = (DifficultyEnum)Random.Range(0, 3); 

        int cardsToShow = 0;

        switch (randomDifficulty)
        {
            case DifficultyEnum.Easy:
                cardsToShow = 6;
                break;
            case DifficultyEnum.Normal:
                cardsToShow = 8;
                break;
            case DifficultyEnum.Hard:
                cardsToShow = 10;
                break;
        }

        Debug.Log("Dificultad aleatoria: " + randomDifficulty + " con " + cardsToShow + " cartas");

        cardListToSort.Clear();

        for (int i = 0; i < cardsToShow; i++)
        {
            cardListToSort.Add(cardList[i]);
            cardListToSort.Add(cardList[i]);
        }

        System.Random rnd = new System.Random();
        IOrderedEnumerable<Card> randomized = cardListToSort.OrderBy(i => rnd.Next());

        foreach (Card card in randomized)
        {
            Transform cardTransform = Instantiate(cardPrefab, cardContainer);
            cardTransform.gameObject.SetActive(true);
            cardTransform.name = card.cardName;
            cardTransform.GetComponent<CardSingleUI>().SetCardImage(card.cardImage);
        }
    }


}
