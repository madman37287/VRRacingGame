using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    //public float rotation = 2.387f;
    CarController parent;

    // Start is called before the first frame update
    void Start()
    {
        // Reference to Parent needed to grab car velocity later on.
        parent = transform.parent.GetComponent<CarController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKey("space"))
        {
            // There are two methods that we can use.
            // method 1:
            // wheel Diameter is about 0.8 meters based on grid (using 2d viewer for support)
            // C = 2*pi*r gives us a circumference of about 2.5132
            // distance = C * number of rotations
            // velocity = distance/time
            // velocity = C * number of rotations/time
            // rotations/second = velocity/C (6/2.5132)
            // rotations/second = 2.387
            //transform.Rotate(0, 0, rotation * 360 * Time.deltaTime);

            // method 2:
            // angular velocity = change in angle vs change in time
            // velocity = radius * angular velocity
            // velocity = radius * radians per second
            // radians per second = velocity/radius (6/0.4)
            // radian * 180/pi = degrees
            // degrees per second = (6/0.4) * 180/pi = 859.462
            // this matches method 1 of 2.387 * 360 = 859.32 (slight change due to rounding)

            // to tie angular velocity of the wheel directly to the velocity of the car:
            float velocity = parent.getVelocity() * -1; // -1 needed because velocity is negative in parent
            float wVelocity = (velocity / 0.4f) * (180f / 3.1415f);
            transform.Rotate(0, 0, wVelocity * Time.deltaTime);
        }
    }
}
