﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectButton : MonoBehaviour
{
    public Button button;
    public InputField input;
    
    // Start is called before the first frame update
    void Start()
    {
        button.interactable = false;
    }

    public void inputAddress()
    {
        if(input.text.Length > 7)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }
}
