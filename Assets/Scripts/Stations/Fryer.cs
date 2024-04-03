using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fryer : MonoBehaviour
{
    bool IsSeatEmpty()
    {
        bool empty = (GetComponentInChildren<Fish>() ? false : true) || (GetComponentInChildren<Cookables>() ? false : true);
        return empty; 
    } 

    private void OnTriggerEnter(Collider other)
    {
        // Lock Onto Fryer if seat is empty 
        if (other.gameObject.CompareTag("Fish") || other.gameObject.CompareTag("Patty"))
        {
            Cookables cookables = other.GetComponent<Cookables>();
            if (!cookables.canCook) return;
            if (!IsSeatEmpty())
            {
                cookables.isValidPosition = false;
                other.transform.position = cookables.originalPosition;
                return;
            }
            else
            {
                other.transform.position = transform.position;
                other.transform.parent = transform;
                cookables.lockOntoNewPosition = true;
                if (!cookables.isCooking && cookables.canCook)
                {
                    cookables.isSeated = true;
                    cookables.StartCooking();
                    Debug.Log("Cooking yippee");
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Lock Onto Chair
        if (other.gameObject.CompareTag("Fish") || other.gameObject.CompareTag("Patty"))
        {
            Cookables cookables = other.GetComponent<Cookables>();
            cookables.isValidPosition = true;
            cookables.lockOntoNewPosition = false;

            if (other.transform.parent == transform)
            {
                cookables.StopCooking();
                other.transform.parent = null;
            } 
        }
    } 
}
