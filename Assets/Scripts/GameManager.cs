using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool end = false;
    public bool start = true;
    public bool isPaused = false;
    public bool isPausing = false;
    public GameObject pausePanel;
    public GameObject resumeButton;
   

    

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        end = false;
        isPaused = false;
        start = true;

        




    }

    private void Update()
    {
        if (Input.GetButtonDown("Xbox_Start") && !end && !start)
        {
            if (!isPaused)
            {
                PauseGame();
                
            } 
            else 
            {
                ResumeGame();
                
            }
                
        }
    }

  

    public void PauseAllAudio()
    {
        AudioSource[] allAudioSources = FindObjectsOfType(typeof(AudioSource), true) as AudioSource[];
        foreach (AudioSource audioS in allAudioSources)
        {
            
            audioS.Pause();
        }
    }

    public void ResumeAllAudio()
    {
        AudioSource[] allAudioSources = FindObjectsOfType(typeof(AudioSource), true) as AudioSource[];
        foreach (AudioSource audioS in allAudioSources)
        {

            audioS.UnPause();
        }
    }

    public void PauseGame()
    {
        if (isPausing)
            return;
        // set pause menu active
        if(!end)
        {
            pausePanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(resumeButton);
        }
        

        PauseAllAudio();
        Time.timeScale = 0;
        isPaused = true;
    }

    public void ResumeGame()
    {
        if (isPausing)
            return;
        // set pause menu active
        Time.timeScale = 1f;
        EventSystem.current.SetSelectedGameObject(null);
        pausePanel.GetComponent<PanelFader>().FadeOut();
        ResumeAllAudio();
        isPaused = false;
    }

}
