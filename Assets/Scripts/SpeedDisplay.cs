using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedDisplay : MonoBehaviour
{
    Text text;
    Rigidbody rb;

    float velocity;

    // Start is called before the first frame update
    void Start()
    {
        // set objects and components
        text = transform.Find("Text").GetComponent<Text>();
        rb   = transform.parent.parent.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // get current velocity of car
        velocity = transform.parent.parent.InverseTransformDirection(rb.velocity).magnitude;
        // display velocity on car dashboard
        text.text = velocity.ToString("#0") + " MPH";
    }
}
