using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAI : Fish
{
    public event Action<FishStates> OnStateChanged;
    private FishStates currentState; 

    // Enters the restaurant chilling
    // From chilling, a coroutine starts that makes it randomly get hungry from 15s of entering to a minute 

    // From Hungry, timer begins. They must be fed within one minute OR they will leave Hangry 
    // From Hungry, if fed, they will be served, give a review in speech bubble, then disappear 
    public FishStates CurrentState
    {
        get { return currentState; }
        set
        {
            if (currentState != value)
            {
                currentState = value;

                OnStateChanged?.Invoke(currentState);

                switch (currentState)
                {
                    case FishStates.Chilling:
                        break;
                    case FishStates.Hungry:
                        break;
                    case FishStates.Cooking:
                        break;
                    case FishStates.Served:
                        break;
                    case FishStates.LeavingHangry:
                        break;
                    default:
                        Debug.LogError("Unhandled fish state!");
                        break;
                }
            }
        }
    }

    public void ChangeState(FishStates newState)
    {
        CurrentState = newState;
    }

    private void Start()
    {
        CurrentState = FishStates.Chilling; 
    }
}
