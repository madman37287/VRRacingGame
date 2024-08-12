using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterOfMass : MonoBehaviour
{
    public Vector3 CoM = new Vector3(0,0,0);
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        // Set the car's center of mass to new location
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = CoM;
    }
}
