using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main S;  // Singleton for Main

    private void Awake()
    {
        //Assign the Singleton
        S = this;
    }

    /// <summary>
    /// Method to restart the game
    /// </summary>
    public void Restart()
    {
        //Reload the Scene
        SceneManager.LoadScene("SceneLevel1");
    }
}
