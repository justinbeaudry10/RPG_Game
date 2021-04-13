using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoor : MonoBehaviour
{
    //Detects collision with game objects
    private void OnCollisionEnter(Collision other)
    {
        //If the collided object is a player and the player has the key on them
        if ((other.gameObject.tag == "Player")&& (Player.hasKey))
        {
            //Door gets destroyed
            Destroy(gameObject);
        }
    }
}
