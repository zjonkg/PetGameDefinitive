using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemShop : MonoBehaviour
{
    public TMP_Text itemNameText;
    public TMP_Text itemPriceText;
    public Image itemImage;
    public Button increaseButton;
    public Button decreaseButton;
    public TMP_Text quantityText;

    [Tooltip("Imagen que se usará si no se encuentra la del JSON")]
    public Sprite defaultSprite;

    private ItemData item;
    private int selectedQuantity = 0;

    // Evento para notificar cambios en el carrito
    public static System.Action<ItemData, int> OnItemUpdated;

    public void Initialize(ItemData itemData)
    {
        item = itemData;
        itemNameText.text = item.name;
        itemPriceText.text = "$" + item.price.ToString();
        selectedQuantity = 0;
        UpdateUI();

        if (increaseButton != null)
            increaseButton.onClick.AddListener(IncreaseQuantity);

        if (decreaseButton != null)
            decreaseButton.onClick.AddListener(DecreaseQuantity);
    }

    private void IncreaseQuantity()
    {
        selectedQuantity++;
        UpdateUI();
        OnItemUpdated?.Invoke(item, selectedQuantity);
    }

    private void DecreaseQuantity()
    {
        if (selectedQuantity > 0)
        {
            selectedQuantity--;
            UpdateUI();
            OnItemUpdated?.Invoke(item, selectedQuantity);
        }
    }

    private void UpdateUI()
    {
        quantityText.text = selectedQuantity.ToString();

        // Cargar imagen o usar por defecto
        Sprite sprite = Resources.Load<Sprite>(item.imagePath);
        itemImage.sprite = sprite != null ? sprite : defaultSprite;
    }
}