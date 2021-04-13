using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : ProjectilePlayer
{
    // Destroys the gO when it collides
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

    // Handles trigger detection with enemy
    private void OnTriggerEnter(Collider other)
    {
        // Printing in console the trigger
        print("Hit: " + other);

        // If tag triggered is enemy
        if (other.CompareTag("Enemy"))
        {
            // Setting an enemy as a local variable
            enemy = other.gameObject.GetComponent<EnemyAI>();
            // Sets the enemy "on fire"
            enemy.SetOnFire();
            // Does 25 damage to the enemy
            enemy.TakeDamage(25);
            // Destroys the fire projectile
            Destroy(gameObject);
        }
        //Otherwise, destroys the gO
        else
        {
            Destroy(gameObject);
        }


    }
}
