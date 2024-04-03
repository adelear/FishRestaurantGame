using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedFish : MonoBehaviour
{
    // Raycast behind it checking for a Fish compare tag 
    // If there is a fish behind it, even if its not hungry, bring the z value closer to the fish
    //if there is a fish behind it that is hungry, change fish state to Served and destroy object
    private float raycastDistance = 15f;

    private void Update()
    {
        // Cast a ray backward from the object's position
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance))
        {
            if (!hit.collider.CompareTag("Fish")) return;
            if (GetComponent<Cookables>().canCook || GetComponent<Cookables>().isBeingDragged) return; 
            Fish fish = hit.collider.GetComponent<Fish>();
            if (fish == null) return;
            // Movinf object's z value closer to the fish
            Vector3 newPos = transform.position;
            newPos.z = hit.collider.transform.position.z - 2f;
            transform.position = newPos;

            if (fish.CurrentState != FishStates.Hungry) return;
            if (fish.isBeingDragged) return; 
            fish.ChangeState(FishStates.Served);
            fish.Served(gameObject); 
            Destroy(gameObject);
        }
        Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red); 
    }
}
