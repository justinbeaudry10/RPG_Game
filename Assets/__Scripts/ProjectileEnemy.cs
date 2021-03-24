using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : MonoBehaviour
{
    public float speed;

    private Transform player;
    private Vector3 target;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerEyes").transform;

        target = new Vector3(player.position.x, player.position.y, player.position.z);

        transform.LookAt(player.position);

    }

    // Update is called once per frame
    void Update()
    {

        //transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        transform.position += transform.forward * speed * Time.deltaTime;


    }

    private void OnCollisionEnter(Collision collision)
    {
        print("Collision");
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("PlayerEyes"))
        {
            print("Hit player");
            Destroy(gameObject);
        }
        else
        {
            print("Trigger");
            Destroy(gameObject);
        }


    }

}