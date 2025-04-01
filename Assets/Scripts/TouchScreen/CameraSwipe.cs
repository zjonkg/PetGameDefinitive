using UnityEngine;
using System.Collections;

public class CameraSwipe : MonoBehaviour
{
    public Transform[] cameraPositions; // Puntos donde se mover� la c�mara
    public float swipeThreshold = 50f;  // Distancia m�nima para detectar el swipe
    public float moveSpeed = 5f;        // Velocidad del movimiento de la c�mara
    private int currentIndex = 0;
    private Vector2 startTouchPosition;

    void Update()
    {
        DetectSwipe();
    }

    void DetectSwipe()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                float deltaX = touch.position.x - startTouchPosition.x;

                if (Mathf.Abs(deltaX) > swipeThreshold)
                {
                    if (deltaX > 0) 
                        MoveCamera(-1);
                    else 
                        MoveCamera(1);
                }
            }
        }
    }

    void MoveCamera(int direction)
    {
        int newIndex = currentIndex + direction;
        if (newIndex >= 0 && newIndex < cameraPositions.Length)
        {
            currentIndex = newIndex;
            StopAllCoroutines();
            StartCoroutine(SmoothMove(transform.position, cameraPositions[currentIndex].position));
        }
    }

    IEnumerator SmoothMove(Vector3 start, Vector3 end)
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }
    }
}
