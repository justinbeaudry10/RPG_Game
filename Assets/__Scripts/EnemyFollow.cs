using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyFollow : MonoBehaviour
{
    public NavMeshAgent enemy;
    public Transform player;
    public Player playerGO;

    public float health = 25f;
    public void TakeDamage(float amnt)
    {
        health -= amnt;
        if (health<=0)
        {
            print("Enemy Died");
            Destroy(gameObject);
        }
        else
        {
            print("Enemy took damage");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        enemy.SetDestination(player.position);
    }

    //Detects collision with PlayerProjectile
    void OnCollisionEnter(Collision coll)
    {
        GameObject otherGO = coll.gameObject;
        switch (otherGO.tag)
        {
            case "PlayerProjectile":
                TakeDamage(playerGO.myWeapon.attackDamage);

                if (health <= 0)
                {
                    //Destroy This Enemy Instance
                    Destroy(this.gameObject);
                }
                Destroy(otherGO);
                break;

            default:
                print("Enemy was hit by non-ProjectileHero: " + otherGO.name);
                break;

        }
    }
}
