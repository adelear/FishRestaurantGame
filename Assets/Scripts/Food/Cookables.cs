using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cookables : Draggable
{
    public Vector3 originalPosition;
    public bool lockOntoNewPosition;
    public bool isValidPosition;

    private float cookingTimer = 0f; 
    private bool isCooking = false;

    private float foodQuality = 0f;
    private bool canCook; 

    private void Start()
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
    }

    protected virtual void OnMouseUp() 
    {
        if (!isValidPosition) transform.position = originalPosition;
    }

    IEnumerator AllowDrag()
    {
        yield return new WaitForSeconds(0.5f);
        lockOntoNewPosition = false;
    }

    // When cooking start a timer: less than 4 is undercooked, 4-5 is just right, more than 5 is overcooked 
    // Undercooked, just right and overcooked affects the quality of the cookable

    private void Update()
    {
        if (isCooking)
        {
            cookingTimer += Time.deltaTime; 

            if (cookingTimer >= 5f)
            {
                DetermineQuality();
            }
        }
    }

    private void StartCooking()
    {
        if (!isCooking && canCook)
        {
            isCooking = true;
            canCook = false; 
            cookingTimer = 0f;
            foodQuality = 0f;
        }
    }

    private void DetermineQuality()
    {
        float qualityPercentage = cookingTimer / 5f; 
        foodQuality = Mathf.Clamp(qualityPercentage * 5f, 0f, 5f); 
        isCooking = false;
        Debug.Log("Cooking done. Food quality: " + foodQuality); 
    }
}
