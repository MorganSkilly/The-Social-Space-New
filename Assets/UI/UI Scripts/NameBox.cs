using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameBox : MonoBehaviour
{
    public Button button;
    public InputField inputField;
    // Start is called before the first frame update
    void Start()
    {
        button.interactable = false;
    }

    public void inputChange()
    {
        if(inputField.text != null)
        {
            button.interactable = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
