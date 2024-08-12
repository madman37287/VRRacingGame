using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipSpawn : MonoBehaviour
{
    // Public variable to call ship prefab
    public GameObject spaceShipPrefab;

    // member variable setup
    float spawnRadius = 10f;
    float spawnHeightMin = 1f;
    float spawnHeightMax = 6f;
    float rotation;
    float wVel = 0;
    Vector3 origin = new Vector3();
    Vector3 position = new Vector3();

    private GameObject spaceShip;

    // Start is called before the first frame update
    void Start()
    {
        ShipSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(origin, Vector3.up, -wVel * Time.deltaTime);
    }

    public void ShipSpawn()
    {
        origin.x = 0;
        origin.y = 0;
        origin.z = 0;

        // Randomly assign a spawn location of the ship within an imaginary cylinder.
        position.x = UnityEngine.Random.Range(0f, spawnRadius);
        position.y = UnityEngine.Random.Range(spawnHeightMin, spawnHeightMax);
        rotation = UnityEngine.Random.Range(0f, 360f);

        // Set a rotational velocity for the ship to be used in the update function between 10 and 40 deg/sec
        wVel = UnityEngine.Random.Range(10f, 40f);

        // set initial location of the spaceship spawn on the xy axis
        transform.position = new Vector3(position.x, position.y, 0);
        transform.eulerAngles = origin;

        // Rotate around will then spin the ship around the y axis, placing the object in a random circular coordinate on the xz plane.
        // This is effectively will create our imaginary cylinder: by creating a circle on the xz plane, with a height of 5 on the y axis
        transform.RotateAround(origin, Vector3.up, rotation);

        // spawn the spaceship prefab at the gameObject's position AND rotation
        spaceShip = Instantiate(spaceShipPrefab, transform.position, transform.rotation);
        spaceShip.transform.Rotate(90, 0, 0);

        // attach prefab clone to parent object
        spaceShip.transform.parent = this.transform;
    }

    // Function to destroy the spaceship prefab
    public void ShipDelete()
    {
        Destroy(spaceShip);
    }

    // Function to change tag once game ends
    public void passive()
    {
        spaceShip.transform.tag = "GameOver";
    }
}
