using RenderHeads.Media.AVProVideo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mediaPlayerInteraction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<MediaPlayer>().AudioSource.transform.position = gameObject.transform.position;
        gameObject.GetComponent<MediaPlayer>().AudioSource.spatialBlend = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
