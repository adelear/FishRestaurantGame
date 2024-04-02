using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Fish : Draggable
{
    Vector3 originalPosition;
    bool lockOntoNewPosition; 

    private void Start()
    {
        originalPosition = transform.position; 
        lockOntoNewPosition = false; 
    }
    protected override void OnMouseDown()
    {
        base.OnMouseDown();
    }

    protected override void OnMouseDrag()
    {
        if (!lockOntoNewPosition) base.OnMouseDrag();
        else
        {
            StartCoroutine(AllowDrag()); 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Lock Onto Chair
        if (other.gameObject.CompareTag("Chair"))
        {
            transform.position = other.transform.position; 
            lockOntoNewPosition = true; 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Lock Onto Chair
        if (other.gameObject.CompareTag("Chair"))
        {
            transform.position = originalPosition;
            lockOntoNewPosition = false;
        }
    }


    protected override void OnMouseUp()
    {
        if (!lockOntoNewPosition)
        {
            transform.position = originalPosition; 
        }
    }

    IEnumerator AllowDrag()
    {
        yield return new WaitForSeconds(0.5f);
        lockOntoNewPosition = false; 
    }
} 
