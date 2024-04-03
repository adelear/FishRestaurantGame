using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    public LayerMask draggableLayer; 
    Vector3 mousePosition;
    private float zOffsetFactor = 0.001f;
    float maxZPos = 8.5f;
    float minZPos = -0.5f;

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
        Plane plane = new Plane(Camera.main.transform.forward, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (plane.Raycast(ray, out distance))
        {
            Vector3 newPos = ray.GetPoint(distance);

            float excludedHeight = 50f;
            float adjustedMouseY = Mathf.Clamp(Input.mousePosition.y + excludedHeight, 0f, Screen.height-50);
            float zMovement = (adjustedMouseY - Screen.height / 2f) * zOffsetFactor;
            newPos.z += zMovement * zOffsetFactor;
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

}
