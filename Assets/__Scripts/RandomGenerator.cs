using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGenerator : MonoBehaviour
{
    public GameObject enemy;
    public int xPos;
    public int zPos;
    public int enemyCount;

    void Start()
    {
        StartCoroutine(EnemyDrop());
    }

    IEnumerator EnemyDrop()
    {
        while (enemyCount < 4)
        {
            xPos = Random.Range(-17, 7);
            zPos = Random.Range(-44, 66);
            Instantiate(enemy, new Vector3(xPos, 1, zPos), Quaternion.identity);
            yield return new WaitForSeconds(20f);
            enemyCount += 1;
        }

    }
}
