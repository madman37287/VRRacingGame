using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class CarController : MonoBehaviour
{
    // velocity set to 6 meters/second.  set to negative since car's local x-Axis faces opposite the front of the car.
    float velocity = -6f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if car position exceeds our range of 100/-100 meters:
        //     Rotate car 180 degrees about the y-axis
        //     teleport back to 100/-100 on the x axis (so as to not trigger conditional again)
        //     set position to the other side of the road
        if (transform.position.x > 100)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
            transform.position = new Vector3(100, transform.position.y, 4);
        }
        else if (transform.position.x < -100)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, -180, transform.eulerAngles.z);
            transform.position = new Vector3(-100, transform.position.y, 0);
        }

        // if space bar is not being held, move
        if (!Input.GetKey("space"))
        { 
            // velocity is -6 meters per second, Time.deltaTime gathers time (in seconds) since last frame update
            // because distance = velocity * time, we can multiply our velocity by time since last frame to get our positional difference used in the Translate function.
            transform.Translate(velocity * Time.deltaTime, 0, 0);
        }
    }

    // function to return velocity to external scripts
    // currently, this is needed in WheelController
    // although velocity could also be set to public, it leaves the variable open to alteration from outside sources
    public float getVelocity()
    {
        return velocity;
    }
}
