using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoor : MonoBehaviour
{
    public GameObject endText;                      //Game object variable for the end text

    private void Start()
    {
        //Ensures that the endText is off
        endText.SetActive(false);

    }

    //Detects collision with game objects
    private void OnTriggerEnter(Collider other)
    {
        //If the collided object is a player and the player has the key on them
        if (other.CompareTag("Player") && Player.hasKey)
        {
            //Door gets destroyed
            Destroy(gameObject);
        }
    }

    //Method is carried out upon destruction of the door
    private void OnDestroy()
    {
        //Shows the end text upon destruction of the final door.
        endText.SetActive(true);
    }
}
