using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : MonoBehaviour
{
    [Header("Enemy Projectile Properties Settings")]
    public float speed;                         //Projectile's speed
    public int bulletDamage = 10;               //Projectile's damage

    [Header("Enemy Projectile Game and System Settings")]
    private GameObject playerGO;            //Variable to reference the Player GameObject
    private Player player;                  //Variable to reference the Player object
    private Transform playerEyesTransform;  //Variable to reference the Player's eyes' transform component
    private Vector3 target;                 //Variable to store the target destination

    protected void Awake()
    {
        //Assigning the player GameObject
        playerGO = GameObject.Find("Player");

        //Assigning the player object
        player = playerGO.GetComponent<Player>();

        //Assigning the player transform component (the player's eyes)
        playerEyesTransform = GameObject.FindGameObjectWithTag("PlayerEyes").transform;
    }

    // Start is called before the first frame update
    protected void Start()
    {   
        //Assigning the target to the player's eyes position
        target = new Vector3(playerEyesTransform.position.x, playerEyesTransform.position.y, playerEyesTransform.position.z);
        
        //Make the projectile look at the player's eyes
        transform.LookAt(playerEyesTransform.position);

    }

    // Update is called once per frame
    protected void Update()
    {
        //Move the projectile forward by the speed
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        //If the enemy projectile hit the player or the player's eyes
        if (other.CompareTag("Player") || other.CompareTag("PlayerEyes"))
        {
            //Print it hit the player
            print("Hit playerTransform");

            //The player takes damage
            player.TakeDamage(bulletDamage);

            //Destory the projectile
            Destroy(gameObject);
        }

        //If it hit something else
        else
        {
            //Print what it hit
            print("Hit" + other.tag);

            //Destory the projectile
            Destroy(gameObject);
        }
    }
}