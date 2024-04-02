using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Fish : Cookables
{
    private void Start()
    {
        originalPosition = transform.position; 
        lockOntoNewPosition = false; 
        isValidPosition = true; 
    }
    protected override void OnMouseDown()
    {
        base.OnMouseDown();
    }

    protected override void OnMouseDrag()
    {
        if (!lockOntoNewPosition) base.OnMouseDrag();
        else StartCoroutine(AllowDrag()); 
    }

    protected override void OnMouseUp()
    {
        if (!isValidPosition) transform.position = originalPosition; 
    }

    IEnumerator AllowDrag()
    {
        yield return new WaitForSeconds(0.5f);
        lockOntoNewPosition = false; 
    } 
} 
 