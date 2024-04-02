using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    Vector3 mousePosition; 

    private Vector3 GetMouseWorldPosition()
    {
        return Camera.main.WorldToScreenPoint(transform.position); 
    }
    protected virtual void OnMouseDown()
    {
        mousePosition = Input.mousePosition - GetMouseWorldPosition();  
    }

    protected virtual void OnMouseDrag()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition); 
    }

    protected virtual void OnMouseUp()
    {
        
    }
}
