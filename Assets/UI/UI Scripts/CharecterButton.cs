using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharecterButton : MonoBehaviour
{
    public CharacterSelection selection;
    private int buttonNum;
    // Start is called before the first frame update
    void Start()
    {
        buttonNum = transform.GetSiblingIndex();
    }

    public void characterSelected()
    {
        selection.charecterSelected(buttonNum);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
