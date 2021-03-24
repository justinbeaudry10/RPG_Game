using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : MonoBehaviour
{
    public float speed;
    public int bulletDamage = 10;

    private GameObject playerGO;
    private Player player;
    private Transform playerTransform;
    private Vector3 target;

    protected void Awake()
    {
        playerGO = GameObject.Find("Player");
        player = playerGO.GetComponent<Player>();
        playerTransform = GameObject.FindGameObjectWithTag("PlayerEyes").transform;
    }

    // Start is called before the first frame update
    protected void Start()
    {
        target = new Vector3(playerTransform.position.x, playerTransform.position.y, playerTransform.position.z);
        
        transform.LookAt(playerTransform.position);

    }

    // Update is called once per frame
    protected void Update()
    {
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
            print("Hit playerTransform");
            player.TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
        else
        {
            print("Trigger");
            Destroy(gameObject);
        }


    }

}