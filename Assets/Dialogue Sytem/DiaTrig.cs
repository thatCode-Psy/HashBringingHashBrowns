using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DiaTrig : MonoBehaviour
{

    [SerializeField]
    UnityEvent diaTrigger;

    public Dialogue tempDialogue; 

    public GameEvent choiceEvent;
    public GameEvent diaEvent; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            tempDialogue.DialogueInit(0); 
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            choiceEvent.Raise(); 
        }
    }


}
