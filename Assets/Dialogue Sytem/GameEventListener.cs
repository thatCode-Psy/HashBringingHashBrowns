using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{

    [SerializeField]
    GameEvent Event;

    [SerializeField]
    UnityEvent Response;

    UnityAction myAction;

    public void Start()
    {
    }
    public void OnEnable()
    {
        Event.RegisterListener(this);
    }

    public void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(int arg = -1)
    {
        Response.Invoke(); 

    }
}
