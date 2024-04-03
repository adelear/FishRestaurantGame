using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Fish : Cookables
{
    public event Action<FishStates> OnStateChanged;
    [Header("Fish Components")] 
    private FishStates currentState;
    public FishType fishType;

    [Header("Animations")]
    private Animator anim;
    public string idleAnim;
    public string hungryAnim;
    public string flailAnim;
    public string angryAnim; 

    // Enters the restaurant chilling
    // From chilling, a coroutine starts that makes it randomly get hungry from 15s of entering to a minute 

    // From Hungry, timer begins. They must be fed within one minute OR they will leave Hangry 
    // From Hungry, if fed, they will be served, give a review in speech bubble, then disappear 

    private Coroutine hungerCoroutine; // coroutine before they get hungry 
    private Coroutine feedingTimeCoroutine; //coroutine that starts where they must be fed before leaving
    private float feedingDuration = 60f;
    private float remainingFeedingTime = 60f; 
    public FishStates CurrentState
    {
        get { return currentState; }
        set
        {
            if (currentState != value)
            {
                currentState = value;

                OnStateChanged?.Invoke(currentState);

                Debug.Log("State Changed to " + currentState); 
                switch (currentState)
                {
                    case FishStates.Chilling:
                        StartChilling(); 
                        break;
                    case FishStates.Hungry:
                        Hungry(); 
                        break;
                    case FishStates.Cooking:
                        break;
                    case FishStates.Served:
                        break;
                    case FishStates.LeavingHangry:
                        LeavingHangry(); 
                        break;
                    default:
                        Debug.LogError("Unhandled fish state!");
                        break;
                }
            }
        }
    }

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>(); 
        isSeated = false;
        CurrentState = FishStates.Chilling;
        StartChilling(); 
    }

    public void ChangeState(FishStates newState)
    {
        CurrentState = newState;
    }

    private void StartChilling()
    {
        anim.Play(idleAnim); 
        hungerCoroutine = StartCoroutine(HungerCoroutine()); 
    }

    private IEnumerator HungerCoroutine()
    {
        float timeUntilHungry = UnityEngine.Random.Range(15f, 30f);
        yield return new WaitForSeconds(timeUntilHungry);
        CurrentState = FishStates.Hungry;
    }

    private void Hungry()
    {
        anim.Play(hungryAnim); 
        feedingTimeCoroutine = StartCoroutine(FeedingTimer());
    }

    private IEnumerator FeedingTimer()
    {
        while (remainingFeedingTime > 0f)
        {
            yield return null;
            remainingFeedingTime -= Time.deltaTime;
            //Debug.Log("Time Left: " + remainingFeedingTime);
        }
        // If not fed within the feeding duration, leave hangry
        CurrentState = FishStates.LeavingHangry;
    }

    public void Served(GameObject nomNoms)
    {
        Cookables cookables = nomNoms.GetComponent<Cookables>();
        if (feedingTimeCoroutine != null) StopCoroutine(feedingTimeCoroutine); 
        StartCoroutine(JudgementTime(cookables)); 
    }

    private void LeavingHangry()
    {
        anim.Play(angryAnim); 
        StartCoroutine(HangryCoroutine()); 
    }

    IEnumerator HangryCoroutine()
    {
        yield return new WaitForSeconds(3f);
        FishSpawner.Instance.currentFishNum--;
        GameManager.Instance.ChangeRating(1f);
        Destroy(gameObject);
    }

    public override void StartCooking()
    {
        base.StartCooking();
        CurrentState = FishStates.Cooking;
        StopAllCoroutines(); 
    }

    private void WalkAround()
    {
        // Walk Around When Not Seated or Picked Up
    }

    IEnumerator JudgementTime(Cookables nomNoms)
    {
        yield return new WaitForSeconds(3f); 

        if (nomNoms.foodQuality < 2)
        {
            Debug.Log("REVIEW: FOOD IS SHIT"); 
        }
        else if (nomNoms.foodQuality >= 2 && nomNoms.foodQuality < 4)
        {
            Debug.Log("HMM.... OKAY I GUESS"); 
        }
        else
        {
            Debug.Log("YUMMY"); 
        }
        yield return new WaitForSeconds(3f);
        GameManager.Instance.ChangeRating(nomNoms.foodQuality); 
        FishSpawner.Instance.currentFishNum--; 
        Destroy(gameObject); 
    }

    public override void DetermineQuality()
    {
        float qualityPercentage = cookingTimer / 5f;

        switch (fishType)
        {
            case FishType.Anchovy:  
                foodQuality = Mathf.Clamp(qualityPercentage * 5f * 0.33f, 0f, 5f);
                break;
            case FishType.Tuna:
                foodQuality = Mathf.Clamp(qualityPercentage * 5f * 0.66f, 0f, 5f);
                break;
            case FishType.Salmon:
                foodQuality = Mathf.Clamp(qualityPercentage * 5f, 0f, 5f);
                break;
            default:
                Debug.LogError("Unhandled fish type!");
                break;
        }

        isCooking = false;
        Debug.Log("Cooking done. Food quality: " + foodQuality);
    }

    protected override void OnMouseDrag()
    {
        if (CurrentState != FishStates.Served) base.OnMouseDrag();
    }

    protected override void Update()
    {
        base.Update();
        if (isBeingDragged) anim.Play(flailAnim);
        else if (CurrentState != FishStates.Hungry)
        {
            anim.Play(idleAnim);
            return; 
        }
        if (isBeingDragged)
        {
            // Animation plays
            // Pause the feeding timer 
            if (feedingTimeCoroutine != null)
            {
                StopCoroutine(feedingTimeCoroutine);
                feedingTimeCoroutine = null; 
            }
        }
        else if (remainingFeedingTime > 0f) // Resume the feeding timer if it's not yet completed
        {
            if (feedingTimeCoroutine == null)
            {
                Hungry(); 
            }
        }
    }

}
