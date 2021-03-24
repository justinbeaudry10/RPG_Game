using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePlayer : MonoBehaviour
{
    public float speed;
    public int bulletDamage = 5;

    private EnemyAI enemy;
    private GameObject playerGO;
    private Player player;
    private Transform playerTransform;
    private Vector3 target;

    private void Awake()
    {
        playerGO = GameObject.Find("Player");
        player = playerGO.GetComponent<Player>();
        playerTransform = GameObject.FindGameObjectWithTag("PlayerEyes").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        //transform.LookAt(position);
        transform.position += new Vector3(0, 1.5f, 0); ;
        transform.position += transform.forward * 2;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("PlayerProjectileCollison");
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Hit: " + other);
        if (other.CompareTag("Enemy"))
        {
            print("Hit Enemy");
            enemy = other.gameObject.GetComponent<EnemyAI>();
            enemy.TakeDamage(50);
            Destroy(gameObject);
        }
        else
        {
            print("PlayerProjectileTrigger");
            Destroy(gameObject);
        }


    }
}
