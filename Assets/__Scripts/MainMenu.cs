using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls main menu behaviour
/// </summary>
public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// Starts the game
    /// </summary>
    public void PlayGame()
    {
        //Loads the first level
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// Quits the game
    /// </summary>
    public void QuitGame()
    {
        print("Quit Game");

        //Close the application
        Application.Quit();
    }

}
