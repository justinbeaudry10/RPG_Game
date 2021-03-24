using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot2 : MonoBehaviour
{
    private float timeBetweenShots;
    public float startTimeBetweenShots;

    public GameObject projectile;
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player").transform;

        timeBetweenShots = startTimeBetweenShots;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeBetweenShots <= 0)
        {
            Instantiate(projectile, transform.position, Quaternion.identity);
            timeBetweenShots = startTimeBetweenShots;
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }

    }
}
