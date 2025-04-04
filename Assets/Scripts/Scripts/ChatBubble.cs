using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatBubble : MonoBehaviour
{

    public static void Create(Transform parent, Vector3 localPosition, IconType iconType, string text)
    {
        // Asegúrate de que pfChatBubble esté asignado en GameAssets
        Transform chatBubbleTransform = Instantiate(GameAssets.i.pfChatBubble, parent);
        chatBubbleTransform.localPosition = localPosition;

        // Inicializa la burbuja de chat
        ChatBubble chatBubble = chatBubbleTransform.GetComponent<ChatBubble>();
        if (chatBubble != null)
        {
            chatBubble.Setup(iconType, text);
            // Destruir la burbuja después de 6 segundos
            Destroy(chatBubble.gameObject, 6f);
        }
        else
        {
            Debug.LogError("No se encontró el componente ChatBubble en el prefab.");
        }
    }

    public enum IconType
    {
        Happy,
        Neutral,
        Angry,
    }

    [SerializeField] private Sprite happyIconSprite;
    [SerializeField] private Sprite neutralIconSprite;
    [SerializeField] private Sprite angryIconSprite;

    private SpriteRenderer backgroundSpriteRenderer;
    private SpriteRenderer iconSpriteRenderer;
    private TextMeshPro textMeshPro;

    private void Awake()
    {
        // Asegúrate de que los componentes estén asignados correctamente
        backgroundSpriteRenderer = transform.Find("Background")?.GetComponent<SpriteRenderer>();
        iconSpriteRenderer = transform.Find("Icon")?.GetComponent<SpriteRenderer>();
        textMeshPro = transform.Find("Text")?.GetComponent<TextMeshPro>();

        // Comprobamos si algún componente no está asignado
        if (backgroundSpriteRenderer == null)
        {
            Debug.LogError("Faltando el SpriteRenderer para Background en el prefab.");
        }
        if (iconSpriteRenderer == null)
        {
            Debug.LogError("Faltando el SpriteRenderer para Icon en el prefab.");
        }
        if (textMeshPro == null)
        {
            Debug.LogError("Faltando el componente TextMeshPro en el prefab.");
        }
    }

    private void Setup(IconType iconType, string text)
    {
        if (textMeshPro != null)
        {
            float maxTextWidth = 20f;
            float paddingX = 2f;
            float paddingY = 2f;

            textMeshPro.enableWordWrapping = true;
            textMeshPro.enableAutoSizing = false;
            textMeshPro.rectTransform.sizeDelta = new Vector2(maxTextWidth, 0f);

            textMeshPro.SetText(text);
            textMeshPro.ForceMeshUpdate();

            Vector2 textSize = textMeshPro.GetRenderedValues(false);

            float backgroundWidth = Mathf.Min(textSize.x, maxTextWidth) + paddingX * 2f;
            float backgroundHeight = textSize.y + paddingY * 2f;
            backgroundSpriteRenderer.size = new Vector2(backgroundWidth, backgroundHeight);

            textMeshPro.rectTransform.localPosition = Vector3.zero;
            backgroundSpriteRenderer.transform.localPosition = Vector3.zero;

            // 📌 Posicionar el ícono arriba del fondo
            if (iconSpriteRenderer != null)
            {
                float spacingAbove = 2f; // Espacio extra entre el fondo y el ícono
                iconSpriteRenderer.transform.localPosition = new Vector3(
                    0f,
                    backgroundHeight / 2f + iconSpriteRenderer.bounds.size.y / 2f + spacingAbove,
                    0f
                );
            }
        }
    }





    private Sprite GetIconSprite(IconType iconType)
    {
        // Retorna el sprite correspondiente basado en el icono
        switch (iconType)
        {
            default:
            case IconType.Happy: return happyIconSprite;
            case IconType.Neutral: return neutralIconSprite;
            case IconType.Angry: return angryIconSprite;
        }
    }
}