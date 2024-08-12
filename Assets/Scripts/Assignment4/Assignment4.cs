using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


public class Assignment4 : MonoBehaviour
{    
    private Transform selectedObj;
    Rigidbody rb;
    Vector3 initialPos  = new Vector3();
    Vector3 destination = new Vector3();
    bool grabbed        = false;
    float airTime       = 0f;

    void Start()
    {
   
    }

    void Update()
    {
        // create new raycast variables and tie it to the camera
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        // If the object is not currently being interracted with, perform highlight detection
        if (!grabbed)
        {
            // setup hit variable using conditional
            if (Physics.Raycast(ray, out hit))
            {
                // if raycast hits grabbable object and selectedObj is not already set
                // This method makes it so a new grabbed object may have 1 or 2 frame offset before highlighting.  This is considered negligible.
                if ((hit.collider.tag == "Grabbable") && (selectedObj == null))
                {
                    // get the transform of the object that is being hit by the raycast
                    // selectedObj is how we will interract with the cubes throughout the code
                    selectedObj = hit.transform;

                    // On hit, highlight the grabbable objects
                    SetHighlight(selectedObj, true);
                }
                // if object is set and selected object does not equal what is currently being hit
                else if ((selectedObj != null) && (hit.transform.gameObject != selectedObj.gameObject))
                {
                    // If object was previously selected, but is no longer detected, undo the highlight
                    SetHighlight(selectedObj, false);

                    // set the object to null now that it is not selected.
                    selectedObj = null;
                }
            }
        }

        // if an object is currently being selected conditional and raycast is connecting (should always be connecting with something)
        if (selectedObj != null && Physics.Raycast(ray, out hit))
        {
            // Conditional using the oculus trigger using the QuestInputs script downloaded from D2L
            if (QuestInputs.Right.Trigger.Current)
            {
                // object is now being interracted with, so this boolean is set to true to prevent potential glitches with object getting set to null prematurely
                grabbed = true;

                // This should only be a one-time call when the trigger press is first detected to set up initial variables
                if (QuestInputs.Right.Trigger.Down)
                {
                    // Change layer to ignore raycast. This prevents the raycast from detecting the held object itself when object is released
                    selectedObj.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                    // get the rigidbody of the grabbable object
                    rb = selectedObj.GetComponent<Rigidbody>();
                    // collect the initial position data from the object (to be used in Lerp as our starting point)
                    initialPos = selectedObj.position;

                    // make the Hand object the parent of the grabbed object (TODO: Move this into Trigger.Down conditional) 
                    selectedObj.parent = transform.GetChild(0);
                }

                // airtime should go from 0 to 0.5 seconds per instruction (this is our flight time from initial position to target)
                if (airTime < 0.5f)
                {
                    // time between frames
                    airTime += Time.deltaTime;

                    // destination should be constantly updated to be 0.2z to the relative position of the controller
                    // This function converts local space to global space.
                    destination = transform.GetChild(0).TransformPoint(0, 0, 0.2f);

                    // Set object position to be interpolated between the initial position and destination, using airtime * 2 since 0.5 * 2 = 1
                    selectedObj.position = Vector3.Lerp(initialPos, destination, airTime * 2f);

                    // although not as important for flight to hand, velocity is set to 0 to avoid potentially glitchy behavior
                    rb.velocity = Vector3.zero;
                    // angular velocity is set to 0 to avoid object from spinning if grabbed during movement
                    rb.angularVelocity = Vector3.zero;
                }
                // once we hit destination
                if (airTime >= 0.5f)
                {
                    // set airtime to its max 0.5 value
                    airTime = 0.5f;
                    
                    // Ensure final position is set to be 0.2z local to the hand object (i.e: right in front of hand)
                    selectedObj.localPosition = new Vector3(0, 0, 0.2f);
                }
            }
            // if object was grabbed
            else if (grabbed)
            {
                // One time setup when trigger is released
                if (QuestInputs.Right.Trigger.Up)
                {

                    destination = transform.GetChild(0).TransformPoint(0, 0, 0.2f);
                    // misleading variable name due to reuse.  initial pos will actually be the final pos to where the hit point location is
                    initialPos = hit.point; // TODO: rename variable to something like "objTargetPos"
                    // since minimum height of cube is 0.15, y is offset by 0.15 so that the object is not halfway in the ground at destination
                    // technically this could be refined by either constantly checking collision or constantly calculating real center distance by rotation, but this works well enough.
                    initialPos.y = initialPos.y + 0.15f;
                    // release grabbed object from being a child of the Hand
                    selectedObj.parent = null;

                    // set layer back to original IgnoreBlobShadow, so that raycast can interract again
                    selectedObj.gameObject.layer = LayerMask.NameToLayer("IgnoreBlobShadow");
                }
                // here we reuse the previous airtime variable for 2 reasons:
                // 1. If object is only halfway to hand when trigger is released, object should fly away at about the same speed, without being drastically slower
                // 2. A variable already existed for this purpose, so simply subtracting from 0.5 to 0 is slightly more optimal.
                if (airTime > 0)
                {
                    // subtract time since last frame
                    airTime -= Time.deltaTime;
                }
                else
                {
                    // upon reaching destination, set airtime to be 0
                    airTime = 0f;
                    // set grabbed flag to false now that object is no longer being interracted with
                    grabbed = false;
                }

                selectedObj.position = Vector3.Lerp(initialPos, destination, airTime * 2f);

                // set velocity to zero to avoid glitchy behavior, clipping, and having the object launch itself once it gets to destination
                rb.velocity = Vector3.zero;
                // although angular velocity should already be zero, this should prevent it from spinning if it has a mid-air collision.
                rb.angularVelocity = Vector3.zero;

                // logic to allow object to not clip through ceiling due to height offset
                if (selectedObj.position.y > 7.4)
                {
                    grabbed = false;
                    airTime = 0f;
                }
            }
        }
    }





    //*****************************Given SetHighlight Method, Not To Be Changed******************************//
    //************ This method takes in a transform and a boolean to highlight or dim the GameObject*********//
    void SetHighlight(Transform t, bool highlight)
    {
        if (highlight)
        {
            t.GetComponent<Renderer>().material.color = Color.cyan;
            t.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineAll;
            transform.GetComponent<LineRenderer>().material.color = new Color(1.0f, 1.0f, 0.0f, 1.0f);
        }
        else
        {
            t.GetComponent<Renderer>().material.color = t.GetComponent<IsHit_S>().originalColorVar;
            t.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineHidden;
            transform.GetComponent<LineRenderer>().material.color = new Color(1.0f, 1.0f, 0.0f, 0.5f);
        }
    }
    //*****************************End of The Given SetHighlight Method**************************************//
}

