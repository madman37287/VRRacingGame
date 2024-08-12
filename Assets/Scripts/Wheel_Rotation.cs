using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel_Rotation : MonoBehaviour
{
    public float maxAcceleration = 2000f;
    public float maxBrake = 2000f;
    float acceleration = 0f;
    float brake = 0;
    float maxVelocity = 15f;
    float velocity = 0f;
    float wVelocity = 0f;
    bool isGameStart;
    Rigidbody rb;
    WheelCollider wheel;

    startCar car;

    // Start is called before the first frame update
    void Start()
    {
        car = transform.parent.parent.GetComponent<startCar>();
        rb = transform.parent.parent.GetComponent<Rigidbody>();
        wheel = transform.GetChild(1).GetComponent<WheelCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        // standard physics equation for determining rotational velocity from forward velocity
        velocity = transform.parent.parent.InverseTransformDirection(rb.velocity).z;
        wVelocity = (velocity / 0.4f) * (180f / 3.1415f);

        //Transform must be done on child mesh, so that we do not rotate the collider as well and avoid gimbal lock
        transform.GetChild(0).Rotate(wVelocity * Time.deltaTime, 0, 0);
    }

    // Physics operations should be done within FixedUpdate
    void FixedUpdate()
    {
        if (car.getStart())
        {
            motorController();
        }
    }

    void motorController()
    {
        // if right trigger is pressed, accelerate forward, so long as max velocity has not been reached
        if (transform.InverseTransformDirection(rb.velocity).z >= maxVelocity)
        {
            acceleration = 0;
        }
        else if (QuestInputs.Right.Trigger.Current) //if (Input.GetKey("up"))
        {
            acceleration = maxAcceleration;
        }
        else if (QuestInputs.Left.Trigger.Current) //else if (Input.GetKey("down"))
        {
            // if left trigger is pressed, accelerate backwards
            acceleration = -maxAcceleration;
        }
        else
        {
            // if neither trigger is pressed, do not accelerate
            acceleration = 0;
        }

        if (QuestInputs.Right.Button1.Current) //if (Input.GetKey("space"))
        {
            // if button is pressed on right controller, apply brakes
            brake = maxBrake;
        }
        else
        {
            brake = 0;
        }

        // apply the torques calculated above
        wheel.motorTorque = acceleration;
        wheel.brakeTorque = brake;
    }

    public void setMaxVel(int vel)
    {
        // set the max velocity before car stops accelerating
        maxVelocity = vel;
    }
}
