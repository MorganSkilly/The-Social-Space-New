using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WelcomeUI : MonoBehaviour
{
    public GameObject Screen1;
    public GameObject Screen2;
    public GameObject Transition;
    
    // Start is called before the first frame update
    void Start()
    {
        Screen2.SetActive(false);
        Transition.SetActive(false);
    }

    public void DisplayNamePicked()
    {
        StartCoroutine(startTransition());
    }

    IEnumerator startTransition()
    {
        Transition.SetActive(true);
        yield return new WaitForSeconds(2f);
        Screen1.SetActive(false);
        Screen2.SetActive(true);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
