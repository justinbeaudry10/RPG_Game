using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyAI
{
    private bool charging = false;
    public int chargeCooldown = 10;
    public int chargeSpeed, chargeDamage;

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

        StartCoroutine(recharge());
    }

    // Update is called once per frame
    public override void Update()
    {
        //Call the parent Update()
        base.Update();

        if (chargeCooldown == 10)
        {
            charging = true;
        }
        else
        {
            charging = false;
        }
            
    }

    public override void ChasePlayer()
    {
        if (charging)
        {
            agent.speed = chargeSpeed;
            Invoke("StopCharge", 3);
        }

        base.ChasePlayer();
    }

    public override void AttackPlayer()
    {
        if (charging)
        {
            damage = chargeDamage;
        }
        else
        {
            damage = 20;
        }

        base.AttackPlayer();

        /*
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
        else
        {
            //Decrease the time between shots by the difference in time
            timeBetweenShots -= Time.deltaTime;
        }
        */
    }

    void StopCharge()
    {
        agent.speed = 1;
        chargeCooldown = 0;
    }

    IEnumerator recharge()
    {
        while (true)
        {
            if(chargeCooldown < 10)
            {
                chargeCooldown++;

                yield return new WaitForSeconds(1);
            }
            else
            {
                yield return null;
            }
        }
    }
}
