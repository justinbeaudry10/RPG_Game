using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAnimator : MonoBehaviour
{
    const float locomotionAnimationSmoothTime = .1f;

    Player player;
    Rigidbody rb;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float speedPercent = rb.velocity.magnitude / Player.moveSpeed;
        animator.SetFloat("speedPercent", 0.5f * speedPercent, locomotionAnimationSmoothTime, Time.deltaTime);
    }
}
