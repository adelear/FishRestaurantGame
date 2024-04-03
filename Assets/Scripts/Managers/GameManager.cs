using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject scoreImg;
    [SerializeField] AudioManager asm; 
    [SerializeField] AudioClip LossSound;
    public static GameManager Instance { get; private set; }
    public enum GameState
    {
        GAME,
        PAUSE,
        DEFEAT,
        WIN
    }
    [SerializeField] private GameState currentState; 

    public event Action OnGameStateChanged;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;

        if (Instance != this)
            Destroy(gameObject);
    }

    public float AverageRating
    {
        get
        {
            if (numberOfRatings == 0)
                return 0f;
            else
                return totalRating / numberOfRatings;
        }
    }

    private float totalRating = 0;
    private int numberOfRatings = 0;
    public UnityEvent<float> OnAverageRatingChanged;

    public void ChangeRating(float newRating)
    {
        float previousAverageRating = AverageRating; // Store the previous average rating

        totalRating += newRating;
        numberOfRatings++;

        if (OnAverageRatingChanged != null)
        {
            OnAverageRatingChanged.Invoke(AverageRating);
            Debug.Log("New Average Rating: " + AverageRating);

            // Calculate the change in average rating
            float ratingChange = AverageRating - previousAverageRating;
            ChangeRatingBar(ratingChange); 
        }
    }

    private float ratingChangeSpeed = 0.5f; 

    private bool isLerping = false;
    private float targetWidthChange = 0f;

    public void ChangeRatingBar(float changeAmount)
    {
        if (scoreImg == null) return;
        float starWidth = 100f;
        float totalWidth = starWidth * 5;
        float widthChange = totalWidth * (changeAmount / 5f);
        float newWidth = scoreImg.GetComponent<RectTransform>().sizeDelta.x + widthChange;
        targetWidthChange = newWidth - scoreImg.GetComponent<RectTransform>().sizeDelta.x;
        if (!isLerping) StartCoroutine(LerpRatingBar());
    }

    private IEnumerator LerpRatingBar()
    {
        isLerping = true;
        float elapsedTime = 0f;
        float startWidth = scoreImg.GetComponent<RectTransform>().sizeDelta.x;

        while (elapsedTime < ratingChangeSpeed)
        {
            float width = Mathf.Lerp(startWidth, startWidth + targetWidthChange, elapsedTime / ratingChangeSpeed);
            scoreImg.GetComponent<RectTransform>().sizeDelta = new Vector2(width, scoreImg.GetComponent<RectTransform>().sizeDelta.y);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        scoreImg.GetComponent<RectTransform>().sizeDelta = new Vector2(startWidth + targetWidthChange, scoreImg.GetComponent<RectTransform>().sizeDelta.y);

        isLerping = false;
    }



    public void ResetRating()
    {
        totalRating = 0;
        numberOfRatings = 0;
    }

    IEnumerator DelayedGameOver(float delay) 
    {
        yield return new WaitForSeconds(delay);
        GameOver();
    }

    void GameOver()
    {
        SwitchState(GameState.DEFEAT); 
        SceneManager.LoadScene("GameOver");
        asm.PlayOneShot(LossSound, false);  

        if (SceneManager.GetActiveScene().name == "GameOver")
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("MainMenu");
                ResetRating(); 
            }
    }

    public GameState GetGameState()
    {
        return currentState;
    }

    public void SwitchState(GameState newState)
    {
        Debug.Log("New state has been set to " + newState); 
        currentState = newState;
        OnGameStateChanged?.Invoke(); 
    }
}

