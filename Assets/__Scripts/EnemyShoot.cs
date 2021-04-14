using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : EnemyAI
{
    [Header("Shooting Enemy and Game System Settings")]
    public GameObject projectile;                   //Variable to reference the enemy projectile prefab

    [Header("Shooting Enemy Attack Settings")]
    private float timeBetweenShots;                 //Stores the attack time between shots
    public float startTimeBetweenShots;             //Stores the start time for attack between shots
    private AudioSource zoombieShootingAudio;       //Variable to reference the audio source for enemy shooting

    private bool frozen = false;

    public override void Awake()
    {
        //Call the parent Awake()
        base.Awake();
    }

    // Start is called before the first frame update
    public override void Start()
    {
        //Call the parent Start()
        base.Start();

        //The time between shots equals the start time
        timeBetweenShots = startTimeBetweenShots;

        //get the audio source component
        zoombieShootingAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public override void Update()
    {
        //Call the parent Update()
        base.Update();
    }

    public override void freeze()
    {
        frozen = true;
    }

    public override void ChasePlayer()
    {
        transform.LookAt(playerTransform);
    }

    public override void AttackPlayer()
    {
        //Make the enemy look at the player
        transform.LookAt(playerTransform);

        if (timeBetweenShots <= 0)
        {
            //Create a projectile (to shoot)
            Instantiate(projectile, transform.position + transform.forward * 2.5f, Quaternion.identity);

            //Play the shooting audio
            zoombieShootingAudio.Play();

            //The time between shots is the start time between shots
            timeBetweenShots = startTimeBetweenShots;
        }
        //If the time between shots is not less than or equal to 0
        else if(!frozen)
        {
            //Decrease the time between shots by the difference in time
            timeBetweenShots -= Time.deltaTime;
        }
    }

}