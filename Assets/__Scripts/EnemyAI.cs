using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Enemy and Game System Settings")]
    public NavMeshAgent agent;                          //Variable to reference the NavMeshAgent (allows for AI type movement)
    public Player player;                               //Variable to reference the Player object
    public GameObject playerGO;                         //Variable to reference the Player GameObject
    public Transform playerTransform;                   //Variable to reference the Player's transform component
    public LayerMask whatIsGround, whatIsPlayer;        //Variables to reference the ground and player layers


    [Header("Enemy Properties Settings")]
    public int damage = 10;     //Variable for the enemy damage
    public int expValue = 3;    //Variables for the enemy's experience value
    public float health;        //Variable for the enemy's health


    [Header("Enemy Attack Settings")]
    public Vector3 walkPoint;                               //Variable to store the enemy's walk point
    bool walkPointSet;                                      //Variable to check if the walk point is set
    public float walkPointRange;                            //Variable to store the walk point range
    public float timeBetweenAttacks;                        //Variable to store the time between attacks
    bool alreadyAttacked;                                   //Variable to check if enemy already attacked
    public float sightRange, attackRange;                   //Variables to store the sight range and attack range
    public bool playerInSightRange, playerInAttackRange;    //Variables to check if the player is in sight range or attack range
    public bool onFire = false;                             //Variable to check if enemy is on fire
    private AudioSource damageAudio;                        //Variable to reference the audio source for player and enemy damage


    public virtual void Awake()
    {
        //Assigning the player GameObject
        playerGO = GameObject.Find("Player");

        //Assigning the player transform component
        playerTransform = GameObject.Find("Player").transform;

        //Assigning the player object
        player = playerGO.GetComponent<Player>();

        //Assigning the NavMeshAgent
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        //Starts a coroutine with burn method
        StartCoroutine(burn());

        //get the audio source component
        damageAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        //Check if player is in sight range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        //Check if player is in attack range
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        //If the player is not in sight and not in attack range
        if (!playerInSightRange && !playerInAttackRange)
        {
            //Patrol
            Patroling();
        }

        //If the player is in sight but not in attack range
        if (playerInSightRange && !playerInAttackRange)
        {
            //Chase the player
            ChasePlayer();
        }

        //If the player is in sight and in attack range
        if (playerInSightRange && playerInAttackRange)
        {
            //Attack the player
            AttackPlayer();
        }

    }

    public virtual void SetOnFire()
    {
        onFire = true;
    }

    /// <summary>
    /// Burning attack
    /// </summary>
    /// <returns></returns>
    IEnumerator burn()
    {
        //While loop
        while (true)
        {
            //If the enemy is on fire
            if (onFire)
            {
                //Take 20 damage
                TakeDamage(20);

                //Print on fire
                print("On fire");

                //Wait for 1 second
                yield return new WaitForSeconds(1);
            }

            //If enemy is not on fire
            else
            {
                //Return null
                yield return null;
            }
        }
    }

    /// <summary>
    /// Method to make the enemy patrol
    /// </summary>
    public void Patroling()
    {
        //If there is not a walk point set
        if (!walkPointSet)
        {
            //Search for a walk point
            SearchWalkPoint();
        }

        //If the walk point is set
        if (walkPointSet)
        {
            //Move towards the walk point
            agent.SetDestination(walkPoint);
        }

        //Variable to store the distance to the walk point
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //If the distance to the walk point is less than 1
        if (distanceToWalkPoint.magnitude < 1f)
        {
            //The walk point is not set (the enemy is at the walk point)
            walkPointSet = false;
        }
    }

    /// <summary>
    /// Method to search for a walk point
    /// </summary>
    public void SearchWalkPoint()
    {
        //Calculate a random point in the walk range for the z and x coordinates
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        //Set the walk point
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        //If the walk point is on the ground
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            //The walk is set
            walkPointSet = true;
        }
    }

    /// <summary>
    /// Method to chase the player
    /// </summary>
    public virtual void ChasePlayer()
    {
        //Move towards the player
        agent.SetDestination(playerTransform.position);
    }

    /// <summary>
    /// Method to attack the player
    /// </summary>
    public virtual void AttackPlayer()
    {
        //Stop enemy from moving
        agent.SetDestination(transform.position);

        //Make the enemy look at the player
        transform.LookAt(playerTransform);

        //If the enemy has not attacked yet
        if (!alreadyAttacked)
        {
            //The enemy is has now attached
            alreadyAttacked = true;

            //The player takes damage
            dealDamage(damage);

            //Reset the attack in "timeBetweenAttacks" seconds
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

    }

    /// <summary>
    /// Method to damage the player
    /// </summary>
    /// <param name="damage"></param>
    public void dealDamage(int damage)
    {
        //Play the damage audio
        damageAudio.Play();

        //The player takes damage
        player.TakeDamage(damage);
    }

    /// <summary>
    /// Method to reset the attack
    /// </summary>
    private void ResetAttack()
    {
        //already attacked is now false (the time between attacks has passed)
        alreadyAttacked = false;
    }

    /// <summary>
    /// Method for the enemy to take damage
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        //Decrease the health by damage
        health -= damage;

        //If the health is less than 0
        if (health <= 0)
        {
            //Destory the enemy
            DestroyEnemy();
        }

    }

    /// <summary>
    /// Freezing attack
    /// </summary>
    public virtual void freeze()
    {
        //The enemy's speed is 0
        agent.speed = 0;
    }


    /// <summary>
    /// Method to destory the enemy
    /// </summary>
    private void DestroyEnemy()
    {
        //The player gains the experince value
        player.gainExp(expValue);

        //Destory the enemy game object
        Destroy(gameObject);
    }

    /// <summary>
    /// Draw spheres of the enemy's sight and attack range
    /// </summary>
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
