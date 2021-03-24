using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePlayer : MonoBehaviour
{
    public float speed = 20;
    public int bulletDamage = 5;

    protected EnemyAI enemy;
    protected GameObject playerGO;
    protected Player player;
    protected Transform playerTransform;
    protected Vector3 target;

    private void Awake()
    {
        playerGO = GameObject.Find("Player");
        player = playerGO.GetComponent<Player>();
        playerTransform = GameObject.FindGameObjectWithTag("PlayerEyes").transform;
    }

    // Start is called before the first frame update
    void Start()
    {

        transform.position += new Vector3(0, 1.5f, 0); ;
        transform.position += transform.forward * 2;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemy = other.gameObject.GetComponent<EnemyAI>();
            enemy.TakeDamage(25);
            Destroy(gameObject);
        }
        if (other.CompareTag("Boundary"))
        {
            print("Player Projectile Tiggered by wall");
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


    }
}
