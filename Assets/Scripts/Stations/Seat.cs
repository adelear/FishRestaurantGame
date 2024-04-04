using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seat : MonoBehaviour
{
    bool IsSeatEmpty()
    {
        return GetComponentInChildren<Fish>()? false: true; 
    }

    private void OnTriggerEnter(Collider other)
    {
        // Lock Onto Chair if seat is empty 
        if (other.gameObject.CompareTag("Fish"))
        {
            Fish fish = other.GetComponent<Fish>();
            if (fish == null) return; 
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
                fish.isSeated = true; 
                fish.lockOntoNewPosition = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Lock Onto Chair
        if (other.gameObject.CompareTag("Fish"))
        {
            Fish fish = other.GetComponent<Fish>();
            if (fish == null) return; 
            fish.isValidPosition = true; 
            fish.lockOntoNewPosition = false;
            fish.isSeated = false; 

            if (other.transform.parent == transform) other.transform.parent = null; 
        }
    } 
}
