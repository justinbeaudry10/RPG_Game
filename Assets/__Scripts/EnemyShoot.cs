using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : EnemyAI
{
    private float timeBetweenShots;
    public float startTimeBetweenShots;

    public GameObject projectile;

    public override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        timeBetweenShots = startTimeBetweenShots;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (timeBetweenShots <= 0)
        {
            Instantiate(projectile, transform.position + transform.forward * 2.5f, Quaternion.identity);
            timeBetweenShots = startTimeBetweenShots;
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }

    }
}