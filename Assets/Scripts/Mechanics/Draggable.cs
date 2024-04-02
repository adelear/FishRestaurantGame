using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    public LayerMask draggableLayer; 
    Vector3 mousePosition;
    public float zOffsetFactor = 0.01f;
    float maxZPos = 3f;
    float minZPos = -2f;

    private Vector3 GetMouseWorldPosition()
    {
        return Camera.main.WorldToScreenPoint(transform.position); 
    }

    protected virtual void OnMouseDown()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, draggableLayer))
        {
            mousePosition = Input.mousePosition - GetMouseWorldPosition();
        }
    }

    protected virtual void OnMouseDrag()
    {
        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);

        float excludedHeight = 50f;
        float adjustedMouseY = Mathf.Clamp(Input.mousePosition.y + excludedHeight, 0f, Screen.height);
        float zMovement = (adjustedMouseY - Screen.height / 2f) * zOffsetFactor;
        newPos.z += zMovement;
        newPos.z = Mathf.Clamp(newPos.z + zMovement, minZPos, maxZPos); 

        Collider[] colliders = Physics.OverlapSphere(newPos, GetComponent<Collider>().bounds.extents.magnitude);
        bool hasCollision = false;
        foreach (Collider collider in colliders)
        {
            if (collider != GetComponent<Collider>())
            {
                hasCollision = true;
                break;
            }
        }

        if (hasCollision && newPos.y < transform.position.y)
        {
            newPos.y = transform.position.y;
            GetComponent<Rigidbody>().velocity = Vector3.zero; 
        }

        transform.position = newPos;
    }
}
