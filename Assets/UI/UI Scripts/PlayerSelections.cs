using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Mirror;

public class PlayerSelections : MonoBehaviour
{
    ///NetworkManager manager;
    [SerializeField]
    InputField inputName;

    [SerializeField]
    CharacterSelection CharacterSelection;

    [SerializeField]
    InputField inputAddress;

    private string displayName;
    private int playerCharacter;
    private string ipAddress;

    [SerializeField]
    NetworkHUD manager;

    private void Awake()
    {
        manager = GetComponent<NetworkHUD>();
    }

    public void collectJoinInfo()
    {
        ipAddress = inputAddress.text;
        displayName = inputName.text;
        playerCharacter = CharacterSelection.selectedCharacter;

        manager.StartButtons(ipAddress);

        gameObject.SetActive(false);
    }

}
