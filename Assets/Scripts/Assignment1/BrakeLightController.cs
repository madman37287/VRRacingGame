using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
using UnityEngine;

public class BrakeLightController : MonoBehaviour
{
    // public variable for materials.  This is to drag and drop the material into the variable from unity
    // Preferably, we would like to make these private and call the material directly from the script,
    // what function achieve this?
    public Material brakeLightOn;
    public Material brakeLightOff;

    // need to grab the mesh renderer component to load in new materials
    MeshRenderer mesh;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        // simple if space is pressed down, switch to red material and switch back to previous material on release
        if (Input.GetKeyDown("space"))
        {
            mesh.material = brakeLightOn;
        }
        else if (Input.GetKeyUp("space"))
        {
            mesh.material = brakeLightOff;
        }
    }
}
