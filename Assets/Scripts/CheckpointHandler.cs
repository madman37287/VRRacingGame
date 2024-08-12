using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointHandler : MonoBehaviour
{
    bool isTouched = false;

    void OnTriggerEnter(Collider collider)
    {
        // If the Car collides with the capsule collider, change flag from false to true
        if (collider.tag == "Car")
        {
            isTouched = true;
        }
        
    }

    public bool getIsTouched()
    {
        // Objectives event handler can call this function and decide what should happen to the checkpoint.
        return isTouched;
    }
}
