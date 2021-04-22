using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelections : MonoBehaviour
{
    public InputField inputName;
    public CharacterSelection CharacterSelection;
    public InputField inputAddress;

    public string displayName;
    public int playerCharacter;
    public string ipAddress;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void collectJoinInfo()
    {
        displayName = inputName.text;
        playerCharacter = CharacterSelection.selectedCharacter;
        ipAddress = inputAddress.text;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
