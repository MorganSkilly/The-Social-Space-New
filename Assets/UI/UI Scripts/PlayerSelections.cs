using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerSelections : MonoBehaviour
{
    NetworkManager manager;

    public InputField inputName;
    public CharacterSelection CharacterSelection;
    public InputField inputAddress;

    public string displayName;
    public int playerCharacter;
    public string ipAddress;

    private void Awake()
    {
        manager = GetComponent<NetworkManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //ipAddress = ;
        Debug.Log("IP address is " + manager.networkAddress + "...");
    }

    public void collectJoinInfo()
    {
        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            if (!NetworkClient.active)
            {
                if (Application.platform != RuntimePlatform.WebGLPlayer)
                {
                    Debug.Log("Joining...");
                    manager.StartClient();
                }
            }
            else
            {
                Debug.Log("Connecting to " + manager.networkAddress + "...");
            }
                
        }
        inputAddress.text = manager.networkAddress;
        displayName = inputName.text;
        playerCharacter = CharacterSelection.selectedCharacter;



    }
    
    // Update is called once per frame
    void Update()
    {

    }

}
