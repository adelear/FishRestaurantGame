using System;
using System.Collections;
using UnityEngine;

public class Fish : Cookables
{

    [Header("Emote Components")]
    public GameObject emoteBubble;
    public Sprite[] emotes; // 0 happy, 1 is mad, 2 is MEH, 3 is hungry
    public SpriteRenderer emoteRenderer;

    public event Action<FishStates> OnStateChanged;
    private FishStates currentState;
    public FishType fishType;

    private Animator anim;
    public string idleAnim;
    public string hungryAnim;
    public string flailAnim;
    public string angryAnim;

    private Coroutine hungerCoroutine;
    private Coroutine feedingTimeCoroutine;
    private float feedingDuration = 60f;
    private float remainingFeedingTime = 60f;
    private float pausedTime; 
    private bool isPaused;

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
        while (!isPaused)
        {
            float timeUntilHungry = UnityEngine.Random.Range(15f, 30f);
            yield return new WaitForSeconds(timeUntilHungry);
            CurrentState = FishStates.Hungry;
        }
    }

    private void Hungry()
    {
        anim.Play(hungryAnim);
        emoteBubble.SetActive(true);
        emoteRenderer.sprite = emotes[3]; 
        feedingTimeCoroutine = StartCoroutine(FeedingTimer());
    }

    private IEnumerator FeedingTimer()
    {
        while (!isPaused && remainingFeedingTime > 0f)
        {
            yield return null;
            remainingFeedingTime -= Time.deltaTime;
            //Debug.Log("Time Left: " + remainingFeedingTime);
        }
        // If not fed within the feeding duration, leave hangry
        if (!isPaused) CurrentState = FishStates.LeavingHangry;
    }

    public void Served(GameObject nomNoms)
    {
        Cookables cookables = nomNoms.GetComponent<Cookables>();
        if (feedingTimeCoroutine != null) StopCoroutine(feedingTimeCoroutine);
        if (cookables.foodQuality < 2)
        {
            emoteRenderer.sprite = emotes[1];
            emoteBubble.SetActive(true);
            Debug.Log("REVIEW: FOOD IS SHIT");
        }
        else if (cookables.foodQuality >= 2 && cookables.foodQuality < 4)
        {
            emoteRenderer.sprite = emotes[2];
            emoteBubble.SetActive(true);
            Debug.Log("HMM.... OKAY I GUESS");
        }
        else
        {
            emoteRenderer.sprite = emotes[0];
            emoteBubble.SetActive(true);
            Debug.Log("YUMMY");
        }
        StartCoroutine(JudgementTime(cookables));
    }

    private void LeavingHangry()
    {
        anim.Play(angryAnim);
        emoteBubble.SetActive(true);
        emoteRenderer.sprite = emotes[1]; 
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
        if (isBeingDragged)
        {
            emoteBubble.SetActive(false);
            anim.Play(flailAnim);
        }
        else if (CurrentState != FishStates.Hungry && CurrentState != FishStates.LeavingHangry)
        {
            anim.Play(idleAnim);
            emoteBubble.SetActive(false); 
            return;
        }
        if (isBeingDragged)
        {
            // Animation plays
            // Pause the feeding timer 
            emoteBubble.SetActive(false); 
            if (feedingTimeCoroutine != null)
            {
                StopCoroutine(feedingTimeCoroutine);
                feedingTimeCoroutine = null;
            }
        }
        else if (remainingFeedingTime > 0f) 
        {
            if (feedingTimeCoroutine == null)
            {
                Hungry();
            }
        }
    }

    public void PauseTimers()
    {
        isPaused = true;
        pausedTime = Time.time; 
    }

    public void ResumeTimers()
    {
        if (isPaused)
        {
            isPaused = false;
            float pauseDuration = Time.time - pausedTime; 
            remainingFeedingTime -= pauseDuration;
            StartCoroutine(FeedingTimer()); 
        }
    }
}
