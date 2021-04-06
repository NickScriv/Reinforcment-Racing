using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void QuitButton()
    {
        Application.Quit();
        
    }

    public void PlayButton()
    {
        SceneManager.LoadScene("Main 2");
    }
}
