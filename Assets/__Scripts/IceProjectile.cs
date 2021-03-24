using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceProjectile : ProjectilePlayer
{
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Hit: " + other);
        if (other.CompareTag("Enemy"))
        {
            enemy = other.gameObject.GetComponent<EnemyAI>();
            enemy.freeze();
            enemy.TakeDamage(25);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


    }
}
