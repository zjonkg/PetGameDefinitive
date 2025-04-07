using UnityEngine;

public class ForceLandscape : MonoBehaviour
{
    void Start()
    {
        // Fuerza la orientaci�n horizontal
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        // Tambi�n puedes bloquear a solo una orientaci�n horizontal si lo deseas:
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.orientation = ScreenOrientation.AutoRotation;
    }
}
