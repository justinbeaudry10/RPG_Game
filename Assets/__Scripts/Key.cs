using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    //Detects collision with game objects
    private void OnCollisionEnter(Collision other)
    {
        //Detects collision with player
        if (other.gameObject.tag == "Player")
        {
            //Changes the hasKey boolean to variable to true
            Player.hasKey = true;
            //Gets rid of key instance in scene
            Destroy(gameObject);
        }
    }
}

    