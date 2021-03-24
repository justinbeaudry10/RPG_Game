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

        transform.SetParent(gunContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        //transform.localScale = Vector3.one;

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

        transform.SetParent(null);

        //Make rigidbody kinematic and box collider a trigger
        rb.isKinematic = false;
        coll.isTrigger = false;

        rb.velocity = player.GetComponent<Rigidbody>().velocity;

        rb.AddForce(fpsCam.forward * dropFowardForce, ForceMode.Impulse);
        rb.AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impulse);

        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 10);



    }


    // Start is called before the first frame update
    void Start()
    {
        //Setup
        if(!equipped)
        {
            gunScript.enabled = false;
            //Make rigidbody kinematic and box collider a trigger
            rb.isKinematic = false;
            coll.isTrigger = false;
        }
        if (equipped)
        {
            gunScript.enabled = true;
            //Make rigidbody kinematic and box collider a trigger
            rb.isKinematic = true;
            coll.isTrigger = true;
            slotFull = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*Vector3 distanceToPlayer = player.position - transform.position;
        if(!equipped&&distanceToPlayer.magnitude<=pickUpRange && Input.GetKeyDown(KeyCode.E)&&!slotFull)
        {
            PickUp();
        }

        if(equipped&& Input.GetKeyDown(KeyCode.Q))
        {
            Drop();
        }*/
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            print("Gun picked up");
            equipped = true;
            slotFull = true;

            transform.SetParent(gunContainer);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            transform.localScale = Vector3.one;

            //Make rigidbody kinematic and box collider a trigger
            rb.isKinematic = true;
            coll.isTrigger = true;

            //Enable script
            gunScript.enabled = true;
        }
    }

}
