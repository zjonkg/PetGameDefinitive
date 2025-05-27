using TMPro;
using UnityEngine;

public class TransformPlayerID : MonoBehaviour
{

    public TextMeshProUGUI text;
    
    void Start ()
    {
        string player_id = "ESBCN10129" + PlayerPrefs.GetString("player_id");
        text.text = "ID: " + player_id;
    }
}
