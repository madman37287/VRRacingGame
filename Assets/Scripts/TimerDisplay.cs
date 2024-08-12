using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerDisplay : MonoBehaviour
{
    startCar car;
    Text text;

    float timeRemSec  = 0f;
    bool isTimeRem = true;
    ParticleSystem particle;

    // Start is called before the first frame update
    void Start()
    {
        // set objects and components
        car      = transform.parent.parent.GetComponent<startCar>();
        text     = transform.Find("Text").GetComponent<Text>();
        particle = transform.Find("EngineSparks").GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (car.getStart())
        {
            if (timeRemSec > 0)
            {
                // display time remaining in seconds
                text.text = "Time Remaining:\n" + stringTime(timeRemSec);
                // remove time from last frame to get current time remaining
                timeRemSec = timeRemSec - Time.deltaTime;
                // if we hit this block, then time still remains, and flag should remain or be set to true
                isTimeRem = true;
                //if time remaining is less than 10 seconds, then activate the engine sparks particle system
                if (timeRemSec < 10 && !particle.isEmitting)
                {
                    particle.Play();
                }
                // if time has been added, then stop the particles.
                else if (timeRemSec >= 10 && particle.isPlaying)
                {
                    particle.Stop();
                }
            }
            else
            {
                // if no time remains, then display that and set flag to false
                text.text = "Time Remaining:\n0:00.00";
                timeRemSec = 0f;
                isTimeRem = false;
            }
        }
    }

    public string stringTime(float totSec)
    {
        // using the fact that converting to int always rounds down, we can easily calculate exact minutes, seconds, and mseconds
        int min  = (int)(totSec/60);
        int sec  = (int)(totSec - (min * 60));
        int msec = (int)((totSec - ((int)totSec)) * 100);

        // convert to string and put in a clock format.
        return min.ToString("#0") + ":" + sec.ToString("00") + "." + msec.ToString("00");
    }

    public void addTime(int sec)
    {
        // add time to the clock
        timeRemSec = timeRemSec + sec;
    }

    public bool getIsTimeRem()
    {
        // return if time has expired yet
        return isTimeRem;
    }
}
