using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePlayer : MonoBehaviour
{
    public float speed = 20;                //The projectile's speed
    public int bulletDamage = 5;            //The projectile's damage

    protected EnemyAI enemy;                //Reference the enemy object
    protected GameObject playerGO;          //Reference the player game object
    protected Player player;                //Reference the player object
    protected Transform playerTransform;    //Reference the player's transform
    protected Vector3 target;               //Variable to store the target direction

    private void Awake()
    {
        //Initalize the player game object
        playerGO = GameObject.Find("Player");

        //Initalize the player object
        player = playerGO.GetComponent<Player>();

        //Initalize the player transform
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Set the projectile's position
        transform.position += new Vector3(0, 1.5f, 0);
        transform.position += transform.forward * 2;

    }

    // Update is called once per frame
    void Update()
    {
        //Move the projectile foward with respect to speed and time
        transform.position += transform.forward * speed * Time.deltaTime;

        //If projectile is past the boundries
        //CURRENTLY COMMENTED OUT BECAUSE INTERFERES WITH LEVEL 2 BOUNDRIES
        /*if (transform.position.z > 90 || transform.position.z < -55 || transform.position.x > 40 || transform.position.x < -55 || transform.position.y > 50 || transform.position.y < -1)
        {
            //Destory the projectile
            Destroy(gameObject);
        }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        //If the projectile hits the enemy
        if (other.CompareTag("Enemy"))
        {
            //Get the enemy's object
            enemy = other.gameObject.GetComponent<EnemyAI>();

            //The enemy takes damage
            enemy.TakeDamage(25);

            //Destroy the player's projectile
            Destroy(gameObject);
        }

        //If the projectile hits a boundry
        if (other.CompareTag("Boundary"))
        {
            //Print the statement
            print("Player Projectile Triggered by wall");

            //Destory the projectile
            Destroy(this.gameObject);
        }
        //If the projectile hit anything else
        else
        {
            //Destory the player's projectile
            Destroy(gameObject);
        }
    }
}
