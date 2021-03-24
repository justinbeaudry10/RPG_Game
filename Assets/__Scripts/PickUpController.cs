using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public HandGun gunScript;
    public Rigidbody rb;
    public BoxCollider coll;
    public Transform player, gunContainer, fpsCam;

    public float pickUpRange;
    public float dropFowardForce, dropUpwardForce;

    public bool equipped;
    public static bool slotFull;

    private void PickUp()
    {
        equipped = true;
        slotFull = true;

        //Make rigidbody kinematic and box collider a trigger
        rb.isKinematic = true;
        coll.isTrigger = true;

        //Enable script
        gunScript.enabled = true;
    }

    private void Drop()
    {
        equipped = false;
        slotFull = false;

        //Make rigidbody kinematic and box collider a trigger
        rb.isKinematic = false;
        coll.isTrigger = false;

        //Enable script
        gunScript.enabled = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 distanceToPlayer = player.position - transform.position;
        if(!equipped&&distanceToPlayer.magnitude<=pickUpRange && Input.GetKeyDown(KeyCode.E)&&!slotFull)
        {
            PickUp();
        }

        if(equipped&& Input.GetKeyDown(KeyCode.Q))
        {
            Drop();
        }
    }
}
