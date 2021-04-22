using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WelcomeUI : MonoBehaviour
{
    public GameObject Screen1;
    public GameObject Screen2;
    public GameObject Screen3;
    public GameObject Transition;
    
    // Start is called before the first frame update
    void Start()
    {
        Screen1.SetActive(true);
        Screen2.SetActive(false);
        Screen3.SetActive(false);
        Transition.SetActive(false);
    }

    public void DisplayNamePicked()
    {
        StartCoroutine(startTransition1());
    }

    public void characterPicked()
    {
        StartCoroutine(startTransition2());
    }

    IEnumerator startTransition1()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();

        Transition.SetActive(true);
        yield return new WaitForSeconds(2f);
        Screen1.SetActive(false);
        Screen2.SetActive(true);
    }

    IEnumerator startTransition2()
    {
        Transition.SetActive(false);

        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();

        Transition.SetActive(true);
        yield return new WaitForSeconds(2f);
        Screen2.SetActive(false);
        Screen3.SetActive(true);

    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
