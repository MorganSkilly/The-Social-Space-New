using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using VideoLibrary;
using UnityEngine.PlayerLoop;

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

     /*   [SyncVar]
        private double frames;*/

        private void Start()
        {
            isStopped = false;
        }

       /* private void Update()
        {
            SynchVideo();
        }*/

        /*    public override void OnStartLocalPlayer()
            {
                cinemaController.cinemaScreen.frame;
            }*/

        /*public override void OnStartAuthority()
        {
            frames = cinemaController.cinemaScreen.time;
        }*/

        void InsertNewVideo(string _Old, string _New)
         {
             //called from sync var hook, to update info on screen for all players 
             //canvasStatusText.text = videoText;
             if (!playerScript.isClientOnly)
             {
                 _Old = cinemaController.cinemaScreen.url;
                 _New = newVideoUrl;
                 cinemaController.PlayNewVideo(newVideoUrl);
             }
             else
             {
                 cinemaController.PlayNewVideo(_New);
             }
         } 


        public void InsertNewVideoUrl()
        {
            if (playerScript != null && playerScript.hasAuthority)
            {
                playerScript.PlayNewVideoUrl();
            }
        }

        [ClientRpc]
        public void PauseVideo()
        {
            if (playerScript != null && playerScript.hasAuthority)
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
            else
                return;           
        }

       /* public void SynchVideo()
        {
            if (playerScript.isClientOnly)
            {
                long clientVideo = cinemaController.cinemaScreen.frame;
                frames = clientVideo;
            }
        }*/

        [ClientRpc]
        public void StopVideo()
        {
            if (playerScript != null && playerScript.hasAuthority)
            {
                cinemaController.Stop();
                cinemaController.Play();
            }
            else
                return;
        }

    }
}