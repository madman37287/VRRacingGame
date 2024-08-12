using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering : MonoBehaviour
{
    public float maxAngle = 90f;
    float rotation        = 0f;
    float turnAngle       = 0f;
    bool lGrabbed         = false;
    bool rGrabbed         = false;

    Transform leftHand;
    Transform rightHand;
    Vector3 leftHandRel;
    Vector3 rightHandRel;
    Vector3 lWheelPos;
    Vector3 rWheelPos;
    Vector3 lWheelAng;
    Vector3 rWheelAng;

    startCar car;

    // Start is called before the first frame update
    void Start()
    {
        car = transform.parent.parent.GetComponent<startCar>();

        // tie objects to player's left and right hands
        leftHand  = GameObject.Find("PlayerTeleport").transform.Find("XRRig").Find("Camera Offset").Find("LeftController");
        rightHand = GameObject.Find("PlayerTeleport").transform.Find("XRRig").Find("Camera Offset").Find("RightController");
    }

    // Update is called once per frame
    void Update()
    {
        if (car.getStart())
        {
            grabSteer();
        
            if (!lGrabbed && !rGrabbed)
            {
                controllerSteer();
            }
        }
    }


    void controllerSteer()
    {
        rotation = QuestInputs.Left.ThumbStick.x * maxAngle;
        transform.localEulerAngles = new Vector3(0, rotation, 0);
    }

    void grabSteer()
    {
        //wheel diameter = 0.444444444444444

        // get the position of the right and left hands relative to the steering wheel
        rightHandRel = transform.parent.InverseTransformPoint(rightHand.position);
        leftHandRel  = transform.parent.InverseTransformPoint(leftHand.position);

        // get the position of the left and right sides of the steering wheel
        lWheelPos = transform.Find("LeftHandPos").position;
        rWheelPos = transform.Find("RightHandPos").position;

        // get the angle of the left and right sides of the steering wheel
        lWheelAng = transform.Find("LeftHandPos").eulerAngles;
        rWheelAng = transform.Find("RightHandPos").eulerAngles;

        // if any hand is within 0.3 units of the steering, enable outline. 
        if ((leftHandRel.magnitude <= 0.3f || rightHandRel.magnitude <= 0.3f) && (!lGrabbed && !rGrabbed))
        {
            transform.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineAll;
        }
        else
        {
            transform.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineHidden;
        }

        // check if left grip is being depressed
        if (QuestInputs.Left.Grip.Current)
        {
            // initialize turn angle to 0
            turnAngle = 0;

            // check if left hand is within the imaginary sphere of radius three around the origin of the steering wheel
            if (leftHandRel.magnitude <= 0.3f && !lGrabbed)
            {
                // if the left hand is within range, then we have grabbed the steering wheel
                lGrabbed = true;
            }
            else if (lGrabbed)
            {
                // set the left hand mesh location to the left side of the steering wheel
                leftHand.GetChild(0).position    = lWheelPos;
                leftHand.GetChild(0).eulerAngles = lWheelAng;

                // relative to the steering wheel, if our left hand is <0 on the x axis, that means that we have exceeded our 90 degree bound.  The steering wheel should then lock at + or - 90 degrees.
                if (leftHandRel.x < 0)
                {
                    // calculate the angle the steering wheel should be equal to by using the toa in sohcahtoa.  Only relative x and z values matter here
                    // Because the left side of the steering wheel lives in the left half of our imaginary unit circle, arctan values must flip signs.
                    turnAngle = Mathf.Rad2Deg * -Mathf.Atan(leftHandRel.z/leftHandRel.x);
                }
                else if (leftHandRel.z < 0 )
                {
                    // if x is positive, and z is negative, our angle should cap at -90
                    turnAngle = -90f;
                }
                else
                {
                    // if x is positive, and z is positive, our angle should cap at 90
                    turnAngle = 90f;
                }
            }

            // Do not set this value if right hand is on the steering wheel also
            // This flag restriction makes our right hand the dominant controller of the steering wheel
            if (!rGrabbed)
            {
                // Vector3.up = 0,1,0.  because we want to rotate about the y axis, we can multiply this vector by our turn angle
                transform.localEulerAngles = Vector3.up * turnAngle;
            }
        }
        else
        {
            // if left grip is released, then reset position and angle of hand mesh
            leftHand.GetChild(0).localPosition     = Vector3.zero;
            leftHand.GetChild(0).localEulerAngles  = Vector3.forward * 90;

            // set grabbed flag to false
            lGrabbed = false;
        }

        // Check if right grip is depressed
        if (QuestInputs.Right.Grip.Current)
        {
            // set default turn angle to 0
            turnAngle = 0;

            // check if left hand is within the imaginary sphere of radius three around the origin of the steering wheel
            if (rightHandRel.magnitude <= 0.3f && !rGrabbed)
            {
                // if the left hand is within range, then we have grabbed the steering wheel
                rGrabbed = true;
            }
            else if (rGrabbed)
            {
                // set the left hand mesh location to the left side of the steering wheel
                rightHand.GetChild(0).position    = rWheelPos;
                rightHand.GetChild(0).eulerAngles = rWheelAng;
                
                // relative to the steering wheel, if our left hand is >0 on the x axis, that means that we have exceeded our 90 degree bound.  The steering wheel should then lock at + or - 90 degrees.
                if (rightHandRel.x > 0)
                {
                    // calculate the angle the steering wheel should be equal to by using the toa in sohcahtoa.  Only relative x and z values matter here
                    turnAngle = Mathf.Rad2Deg * -Mathf.Atan(rightHandRel.z/rightHandRel.x);
                }
                else if (rightHandRel.z > 0 )
                {
                    // if x is negative, and z is positive, our angle should cap at -90
                    turnAngle = -90f;
                }
                else
                {
                    // if x is negative, and z is negative, our angle should cap at 90
                    turnAngle = 90f;
                }
            }

            // Vector3.up = 0,1,0.  because we want to rotate about the y axis, we can multiply this vector by our turn angle
            transform.localEulerAngles = Vector3.up * turnAngle;
        }
        else
        {
            // if right grip is released, then reset position and angle of hand mesh
            rightHand.GetChild(0).localPosition    = Vector3.zero;
            rightHand.GetChild(0).localEulerAngles = Vector3.forward * -90;

            // set grabbed flag to false
            rGrabbed = false;
        }
    }

}
