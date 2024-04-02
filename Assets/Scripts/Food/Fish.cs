using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Fish : Cookables
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

    protected override void Start()
    {
        base.Start();
        isSeated = false;
        CurrentState = FishStates.Chilling;
    }

    public void ChangeState(FishStates newState)
    {
        CurrentState = newState;
    }

    private void Chilling()
    {

    }

    private void Hungry()
    {

    }

    private void Served(Cookables nomNoms)
    {

    }

    private void LeavingHangry()
    {

    }
    private void PickedUp()
    {

    }
} 
 