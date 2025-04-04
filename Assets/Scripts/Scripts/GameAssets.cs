/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */
 
using UnityEngine;

public class GameAssets : MonoBehaviour {

    private static GameAssets _i;

    public static GameAssets i
    {
        get
        {
            if (_i == null)
            {
                GameObject go = Resources.Load("GameAssets") as GameObject;
                if (go == null)
                {
                    Debug.LogError("No se pudo cargar el prefab 'GameAssets' desde Resources.");
                    return null;  // O puedes retornar un valor por defecto si prefieres.
                }
                _i = go.GetComponent<GameAssets>();
                if (_i == null)
                {
                    Debug.LogError("No se pudo encontrar el componente GameAssets en el objeto cargado.");
                    return null;
                }
            }
            return _i;
        }
    }


    public Transform pfChatBubble;
    public Sprite codeMonkeyHeadSprite;

}
