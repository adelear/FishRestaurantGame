using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedFish : MonoBehaviour
{
    private float spherecastRadius = 2f;
    private float spherecastDistance = 2f;

    private void Update()
    {
        // Cast a sphere forward from the object's position
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, spherecastRadius, transform.forward, out hit, spherecastDistance))
        {
            DebugDrawSphereCast(transform.position, transform.forward, hit.distance, spherecastRadius, Color.red); 
            if (hit.collider.CompareTag("Fish"))
            {
                Cookables cookables = GetComponent<Cookables>();
                if (cookables != null) if (cookables.canCook) return;
                Fish fish = hit.collider.GetComponent<Fish>();
                if (fish != null && fish.CurrentState == FishStates.Hungry ) 
                {
                    // Move the object's z value closer to the fish
                    Vector3 newPos = transform.position;
                    newPos.z = hit.collider.transform.position.z - 2f;
                    transform.position = newPos;

                    fish.ChangeState(FishStates.Served);
                    fish.Served(gameObject);
                    Destroy(gameObject);
                }
            }
        }
    }

    private void DebugDrawSphereCast(Vector3 origin, Vector3 direction, float distance, float radius, Color color)
    {
        Vector3 end = origin + direction * distance;
        Debug.DrawLine(origin - Vector3.right * radius, end - Vector3.right * radius, color);
        Debug.DrawLine(origin + Vector3.right * radius, end + Vector3.right * radius, color);
        Debug.DrawLine(origin - Vector3.up * radius, end - Vector3.up * radius, color);
        Debug.DrawLine(origin + Vector3.up * radius, end + Vector3.up * radius, color);
        Debug.DrawLine(origin - Vector3.forward * radius, end - Vector3.forward * radius, color);
        Debug.DrawLine(origin + Vector3.forward * radius, end + Vector3.forward * radius, color);

        Debug.DrawLine(end - Vector3.right * radius, end + Vector3.right * radius, color);
        Debug.DrawLine(end - Vector3.up * radius, end + Vector3.up * radius, color);
        Debug.DrawLine(end - Vector3.forward * radius, end + Vector3.forward * radius, color);
    } 
}
