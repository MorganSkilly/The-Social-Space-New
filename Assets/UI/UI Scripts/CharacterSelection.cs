using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public int selectedCharacter;
    public Sprite unselectedImage;
    public Sprite selectedImage;
    public Color selectedColor;
    public Color unselectedColor;
    public List<Button> buttons;
    
    // Start is called before the first frame update
    void Start()
    {

        buttons[selectedCharacter].image.sprite = selectedImage;
        buttons[selectedCharacter].image.color = selectedColor;
    }

    public void charecterSelected(int buttonNum)
    {
        buttons[buttonNum].image.sprite = selectedImage;
        buttons[buttonNum].image.color = selectedColor;
        buttons[selectedCharacter].image.sprite = unselectedImage;
        buttons[selectedCharacter].image.color = unselectedColor;

        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();

        selectedCharacter = buttonNum;

        // change 3d model being shown
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
