using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    //Detects collision with game objects
    private void OnTriggerEnter(Collider other)
    {
        print("Coll w key");
        //Detects collision with player
        if (other.CompareTag("Player"))
        {
            //Changes the hasKey boolean to variable to true
            Player.hasKey = true;
            //Gets rid of key instance in scene
            Destroy(gameObject);
        }
    }
}

    