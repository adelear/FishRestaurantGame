using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cookables : Draggable
{
    [Header("Position Components")] 
    public bool isSeated; // Controls whether or not the fish can move around the room 
    public bool isBeingDragged; // Controls whether or not the fish is flailing  
    public Vector3 originalPosition;
    public bool lockOntoNewPosition;
    public bool isValidPosition;

    [Header("Cooking Components")] 
    public float cookingTimer = 0f; 
    public bool isCooking = false;
    public float foodQuality = 0f;
    public bool canCook; 

    protected virtual void Start()
    {
        originalPosition = transform.position;
        lockOntoNewPosition = false;
        isValidPosition = true;
        canCook = true; 
    }
    protected override void OnMouseDown()
    {
        base.OnMouseDown();
    }

    protected override void OnMouseDrag() 
    {
        if (!lockOntoNewPosition) base.OnMouseDrag();
        else StartCoroutine(AllowDrag());

        isBeingDragged = true; 
    }

    protected void OnMouseUp() 
    {
        if (!isValidPosition) transform.position = originalPosition;
        isBeingDragged = false; 
    }

    IEnumerator AllowDrag()
    {
        yield return new WaitForSeconds(0.5f);
        lockOntoNewPosition = false;
    }

    // When cooking start a timer: less than 4 is undercooked, 4-5 is just right, more than 5 is overcooked 
    // Undercooked, just right and overcooked affects the quality of the cookable

    protected virtual void Update()
    {
        if (isCooking)
        {
            cookingTimer += Time.deltaTime;
            Debug.Log("Cookign Timer: " + cookingTimer);
        }
    }

    public virtual void StartCooking()
    {
        if (!isCooking && canCook)
        {
            isCooking = true;
            canCook = false; 
        }
    }

    public void StopCooking()
    {
        if (isCooking) DetermineQuality();
    }

    public void DetermineQuality()
    {
        float qualityPercentage = cookingTimer / 5f; 
        foodQuality = Mathf.Clamp(qualityPercentage * 5f, 0f, 5f); 
        isCooking = false;
        Debug.Log("Cooking done. Food quality: " + foodQuality); 
    }
}
