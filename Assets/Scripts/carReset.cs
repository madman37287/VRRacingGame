using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carReset : MonoBehaviour
{
    Vector3 gameResetPos;
    Vector3 resetPos;
    Rigidbody rb;
    ObjectiveHandler objHandle;

    float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        // Set gameResetPos to initial position
        gameResetPos = transform.position;
        // set resetPos to initial position TODO: update resetPos to last checkpoint location
        resetPos = transform.position;
        // set the objective handler to a variable
        objHandle = GameObject.Find("Objectives").GetComponent<ObjectiveHandler>();

        // grab rigidbody
        rb = transform.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // check if the "X" button is being held
        if (QuestInputs.Left.Button1.Current) //if (Input.GetKey("space"))
        {
            if (timer >= 0)
            {
                // increase the timer
                timer = timer + Time.deltaTime;
                if (timer >= 3)
                {
                    // if player holds "X" for 3 seconds, reset the car position
                    reset();
                }
            }
        }
        else
        {
            timer = 0;
        }
    }

    public void reset()
    {
        // get current checkpoint location
        Vector3 direction  = objHandle.CurObjLoc();
        // set player to initial location
        transform.position = resetPos;
        // set direction height to same height as player
        direction.y        = resetPos.y;
        // reset velocity
        rb.velocity        = Vector3.zero;
        // reset timer
        timer              = -1;

        // have player facing the checkpoint on reset        
        transform.LookAt(direction, Vector3.up);
    }

    public void resetGame()
    {
        // get current checkpoint location
        Vector3 direction  = objHandle.CurObjLoc();
        // set player to initial location
        transform.position = gameResetPos;
        // set direction height to same height as player
        direction.y        = gameResetPos.y;
        // reset velocity
        rb.velocity        = Vector3.zero;
        // reset timer
        timer              = -1;

        // have player facing the checkpoint on reset        
        transform.LookAt(direction, Vector3.up);
    }
}
