using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatBubble : MonoBehaviour
{


    public static void Create(Transform parent, Vector3 localPosition, IconType iconType, string text)
    {
        // Instanciamos la burbuja
        Transform chatBubbleTransform = Instantiate(GameAssets.i.pfChatBubble, parent);
        chatBubbleTransform.localPosition = localPosition;

        // Inicializamos
        ChatBubble chatBubble = chatBubbleTransform.GetComponent<ChatBubble>();
        if (chatBubble != null)
        {
            chatBubble.Setup(iconType, text);

            // Duración basada en el largo del texto
            float baseTime = 1.5f;
            float timePerCharacter = 0.06f;
            float duration = Mathf.Clamp(baseTime + text.Length * timePerCharacter, 2f, 12f);

            // Destruimos después de la duración calculada
            Destroy(chatBubble.gameObject, duration);
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

        string[] words = fullText.Split(' ');
        List<string> lines = new List<string>();
        string currentLine = "";

        foreach (string word in words)
        {
            string testLine = string.IsNullOrEmpty(currentLine) ? word : currentLine + " " + word;
            textMeshPro.text = testLine;
            textMeshPro.ForceMeshUpdate();

            if (textMeshPro.textInfo.lineCount > 2)
            {
                // Guardamos la línea anterior y empezamos nueva
                lines.Add(currentLine);
                currentLine = word;
            }
            else
            {
                currentLine = testLine;
            }
        }

        // Añadimos la última línea
        if (!string.IsNullOrEmpty(currentLine))
            lines.Add(currentLine);

        textMeshPro.text = "";

        // Ahora escribimos cada línea, con efecto de letra por letra
        foreach (string line in lines)
        {
            string current = "";
            foreach (char c in line)
            {
                current += c;
                textMeshPro.text = current;
                yield return new WaitForSeconds(textSpeed);
            }

            yield return new WaitForSeconds(1f); // Espera antes de la siguiente línea
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