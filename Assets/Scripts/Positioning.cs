using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Positioning : MonoBehaviour
{
    public List<RacingLogic> cars;
    public RacingLogic playerRL;
    public Text lapText;
    public Text posText;
    float delay = 0.2f;
    float current = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        current += Time.deltaTime;

        if(current >= delay)
        {
            current = 0.0f;
        }
        else
        {
            return;
        }

        cars.Sort((r1, r2) =>
        {
            if (r2.laps != r1.laps)
                return r2.laps.CompareTo(r1.laps);

            int r1Index = r1.GetNextCorrectIndexCheckpoint();
            int r2Index = r2.GetNextCorrectIndexCheckpoint();
            if (r1Index != r2Index)
                return r2Index.CompareTo(r1Index);

          /*  Vector3 ahead = r1.GetTrans().position - r2.GetTrans().position;
            float dot = Vector3.Dot(ahead, r1.currentCheckPoint.forward);

            if (dot > 0)
            {
                // r1 is ahead
                return -1;
            }

            if (dot < 0)
            {
                // r2 is ahead
                return 1;
            }

            
             return 0;*/
            

           return r1.GetDistanceToCheckpoint().CompareTo(r2.GetDistanceToCheckpoint());
        });

        if(playerRL.laps <= 3)
        {
            lapText.text = playerRL.laps + "/" + 3;
        }
        

        posText.text = (cars.IndexOf(playerRL) + 1) + "/" + 6;
        playerRL.position = cars.IndexOf(playerRL) + 1;
     

    }  
}
