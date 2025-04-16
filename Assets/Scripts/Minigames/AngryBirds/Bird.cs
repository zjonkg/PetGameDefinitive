using UnityEngine;

public class Bird : MonoBehaviour
{
    private Rigidbody2D rb;
    private Camera mainCamera;
    private Vector2 startPosition, clampedPosition;
    [SerializeField] private float force = 300f;
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private int maxThrows = 3;
    private int currentThrows = 0;
    private bool isDragging = false;
    private bool canThrow => currentThrows < maxThrows;

    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        startPosition = transform.position;
    }

    void Update()
    {
        if (!canThrow) return;

#if UNITY_EDITOR
        // Para pruebas en el Editor con el mouse
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 touchPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(touchPosition, transform.position) <= 1f)
            {
                isDragging = true;
            }
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            SetPosition(mainCamera.ScreenToWorldPoint(Input.mousePosition));
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            Throw();
        }
#else
        // Para dispositivos móviles
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = mainCamera.ScreenToWorldPoint(touch.position);

            if (touch.phase == TouchPhase.Began)
            {
                if (Vector2.Distance(touchPosition, transform.position) <= 1f)
                {
                    isDragging = true;
                }
            }

            if (touch.phase == TouchPhase.Moved && isDragging)
            {
                SetPosition(touchPosition);
            }

            if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && isDragging)
            {
                isDragging = false;
                Throw();
            }
        }
#endif
    }

    private void SetPosition(Vector2 dragPosition)
    {
        clampedPosition = dragPosition;

        float dragDistance = Vector2.Distance(startPosition, clampedPosition);
        if (dragDistance > maxDistance)
        {
            clampedPosition = startPosition + (dragPosition - startPosition).normalized * maxDistance;
        }

        if (clampedPosition.x > startPosition.x)
        {
            clampedPosition.x = startPosition.x;
        }

        transform.position = clampedPosition;
    }

    private void Throw()
    {
        if (!canThrow) return;

        rb.isKinematic = false;
        Vector2 throwVector = startPosition - clampedPosition;
        rb.AddForce(throwVector * force);

        currentThrows++;
        Invoke("Reset", 5f);
    }

    private void Reset()
    {
        if (!canThrow)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            return;
        }

        transform.position = startPosition;
        transform.rotation = Quaternion.identity;
        rb.isKinematic = true;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        mainCamera.GetComponent<CameraMovement>().ResetPosition();
    }
}
