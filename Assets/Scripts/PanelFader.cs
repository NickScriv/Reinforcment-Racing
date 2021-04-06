using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PanelFader : MonoBehaviour
{
    public float textDuration = 0.4f;
    public AudioClip buttonSound;
    AudioSource source;
    CanvasGroup cg;
    Image blur;
    bool start = false;


    private void OnEnable()
    {
        if(!start)
        {
            cg = GetComponent<CanvasGroup>();
            blur = GetComponent<Image>();
            source = GetComponent<AudioSource>();
            start = true;
        }
        
        StartCoroutine(FadePanel(cg.alpha, 1, 0, 1));
    
    }


    public void FadeOut()
    {
        StartCoroutine(FadePanel(cg.alpha, 0, 1, 0));
        source.PlayOneShot(buttonSound);

    }

 

    IEnumerator FadePanel(float start, float end, float startBlur, float endBlur)
    {
        GameManager.Instance.isPausing = true;
        float count = 0.0f;
        //Debug.Log(count / textDuration);
        while (count < textDuration)
        {
            
            count += Time.unscaledDeltaTime;
            cg.alpha =  Mathf.Lerp(start, end, count / textDuration);
            blur.material.SetFloat("_BlurAmount", Scale(0.0f, 0.006f, 0.0f, 1.0f, Mathf.Lerp(startBlur, endBlur, count / textDuration)));
            yield return null;
        }
        GameManager.Instance.isPausing = false;
        if (end == 0)
            gameObject.SetActive(false);
    }

    private float Scale(float from, float to, float from2, float to2, float val)
    {
        if (val <= from2)
            return from;
        else if (val >= to2)
            return to;
        return (to - from) * ((val - from2) / (to2 - from2)) + from;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Xbox_B") && gameObject.name == "GameOverPanel")
        {
            BackToMenu();
        }
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        source.PlayOneShot(buttonSound);
        SceneManager.LoadScene("MainMenu");
    }

}
