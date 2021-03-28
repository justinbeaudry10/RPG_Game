using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //Weapon attributes (IN PROGRESS FOR MULTIPLE WEAPONS)
    public int attackDamage;
    public float attackRange;
    public float attackCoolDown;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
