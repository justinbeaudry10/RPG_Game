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
    public GameObject crossHair, deathText, hand, projectile, playerProjectile, fireBullet, iceBullet, handGun, conText;
    public static GameObject playerHat;                     //References the player's hat
    private static Material hatMaterial;                    //References the player's hat material
    public static bool hasKey = false;                              //Indicates whether or not the player has the key on them


    [Header ("Player Properties Settings")]
    public Rigidbody rig;                                  //References the player's rigidbody
    public static float moveSpeed = 5;                                //Variable to control the player's movement speed
    public static int maxHealth = 100;                            //The player's maximum health
    public static int maxExp = 100;                                //The player's maximum experience points
    public static int currentHealth;                              // The player's current health
    public static int currentExp;                          //The player's experience points
    public static int currentLevel = 1;                    //The player's current level
    public int abilityCooldown = 10;                       //Cool down time before using an ability
    private int fireCooldown, iceCooldown, shieldCooldown; //Stores the cool down tims for the different types of ablilities
    private AudioSource playerShootingAudio;               //Variable to reference the audio source for player shooting
    public static float moveSpeedIncrement = 1;                       //Floating variable to determine speed increase increment upon level up
    public static int meleeAttackIncrement = 5;                     //Floating variable to determine melee attack increase increment upon level up
    public static int maxHealthIncrement = 10;                         //Floating variable to determine max health increase increment upon level up

    [Header("Player Jump Settings")]
    public Transform groundCheck;           //References the GroundCheck GO on the player (used to check if player touches the ground)
    public bool isGrounded;                 //Variable to check if the player is touching the ground
    public LayerMask groundMask;            //References the layer for ground
    private float _jumpForce = 5f;          //Variable to control the player's jump force
    private float _groundDistance = 0.2f;   //Distance the player should be from the ground to jump


    [Header("Player Attack Settings")]
    public static bool fireClass;
    public static bool iceClass;
    public static bool shieldClass;
    private bool shieldEnabled = false;
    private float attackTimer;
    public float meleeAttackRange;
    public bool playerInMeleeAttackRange;
    public LayerMask whatIsEnemy;           //References the layer for enemies
    public static bool gunInInventory;      //Checks if player has gun in inventory



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

        if (SceneManager.GetActiveScene().name == "Level1")
        {
            conText.SetActive(true);
            Weapon.attackDamage = 20;
        }

    }

    private void Start()
    {
        // Sets health to max when level starts
        currentHealth = maxHealth;

        // Set health to the max health
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(maxHealth);

        // Set cooldown time for all class abilities
        fireCooldown = iceCooldown = shieldCooldown = abilityCooldown;

        // Setting max experience value
        expBar.setMaxExp(maxExp);

        expBar.setExp(currentExp);
        levelText.text = "Level " + currentLevel.ToString();

        // Making sure the death text is off
        deathText.SetActive(false);
        // Turning off class ablility bars

        //get the audio source component
        playerShootingAudio = GetComponent<AudioSource>();

        if (fireClass)
        {
            fireBar.ShowBar();
        }
        else if (iceClass)
        {
            iceBar.ShowBar();
        }
        else if (shieldClass)
        {
            shieldBar.ShowBar();
        }

        StartCoroutine(addHealth());
        StartCoroutine(fireRegen());
        StartCoroutine(iceRegen());
        StartCoroutine(shieldRegen());

        //Gets the player's hat material and assigns it
        hatMaterial = gameObject.transform.GetChild(5).gameObject.GetComponent<MeshRenderer>().material;

        //Gets the player's hat and assigns it
        playerHat = gameObject.transform.GetChild(5).gameObject;

        //The gun is not equipped
        PickUpController.equipped = false;

        //Player does not have key on them
        hasKey = false;

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
        //Loop forever
        while (true)
        {
            //If the fire cool down is less than 10
            if (fireCooldown < 10)
            {
                //Increment the cool down by 1
                fireCooldown++;
                
                //Update the fire bar
                fireBar.SetValue(fireCooldown);

                //Wait for 1 second
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
        //Loop forever
        while (true)
        {
            //If the ice cool down is less than 10
            if (iceCooldown < 10)
            {
                //Increment the cool down by 1
                iceCooldown++;

                //Update the ice bar
                iceBar.SetValue(iceCooldown);

                //Wait for 1 second
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
        //Loop forever
        while (true)
        {
            //If the shield cool down is less than 10
            if (shieldCooldown < 10)
            {
                //Increment it by 1
                shieldCooldown++;

                //Update the shield bar 
                shieldBar.SetValue(shieldCooldown);

                //Wait for 1 second
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
        //If shield is not enabled
        if (!shieldEnabled)
        {
            //The player takes damage
            currentHealth -= damage;

            //Update the health bar 
            healthBar.SetHealth(currentHealth);

            // If dead, respawn will full health
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                crossHair.SetActive(false);
                deathText.SetActive(true);

                //Wait 3 seconds before respawning
                Invoke("Respawn", 3f);
            }
        }
    }

    /// <summary>
    /// Method to respawn
    /// </summary>
    private void Respawn()
    {
        fireClass = false;
        iceClass = false;
        shieldClass = false;
        currentExp = 0;
        currentLevel = 1;

        //Restart the game
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
        //Make the health text show the current health
        healthText.text = currentHealth.ToString();

        //Update the attack timer
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

        //If the player right-clicks the mouse and the cool down is done and the gun is picked
        if (Input.GetMouseButtonUp(1) && attackTimer >= myWeapon.attackCoolDown && PickUpController.equipped)
        {
            //Shoot bullets
            Shoot();
            //Play the shooting audio
            playerShootingAudio.Play();
        }

        //If the player presses Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //Shoot fire
            ShootFire();
        }

        //If the player presses E
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Shoot ice
            ShootIce();
        }

        //If the player presses R
        if (Input.GetKeyDown(KeyCode.R))
        {
            //Enable the shield
            EnableShield();
        }

        //If the context UI object is active and the player presses C 
        if (conText.activeInHierarchy && Input.GetKeyDown(KeyCode.C))
        {
            conText.SetActive(false);
        }

        //If the fire class is chosen and the player hat is not active
        if (fireClass && playerHat.activeSelf == false)
        {
            //The player hat is active
            playerHat.SetActive(true);

            //Change the hat's colour to red
            hatMaterial.SetColor("_Color", Color.red);
        }

        //If the ice class is chosen and the player hat is not active
        else if (iceClass && playerHat.activeSelf == false)
        {
            //The player hat is active
            playerHat.SetActive(true);

            //Change the hat's colour to blue
            hatMaterial.SetColor("_Color", Color.blue);
        }

        //If the shield class is chosen and the player hat is not active
        else if (shieldClass && playerHat.activeSelf == false)
        {
            //The player hat is active
            playerHat.SetActive(true);

            //Change the hat's colour to gren
            hatMaterial.SetColor("_Color", Color.green);
        }

        //If the player has the gun in inventory and the gun container has no children (no hand gun)
        if (gunInInventory && transform.Find("GunContainer").childCount == 0)
        {
            //Create a hand gun game object and instantiate it
            GameObject handGunPrefab = Instantiate(handGun, transform.Find("GunContainer").position, Quaternion.identity);

            //Make the hand gun a child of the gun container
            handGunPrefab.transform.parent = transform.Find("GunContainer");
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
            if (Physics.Raycast(ray, out hit, Weapon.attackDamage))
            {
                //If the collider is a regular enemy
                if (hit.collider.tag == "Enemy" && hit.collider.GetComponent<EnemyAI>())
                {
                    //Get health of the enemy
                    EnemyAI ehealth = hit.collider.GetComponent<EnemyAI>();

                    //Reduce the enemy's health
                    ehealth.TakeDamage(Weapon.attackDamage);
                }

                //If the collider is a shooting enemy
                else if (hit.collider.tag == "Enemy" && hit.collider.GetComponent<EnemyShoot>())
                {
                    //Get the shooting enemy's health
                    EnemyShoot ehealth = hit.collider.GetComponent<EnemyShoot>();

                    //Reduce the shooting enemy's health
                    ehealth.TakeDamage(Weapon.attackDamage);
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

    /// <summary>
    /// Method to shoot fire
    /// </summary>
    private void ShootFire()
    {
        //If fire class is enabled and the cooldown is at 10
        if (fireClass && fireCooldown == 10)
        {
            //The cooldown is 0
            fireCooldown = 0;

            //Update the fire bar
            fireBar.SetValue(fireCooldown);

            //Create a fire bullet
            Instantiate(fireBullet, transform.position, Camera.main.transform.rotation);
        }
    }

    /// <summary>
    /// Method to shoot ice
    /// </summary>
    private void ShootIce()
    {
        //If ice class is enabled and the cooldown is at 10
        if (iceClass && iceCooldown == 10)
        {
            //The cooldown is 0
            iceCooldown = 0;

            //Update the ice bar
            iceBar.SetValue(iceCooldown);

            //Create a ice bullet
            Instantiate(iceBullet, transform.position, Camera.main.transform.rotation);
        }
    }

    /// <summary>
    /// Method to enable the shield
    /// </summary>
    private void EnableShield()
    {
        //If shield class is true and the cooldown is at 10
        if (shieldClass && shieldCooldown == 10)
        {
            //The shield is enabled
            shieldEnabled = true;

            //The cooldown is 0
            shieldCooldown = 0;

            //Update the shield bar
            shieldBar.SetValue(shieldCooldown);

            //Disable the shield in 5 seconds
            Invoke("DisableShield", 5);
        }
    }

    /// <summary>
    /// Method to disable the shield
    /// </summary>
    private void DisableShield()
    {
        //Disable the shield
        shieldEnabled = false;
    }

    /// <summary>
    /// Method to gain experince points
    /// </summary>
    /// <param name="exp"></param>
    public void gainExp(int exp)
    {
        //Update the current exp
        currentExp += exp;

        //If the current exp is greater than the max exp
        if (currentExp >= maxExp)
        {
            //Store the extra exp in a variable
            int extraExp = currentExp - maxExp;

            //Increase the level by 1
            LevelUp();

            //Make the current exp equal the extra exp
            currentExp = extraExp;

            //Update the exp bar
            expBar.setMaxExp(maxExp);
        }

        //Update the exp bar
        expBar.setExp(currentExp);

        //Update the level text
        levelText.text = "Level " + currentLevel.ToString();
    }

    /// <summary>
    /// Method to handling leveling up
    /// </summary>
    void LevelUp()
    {
        //Increases current level by 1
        currentLevel++;
        //Increases max health value by the given increment
        IncreaseMaxHealth(maxHealthIncrement);
        // Increases melee damage by given increment;
        IncreaseMeleeDamage(meleeAttackIncrement);
        // Increases movement speed by given increment
        IncreasePlayerMovementSpeed(moveSpeedIncrement);

        //Logging change in console
        print("Player has levelled up.");
        print("Current Max Health: " + maxHealth + " Current Melee Attack Damage: " + Weapon.attackDamage + " Current Movement Speed: " + moveSpeed);

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
        //If the player picks a fire potion
        if (coll.CompareTag("FirePotion"))
        {
            //Enable the fire class
            fireClass = true;

            //Show the fire bar
            fireBar.ShowBar();

            //Destory the potions
            DestroyPotions();

            //The player's hat is visible
            playerHat.SetActive(true);

            //Change the hat's colour to red
            hatMaterial.SetColor("_Color", Color.red);
        }

        //If the player picks a ice potion
        else if (coll.CompareTag("IcePotion"))
        {
            //Enable the ice class
            iceClass = true;

            //Show the ice bar
            iceBar.ShowBar();

            //Destory the potions
            DestroyPotions();

            //The player's hat is visible
            playerHat.SetActive(true);

            //Change the hat's colour to blue
            hatMaterial.SetColor("_Color", Color.blue);
        }

        //If the player picks a shield potion
        else if (coll.CompareTag("ShieldPotion"))
        {
            //Enable the shield class 
            shieldClass = true;

            //Show the shield bar
            shieldBar.ShowBar();

            //Destory the potions
            DestroyPotions();

            //The player's hat is visible
            playerHat.SetActive(true);

            //Change the hat's colour to green
            hatMaterial.SetColor("_Color", Color.green);
        }
    }

    /// <summary>
    /// Method to destory the potions
    /// </summary>
    private void DestroyPotions()
    {
        //Find the potion game objects
        GameObject ShieldPotion = GameObject.FindGameObjectWithTag("ShieldPotion");
        GameObject FirePotion = GameObject.FindGameObjectWithTag("FirePotion");
        GameObject IcePotion = GameObject.FindGameObjectWithTag("IcePotion");

        //Destory the potions
        Destroy(ShieldPotion);
        Destroy(FirePotion);
        Destroy(IcePotion);
    }

    /// <summary>
    /// Increase max health upon level increase
    /// </summary>
    private void IncreaseMaxHealth(int factor)
    {

        maxHealth += factor;

        //Reflect changes in the health bar
        healthBar.SetMaxHealth(maxHealth);
    }


    /// <summary>
    /// Increases melee attack damage for the player
    /// </summary>
    private void IncreaseMeleeDamage(int factor)
    {
        //Increase Melee Damage by the given factor
        myWeapon.increaseAttackDamage(factor);

    }

    /// <summary>
    /// Increases movement speed for the player
    /// </summary>
    /// <param name="factor"></param>
    private void IncreasePlayerMovementSpeed(float factor)
    {
        moveSpeed += factor;
    }
    

}
