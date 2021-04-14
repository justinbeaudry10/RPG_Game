using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main S;  // Singleton for Main

    public GameObject enemy;
    private int xPos;
    private int zPos;
    public int enemyCount;

    private void Awake()
    {
        //Assign the Singleton
        S = this;
    }

    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "Level1")
        {
            // Spawning enemies
            for (int enemies = 0; enemies < enemyCount; enemies++)
            {
                xPos = Random.Range(-10, 0);
                zPos = Random.Range(-20, 50);
                Instantiate(enemy, new Vector3(xPos, 1, zPos), Quaternion.identity);
            }

        }
       
    }
    /// <summary>
    /// Method to restart the game
    /// </summary>
    public void Restart()
    {
        Player.gunInInventory = false;
        PickUpController.equipped = false;
        PickUpController.slotFull = false;
        //Reload the Scene
        SceneManager.LoadScene("Level1");     
    }
}
