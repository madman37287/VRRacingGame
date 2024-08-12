using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class velController : MonoBehaviour
{
    Wheel_Rotation lWheel;
    Wheel_Rotation rWheel;
    Rigidbody rb;
    int fastestSpeed;

    // flag to avoid race condition bug
    bool started;

    // Start is called before the first frame update
    void Start()
    {
        // assign left wheel object
        lWheel = transform.Find("Tire_Front_L").GetComponent<Wheel_Rotation>();
        // assign right wheel object
        rWheel = transform.Find("Tire_Front_R").GetComponent<Wheel_Rotation>();
        // assign the parent's rigidbody
        rb     = transform.parent.GetComponent<Rigidbody>();

        // flag to avoid race condition bug
        started = true;
    }

    void Update()
    {
        // if the car is moving faster that the recorded max, replace fastest speed with new max.
        if ((int)rb.velocity.magnitude > fastestSpeed)
        {
            fastestSpeed = (int)rb.velocity.magnitude;
        }
    }

    public void setMaxVel(int vel)
    {
        // This avoids race condition of objective handler
        if (started)
        {
            // set max value within left and right wheels to the input velocity given to this function.
            lWheel.setMaxVel(vel);
            rWheel.setMaxVel(vel);
        }
    }

    public int getFastestSpeed()
    {
        // return the fastest speed the car has traveled
        return fastestSpeed;
    }

    public void resetFastestSpeed()
    {
        // reset fastest speed (to be called before each run)
        fastestSpeed = 0;
    }
}
