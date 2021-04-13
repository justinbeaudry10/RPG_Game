using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //Weapon attributes (IN PROGRESS FOR MULTIPLE WEAPONS)
    public static  int attackDamage;
    public float attackRange;
    public float attackCoolDown;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    /// <summary>
    /// Increases the attack damage
    /// </summary>
    /// <param name="i"></param>
    public void increaseAttackDamage(int i)
    {
        //Increments attackDamage parameter by the value entered
        attackDamage += i;
    }
}
