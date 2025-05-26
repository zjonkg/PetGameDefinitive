using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BurbujaFactory
{
    public static GameObject CrearBurbuja(string texto, bool esEnviado, Transform padre)
    {
        // Crear GameObject contenedor
        GameObject burbujaGO = new GameObject("BurbujaMensaje", typeof(RectTransform), typeof(Image), typeof(ContentSizeFitter), typeof(LayoutElement), typeof(VerticalLayoutGroup));
        burbujaGO.transform.SetParent(padre, false);

        // Fondo de la burbuja
        Image image = burbujaGO.GetComponent<Image>();
        image.color = esEnviado ? new Color32(220, 248, 198, 255) : new Color32(237, 237, 237, 255);
        image.raycastTarget = false;

        // RectTransform ajustes
        RectTransform rect = burbujaGO.GetComponent<RectTransform>();
        rect.pivot = new Vector2(0.5f, 1);
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(1, 1);
        rect.anchoredPosition = Vector2.zero;
        rect.offsetMin = new Vector2(0, rect.offsetMin.y);
        rect.offsetMax = new Vector2(0, rect.offsetMax.y);

        // Layout group padding y alineación
        VerticalLayoutGroup layoutGroup = burbujaGO.GetComponent<VerticalLayoutGroup>();
        layoutGroup.padding = new RectOffset(16, 16, 12, 12);
        layoutGroup.childAlignment = esEnviado ? TextAnchor.MiddleRight : TextAnchor.MiddleLeft;
        layoutGroup.childForceExpandHeight = false;
        layoutGroup.childForceExpandWidth = false;

        // Ajuste de tamaño automático
        ContentSizeFitter fitter = burbujaGO.GetComponent<ContentSizeFitter>();
        fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        // LayoutElement (ajuste dinámico)
        LayoutElement layout = burbujaGO.GetComponent<LayoutElement>();
        layout.flexibleWidth = 1f;
        layout.minWidth = 100;

        // Texto
        GameObject textoGO = new GameObject("Texto", typeof(TextMeshProUGUI));
        textoGO.transform.SetParent(burbujaGO.transform, false);

        TextMeshProUGUI tmp = textoGO.GetComponent<TextMeshProUGUI>();
        tmp.text = texto;
        tmp.fontSize = 34;
        tmp.color = Color.black;
        tmp.alignment = TextAlignmentOptions.TopLeft;
        tmp.enableWordWrapping = true;
        tmp.raycastTarget = false;

        // Limitar el ancho del texto
        LayoutElement textoLayout = textoGO.AddComponent<LayoutElement>();
        textoLayout.preferredWidth = 500; // límite para evitar desbordamientos
        textoLayout.flexibleWidth = 0;

        return burbujaGO;
    }
}
