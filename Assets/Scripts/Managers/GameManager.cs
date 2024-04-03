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

    public void ChangeRatingBar(float changeAmount)
    {
        if (scoreImg == null) return;
        float starWidth = 100f;
        float totalWidth = starWidth * 5; 

        // Calculate the change in width for the rating bar
        float widthChange = totalWidth * (changeAmount / 5f); 
        RectTransform starRectTransform = scoreImg.GetComponent<RectTransform>();
        starRectTransform.sizeDelta += new Vector2(widthChange, 0f);
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

