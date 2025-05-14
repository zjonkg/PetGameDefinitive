using UnityEngine;

public class ReordenadorUI : MonoBehaviour
{
    [SerializeField] private RectTransform imagenAFondo;

    void Start()
    {
        // Mover la imagen al fondo de la jerarquía (índice 0)
        if (imagenAFondo != null)
        {
            imagenAFondo.SetSiblingIndex(0);
        }
    }

    // También podrías exponerlo públicamente
    public void EnviarAlFondo(RectTransform imagen)
    {
        if (imagen != null)
        {
            imagen.SetSiblingIndex(0);
        }
    }

    public void TraerAlFrente(RectTransform imagen)
    {
        if (imagen != null)
        {
            imagen.SetAsLastSibling(); // Lo pone al frente
        }
    }
}

