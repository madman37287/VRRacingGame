using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class TreeGeneration : MonoBehaviour
{
    public GameObject treePrefabVar;

    // Start is called before the first frame update
    void Start()
    {
        // initial variables that will be used to calculate tree positions
        Vector3[] positions = new Vector3[10];
        float[] blacklist1 = new float[5];
        float[] blacklist2 = new float[5];
        bool isBlacklist;

        for (int i = 0; i < 10; i++)
        {
            // set to true at beginning of loop to trigger while loop
            isBlacklist = true;

            // call to randomly assign a position on the x axis from -100 to 100
            positions[i].x = UnityEngine.Random.Range(-100, 100);

            // each side of road is handled separately
            if (i < 5) {
                // Manually moving game object at 0,0,0 to desired Z location gave us 11.0f
                positions[i].z = 11.0f;

                while(isBlacklist) {
                    // Set blacklist check to false by default before beginning check.  This is our while escape condition.
                    isBlacklist = false;
                    for (int j = 0; j < i; j++)
                    {
                        // Check if the new position is further than 5 meters away from previously placed tree
                        if ((positions[i].x > (blacklist1[j]-5)) && (positions[i].x < (blacklist1[j]+5)))
                        {
                            // if tree is too close to another tree, re-roll the position of the tree and run the check again
                            // This will keep re-rolling until the tree is more than 5 meters away from all previously placed trees
                            isBlacklist = true;
                            UnityEngine.Debug.Log("blacklist1 reroll");
                            positions[i].x = UnityEngine.Random.Range(-100, 100);
                        }
                    }
                }

                // after tree position is finalized, add the position to the blacklist
                blacklist1[i] = positions[i].x;
            }
            else
            {
                // Manually moving game object at 0,0,0 to desired Z location gave us -6.5f
                positions[i].z = -6.5f;

                // Mirror of while loop in previous if statement
                while (isBlacklist)
                {
                    isBlacklist = false;
                    for (int j = 0; j < i-5; j++)
                    {
                        if ((positions[i].x > (blacklist2[j] - 5)) && (positions[i].x < (blacklist2[j] + 5)))
                        {
                            isBlacklist = true;
                            UnityEngine.Debug.Log("blacklist2 reroll");
                            positions[i].x = UnityEngine.Random.Range(-100, 100);
                        }
                    }
                }
                // because this side of the road i ranges between 6 and 10, we must subtract the difference within the array to get our 1 through 5 range
                blacklist2[i-5] = positions[i].x;
            }

            // Instantiate treePrefab to the positions calculated above.
            Instantiate(treePrefabVar, positions[i], Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
