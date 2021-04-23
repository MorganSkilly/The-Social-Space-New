using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using VideoLibrary;

public class VideoController : NetworkBehaviour
{
    [SerializeField]
    public InputField iField;

    [SerializeField]
    public VideoClip defaultVideo;

    [SerializeField]
    public VideoPlayer cinemaScreen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Play(string vidLink)
    {
        cinemaScreen.url = vidLink;
        cinemaScreen.Play();
    }
}
