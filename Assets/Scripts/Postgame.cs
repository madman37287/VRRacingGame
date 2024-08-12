using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Postgame : MonoBehaviour
{
    int laps;
    // Start is called before the first frame update
    void Start()
    {
        // set game components and objects to variables
        Text text                   = transform.Find("Stats").GetComponent<Text>();
        ParticleSystem particle1    = transform.Find("SparksAsFireworksRight").GetComponent<ParticleSystem>();
        ParticleSystem particle2    = transform.Find("SparksAsFireworksLeft").GetComponent<ParticleSystem>();
        ObjectiveHandler objectives = GameObject.Find("Objectives").GetComponent<ObjectiveHandler>();
        TimerDisplay timeConv       = GameObject.Find("Cartoon_SportCar_B01").transform.Find("carrosserie").Find("TimerDisplay").GetComponent<TimerDisplay>();

        // begin gathering statistics to display
        int speed                   = GameObject.Find("Cartoon_SportCar_B01").transform.Find("Tires").GetComponent<velController>().getFastestSpeed();
        int numObs                  = laps*500;
        float time                  = objectives.getTotalTime();
        float fastestLap            = objectives.getFastestLap();
        string timeStr              = timeConv.stringTime(time);
        string fastestLapStr        = timeConv.stringTime(fastestLap);

        // We do not need to ever stop this particle system as the prefab will kill itself.
        particle1.Play();
        particle2.Play();

        // if lap hasnt been complete, display N/A (1000 is an impossible time, as the player only has 300 seconds to complete the first lap)
        if (fastestLap >= 1000)
        {
            fastestLapStr = "N/A";
        }

        // display statistics on postgame menu
        text.text = "Laps Completed: " + laps.ToString("#0") +"\nFastest lap time: " + fastestLapStr +"\nMax speed: " + speed.ToString("#0") +"\nTotal race time: " + timeStr +"\nNumber of trees: " + numObs.ToString("#0");

    }
    public void setLaps(int raceLaps)
    {
        // set the number of laps to diplay
        laps = raceLaps;
    }
}
