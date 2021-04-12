using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    public AICarAgent[] AIAgents;
    public RCC_CarControllerV3[] AICars;
    public RCC_CarControllerV3 player;
    public Text countdownText;
    float currentTime = 6.0f;
    int lastCurrentTime = 6;
    public AudioClip countSingle;
    public AudioClip countFinal;
    AudioSource countAudio;
    public GameObject startPanel;
    Image panelBack;
    public float duration;

    private void Awake()
    {
        panelBack = startPanel.GetComponent<Image>();
    }

    private void Start()
    {
        StartCoroutine(FadePanel(panelBack.color.a, 0.0f));
    }

    IEnumerator FadePanel(float start, float end)
    {
        float count = 0.0f;
        //Debug.Log(count / textDuration);
        while (count < duration)
        {

            count += Time.deltaTime;
            Color tempColor = panelBack.color;
            tempColor.a = Mathf.Lerp(start, end, count / duration);
            panelBack.color = tempColor;
            yield return null;
        }
        GameManager.Instance.start = false;
        startPanel.gameObject.SetActive(false);
        StartCountdown();
    }

    void StartCountdown()
    {
        countAudio = GetComponent<AudioSource>();
        StartCoroutine(CountDownTimer());
    }

    IEnumerator CountDownTimer()
    {
        int time = Mathf.FloorToInt(currentTime);
        countAudio.clip = countSingle;
        while (time > 0)
        {
           
            currentTime -= Time.deltaTime;
            time = Mathf.FloorToInt(currentTime);
            countdownText.text = time.ToString("0");
            if (time == 0)
            {
                countdownText.color = Color.red;
            }


            if (time != lastCurrentTime && time != 5)
            {
                lastCurrentTime = time;
                countAudio.Play();

            }

           
            yield return null;
        }
        countdownText.text = "0";
        
        countAudio.clip = countFinal;
        countAudio.Play();
        player.enabled = true;

        foreach (RCC_CarControllerV3 car in AICars)
        {

            car.enabled = true;
        }

        foreach (AICarAgent agent in AIAgents)
        {

            agent.enabled = true;
        }
        Invoke("Disable", 2.5f);


    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
