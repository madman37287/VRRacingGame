using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacleHandler : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public int numObstacles = 3000;

    List<GameObject> obstacle = new List<GameObject>();
    

    int curObj;
    int curObjEnabled = 0;

    int fabSel;

    int maxX = 440;
    int minX = -535;
    int maxZ = 530;
    int minZ = -445;

    // Start is called before the first frame update
    void Start()
    {
        // Code below is a modified solution to that used in Assignment 1

        // initial variables that will be used to calculate tree positions
        Vector3[] positions = new Vector3[numObstacles];
        bool isBlacklist;
        curObj = numObstacles;
        for (int i = 0; i < numObstacles; i++)
        {
            positions[i].y = 500;
            // set to true at beginning of loop to trigger while loop
            isBlacklist = true;

            // call to randomly assign a position on the x axis from -100 to 100
            positions[i].x = UnityEngine.Random.Range(minX, maxX);
            positions[i].z = UnityEngine.Random.Range(minZ, maxZ);

            while(isBlacklist) 
            {
                // Set blacklist check to false by default before beginning check.  This is our while escape condition.
                isBlacklist = false;
                for (int j = 0; j < i; j++)
                {
                    // Check if the new position is further than 5 meters away from previously placed tree
                    if (((positions[i].x > (positions[j].x-5)) && (positions[i].x < (positions[j].x+5)))    &&    ((positions[i].z > (positions[j].z-5)) && (positions[i].z < (positions[j].z+5))))
                    {
                        // if tree is too close to another tree, re-roll the position of the tree and run the check again
                        // This will keep re-rolling until the tree is more than 5 meters away from all previously placed trees
                        isBlacklist = true;
                        positions[i].x = UnityEngine.Random.Range(minX, maxX);
                        positions[i].z = UnityEngine.Random.Range(minZ, maxZ);
                    }
                }
            }
            // randomly select tree or tree_conifer prefab
            fabSel = UnityEngine.Random.Range(0, 2);
            // Instantiate treePrefab to the positions calculated above.
            obstacle.Add(Instantiate(obstaclePrefabs[fabSel], positions[i], Quaternion.identity));
            obstacle[i].transform.parent = transform;
            // obstacles instantiate sideways, rotate -90 to fix
            obstacle[i].transform.Rotate(-90, 0, 0);
        }
    }

    public void addObj(int newObj)
    {
        // if new objects are to be added, increment the curObj counter keeping track of currently enabled prefabs
        curObj = curObj + newObj;
        if (curObj > numObstacles)
        {
            // do not let curObj exceed the max number of obstacles
            curObj = numObstacles;
        }

        // for all disabled obstacles
        for (int i = curObjEnabled; i < curObj; i++)
        {
            // in the event a tree has spawned above the car, we want to ignore it and keep it invisible
            if (obstacle[i].transform.position.y < 100)
            {
                // make all new obstacles visible and enable their colliders
                obstacle[i].GetComponent<MeshRenderer>().enabled = true;
                obstacle[i].GetComponent<CapsuleCollider>().enabled = true;
            }
        }
        // update curObjEnables to show how many obstacles have been enabled total for next time function is called
        curObjEnabled = curObj;
    }

    public void resetObj()
    {
        // for all currently enabled obstacles
        for (int i = 0; i < curObj; i++)
        {
            // in the event a tree has spawned above the car, we want to ignore it and keep it invisible
            if (obstacle[i].transform.position.y < 100)
            {
                // disable all currently enabled obstacles.
                obstacle[i].GetComponent<MeshRenderer>().enabled = false;
                obstacle[i].GetComponent<CapsuleCollider>().enabled = false;
            }
        }
        curObj = 0;
        curObjEnabled = 0;
    }

}
