using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyAI
{
    private bool charging = false;
    public int chargeCooldown = 10;
    public int chargeSpeed, chargeDamage;
    private bool frozen;
    public GameObject key;


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

    }

    public override void SetOnFire()
    {
        base.SetOnFire();
        Invoke("StopBurning", 5);
    }

    public void StopBurning()
    {
        onFire = false;
    }

    public override void freeze()
    {
        frozen = true;
        base.freeze();
        Invoke("UnFreeze", 3);
    }

    public void UnFreeze()
    {
        frozen = false;
    }

    IEnumerator recharge()
    {
        while (true)
        {
            if(chargeCooldown < 10)
            {
                charging = false;
                if (!frozen)
                {
                    chargeCooldown++;
                    agent.speed = 1;
                }
                yield return new WaitForSeconds(1);
            }
            else if(chargeCooldown == 10)
            {
                chargeCooldown = 0;
                agent.speed = 10;
                charging = true;
                yield return new WaitForSeconds(3);
            }
        }
    }

    //Called upon destruction of Boss gO
    private void OnDestroy()
    {
        //Instantiates the key in its place of death
        Instantiate(key, transform.position, key.transform.rotation);      
    }
}
