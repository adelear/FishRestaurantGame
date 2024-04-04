using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garbage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Cookables>())
        {
            if (!other.GetComponent<Cookables>().canCook)
                Destroy(other.gameObject); 
        }
    }
}
