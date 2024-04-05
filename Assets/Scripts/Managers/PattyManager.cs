using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PattyManager : MonoBehaviour
{
    public GameObject[] patties;
    public GameObject dialogueBox;  
    int pattyCount;

    public static PattyManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Another instance of FishSpawner already exists. Destroying this one.");
            Destroy(gameObject);
        }
    } 

    private void Start()
    {
        pattyCount = patties.Length; 
    }
    public void DecreasePattyCount()
    {
        pattyCount--; 
        if (pattyCount <= 0) dialogueBox.SetActive(true); 
    }

    
}
