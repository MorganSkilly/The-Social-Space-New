// vis2k: GUILayout instead of spacey += ...; removed Update hotkeys to avoid
// confusion if someone accidentally presses one.
using UnityEngine;
using Mirror;


    /// <summary>
    /// An extension for the NetworkManager that displays a default HUD for controlling the network state of the game.
    /// <para>This component also shows useful internal state for the networking system in the inspector window of the editor. It allows users to view connections, networked objects, message handlers, and packet statistics. This information can be helpful when debugging networked games.</para>
    /// </summary>
/*    [DisallowMultipleComponent]
    [AddComponentMenu("Network/NetworkHUD")]
    [RequireComponent(typeof(NetworkManager))]
    [HelpURL("https://mirror-networking.com/docs/Articles/Components/NetworkManagerHUD.html")]*/
public class NetworkHUD : MonoBehaviour
{
    NetworkManager manager;

    PlayerSelections input;

    //private string networkAddress;

    void Awake()
    {
        manager = GetComponent<NetworkManager>();
        input = GetComponent<PlayerSelections>();
    }

    private void Start()
    {
        input.ipAddress = manager.networkAddress;

        //input.collectJoinInfo(manager.networkAddress);

        Debug.Log("IP address " + input.ipAddress);

        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            StartButtons();
        }
        else
        {
            //StatusLabels();
            Debug.Log("Labels");
        }

        // client ready
        if (NetworkClient.isConnected && !ClientScene.ready)
        {
            ClientScene.Ready(NetworkClient.connection);

            if (ClientScene.localPlayer == null)
            {
                ClientScene.AddPlayer(NetworkClient.connection);
            }

        }

        StopButtons();
    }
    
 
    public void StartButtons()
    {
        if (!NetworkClient.active)
        {
            // Server + Client
           /* if (Application.platform != RuntimePlatform.WebGLPlayer)
            {
                manager.StartHost();                   
            }
*/
            // Client + IP

            manager.StartClient();
                
            //manager.networkAddress = GUILayout.TextField(manager.networkAddress);

            /*// Server Only
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                // cant be a server in webgl build
                GUILayout.Box("(  WebGL cannot be server  )");
            }
            else
            {
                if (GUILayout.Button("Server Only")) manager.StartServer();
            }*/
        }
        else
        {
            // Connecting

            manager.StopClient();
                
        }
    }

    void StatusLabels()
    {
        // server / client status message
        if (NetworkServer.active)
        {
            GUILayout.Label("Server: active. Transport: " + Transport.activeTransport);
        }
        if (NetworkClient.isConnected)
        {
            GUILayout.Label("Client: address=" + input.ipAddress);
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
    }
}
