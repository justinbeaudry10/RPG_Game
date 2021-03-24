using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target;

    public float speed = 70f;

    public void Seek(Transform _target)
    {
        target = _target;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target==null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 pos = target.position;
        pos.y = pos.y + 1.5f;

        Vector3 dir = pos - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if(dir.magnitude<=distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

    }

    void HitTarget()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(gameObject.tag=="Enemy")
        {
            print("Touching enemy");
            return;
        }
        print("Destroy");
        Destroy(gameObject);
    }

}
