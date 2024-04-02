using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fryer : MonoBehaviour
{
    bool IsSeatEmpty()
    {
        return GetComponentInChildren<Fish>()? false: true;  
    } 
     
    private void OnTriggerEnter(Collider other)
    {
        // Lock Onto Fryer if seat is empty 
        if (other.gameObject.CompareTag("Fish"))
        {
            Fish fish = other.GetComponent<Fish>();
            if (!fish.canCook) return; 
            if (!IsSeatEmpty())
            {
                fish.isValidPosition = false;
                other.transform.position = fish.originalPosition; 
                return; 
            }
            else
            {
                other.transform.position = transform.position;
                other.transform.parent = transform; 
                fish.lockOntoNewPosition = true;
                if (!fish.isCooking && fish.canCook) fish.StartCooking();
                Debug.Log("Cooking yippee"); 
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Lock Onto Chair
        if (other.gameObject.CompareTag("Fish"))
        {
            Fish fish = other.GetComponent<Fish>();
            fish.isValidPosition = true; 
            fish.lockOntoNewPosition = false;

            if (other.transform.parent == transform)
            {
                fish.StopCooking();
                other.transform.parent = null;
            } 
        }
    } 
}
