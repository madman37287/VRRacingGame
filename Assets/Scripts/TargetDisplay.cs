using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetDisplay : MonoBehaviour
{
    Text text;
    ObjectiveHandler objHandle;
    Vector3 checkPos;
    Vector3 checkPosRel;

    // Start is called before the first frame update
    void Start()
    {
        text = transform.Find("Text").GetComponent<Text>();
        objHandle = GameObject.Find("Objectives").GetComponent<ObjectiveHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        // get current target position
        checkPos = objHandle.CurObjLoc();
        // get the current target position relative to the car
        checkPosRel = transform.InverseTransformPoint(checkPos);

        // if the checkpoint position is at 1000, 1000, 1000, then it does not exist.  Show error.
        if (checkPos.x >= 999)
        {
            text.text = "Checkpoint Location:\nERROR";
        }
        // if x is positive, player needs to turn right, otherwise the player must turn left
        else if (checkPosRel.x >=0)
        {
            text.text = "Checkpoint Location:\n->";
        }
        else
        {
            text.text = "Checkpoint Location:\n<-";
        }
    }
}
