using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public HandGun gunScript;               //Reference the gunScript
    public Rigidbody rb;                    //Reference the pickups rigidbody
    public BoxCollider coll;                //Reference the pickups collider
    public Transform playerTransform, gunContainer, cam;    //Reference the player, gun container and camera

    public float pickUpRange;                       //Variable to store the pick up range
    public float dropFowardForce, dropUpwardForce;  //Variable to store the dropping forces
    public bool playerInPickUpRange;                //Variable to check if the player is in the pickup range
    public LayerMask whatIsPlayer;                  //Variables to reference the player layers
    public Player player;                           //Variable to reference the Player object

    public bool equipped;                           //Checks if weapon is equipped
    public bool slotFull;                    //Variable to check if slot is full

    /// <summary>
    /// Method to pick up the weapon
    /// </summary>
    private void PickUp()
    {
        //The weapon is equipped
        equipped = true;

        //The slot is full
        slotFull = true;

        //Set the parent of the weapon to the gun container on the player game object
        transform.SetParent(gunContainer);

        //Update the weapon's transform to correspond with the gun container (moves the weapon to the player's hand)
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;

        //Make rigidbody kinematic and box collider a trigger
        rb.isKinematic = true;
        coll.isTrigger = true;

        //The player has the gun picked
        player.gunPicked = true;
        print("Player picked up gun");

        //Stop the sparkle effect
        for (int i = 3; i < 7; i++)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(false);
        }

        //Enable script
        gunScript.enabled = true;
    }

    /// <summary>
    /// Method to drop the weapon
    /// </summary>
    private void Drop()
    {
        //Weapon is not equipped
        equipped = false;

        //The slot is not full
        slotFull = false;

        //Remove it from the parent
        transform.SetParent(null);

        //Make rigidbody kinematic and box collider a trigger
        rb.isKinematic = false;
        coll.isTrigger = false;

        //The player doesn't have the gun picked
        player.gunPicked = false;
        print("Player dropped up gun");

        //Play the sparkle effect
        for (int i = 3; i < 7; i++)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(true);
        }

        //Disable script
        gunScript.enabled = false;

        //Make the weapons velocity equal the player's velocity
        rb.velocity = playerTransform.GetComponent<Rigidbody>().velocity;

        //Add a force when dropping the weapon
        rb.AddForce(cam.forward * dropFowardForce, ForceMode.Impulse);
        rb.AddForce(cam.up * dropUpwardForce, ForceMode.Impulse);

        //Give the weapon a random torque (for dropping effect)
        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 10);
    }


    // Start is called before the first frame update
    void Start()
    {
        //If the weapon is not equipped
        if(!equipped)
        {
            //Disable the script
            gunScript.enabled = false;

            //Make rigidbody kinematic and box collider a trigger
            rb.isKinematic = false;
            coll.isTrigger = false;
        }

        //If the weapon is equipped
        if (equipped)
        {
            //Enable the script
            gunScript.enabled = true;

            //Make rigidbody kinematic and box collider a trigger
            rb.isKinematic = true;
            coll.isTrigger = true;

            //The slot is full
            slotFull = true;
        }

        //Assign the player object
        player = playerTransform.gameObject.GetComponent<Player>();

    }

    // Update is called once per frame
    void Update()
    {
        //Check if player is in pickup range
        playerInPickUpRange = Physics.CheckSphere(transform.position, pickUpRange, whatIsPlayer);

        //If the weapon is not equipped and player is in pick up range and the slot is not full and the player presses P
        if (!equipped && playerInPickUpRange && Input.GetKeyDown(KeyCode.P) && !slotFull)
        {
            //Pick up the weapon
            PickUp();
        }

        //If the weapon is equipped and the player presses O
        if (equipped && Input.GetKeyDown(KeyCode.O))
        {
            //Drop the weapon
            Drop();
        }
    }

    /// <summary>
    /// Draws the weapons pick up range
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pickUpRange);
    }
}       