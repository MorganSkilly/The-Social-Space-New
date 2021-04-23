using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using VideoLibrary;

namespace QuickStart
{
    public class SceneScript : NetworkBehaviour
    {
        public Text canvasStatusText;
        public Text stopStatusText;
        public InputField canvasInputText;
        public FPSController playerScript;
        public VideoController cinemaController;

        private bool isStopped;

        [SyncVar(hook = nameof(InsertNewVideo))]
        public string newVideoUrl;

        // [SyncVar]

        private void Start()
        {
            isStopped = false;
        }


        void InsertNewVideo(string _Old, string _New)
        {
            //called from sync var hook, to update info on screen for all players
            //canvasStatusText.text = videoText;
            _Old = cinemaController.cinemaScreen.url;
            _New = newVideoUrl;
            cinemaController.PlayNewVideo(newVideoUrl);
        }

        public void PauseVideo()
        {
            if (!isStopped)
            {
                cinemaController.Pause();
                isStopped = true;
            }
            else
            {
                cinemaController.Play();
                isStopped = false;
            }
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