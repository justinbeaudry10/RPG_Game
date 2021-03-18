using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public HealthBar healthBar;
    public int maxHealth = 100;
    public int currentHealth;

    public float moveSpeed;

    public Rigidbody rig;

    public float jumpForce;
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;
    bool isGrounded;

    public Camera cam;
    public GameObject hand;
    public Weapon myWeapon;

    private float attackTimer;

    public float meleeAttackRange;
    public bool playerInMeleeAttackRange;
    public LayerMask whatIsEnemy;


    private void Start()
    {
        // Sets health to max when level starts
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
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
        attackTimer += Time.deltaTime;

        Move();   

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump2();

        }

        if (Input.GetMouseButtonUp(0) && attackTimer>=myWeapon.attackCoolDown)
        {


            //DoAttack();
            DoMeleeAttack();
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
                    EnemyFollow ehealth = hit.collider.GetComponent<EnemyFollow>();
                    ehealth.TakeDamage(myWeapon.attackDamage);
                }
            }
        }
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
    /*void Jump()
    {
        if (CanJump())
        {
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    bool CanJump()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 0.5f))
        {
            return hit.collider != null;
        }
        return false;
    }*/

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeAttackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }

}
