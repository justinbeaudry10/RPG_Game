using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour
{
    static public Player S; // Singleton for player

    public HealthBar healthBar;
    public ExpBar expBar;
    public AbilityBar fireBar, iceBar, shieldBar;
    public TextMeshProUGUI healthText, levelText;
    public Camera cam;
    public Weapon myWeapon;
    public GameObject hand, projectile, playerProjectile, fireBullet, iceBullet, handGun;

    public int maxHealth = 100;
    public int maxExp = 5;
    public int currentHealth, currentExp;
    public int currentLevel = 1;
    public int abilityCooldown = 10;
    private int fireCooldown, iceCooldown, shieldCooldown;

    public float moveSpeed;

    public Rigidbody rig;

    public float jumpForce;
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;
    bool isGrounded;

    public bool fireClass = false;
    public bool iceClass = false;
    public bool shieldClass = false;

    private bool shieldEnabled = false;

    private float attackTimer;

    public float meleeAttackRange;
    public bool playerInMeleeAttackRange;
    public LayerMask whatIsEnemy;

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
            if(currentHealth < maxHealth)
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
                Invoke("Respawn", 3f);
            }
        }
    }

    private void Respawn()
    {
        Main.S.Restart();
    }

    void Jump2()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded)
        {
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }


    void Update()
    {
        healthText.text = currentHealth.ToString();
        attackTimer += Time.deltaTime;

        Move();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump2();

        }


        if (Input.GetMouseButtonUp(0) && attackTimer >= myWeapon.attackCoolDown)
        {

            //DoAttack();
            DoMeleeAttack();
        }

        if (Input.GetMouseButtonUp(1) && attackTimer >= myWeapon.attackCoolDown)
        {
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

    private void DoAttack()
    {
        
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, myWeapon.attackDamage))
        {
            if(hit.collider.tag == "Enemy")
            {
                EnemyFollow ehealth = hit.collider.GetComponent<EnemyFollow>();
                ehealth.TakeDamage(myWeapon.attackDamage);
            }
        }
    }

    private void DoMeleeAttack()
    {
        playerInMeleeAttackRange = Physics.CheckSphere(transform.position, meleeAttackRange, whatIsEnemy);
        
        if (playerInMeleeAttackRange)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, myWeapon.attackDamage))
            {
                if (hit.collider.tag == "Enemy")
                {
                    EnemyAI ehealth = hit.collider.GetComponent<EnemyAI>();
                    ehealth.TakeDamage(myWeapon.attackDamage);
                }
            }
        }
    }

    private void Shoot()
    {
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

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 dir = transform.right * x + transform.forward * z;
        dir *= moveSpeed;
        dir.y = rig.velocity.y;

        rig.velocity = dir;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeAttackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
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
