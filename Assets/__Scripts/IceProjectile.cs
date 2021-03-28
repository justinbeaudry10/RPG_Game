using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceProjectile : ProjectilePlayer
{
    // Destroys the gO when collides with anything
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

    // Handles trigger detection with enemy
    private void OnTriggerEnter(Collider other)
    {
        // Printing the trigger in console
        print("Hit: " + other);

        // If tag triggered is the enemy tag
        if (other.CompareTag("Enemy"))
        {
            // Setting the enemy as a local variable
            enemy = other.gameObject.GetComponent<EnemyAI>();
            // Freezes the enemy
            enemy.freeze();
            // Deals 25 damage to the enemy
            enemy.TakeDamage(25);
            //Destroys the fire projectile
            Destroy(gameObject);
        }

        //Otherwise, simply destroys gO
        else
        {
            Destroy(gameObject);
        }


    }
}
