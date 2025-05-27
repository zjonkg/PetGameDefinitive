using UnityEngine;

public class ToggleChatPanel : MonoBehaviour
{
    public GameObject targetObject; 

    public void ToggleVisibility()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(!targetObject.activeSelf);
        }
    }
}
