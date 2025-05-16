using UnityEngine;
using DG.Tweening;
using TMPro;

public class PopupMessage : MonoBehaviour
{
    public static PopupMessage Instance;

    [Header("Referencias UI")]
    public RectTransform popupPanel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI messageText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        popupPanel.localScale = Vector3.zero;
    }

    public void ShowPopup(string title, string message)
    {
        titleText.text = title;
        messageText.text = message;

        popupPanel.localScale = Vector3.zero;
        popupPanel.DOScale(1.2f, 0.2f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                popupPanel.DOScale(1f, 0.1f)
                    .OnComplete(() =>
                    {

                        DOVirtual.DelayedCall(2.5f, () => HidePopup());
                    });
            });
    }


    public void HidePopup()
    {
        popupPanel.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
    }
}
