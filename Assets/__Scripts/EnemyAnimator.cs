using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimator : MonoBehaviour
{
    const float locomotionAnimationSmoothTime = .1f;

    EnemyAI enemy;
    Rigidbody rb;
    NavMeshAgent navMesh;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<EnemyAI>();
        rb = GetComponent<Rigidbody>();
        navMesh = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float speedPercent;

        if(navMesh.velocity != Vector3.zero)
        {
            speedPercent = 1;
        }
        else
        {
            speedPercent = 0;
        }

        animator.SetFloat("speedPercent", speedPercent, locomotionAnimationSmoothTime, Time.deltaTime);
    }
}
