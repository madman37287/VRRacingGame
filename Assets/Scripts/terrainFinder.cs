using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class terrainFinder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // create new raycast variables and tie it to the object
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward * -1);
        if (Physics.Raycast(ray, out hit) && (hit.collider.tag == "Ground"))
        {
            // if raycast finds the ground, teleport to the ground and delete the finder script
            transform.position = hit.point;
            Destroy(transform.GetComponent<terrainFinder>());
        }
        else
        {
            // Trees have likely found the car.  we do not want to teleport.
            // cannot destroy due to obstacle logic
            transform.GetComponent<MeshRenderer>().enabled = false;
            transform.GetComponent<CapsuleCollider>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
