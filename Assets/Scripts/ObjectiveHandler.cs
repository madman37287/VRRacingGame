using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveHandler : MonoBehaviour
{
    public GameObject checkpointPrefab;
    public GameObject FinishPrefab;
    public GameObject postgamePrefab;

    startCar car;
    TimerDisplay timer;
    obstacleHandler obstacle;
    velController maxVel;

    Vector3[] checkPos = new Vector3[9];

    // Bounds for checkpoint positions 
    private int maxX = 440;
    private int minX = -535;
    private int maxZ = 530;
    private int minZ = -445;

    // lap num is initialized to -1 to help with increment logic in the GenerateCheckpoint function
    int lapNum   = -1;
    int curCheck = 0;

    float totalTime = 0f;
    float fastestLap = 9999f;
    float currentLap = 0f;

    GameObject checkpoint;
    CheckpointHandler checkpointHandle;
    // Start is called before the first frame update
    void Start()
    {
        // set gameobject components
        car      = GameObject.Find("Cartoon_SportCar_B01").GetComponent<startCar>();
        timer    = GameObject.Find("Cartoon_SportCar_B01").transform.Find("carrosserie").Find("TimerDisplay").GetComponent<TimerDisplay>();
        obstacle = GameObject.Find("Obstacles").GetComponent<obstacleHandler>();
        maxVel   = GameObject.Find("Cartoon_SportCar_B01").transform.Find("Tires").GetComponent<velController>();

        // set overall ranges for quadrants
        int rangeX = maxX - minX;
        int rangeZ = maxZ - minZ;

        // set the amount to increment by for quadrants
        int incX = rangeX/3;
        int incZ = rangeZ/3;

        // initialize outside of loop for optimization
        int xValue;
        int zValue;

        for (int i = 0; i <= 2; i++)
        {
            for (int j = 0; j <= 2; j++)
            {
                // create a random x,z coordinate within each of the 9 quadrants.  This will be where our checkpoints spawn. 
                xValue = UnityEngine.Random.Range(minX + incX*i, minX + incX*(i+1));
                zValue = UnityEngine.Random.Range(minZ + incZ*j, minZ + incZ*(j+1));

                // Set checkpoint position incrementally
                checkPos[i*3 + j] = new Vector3(xValue, 0, zValue);
            }
        }

        // reposition checkpoint positions within the array.  This orientation will create a circular path around the center, and end in the center. (think of a tic tac toe board as the quadrants)
        checkPos = new Vector3[]{checkPos[1], checkPos[2], checkPos[5], checkPos[8], checkPos[7], checkPos[6], checkPos[3], checkPos[0], checkPos[4]};
        GenerateCheckpoints();
    }

    // Update is called once per frame
    void Update()
    {
        // if checkpoint object exists, check if the car has made contact
        if (checkpointHandle != null)
        {
            if (checkpointHandle.getIsTouched())
            {
                // after car makes contact, delete the checkpoint
                checkpointHandle = null;
                Destroy(checkpoint);

                // Generate next checkpoint
                GenerateCheckpoints();
            }
        }
        else
        {
            // In theory, this should never print
            print("Something went wrong");
        }

        // if game has started and no time remains
        if (!timer.getIsTimeRem() && car.getStart())
        {
            // tell car to stop game, and it will pass the information along to all its children
            car.StopGame();
            // instantiate the postgame menu
            GameObject postgame = Instantiate(postgamePrefab, new Vector3(0, 500, 10), Quaternion.identity);
            // hand off number of laps completed to postgame object before resetting the lap number
            postgame.GetComponent<Postgame>().setLaps(lapNum);
            // reset lap number
            lapNum = -1;
            // reset checkpoint number
            curCheck = 0;
            //Destroy checkpoint
            checkpointHandle = null;
            Destroy(checkpoint);

            // Generate the first checkpoint to be visible from postgame menu
            GenerateCheckpoints();
        }
        else if (car.getStart())
        {
            // gather the total time game has been running for the statistics menu
            totalTime = totalTime + Time.deltaTime;
            // gather lap time
            currentLap = currentLap + Time.deltaTime;
        }
        else
        {
            // if game is stopped, reset the timer and current lap timer
            totalTime  = 0;
            currentLap = 0;
        }

    }

    public Vector3 CurObjLoc()
    {
        if (checkpoint != null)
        {
            // return current checkpoint location
            return checkpoint.transform.position;
        }
        else
        {
            //because 1000 is an impossible x/z value for our checkpoints, we will use it for error handling
            return new Vector3(1000, 1000, 1000);
        }
    }

    void GenerateCheckpoints()
    {
        if (curCheck < 8)
        {
            if (curCheck == 0)
            {
                if(lapNum >= 0)
                {
                    //lap complete logic to be added here
                    //------------------------------------
                    // set fastest lap time if current lap faster than previous
                    if (currentLap < fastestLap) {fastestLap = currentLap;}
                    // add time per lap.  each lap receives 1 minute less time
                    if (lapNum <= 5) {timer.addTime(300 - 60*lapNum);}
                    // add 500 obstacles per lap (will not exceed 3000 total)
                    obstacle.addObj(500);
                    // play finishline sound
                    transform.Find("lapSound").GetComponent<AudioSource>().Play();
                    //------------------------------------
                }

                // reset current lap time to 0 seconds
                currentLap = 0;
                // increase lap number by 1
                lapNum     = lapNum+1;
                // increase max velocity by 5 per lap
                maxVel.setMaxVel(15 + 5*lapNum);
            }
            else
            {
                // play checkpoint sound
                transform.Find("checkSound").GetComponent<AudioSource>().Play();
            }
            // instantiate new checkpoint
            checkpoint = Instantiate(checkpointPrefab, checkPos[curCheck], Quaternion.identity);
            // increment checkpoint counter
            curCheck = curCheck+1;
        }
        else
        {
            // instantiate finishline
            checkpoint = Instantiate(FinishPrefab, checkPos[curCheck], Quaternion.identity);
            //reset checkpoint counter
            curCheck = 0;
        }
        // get checkpoint/finishline script
        checkpointHandle = checkpoint.GetComponent<CheckpointHandler>();
    }

    public int getLaps()
    {
        // return current lap
        return lapNum;
    }

    public float getTotalTime()
    {
        // return total time game has been played
        return totalTime;
    }

    public float getFastestLap()
    {
        // return fastest lap time
        return fastestLap;
    }
}
