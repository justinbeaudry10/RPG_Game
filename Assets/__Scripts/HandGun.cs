using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    public Camera fpsCam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }*/
    }

    void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            print(hit.transform.name);
        }
    }
}
