using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patty : Cookables
{
    public override void StartCooking()
    {
        base.StartCooking();
    }

    public override void StopCooking()
    {
        base.StopCooking();
    }

    public override void DetermineQuality()
    {
        base.DetermineQuality();
    }

    protected override void OnMouseDown()
    {
        base.OnMouseDown();
    }

    protected override void OnMouseDrag()
    {
        base.OnMouseDrag();
    }

    protected override void Update()
    {
        base.Update(); 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Fish"))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null) rb.velocity = Vector3.zero;
        }
    } 
}
