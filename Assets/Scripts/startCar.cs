using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startCar : MonoBehaviour
{
    bool isStartGame = false;
    public void StartGame()
    {
        // play start game audio and set game started flag to true
        transform.GetComponent<AudioSource>().Play();
        isStartGame = true;
    }

    public void StopGame()
    {
        // set start game flag to false 
        isStartGame = false;
        // set player object
        Transform player = transform.Find("PlayerTeleport");
        // disown the player from parent
        player.parent = null;
        // teleport player to sky menu
        player.position = new Vector3(0, 500, 0);
        // reset player angles
        player.eulerAngles = Vector3.zero;
        // reset offset angles and position
        player.GetChild(0).GetChild(0).localPosition = Vector3.zero;
        player.GetChild(0).GetChild(0).localEulerAngles = Vector3.zero;
    }

    public bool getStart()
    {
        // pass isStartGame flag to children so they may enable/disable functionality accordingly
        return isStartGame;
    }
}
