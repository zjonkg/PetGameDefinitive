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

    void Start()
    {
        m_cam = Camera.main;

        if (m_cam.GetComponent<PhysicsRaycaster>() == null)
        {
            m_cam.gameObject.AddComponent<PhysicsRaycaster>();
        }

        initialPosition = transform.localPosition; // <--- CAMBIO AQUÍ
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
}
