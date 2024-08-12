using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTurn : MonoBehaviour
{
    Transform steering;
    Vector3 rotation = new Vector3();
    WheelCollider wheel;
    // Start is called before the first frame update
    void Start()
    {
        wheel = transform.GetChild(1).GetComponent<WheelCollider>();
        steering = transform.parent.parent.Find("Steering").Find("Steering_wheel");
        rotation = new Vector3(0,0,0);
    }

    // Update is called once per frame
    void Update()
    {

        // This if else is to move the range between -180 to 180,  This helps with dividing operations not screwing up our angles
        if (steering.localEulerAngles.y > 180)
        {
            //if in the bottom half of the unit circle, subtract 360 degrees to give the negative equivalent values
            rotation.y = (steering.localEulerAngles.y - 360)/2;
        }
        else
        {
            // divide by 2 so that max turn is 45 degrees instead of 90
            rotation.y = steering.localEulerAngles.y/2;
        }

        // wheel collider force direction is directly altered by the angle of the steering wheel
        wheel.steerAngle = rotation.y;

        //Transform must not be done on mesh child, or we can impose a gimbal lock
        transform.localEulerAngles = rotation;
    }
}
