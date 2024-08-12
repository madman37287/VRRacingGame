using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HapticFeedbackSample : MonoBehaviour
{
    void Update()
    {
        if(QuestInputs.Right.Button1.Down) QuestInputs.Right.Vibrate();
        if(QuestInputs.Left.Button1.Down) QuestInputs.Left.Vibrate();
    }
}
