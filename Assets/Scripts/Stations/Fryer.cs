using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fryer : MonoBehaviour
{
    [SerializeField] ParticleSystem steam; 
    bool IsSeatEmpty()
    {
        if (GetComponentInChildren<Cookables>()) return false;
        else return true; 
    } 

    private void OnTriggerEnter(Collider other)
    {
        // Lock Onto Fryer if seat is empty 
        if (!IsSeatEmpty()) return; 
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
                    steam.Play(); 
                    Debug.Log("Cooking yippee");
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Fish") || other.gameObject.CompareTag("Patty") )
        {
            Cookables cookables = other.GetComponent<Cookables>();

            // If the exiting cookable is the one currently cooking in the fryer,
            // don't remove it from the parent and stop cooking
            if (cookables.isSeated && cookables.isCooking)
            {
                cookables.isValidPosition = true;
                cookables.lockOntoNewPosition = false;
                cookables.StopCooking();
                cookables.transform.parent = null; 
            }
            else 
            {
                cookables.isValidPosition = true;
                cookables.lockOntoNewPosition = false;
                other.transform.parent = null;
            }
        }
    }

}
