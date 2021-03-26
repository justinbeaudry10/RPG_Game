using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Player player;
    public GameObject playerGO;
    public Transform playerTransform;

    public int damage = 10;
    public int expValue = 3;

    public LayerMask whatIsGround, whatIsPlayer;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    public float timeBetweenAttacks;
    bool alreadyAttacked;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public bool onFire = false;
    public bool frozen = false;

    public float health;

    public virtual void Awake()
    {
        playerGO = GameObject.Find("Player");
        playerTransform = GameObject.Find("Player").transform;
        player = playerGO.GetComponent<Player>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        StartCoroutine(burn());
    }

    // Update is called once per frame
    public virtual void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        
        if (!playerInSightRange && !playerInAttackRange)
        {
            Patroling();
        }

        if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }

        if (playerInAttackRange && playerInSightRange)
        {
            AttackPlayer();
        }

    }

    IEnumerator burn()
    {

        while (true)
        {
            if (onFire) {
                TakeDamage(20);
                print("On fire");
                yield return new WaitForSeconds(1);
            }
            else
            {
                yield return null;
            }
        }
    }

    public void Patroling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude<1f)
        {
            walkPointSet = false;
        }

    }

    public void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }

    }

    public void ChasePlayer()
    {
        agent.SetDestination(playerTransform.position);
    }

    public void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(playerTransform);

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            dealDamage(damage);
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

    }

    // Damaging player
    public void dealDamage(int damage)
    {
        player.TakeDamage(damage);
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        
        if (health<=0)
        {
            DestroyEnemy();
        }

    }

    // Freezing attack
    public void freeze()
    {
        agent.speed = 0;
    }


    private void DestroyEnemy()
    {
        player.gainExp(expValue);
        Destroy(gameObject);
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

}
