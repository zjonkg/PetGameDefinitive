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
            Destroy(chatBubble.gameObject, 5.5f);
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


    [SerializeField] private float textSpeed = 0.02f;
    private Coroutine typingCoroutine;
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
            textMeshPro.maxVisibleLines = 2;
            textMeshPro.overflowMode = TextOverflowModes.Overflow;

            textMeshPro.rectTransform.sizeDelta = new Vector2(maxTextWidth, textMeshPro.fontSize * 0.001f + paddingY * 1f);
            textMeshPro.text = "";

            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeText(text));

            Vector2 textSize = new Vector2(maxTextWidth, textMeshPro.fontSize * 2);

            float backgroundWidth = textSize.x + paddingX * 4f;
            float backgroundHeight = textSize.y / 4f;
            backgroundSpriteRenderer.size = new Vector2(backgroundWidth, backgroundHeight);

            textMeshPro.rectTransform.localPosition = Vector3.zero;
            backgroundSpriteRenderer.transform.localPosition = Vector3.zero;

            // Posicionamiento del ícono
            if (iconSpriteRenderer != null)
            {
                float spacingAbove = 2f;
                iconSpriteRenderer.transform.localPosition = new Vector3(
                    0f,
                    backgroundHeight / 2f + iconSpriteRenderer.bounds.size.y / 2f + spacingAbove,
                    0f
                );
            }
        }
    }

    IEnumerator TypeText(string fullText)
    {
        textMeshPro.text = "";
        int segmentStart = 0;

        while (segmentStart < fullText.Length)
        {
            textMeshPro.text = "";
            int lastSpaceIndex = -1;
            int lastValidCharIndex = segmentStart;

            for (int i = segmentStart + 1; i <= fullText.Length; i++)
            {
                string currentText = fullText.Substring(segmentStart, i - segmentStart);
                textMeshPro.text = currentText;
                textMeshPro.ForceMeshUpdate();

                if (textMeshPro.textInfo.lineCount > 2)
                {
                    // Si nos pasamos, cortamos hasta el último espacio (si existe)
                    if (lastSpaceIndex != -1)
                        lastValidCharIndex = lastSpaceIndex;
                    break;
                }

                // Guardamos el último espacio para cortar de forma segura
                if (fullText[i - 1] == ' ')
                {
                    lastSpaceIndex = i - 1;
                }

                lastValidCharIndex = i;
                yield return new WaitForSeconds(textSpeed);
            }

            // Mostramos el segmento válido
            textMeshPro.text = fullText.Substring(segmentStart, lastValidCharIndex - segmentStart);

            yield return new WaitForSeconds(1f);
            segmentStart = lastValidCharIndex;
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