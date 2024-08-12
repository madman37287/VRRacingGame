using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

public class LaserRayController : MonoBehaviour
{
    public GameObject[] asteroidPrefabs;
    public GameObject particlePrefab;
    public Material yellowMaterial;
    public Material blueMaterial;
    public GameObject explosionParticlePrefab;// to assign in Inspector

    private SpaceShipSpawn spaceShip;
    private GameObject particleObject;
    private float gazeDuration = 0f;
    private int successInstances = 0;
    private Vector3 asteroidNextPosition = new Vector3();
    // Start is called before the first frame update
    void Start()
    {
        asteroidNextPosition.x = -5f;
        asteroidNextPosition.z = 2f;
        transform.GetComponentInChildren<LineRenderer>();
        spaceShip = GameObject.Find("ShipObject").GetComponent<SpaceShipSpawn>();
    }

    // Update is called once per frame
    void Update()
    {
        // create new raycast variables and tie it to the camera
        RaycastHit hit;
        Transform camera = Camera.main.transform;
        Ray ray = new Ray(camera.position, camera.transform.forward);

        // condition set to true if ray hits a object with tag "EnemyShip"
        if (Physics.Raycast(ray, out hit) && (hit.collider.tag == "EnemyShip"))
        {
            // On hit, set the line material to yellow
            transform.GetComponentInChildren<LineRenderer>().material = yellowMaterial;

            // if object is not instantiated, instantiate the object
            if (particleObject == null)
            {
                particleObject = Instantiate(particlePrefab, hit.point, Quaternion.identity);
            }

            // if the particles are not emitting, begin emission
            if (!particleObject.GetComponent<ParticleSystem>().isEmitting)
            {
                particleObject.GetComponent<ParticleSystem>().Play();
            }

            // move particle system to point of contact
            particleObject.transform.position = hit.point;

            // start collecting time data for how long the raycast has detected the ship.
            gazeDuration += Time.deltaTime;

            // After 3 seconds, trigger success
            if (gazeDuration >= 3.0f)
            {
                HandleSuccessInstance(hit.point);
                gazeDuration = 0f; // to reset gaze duration after success instance
            }
        }
        else
        {
            // if collision not detected, laser should turn blue
            transform.GetComponentInChildren<LineRenderer>().material = blueMaterial;

            // reset gaze on no-contact
            gazeDuration = 0f;

            // if object exists, perform actions
            if (!(particleObject == null))
            {

                // stop emitting particles if still playing
                // Note: this is not exactly what was shown in the video, but
                //       I thought allowing the particles to finish after emission added more
                //       realism than using the immediate pause/destroy functionality.
                if (particleObject.GetComponent<ParticleSystem>().isPlaying)
                {
                    particleObject.GetComponent<ParticleSystem>().Stop();
                }
                else
                {
                    //Destroy prefab once particles are finished hitting ground/disappearing
                    Destroy(particleObject);
                }
            }
        }
    }
    
    void HandleSuccessInstance(Vector3 hitPoint)
    {
        // instantiating variable that is only used in this funcition.
        int prefabSelect = 0;

        // Delete the ship on success
        spaceShip.ShipDelete();

        if (successInstances < 10)
        {
            // Spawn a new ship at new location using original function in SpaceShipSpawn.cs
            spaceShip.ShipSpawn();

            // to instantiate and destroy the explosion particle effect after 2 seconds
            GameObject explosionInstance = Instantiate(explosionParticlePrefab, hitPoint, Quaternion.identity);
            Destroy(explosionInstance, 2f);

            // Randomly select which asteroid to use
            prefabSelect = UnityEngine.Random.Range(0, asteroidPrefabs.Length);

            // This is so the astroid is not half in the ground
            // I pull the astroid prefab y value to place it just above ground level as seen in the instructor's video
            asteroidNextPosition.y = asteroidPrefabs[prefabSelect].transform.position.y;

            // to instantiate a random asteroid and assign a random color
            GameObject asteroid = Instantiate(asteroidPrefabs[prefabSelect], asteroidNextPosition, Quaternion.identity);
            asteroid.GetComponent<Renderer>().material.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
            asteroidNextPosition.x += 1.0f; // to update position for next asteroid 

            successInstances++; // to make success instances count

            // if we have hit the max success count, we will change the tag on the ship,
            // thus making it no longer interractable.
            if (successInstances == 10)
            {
                spaceShip.passive();
            }
        }
    }
}
