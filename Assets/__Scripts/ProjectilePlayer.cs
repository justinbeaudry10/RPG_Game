using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePlayer : MonoBehaviour
{
    public float speed;
    public int bulletDamage = 5;

    private GameObject playerGO;
    private Player player;
    private Transform playerTransform;
    private Vector3 target;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        transform.LookAt(position);

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
        if (other.CompareTag("Enemy"))
        {
            print("Hit Enemy");
            Destroy(other);
            Destroy(gameObject);
        }
        else
        {
            print("PlayerProjectileTrigger");
            Destroy(gameObject);
        }


    }
}
