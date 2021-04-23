using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

namespace QuickStart
{
    public class SceneScript : NetworkBehaviour
    {
        public Text canvasStatusText;
        public InputField canvasInputText;
        public FPSController playerScript;
        public VideoController cinemaController;

        [SyncVar(hook = nameof(OnStatusTextChanged))]
        public string statusText;

        void OnStatusTextChanged(string _Old, string _New)
        {
            //called from sync var hook, to update info on screen for all players
            canvasStatusText.text = statusText;
            cinemaController.Play(statusText);
        }

        public void ButtonSendMessage()
        {
            if (playerScript != null)
            {
                playerScript.CmdSendPlayerMessage();
            }
        }
    }
}