using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CardSingleUI : MonoBehaviour
{
    private CardGroup cardGroup;

    [SerializeField] private Button cardBackButton;
    [SerializeField] private Image cardBackBackground;
    [SerializeField] private Image cardFrontImage;

    [SerializeField] private GameObject cardBack;
    [SerializeField] private GameObject cardFront;

    private bool objectMatch;

    [Header("DoTween Animation")]
    private Vector3 selectRotation = new Vector3(0, 180, 0); // Back position
    private Vector3 deselectRotation = new Vector3(0, 0, 0);   // Front position
    private float duration = 1f; // A bit slower for clarity
    private Tweener[] tweener = new Tweener[3];

    private void Awake()
    {
        if (cardGroup == null)
        {
            cardGroup = transform.parent.GetComponent<CardGroup>();
        }

        if (cardGroup != null)
        {
            cardGroup.Subscribe(this);
        }
    }

    private void Start()
    {
        cardBackButton.onClick.AddListener(OnClick);

        // Inicia con la carta mostrando el front
        transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0)); // Muestra el frente
        cardFront.SetActive(true);
        cardBack.SetActive(false);

        // Empezamos la animación de la carta (mostrar la parte frontal al inicio)
        StartCoroutine(WaitingToHide());

        // Llamar a SetRotationToZero para asegurar que las cartas inicien con rotación 0.
        SetRotationToZero();

        MemoryGameManagerUI.Instance.Subscribe(this);
    }


    private void OnClick()
    {
        cardGroup.OnCardSelected(this);
    }

    public void Select()
    {
        tweener[0] = transform.DORotate(selectRotation, duration)
            .SetEase(Ease.InOutElastic)
            .OnUpdate(CheckSelectHalfDuration);
    }

    public void Deselect()
    {
        tweener[1] = transform.DORotate(deselectRotation, duration)
            .SetEase(Ease.InOutElastic)
            .OnUpdate(CheckDeselectHalfDuration);
    }

    private IEnumerator WaitingToHide()
    {
        yield return new WaitForSeconds(3f);

        // Aquí inicia el giro hacia la parte trasera después de 3 segundos
        tweener[2] = transform.DORotate(selectRotation, duration)
            .SetEase(Ease.InOutElastic)
            .OnUpdate(CheckWaitingToHide);
    }

    public void SetRotationToZero()
    {
        // Aseguramos que la rotación de la carta se establezca explícitamente en 0 grados en el eje Y.
        transform.rotation = Quaternion.Euler(0f, 0f, 0f); // Fuerza la rotación a 0 grados
    }


    private void CheckWaitingToHide()
    {
        float elapsed = tweener[2].Elapsed();
        float halfDuration = tweener[2].Duration() / 2f;

        if (elapsed >= halfDuration)
        {
            // Cambiar la visibilidad después de que se haya realizado la mitad de la animación
            cardFront.SetActive(false);
            cardBack.SetActive(true);
        }

        // Aseguramos que la rotación final quede en 0 grados, en lugar de -180 grados.
        if (elapsed >= tweener[2].Duration())
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f); // Fuerza la rotación a 0 grados en Y
        }
    }


    private void CheckSelectHalfDuration()
    {
        float elapsed = tweener[0].Elapsed();
        float halfDuration = tweener[0].Duration() / 2f;

        if (elapsed >= halfDuration)
        {
            // Mostrar la cara frontal después de la animación
            cardBack.SetActive(false);
            cardFront.SetActive(true);
        }
    }

    private void CheckDeselectHalfDuration()
    {
        float elapsed = tweener[1].Elapsed();
        float halfDuration = tweener[1].Duration() / 2f;

        if (elapsed >= halfDuration)
        {
            // Mostrar la cara trasera después de la animación
            cardFront.SetActive(false);
            cardBack.SetActive(true);
        }
    }

    public Image GetCardBackBackground() => cardBackBackground;

    public void SetObjectMatch()
    {
        objectMatch = true;
    }

    public void SetCardImage(Sprite sprite)
    {
        cardFrontImage.sprite = sprite;
    }

    public bool GetObjectMatch() => objectMatch;

    public void DisableCardBackButton() => cardBackButton.interactable = false;
}
