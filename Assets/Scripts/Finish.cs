using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Finish : MonoBehaviour
{
    public GameObject EndPanel;
    public Text placing;
    bool first = true;

    private void OnTriggerEnter(Collider other)
    {

       
        Transform t = other.transform.parent.parent;
        if (t.CompareTag("AICar"))
        {
            
            if(t.GetComponentInParent<RacingLogic>().laps == 4)
            {
                
                t.GetComponent<AICarAgent>().enabled = false;
                StartCoroutine(DisableCar(t.GetComponent<AICarAgent>().gameObject));
            }
        }

        if (t.CompareTag("Player"))
        {
            
            
            if (t.GetComponent<RacingLogic>().laps == 4)
            {
                //GameManager.Instance.PauseGame();
                GameManager.Instance.end = true;
                GameManager.Instance.PauseGame();
                GameManager.Instance.PauseAllAudio();

                EndPanel.SetActive(true);

               
                switch(t.GetComponent<RacingLogic>().position)
                {
                    case 1:
                        placing.text = "1st Place";
                        
                        break;
                    case 2:
                        placing.text = "2nd Place";
                     
                        break;
                    case 3:
                        placing.text = "3rd Place";
                      
                        break;
                    case 4:
                        placing.text = "4th Place";
                  
                        break;
                    case 5:
                        placing.text = "5th Place";
                   
                        break;
                    case 6:
                        placing.text = "Last Place";
                       
                        break;

                }
                
         



            }
        }

    }

    IEnumerator DisableCar(GameObject go)
    {
        yield return new WaitForSeconds(2f);
        go.SetActive(false);
    }
}
