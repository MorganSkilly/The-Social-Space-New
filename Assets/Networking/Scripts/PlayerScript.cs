using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace QuickStart
{
    public class PlayerScript : NetworkBehaviour
    {
        private SceneScript sceneScript;

        void Awake()
        {
            //allow all players to run this
            sceneScript = GameObject.FindObjectOfType<SceneScript>();
        }
        [Command]
        public void CmdSendPlayerMessage()
        {
            if (sceneScript)
            {
                sceneScript.statusText = sceneScript.canvasInputText.text;
                sceneScript.cinemaController.YoutubeTest(sceneScript.statusText);
            }
        }
        public override void OnStartLocalPlayer()
        {
            sceneScript.playerScript = this;
            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = new Vector3(0, 0, 0);
        }

        void Update()
        {
            if (!isLocalPlayer) { return; }

            float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * 110.0f;
            float moveZ = Input.GetAxis("Vertical") * Time.deltaTime * 4f;

            transform.Rotate(0, moveX, 0);
            transform.Translate(0, 0, moveZ);
        }
    }
}