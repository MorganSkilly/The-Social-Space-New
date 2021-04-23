using UnityEngine;
using Mirror;

public class NetworkHUD : MonoBehaviour
{
    NetworkManager manager;


    void Awake()
    {
        manager = GetComponent<NetworkManager>();
    }

    public void StartButtons(string ipaddress)
    {
        if (!NetworkClient.active)
        {
            manager.StartClient();
            manager.networkAddress = ipaddress; //GUILayout.TextField(manager.networkAddress);
        }
        else
        {
            // Connecting
            manager.StopClient(); 
        }
    }

   /* void StatusLabels()
    {
        // server / client status message
        if (NetworkServer.active)
        {
            GUILayout.Label("Server: active. Transport: " + Transport.activeTransport);
        }
        if (NetworkClient.isConnected)
        {
            GUILayout.Label("Client: address=" + manager.networkAddress);
        }
    }

    void StopButtons()
    {
        // stop host if host mode
        if (NetworkServer.active && NetworkClient.isConnected)
        {

            manager.StopHost();
                
        }
        // stop client if client-only
        else if (NetworkClient.isConnected)
        {
            manager.StopClient();
        }
        // stop server if server-only
        else if (NetworkServer.active)
        {

            manager.StopServer();
                
        }
    }*/
}
