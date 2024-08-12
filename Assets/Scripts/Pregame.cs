using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pregame : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // if A button is pressed
        if (QuestInputs.Right.Button1.Current) //if (Input.GetKey("space"))
        {
            // set objects and components.
            GameObject player        = GameObject.Find("PlayerTeleport");
            GameObject car           = GameObject.Find("Cartoon_SportCar_B01");
            TimerDisplay timer       = GameObject.Find("Cartoon_SportCar_B01").transform.Find("carrosserie").Find("TimerDisplay").GetComponent<TimerDisplay>();
            obstacleHandler obstacle = GameObject.Find("Obstacles").GetComponent<obstacleHandler>();
            Transform cameraOff      = player.transform.Find("XRRig").Find("Camera Offset");
            Transform camera         = cameraOff.Find("Camera");
            Transform driverSeat     = car.transform.Find("DriverRef");

            // reset the car position
            car.GetComponent<carReset>().resetGame();
            
            // set player to be a child of the car
            player.transform.parent   = car.transform;
            // reset player position
            player.transform.localPosition = Vector3.zero;
            // reset player rotation
            player.transform.localEulerAngles = Vector3.zero;

            // using pothagorean theorem to set player's head to the correct place in the car
            cameraOff.localPosition = driverSeat.localPosition - cameraOff.InverseTransformPoint(camera.position);
            // rotate the player's head to face forward in the car.  to not offset the head, this is done by rotating the parent around the child.
            cameraOff.RotateAround(camera.position, cameraOff.up, -camera.localEulerAngles.y);

            // initialize game
            GameObject.Find("Cartoon_SportCar_B01").transform.Find("Tires").GetComponent<velController>().resetFastestSpeed();
            obstacle.resetObj();
            timer.addTime(300);
            car.GetComponent<startCar>().StartGame();

            // destroy game menu
            Destroy(transform.gameObject);
        }
    }
}
