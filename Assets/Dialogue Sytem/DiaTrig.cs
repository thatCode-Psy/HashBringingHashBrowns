﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DiaTrig : MonoBehaviour
{

    [SerializeField]
    UnityEvent diaTrigger; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            diaTrigger.Invoke();
        }
    }


}
