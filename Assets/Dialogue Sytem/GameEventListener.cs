using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    GameEvent Event;

    [SerializeField]
    UnityEvent[] Response;

    public void OnEnable()
    {
        Event.RegisterListener(this);
    }

    public void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        for (int i = 0; i < Response.Length; i++)
            Response[i].Invoke(); 
    }
}
