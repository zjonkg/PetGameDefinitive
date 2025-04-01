using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider))]
public class MakeA3DObjectDraggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Camera m_cam;
    private Vector3 initialPosition;

    void Start()
    {
        m_cam = Camera.main;

        if (m_cam.GetComponent<PhysicsRaycaster>() == null)
        {
            m_cam.gameObject.AddComponent<PhysicsRaycaster>(); // Agrega un PhysicsRaycaster si no existe
        }

        initialPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Ray R = m_cam.ScreenPointToRay(eventData.position); // Usar eventData.position en lugar de Input.mousePosition
        Vector3 PO = transform.position;
        Vector3 PN = -m_cam.transform.forward;
        float t = Vector3.Dot(PO - R.origin, PN) / Vector3.Dot(R.direction, PN);
        Vector3 P = R.origin + R.direction * t;

        transform.position = P;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Opcional: Cambiar color o efectos visuales
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = initialPosition; // Restaurar posición inicial
    }
}
