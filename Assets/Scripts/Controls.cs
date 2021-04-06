using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Controls : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject startButton;
    public AudioSource sound;
 
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Xbox_B"))
        {
            gameObject.SetActive(false);
            mainMenu.SetActive(true);
            sound.PlayOneShot(sound.clip);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(startButton);
        }
    }
}
