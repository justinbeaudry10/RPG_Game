using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoor : MonoBehaviour
{
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
}
