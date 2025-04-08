using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGroup : MonoBehaviour
{
    [SerializeField] private List<CardSingleUI> cardSingleUIList = new List<CardSingleUI>();
    [SerializeField] private List<CardSingleUI> selectedCardList = new List<CardSingleUI>();

    [SerializeField] private Sprite cardIdle;
    [SerializeField] private Sprite cardActive;

    public event EventHandler OnCardMatch;

    public void Subscribe(CardSingleUI cardSingleUI)
    {
        if (cardSingleUIList == null)
        {
            cardSingleUIList = new List<CardSingleUI>();
        }

        if (!cardSingleUIList.Contains(cardSingleUI))
        {
            cardSingleUIList.Add(cardSingleUI);
        }
    }

    public void OnCardSelected(CardSingleUI cardSingleUI)
    {
        // Si ya hay 2 cartas seleccionadas, no hacer nada más
        if (selectedCardList.Count == 2)
        {
            return;
        }

        // Agregar la carta seleccionada a la lista
        selectedCardList.Add(cardSingleUI);

        cardSingleUI.Select();

        if (selectedCardList.Count == 2)
        {
            if (CheckIfMatch())
            {
                foreach (CardSingleUI cardSingle in selectedCardList)
                {
                    cardSingle.DisableCardBackButton();
                    cardSingle.SetObjectMatch();
                }
                selectedCardList.Clear(); // Limpiar la lista de cartas seleccionadas después de hacer un match
                OnCardMatch?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                StartCoroutine(DontMatch());
            }
        }

        // Cambiar el sprite del fondo después de la selección
        ResetTabs();
    }

    public void ResetTabs()
    {
        foreach (CardSingleUI cardSingleUI in selectedCardList)
        {
            if (selectedCardList != null && selectedCardList.Count < 3) continue;

            cardSingleUI.GetCardBackBackground().sprite = cardIdle;
        }
    }

    private IEnumerator DontMatch()
    {
        yield return new WaitForSeconds(0.5f);

        // Desmarcar las cartas después de un pequeño retraso si no coinciden
        foreach (CardSingleUI cardSingleUI in selectedCardList)
        {
            cardSingleUI.Deselect();
        }

        // Limpiar la lista después de intentar hacer match
        selectedCardList.Clear();
    }

    private bool CheckIfMatch()
    {
        // Compara las cartas seleccionadas para ver si son iguales
        CardSingleUI firstCard = selectedCardList[0];

        foreach (CardSingleUI cardSingleUI in selectedCardList)
        {
            if (cardSingleUI.name != firstCard.name)
            {
                return false;
            }
        }

        return true;
    }
}
