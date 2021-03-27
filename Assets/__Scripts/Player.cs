using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour
{
    static public Player S; // Singleton for player

    [Header("Player and Game System Settings")]
    public HealthBar healthBar;
    public ExpBar expBar;
    public AbilityBar fireBar, iceBar, shieldBar;
    public TextMeshProUGUI healthText, levelText;
    public Camera cam;
    public Weapon myWeapon;
    public GameObject crossHair, deathText, hand, projectile, playerProjectile, fireBullet, iceBullet, handGun;

    [Header ("Player Properties Settings")]
    public Rigidbody rig;                                   //References the player's rigidbody
    public float moveSpeed;                                 //Variable to control the player's movement speed
    public int maxHealth = 100;                             //The player's maximum health
    public int maxExp = 5;                                  //The player's maximum experience points
    public int currentHealth, currentExp;                   //The player's current health and experience points
    public int currentLevel = 1;                            //The player's current level
    public int abilityCooldown = 10;                        //Cool down time before using an ability
    private int fireCooldown, iceCooldown, shieldCooldown;  //Stores the cool down tims for the different types of ablilities 

    [Header("Player Jump Settings")]
    public Transform groundCheck;           //References the GroundCheck GO on the player (used to check if player touches the ground)
    public bool isGrounded;                 //Variable to check if the player is touching the ground
    public LayerMask groundMask;            //References the layer for ground
    private float _jumpForce = 5f;          //Variable to control the player's jump force
    private float _groundDistance = 0.2f;   //Distance the player should be from the ground to jump


    [Header("Player Attack Settings")]
    public bool fireClass = false;
    public bool iceClass = false;
    public bool shieldClass = false;
    private bool shieldEnabled = false;
    private float attackTimer;
    public float meleeAttackRange;
    public bool playerInMeleeAttackRange;
    public LayerMask whatIsEnemy;           //References the layer for enemies
    public bool gunPicked = false;


    private void Awake()
    {
        if (S == null)
        {
            S = this; // Set the Singleton
        }
        else
        {
            Debug.LogError("Player.Awake() - Attempted to assign second Player.S!");
        }
    }

    private void Start()
    {
        // Sets health to max when level starts
        currentHealth = maxHealth;
        currentExp = 0;
        healthBar.SetMaxHealth(maxHealth);

        fireCooldown = iceCooldown = shieldCooldown = abilityCooldown;

        expBar.setMaxExp(maxExp);
        deathText.SetActive(false);
        fireBar.HideBar();
        iceBar.HideBar();
        shieldBar.HideBar();
        StartCoroutine(addHealth());
        StartCoroutine(fireRegen());
        StartCoroutine(iceRegen());
        StartCoroutine(shieldRegen());
    }

    IEnumerator addHealth()
    {
        // Loops forever
        while (true)
        {   // If current health is less than max, regen
            if (currentHealth < maxHealth && currentHealth > 0)
            {
                currentHealth += 5;
                healthBar.SetHealth(currentHealth);
                yield return new WaitForSeconds(5);
            }
            else
            {
                yield return null;
            }
        }
    }

    IEnumerator fireRegen()
    {
        while (true)
        {
            if (fireCooldown < 10)
            {
                fireCooldown++;
                fireBar.SetValue(fireCooldown);
                yield return new WaitForSeconds(1);
            }
            else
            {
                yield return null;
            }
        }
    }

    IEnumerator iceRegen()
    {
        while (true)
        {
            if (iceCooldown < 10)
            {
                iceCooldown++;
                iceBar.SetValue(iceCooldown);
                yield return new WaitForSeconds(1);
            }
            else
            {
                yield return null;
            }
        }
    }

    IEnumerator shieldRegen()
    {
        while (true)
        {
            if (shieldCooldown < 10)
            {
                shieldCooldown++;
                shieldBar.SetValue(shieldCooldown);
                yield return new WaitForSeconds(1);
            }
            else
            {
                yield return null;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (!shieldEnabled)
        {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);

            // If dead, respawn will full health
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                crossHair.SetActive(false);
                deathText.SetActive(true);
                Invoke("Respawn", 3f);
            }
        }
    }

    private void Respawn()
    {
        Main.S.Restart();
    }

    /// <summary>
    /// Controls the player's jump movement
    /// </summary>
    void Jump()
    {
        //Checks if the player is touching the ground
        //CheckSphere checks if any colliders overlapping the sphere
        isGrounded = Physics.CheckSphere(groundCheck.position, _groundDistance, groundMask);

        //If the player is touching the ground
        if (isGrounded)
        {
            //The player can jump
            //Add a force in the upward direction to create the jump
            rig.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }


    void Update()
    {
        healthText.text = currentHealth.ToString();
        attackTimer += Time.deltaTime;

        //Moves the player
        Move();

        //If the user presses the space bar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //The player jumps
            Jump();
        }

        //If the player left-clicks and the attack cool down is done
        if (Input.GetMouseButtonUp(0) && attackTimer >= myWeapon.attackCoolDown)
        {
            //Do a melee attack
            DoMeleeAttack();
        }

        //If the player right-clicks the mouse an the cool down is done
        if (Input.GetMouseButtonUp(1) && attackTimer >= myWeapon.attackCoolDown)
        {
            //Shoot bullets
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ShootFire();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ShootIce();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            EnableShield();
        }


    }

    /// <summary>
    /// Controls the player's melee attack
    /// </summary>
    private void DoMeleeAttack()
    {
        //Checks if the player is within attack range of the enemy
        playerInMeleeAttackRange = Physics.CheckSphere(transform.position, meleeAttackRange, whatIsEnemy);

        //If the player is within attack range
        if (playerInMeleeAttackRange)
        {
            //Create a ray at the mouses position
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            //Creating a variable to read information from the ray
            RaycastHit hit;

            //If the raycast hits something withing the range
            if (Physics.Raycast(ray, out hit, myWeapon.attackDamage))
            {
                //If the collider is a regular enemy
                if (hit.collider.tag == "Enemy" && hit.collider.GetComponent<EnemyAI>())
                {
                    //Get health of the enemy
                    EnemyAI ehealth = hit.collider.GetComponent<EnemyAI>();

                    //Reduce the enemy's health
                    ehealth.TakeDamage(myWeapon.attackDamage);
                }

                //If the collider is a shooting enemy
                else if (hit.collider.tag == "Enemy" && hit.collider.GetComponent<EnemyShoot>())
                {
                    //Get the shooting enemy's health
                    EnemyShoot ehealth = hit.collider.GetComponent<EnemyShoot>();

                    //Reduce the shooting enemy's health
                    ehealth.TakeDamage(myWeapon.attackDamage);
                }
            }
        }
    }

    /// <summary>
    /// Control the player's shooting
    /// </summary>
    private void Shoot()
    {
        //Create a projectile 
        Instantiate(playerProjectile, transform.position, Camera.main.transform.rotation);
    }

    private void ShootFire()
    {
        if (fireClass && fireCooldown == 10)
        {
            fireCooldown = 0;
            fireBar.SetValue(fireCooldown);
            Instantiate(fireBullet, transform.position, Camera.main.transform.rotation);
        }
    }

    private void ShootIce()
    {
        if (iceClass && iceCooldown == 10)
        {
            iceCooldown = 0;
            iceBar.SetValue(iceCooldown);
            Instantiate(iceBullet, transform.position, Camera.main.transform.rotation);
        }
    }

    private void EnableShield()
    {
        if (shieldClass && shieldCooldown == 10)
        {
            shieldEnabled = true;
            shieldCooldown = 0;
            shieldBar.SetValue(shieldCooldown);
            Invoke("DisableShield", 5);
        }
    }

    private void DisableShield()
    {
        shieldEnabled = false;
    }

    public void gainExp(int exp)
    {
        currentExp += exp;

        if (currentExp > maxExp)
        {
            int extraExp = currentExp - maxExp;
            currentLevel++;
            currentExp = extraExp;
            expBar.setMaxExp(maxExp);
        }

        expBar.setExp(currentExp);
        levelText.text = "Level " + currentLevel.ToString();
    }

    /// <summary>
    /// Controls the player's movement
    /// </summary>
    void Move()
    {
        //Get the keyboard's axis for movement
        float x = Input.GetAxis("Horizontal");  //To move left and right
        float z = Input.GetAxis("Vertical");    //To move foward and backward

        //Store the players movement direction
        Vector3 dir = transform.right * x + transform.forward * z;

        //Multiply the movement direction vector by the movement speed
        dir *= moveSpeed;

        //Set the y value of the direction equal to the rigidbody's y value
        //For physics calculations such as gravity
        dir.y = rig.velocity.y;

        //Set the direction
        rig.velocity = dir;
    }

    /// <summary>
    /// Draws the melee attack range and ground range
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeAttackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, _groundDistance);
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("FirePotion"))
        {
            print("Player has chosen fire potion");
            fireClass = true;
            fireBar.ShowBar();
            DestroyPotions();
        }

        else if (coll.CompareTag("IcePotion"))
        {
            print("Player has chosen ice potion");
            iceClass = true;
            iceBar.ShowBar();
            DestroyPotions();
        }
        else if (coll.CompareTag("ShieldPotion"))
        {
            print("Player has chosen shield potion");
            shieldClass = true;
            shieldBar.ShowBar();
            DestroyPotions();
        }


    }

    private void DestroyPotions()
    {
        GameObject ShieldPotion = GameObject.FindGameObjectWithTag("ShieldPotion");
        GameObject FirePotion = GameObject.FindGameObjectWithTag("FirePotion");
        GameObject IcePotion = GameObject.FindGameObjectWithTag("IcePotion");

        Destroy(ShieldPotion);
        Destroy(FirePotion);
        Destroy(IcePotion);

    }
}
