using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class MakeA3DObjectDraggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Camera m_cam;
    private Vector3 initialPosition;
    private Coroutine moveBackCoroutine;
    public float returnSpeed = 5f;

    private Inventory3DCarousel carousel;

    public void SetCarousel(Inventory3DCarousel refCarousel)
    {
        carousel = refCarousel;
    }

    void Start()
    {
        m_cam = Camera.main;

        if (m_cam.GetComponent<PhysicsRaycaster>() == null)
        {
            m_cam.gameObject.AddComponent<PhysicsRaycaster>();
        }

        initialPosition = transform.localPosition; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (moveBackCoroutine != null)
        {
            StopCoroutine(moveBackCoroutine);
            moveBackCoroutine = null;
        }

        Ray R = m_cam.ScreenPointToRay(eventData.position);
        Vector3 PO = transform.position;
        Vector3 PN = -m_cam.transform.forward;
        float t = Vector3.Dot(PO - R.origin, PN) / Vector3.Dot(R.direction, PN);
        Vector3 P = R.origin + R.direction * t;

        transform.position = P;
    }

    public void setInitialPosition(Vector3 newPosition)
    {
        initialPosition = newPosition; 
    }

    public void OnBeginDrag(PointerEventData eventData) { }

    public void OnEndDrag(PointerEventData eventData)
    {
        CheckPositionAndAct();
        moveBackCoroutine = StartCoroutine(SmoothReturn());
    }

    private IEnumerator SmoothReturn()
    {
        Vector3 start = transform.localPosition;
        float elapsed = 0f;

        while (Vector3.Distance(transform.localPosition, initialPosition) > 0.01f)
        {
            elapsed += Time.deltaTime * returnSpeed;
            transform.localPosition = Vector3.Lerp(start, initialPosition, elapsed);
            yield return null;
        }

        transform.localPosition = initialPosition;
    }


    void CheckPositionAndAct()
    {
        Vector3 pos = transform.position;

        bool isInXRange = pos.x >= -0.90f && pos.x <= -0.30f;
        bool isInYRange = pos.y >= 1.5f && pos.y <= 2.5f;

        if (isInXRange && isInYRange)
        {
            Debug.Log("¡Está dentro del área objetivo!");
            carousel.EatCurrentItem();
        }
    }

}
